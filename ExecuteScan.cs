//******************************************************************************************************
//  MainForm_ExecuteScan.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/10/2020 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GSF;
using GSF.Data.Model;
using GSF.PhasorProtocols;
using PhasorProtocolAdapters;
using AutoConfigPortScanner.Model;
using GSF.Diagnostics;

namespace AutoConfigPortScanner
{
    partial class MainForm
    {
        // Connection string parameters for active COM connection
        private const string ActiveScanConnectionString = "autoStartDataParsingSequence = true; skipDisableRealTimeData = false; disableRealTimeDataOnStop = true";

        // Connection string parameters for passive COM connection, e.g., config frame once per minute
        private const string PassiveScanConnectionString = "autoStartDataParsingSequence = false; skipDisableRealTimeData = true; disableRealTimeDataOnStop = false";

        // Connection string template
        private const string ConnectionStringTemplate = "transportProtocol=Serial; port=COM{0}; baudrate={1}; parity={2}; stopbits={3}; databits={4}; dtrenable={5}; rtsenable={6}; {7}";

        private const string Tab1 = "    ";
        private const string Tab2 = Tab1 + Tab1;
        private const string Tab3 = Tab2 + Tab1;

        private ManualResetEventSlim m_configurationWaitHandle;
        private ManualResetEventSlim m_bytesReceivedWaitHandle;
        private MultiProtocolFrameParser m_frameParser;
        private IConfigurationFrame m_configurationFrame;
        private bool m_autoStartParsingSequenceForScan;

        private void ExecuteScan(ScanParameters scanParams, CancellationToken cancellationToken)
        {
            // Create wait handles to use to wait for configuration frame
            m_configurationWaitHandle ??= new ManualResetEventSlim(false);
            m_bytesReceivedWaitHandle ??= new ManualResetEventSlim(false);

            // Create a new phasor protocol frame parser used to dynamically request device configuration frames
            // and return them to remote clients so that the frame can be used in system setup and configuration
            if (m_frameParser is null)
            {
                m_frameParser = new MultiProtocolFrameParser();

                // Attach to events on new frame parser reference
                m_frameParser.ConnectionAttempt += m_frameParser_ConnectionAttempt;
                m_frameParser.ConnectionEstablished += m_frameParser_ConnectionEstablished;
                m_frameParser.ConnectionException += m_frameParser_ConnectionException;
                m_frameParser.ConnectionTerminated += m_frameParser_ConnectionTerminated;
                m_frameParser.ExceededParsingExceptionThreshold += m_frameParser_ExceededParsingExceptionThreshold;
                m_frameParser.ParsingException += m_frameParser_ParsingException;
                m_frameParser.ReceivedConfigurationFrame += m_frameParser_ReceivedConfigurationFrame;
                m_frameParser.BufferParsed += m_frameParser_BufferParsed;

                // We only want to try to connect to device and retrieve configuration as quickly as possible
                m_frameParser.MaximumConnectionAttempts = 1;
                m_frameParser.SourceName = Name;
                m_frameParser.AutoRepeatCapturedPlayback = false;
                m_frameParser.AutoStartDataParsingSequence = false;
                m_frameParser.SkipDisableRealTimeData = true;
            }

            Task.Run(() =>
            {
                ClearFeedback();
                UpdateFeedback();

                Ticks scanStartTime = DateTime.UtcNow.Ticks;

                try
                {
                    SetControlEnabledState(buttonScan, false);

                    TableOperations<Device> deviceTable = scanParams.DeviceTable;
                    ushort[] comPorts = scanParams.ComPorts;
                    ushort[] idCodes = scanParams.IDCodes;
                    bool rescan = scanParams.Rescan;
                    int scannedIDCodes = 0;
                    HashSet<ushort> configuredPorts = new();
                    HashSet<ushort> configuredIDCodes = new();
                    HashSet<ushort> idCodeSettings = new(idCodes);
                    int discoveredDevices = 0;
                    long totalComScanTime = 0L;
                    long totalComScans = 0L;

                    ShowUpdateMessage("Reading existing configuration...");
                    Device[] devices = deviceTable.QueryRecordsWhere("IsConcentrator = 0").ToArray();

                    // If re-scanning all ports, we will not skip pre-configured ports and ID codes
                    if (rescan)
                    {
                        ShowUpdateMessage($"{Tab1}Discovered {devices.Length:N0} existing devices{Environment.NewLine}");
                    }
                    else
                    {
                        foreach (Device device in devices)
                        {
                            if (device is null)
                                continue;

                            Dictionary<string, string> settings = device.ConnectionString.ParseKeyValuePairs();

                            if (settings.TryGetValue("port", out string portVal) && ushort.TryParse(portVal.Substring(3), out ushort port))
                                configuredPorts.Add(port);

                            configuredIDCodes.Add((ushort)device.AccessID);
                        }

                        ShowUpdateMessage($"{Tab1}Discovered {devices.Length:N0} existing devices, {configuredPorts.Count:N0} configured COM ports and {configuredIDCodes.Count:N0} unique ID codes{Environment.NewLine}");
                    }

                    // Hold onto to device list, useful when saving configurations later (no need to re-query)
                    scanParams.Devices = devices;

                    // Only control progress bar for manual (non-import) scans
                    if (buttonImport.Enabled)
                    {
                        SetProgressBarMinMax(0, idCodes.Length);
                        UpdateProgressBar(0);
                    }

                    foreach (ushort idCode in idCodes)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (configuredIDCodes.Contains(idCode))
                        {
                            ShowUpdateMessage($"Skipping scan for already configured ID code {idCode}...");
                        }
                        else
                        {
                            Ticks idScanStartTime = DateTime.UtcNow.Ticks;

                            try
                            {
                                ShowUpdateMessage($"Starting scan for ID code {idCode}...");
                                int comScans = 0;
                                bool found = false;

                                foreach (ushort comPort in comPorts)
                                {
                                    cancellationToken.ThrowIfCancellationRequested();

                                    Ticks comScanStartTime = DateTime.UtcNow.Ticks;

                                    try
                                    {
                                        if (configuredPorts.Contains(comPort))
                                        {
                                            ShowUpdateMessage($"Skipping scan for already configured COM{comPort}...");
                                            continue;
                                        }

                                        if (!ScanPortWithIDCode(comPort, idCode, scanParams, cancellationToken))
                                            continue;

                                        // Shorten COM port scan list as new devices are detected
                                        configuredPorts.Add(comPort);
                                        UpdateFeedback(null, ++discoveredDevices);
                                        found = true;
                                        break;
                                    }
                                    finally
                                    {
                                        Ticks comScanTime = DateTime.UtcNow.Ticks - comScanStartTime;
                                        ShowUpdateMessage($"{Environment.NewLine}>> Scan time for COM{comPort} for ID code {idCode}: {comScanTime.ToElapsedTimeString(3)}.{Environment.NewLine}");

                                        comScans++;
                                        totalComScanTime += comScanTime.Value;
                                        totalComScans++;

                                        long remainingTimeEstimate =
                                            (idCodes.Length - configuredIDCodes.Count - scannedIDCodes - 1) *
                                            (comPorts.Length - configuredPorts.Count);
                                            
                                        if (!found)
                                            remainingTimeEstimate += comPorts.Length - configuredPorts.Count - comScans;

                                        remainingTimeEstimate *= totalComScanTime / totalComScans;
                                        UpdateFeedback(new Ticks(remainingTimeEstimate).ToElapsedTimeString(0));
                                    }
                                }

                                ShowUpdateMessage($"Completed scan for ID code {idCode}.");
                            }
                            finally
                            {
                                ShowUpdateMessage($"{Environment.NewLine}>>>> Scan time for ID code {idCode}: {(DateTime.UtcNow.Ticks - idScanStartTime).ToElapsedTimeString(3)}.{Environment.NewLine}");
                            }
                        }

                        scannedIDCodes++;

                        // Only control progress bar for manual (non-import) scans
                        if (buttonImport.Enabled)
                            UpdateProgressBar(scannedIDCodes);

                        // Serialize reduced ID code list
                        try
                        {
                            // In case app needs to restart do not rescan existing ID codes
                            if (Settings.AutoRemoveIDs && idCodeSettings.Remove(idCode))
                            {
                                Settings.IDCodes = idCodeSettings.ToArray();
                                Settings.Save();
                            }
                        }
                        catch (Exception ex)
                        {
                            m_log.Publish(MessageLevel.Error, "UpdateIDCodes", "Failed while reducing ID code list", exception: ex);
                        }
                    }

                    ShowUpdateMessage($"Completed scan for {scannedIDCodes:N0} ID codes over {comPorts.Length:N0} COM ports.{Environment.NewLine}");
                    UpdateFeedback("None -- Operation Complete");
                }
                catch (OperationCanceledException)
                {
                    ShowUpdateMessage($"{Environment.NewLine}Serial port scan cancelled.{Environment.NewLine}");
                    UpdateFeedback("None -- Operation Cancelled");
                }
                catch (Exception ex)
                {
                    ShowUpdateMessage($"{Environment.NewLine}ERROR: Failed during serial port scan: {ex.Message}");
                    UpdateFeedback("None -- Operation Failed");
                }
                finally
                {
                    ShowUpdateMessage($">>>>>> Total scan time: {(DateTime.UtcNow.Ticks - scanStartTime).ToElapsedTimeString(3)}.{Environment.NewLine}");
                    SetControlEnabledState(buttonScan, true);
                    m_scanExecutionComplete.Set();
                }
            },
            cancellationToken);
        }

        private bool ScanPortWithIDCode(ushort comPort, ushort idCode, ScanParameters scanParams, CancellationToken cancellationToken)
        {
            bool autoStartParsingSequenceForScan = scanParams.AutoStartParsingSequenceForScan;
            string scanConnectionMode = autoStartParsingSequenceForScan ? ActiveScanConnectionString : PassiveScanConnectionString;
            string connectionString = string.Format(ConnectionStringTemplate, comPort, Settings.BaudRate, Settings.Parity, Settings.StopBits, Settings.DataBits, Settings.DtrEnable, Settings.RtsEnable, scanConnectionMode);

            ShowUpdateMessage($"{Tab1}Scanning COM{comPort} with ID code {idCode}...");

            IConfigurationFrame configFrame = RequestDeviceConfiguration(connectionString, idCode, scanParams, cancellationToken);
            
            return configFrame is not ConfigurationErrorFrame && SaveDeviceConfiguration(configFrame, comPort, idCode, scanParams);
        }

        public IConfigurationFrame RequestDeviceConfiguration(string connectionString, int idCode, ScanParameters scanParams, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                ShowUpdateMessage($"{Tab2}ERROR: No connection string was specified, request for configuration canceled.");
                return new ConfigurationErrorFrame();
            }

            try
            {
                bool autoStartParsingSequenceForScan = scanParams.AutoStartParsingSequenceForScan;
                int responseTimeout = scanParams.ResponseTimeout;
                int configFrameTimeout = scanParams.ConfigFrameTimeout;

                // Most of the parameters in the connection string will be for the data source in the frame parser
                // so we provide all of them, other parameters will simply be ignored
                m_frameParser.ConnectionString = connectionString;

                // Provide access ID to frame parser as this may be necessary to make a phasor connection
                m_frameParser.DeviceID = (ushort)idCode;

                // Clear any existing configuration frame
                m_configurationFrame = null;

                // Set auto-start parsing sequence for scan state
                m_autoStartParsingSequenceForScan = autoStartParsingSequenceForScan;

                // Inform user of temporary loss of command access
                ShowUpdateMessage($"{Tab2}Requesting device configuration...");

                // Make sure the wait handles are not set
                m_configurationWaitHandle.Reset();
                m_bytesReceivedWaitHandle.Reset();

                // Start the frame parser - this will attempt connection
                m_frameParser.Start();

                // Wait for any bytes received within configured response timeout
                if (!m_bytesReceivedWaitHandle.Wait(responseTimeout, cancellationToken))
                {
                    ShowUpdateMessage($"{Tab2}Timed-out waiting for device response.");
                }
                else
                {
                    // Wait to receive the configuration frame
                    if (!m_configurationWaitHandle.Wait(configFrameTimeout, cancellationToken))
                        ShowUpdateMessage($"{Tab2}Timed-out waiting to receive remote device configuration.");
                }

                // Terminate connection to device
                m_frameParser.Stop();

                if (m_configurationFrame is null)
                {
                    m_configurationFrame = new ConfigurationErrorFrame();
                    ShowUpdateMessage($"{Tab2}Failed to retrieve remote device configuration.");
                }

                return m_configurationFrame;
            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"{Tab2}ERROR: Failed to request configuration due to exception: {ex.Message}");
            }

            return new ConfigurationErrorFrame();
        }

        private void m_frameParser_ReceivedConfigurationFrame(object sender, EventArgs<IConfigurationFrame> e)
        {
            // Cache received configuration frame
            m_configurationFrame = e.Argument;

            ShowUpdateMessage($"{Tab2}Successfully received configuration frame!");

            m_configurationWaitHandle.Set();
        }

        private void m_frameParser_ConnectionTerminated(object sender, EventArgs e)
        {
            // Communications layer closed connection (close not initiated by system) - so we cancel request
            if (m_frameParser.Enabled)
                ShowUpdateMessage($"{Tab2}ERROR: Connection closed by remote device, request for configuration canceled.");

            m_configurationWaitHandle.Set();
        }

        private void m_frameParser_ConnectionEstablished(object sender, EventArgs e)
        {
            if (m_autoStartParsingSequenceForScan)
            {
                ShowUpdateMessage($"{Tab2}Serial port opened, requesting configuration frame...");

                m_frameParser.SendDeviceCommand(DeviceCommand.DisableRealTimeData);
                Thread.Sleep(Settings.DisableDataDelay);

                const DeviceCommand command = DeviceCommand.SendConfigurationFrame2;
                m_frameParser.SendDeviceCommand(command);
                ShowUpdateMessage($"{Tab2}Sent device command \"{command}\"...");
            }
            else
            {
                ShowUpdateMessage($"{Tab2}Serial port opened, checking for device response...");
                ShowUpdateMessage($"{Tab3}(Configuration frame NOT requested, auto-start parsing sequence for scan is unchecked)");
            }
        }

        private void m_frameParser_ConnectionException(object sender, EventArgs<Exception, int> e)
        {
            ShowUpdateMessage($"{Tab2}ERROR: Connection attempt failed, request for configuration canceled: {e.Argument1.Message}");

            m_configurationWaitHandle.Set();
        }

        private void m_frameParser_ParsingException(object sender, EventArgs<Exception> e)
        {
            ShowUpdateMessage($"{Tab2}ERROR: Parsing exception: {e.Argument.Message}");
        }

        private void m_frameParser_ExceededParsingExceptionThreshold(object sender, EventArgs e)
        {
            ShowUpdateMessage($"{Tab2}Request for configuration canceled due to an excessive number of exceptions...");

            m_configurationWaitHandle.Set();
        }

        private void m_frameParser_ConnectionAttempt(object sender, EventArgs e)
        {
            ShowUpdateMessage($"{Tab2}Attempting {m_frameParser.PhasorProtocol.GetFormattedProtocolName()} {m_frameParser.TransportProtocol.ToString().ToUpper()} based connection...");
        }

        private void m_frameParser_BufferParsed(object sender, EventArgs e)
        {
            if (m_bytesReceivedWaitHandle.IsSet)
                return;

            m_bytesReceivedWaitHandle.Set();
            ShowUpdateMessage($"{Tab2}Received device response, waiting for configuration frame...");
        }
    }
}