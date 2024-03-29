﻿//******************************************************************************************************
//  MainForm.cs - Gbtc
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
//  10/11/2020 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.Diagnostics;
using GSF.IO;
using GSF.Reflection;
using GSF.Threading;
using GSF.Units;
using Microsoft.VisualBasic.FileIO;
using AutoConfigPortScanner.Model;

namespace AutoConfigPortScanner
{
    public partial class MainForm : Form
    {
        private readonly LogPublisher m_log;
        private bool m_formLoaded;
        private volatile bool m_formClosing;
        private CancellationTokenSource m_cancellationTokenSource;
        private readonly StringBuilder m_messages;
        private readonly ShortSynchronizedOperation m_appendOutputMessages;
        private ManualResetEventSlim m_scanExecutionComplete;
        private readonly bool m_debugBuild;
        private string m_lastTimeRemaining = "Calculating...";
        private int m_lastDiscoveredDevices;

        public Settings Settings { get; set; }

        public MainForm()
        {
            InitializeComponent();

            m_log = Program.Log;
            m_messages = new StringBuilder();
            m_appendOutputMessages = new ShortSynchronizedOperation(AppendOutputMessages, 
                ex => m_log.Publish(MessageLevel.Error, "AppendOutput", "Append Output Message Exception", exception: ex));

        #if DEBUG
            m_debugBuild = true;
        #else
            m_debugBuild = false;
        #endif
        }

        #region [ UI Event Handlers ]

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                labelVersion.Text = $"Version {AssemblyInfo.EntryAssembly.Version.Major}.{AssemblyInfo.EntryAssembly.Version.Minor}.{AssemblyInfo.EntryAssembly.Version.Build}";

                StringBuilder message = new();

                message.AppendLine($"System COM Port Range: COM{Settings.MinPortNumber} to COM{Settings.MaxPortNumber}");
                message.AppendLine();
                message.AppendLine("Loaded COM Port Settings:");
                message.AppendLine($"      Baud Rate: {Settings.BaudRate}");
                message.AppendLine($"      Data Bits: {Settings.DataBits}");
                message.AppendLine($"         Parity: {Settings.Parity}");
                message.AppendLine($"      Stop Bits: {Settings.StopBits}");
                message.AppendLine($"    DTR Enabled: {Settings.DtrEnable}");
                message.AppendLine($"    RTS Enabled: {Settings.RtsEnable}");
                message.AppendLine();
                message.AppendLine("Loaded non-UI Port Scan Settings:");
                message.AppendLine($"             COM Timeout: {Time.ToElapsedTimeString(TimeSpan.FromMilliseconds(Settings.ResponseTimeout).TotalSeconds, 3)}");
                message.AppendLine($"          Config Timeout: {Time.ToElapsedTimeString(TimeSpan.FromMilliseconds(Settings.ConfigFrameTimeout).TotalSeconds, 3)}");
                message.AppendLine($"      Disable Data Delay: {Time.ToElapsedTimeString(TimeSpan.FromMilliseconds(Settings.DisableDataDelay).TotalSeconds, 3)}");

                ShowUpdateMessage(message.ToString());

                // Restore UI settings
                textBoxStartComPort.Text = Settings.StartComPort.ToString();
                textBoxEndComPort.Text = Settings.EndComPort.ToString();

                if (!Settings.StartStopComPortsFromCommandLine)
                    textBoxComPorts.Text = string.Join(", ", Settings.ComPorts);

                textBoxStartIDCode.Text = Settings.StartIDCode.ToString();
                textBoxEndIDCode.Text = Settings.EndIDCode.ToString();

                if (!Settings.StartStopIDCodesFromCommandLine)
                    textBoxIDCodes.Text = string.Join(", ", Settings.IDCodes);

                checkBoxRescan.Checked = Settings.Rescan;
                checkBoxAutoStartParsingSequence.Checked = Settings.AutoStartParsingSequenceForScan;
                textBoxSourceConfig.Text = Settings.SourceConfig;
                radioButtonControlling.Checked = Settings.ControllingConnection;
                radioButtonListening.Checked = !Settings.ControllingConnection;

                m_formLoaded = true;
                
                ShowAssignedConnectionType();

                if (Settings.AutoScan)
                    buttonScan_Click(sender, e);
            }
            catch (Exception ex)
            {
                m_log.Publish(MessageLevel.Error, "FormLoad", "Failed while loading settings", exception: ex);

            #if DEBUG
                throw;
            #endif
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_formClosing = true;

                // Save UI settings
                if (ushort.TryParse(textBoxStartComPort.Text, out ushort startComPort))
                    Settings.StartComPort = startComPort;

                if (ushort.TryParse(textBoxEndComPort.Text, out ushort endComPort))
                    Settings.EndComPort = endComPort;

                Settings.ComPorts = Settings.ParseUniqueUInt16Values(textBoxComPorts.Text);

                if (ushort.TryParse(textBoxStartIDCode.Text, out ushort startIDCode))
                    Settings.StartIDCode = startIDCode;

                if (ushort.TryParse(textBoxEndIDCode.Text, out ushort endIDCode))
                    Settings.EndIDCode = endIDCode;

                if (!Settings.AutoRemoveIDs)
                    Settings.IDCodes = Settings.ParseUniqueUInt16Values(textBoxIDCodes.Text);

                Settings.Rescan = checkBoxRescan.Checked;
                Settings.AutoStartParsingSequenceForScan = checkBoxAutoStartParsingSequence.Checked;
                Settings.SourceConfig = textBoxSourceConfig.Text;
                Settings.ControllingConnection = radioButtonControlling.Checked;
            }
            catch (Exception ex)
            {
                m_log.Publish(MessageLevel.Error, "FormClosing", "Failed while saving settings", exception: ex);

            #if DEBUG
                throw;
            #endif
            }
        }

        private void textBoxStartComPort_Validating(object sender, CancelEventArgs e)
        {
            if (!ushort.TryParse(textBoxStartComPort.Text.Trim(), out ushort startCOMPort))
                SetError(sender, e, "Invalid start COM port");

            if (startCOMPort < Settings.MinPortNumber || startCOMPort > Settings.MaxPortNumber && !m_debugBuild)
                SetError(sender, e, $"Start COM port is out of range: COM{Settings.MinPortNumber} to COM{Settings.MaxPortNumber}");

            if (ushort.TryParse(textBoxEndComPort.Text.Trim(), out ushort endCOMPort) && startCOMPort > endCOMPort)
                SetError(sender, e, "Start COM port must be less then end COM port");
        }

        private void textBoxStartComPort_Validated(object sender, EventArgs e)
        {
            Settings.StartComPort = ushort.Parse(textBoxStartComPort.Text.Trim());
            ClearError(sender);
        }

        private void textBoxEndComPort_Validating(object sender, CancelEventArgs e)
        {
            if (!ushort.TryParse(textBoxEndComPort.Text.Trim(), out ushort endCOMPort))
                SetError(sender, e, "Invalid end COM port");

            if (endCOMPort < Settings.MinPortNumber || endCOMPort > Settings.MaxPortNumber && !m_debugBuild)
                SetError(sender, e, $"End COM port is out of range: COM{Settings.MinPortNumber} to COM{Settings.MaxPortNumber}");

            if (ushort.TryParse(textBoxStartComPort.Text.Trim(), out ushort startCOMPort) && endCOMPort < startCOMPort)
                SetError(sender, e, "End COM port must be greater then start COM port");
        }

        private void textBoxEndComPort_Validated(object sender, EventArgs e)
        {
            Settings.EndComPort = ushort.Parse(textBoxEndComPort.Text.Trim());
            ClearError(sender);
        }

        private void textBoxComPorts_TextChanged(object sender, EventArgs e)
        {
            textBoxStartComPort.Enabled = textBoxEndComPort.Enabled = string.IsNullOrWhiteSpace(textBoxComPorts.Text);
        }

        private void textBoxComPorts_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxComPorts.Text) && Settings.ParseUniqueUInt16Values(textBoxComPorts.Text).Length == 0)
                SetError(sender, e, "Invalid COM port list");
        }

        private void textBoxComPorts_Validated(object sender, EventArgs e)
        {
            Settings.ComPorts = Settings.ParseUniqueUInt16Values(textBoxComPorts.Text);
            textBoxComPorts.Text = string.Join(", ", Settings.ComPorts);
            ClearError(sender);
        }

        private void buttonBrowseComPorts_Click(object sender, EventArgs e)
        {
            if (openFileDialogSelectComPorts.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                ushort[] comPorts = Settings.ParseUniqueUInt16Values(File.ReadAllText(openFileDialogSelectComPorts.FileName));
                textBoxComPorts.Text = string.Join(", ", comPorts);
            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"ERROR: Failed while attempting to load COM ports from  \"{Path.GetFileName(openFileDialogSelectComPorts.FileName)}\": {ex.Message}");
                m_log.Publish(MessageLevel.Error, nameof(AutoConfigPortScanner), exception: ex);
            }
        }

        private void textBoxStartIDCode_Validating(object sender, CancelEventArgs e)
        {
            if (!ushort.TryParse(textBoxStartIDCode.Text.Trim(), out ushort startIDCode))
                SetError(sender, e, "Invalid start ID code");

            if (ushort.TryParse(textBoxEndIDCode.Text.Trim(), out ushort endIDCode) && startIDCode > endIDCode)
                SetError(sender, e, "Start ID code must be less then end ID code");
        }

        private void textBoxStartIDCode_Validated(object sender, EventArgs e)
        {
            Settings.StartIDCode = ushort.Parse(textBoxStartIDCode.Text.Trim());
            ClearError(sender);
        }

        private void textBoxEndIDCode_Validating(object sender, CancelEventArgs e)
        {
            if (!ushort.TryParse(textBoxEndIDCode.Text.Trim(), out ushort endIDCode))
                SetError(sender, e, "Invalid end ID code");

            if (ushort.TryParse(textBoxStartIDCode.Text.Trim(), out ushort startIDCode) && endIDCode < startIDCode)
                SetError(sender, e, "End ID code must be greater then start ID code");
        }

        private void textBoxEndIDCode_Validated(object sender, EventArgs e)
        {
            Settings.EndIDCode = ushort.Parse(textBoxEndIDCode.Text.Trim());
            ClearError(sender);
        }

        private void textBoxIDCodeValues_TextChanged(object sender, EventArgs e)
        {
            textBoxStartIDCode.Enabled = textBoxEndIDCode.Enabled = string.IsNullOrWhiteSpace(textBoxIDCodes.Text);
        }

        private void textBoxIDCodeValues_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxIDCodes.Text) && Settings.ParseUniqueUInt16Values(textBoxIDCodes.Text).Length == 0)
                SetError(sender, e, "Invalid ID code list");
        }

        private void textBoxIDCodeValues_Validated(object sender, EventArgs e)
        {
            Settings.IDCodes = Settings.ParseUniqueUInt16Values(textBoxIDCodes.Text);
            textBoxIDCodes.Text = string.Join(", ", Settings.IDCodes);
            ClearError(sender);
        }

        private void buttonBrowseIDCodes_Click(object sender, EventArgs e)
        {
            if (openFileDialogSelectIDCodes.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                ushort[] idCodes = Settings.ParseUniqueUInt16Values(File.ReadAllText(openFileDialogSelectIDCodes.FileName));
                textBoxIDCodes.Text = string.Join(", ", idCodes);
            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"ERROR: Failed while attempting to load ID codes from  \"{Path.GetFileName(openFileDialogSelectIDCodes.FileName)}\": {ex.Message}");
                m_log.Publish(MessageLevel.Error, nameof(AutoConfigPortScanner), exception: ex);
            }
        }

        private void buttonBrowseConfig_Click(object sender, EventArgs e)
        {
            if (openFileDialogSelectConfig.ShowDialog(this) == DialogResult.OK)
                textBoxSourceConfig.Text = openFileDialogSelectConfig.FileName;
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            string configFile = FilePath.GetAbsolutePath(textBoxSourceConfig.Text);

            m_scanExecutionComplete ??= new ManualResetEventSlim(false);
            m_scanExecutionComplete.Reset();

            (Guid nodeID, string connectionString, string dataProviderString, bool error) = LoadConnectionSettings(configFile);

            if (error)
                return;

            try
            {
                AdoDataConnection connection = new(connectionString, dataProviderString);
                StringBuilder message = new();

                message.AppendLine();
                message.AppendLine($"Opened database configured in \"{Path.GetFileName(configFile)}\":");
                message.AppendLine($"        Node ID: {nodeID}");
                
                ShowUpdateMessage(message.ToString());

                m_cancellationTokenSource = new CancellationTokenSource();

                ushort startComPort = ushort.Parse(textBoxStartComPort.Text);
                ushort endComPort = ushort.Parse(textBoxEndComPort.Text);

                ushort[] comPorts = string.IsNullOrWhiteSpace(textBoxComPorts.Text) ?
                    Enumerable.Range(startComPort, endComPort - startComPort + 1).Select(value => (ushort)value).ToArray() :
                    Settings.ParseUniqueUInt16Values(textBoxComPorts.Text);

                ushort startIDCode = ushort.Parse(textBoxStartIDCode.Text);
                ushort endIDCode = ushort.Parse(textBoxEndIDCode.Text);

                ushort[] idCodes = string.IsNullOrWhiteSpace(textBoxIDCodes.Text) ?
                    Enumerable.Range(startIDCode, endIDCode - startIDCode + 1).Select(value => (ushort)value).ToArray() :
                    Settings.ParseUniqueUInt16Values(textBoxIDCodes.Text);

                ExecuteScan(new ScanParameters
                {
                    Connection = connection,
                    DeviceTable = new TableOperations<Device>(connection),
                    NodeID = nodeID,
                    ComPorts = comPorts,
                    IDCodes = idCodes,
                    Rescan = checkBoxRescan.Checked,
                    AutoStartParsingSequenceForScan = checkBoxAutoStartParsingSequence.Checked,
                    ControllingConnection = Settings.ControllingConnection,
                    ResponseTimeout = Settings.ResponseTimeout,
                    ConfigFrameTimeout = Settings.ConfigFrameTimeout,
                    SourceConfig = configFile
                },
                m_cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"ERROR: Failed while attempting to open database configured in \"{Path.GetFileName(configFile)}\": {ex.Message}");
                m_log.Publish(MessageLevel.Error, nameof(AutoConfigPortScanner), exception: ex);
                m_scanExecutionComplete.Set();
            }
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", GetSettingsFileName());
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (openFileDialogSelectCSVImport.ShowDialog(this) != DialogResult.OK)
                return;

            string csvFile = FilePath.GetAbsolutePath(openFileDialogSelectCSVImport.FileName);

            UpdateProgressBar(0);
            ShowUpdateMessage("");

            if (!File.Exists(csvFile))
            {
                ShowUpdateMessage($"CSV import file \"{csvFile}\" was not found. Import canceled.");
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    SetControlEnabledState(buttonImport, false);
                    m_cancellationTokenSource = new CancellationTokenSource();

                    Dictionary<int, int> comPortIDCodeMap = new();

                    try
                    {
                        using TextFieldParser parser = new(csvFile);

                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");

                        while (!parser.EndOfData)
                        {
                            string[] fields = parser.ReadFields();

                            if (!(fields?.Length > 1))
                                continue;

                            if (fields[0].StartsWith("COM", StringComparison.OrdinalIgnoreCase) && fields.Length > 3)
                                fields[0] = fields[0].Substring(3);

                            if (ushort.TryParse(fields[0], out ushort comPort) && ushort.TryParse(fields[1], out ushort idCode))
                                comPortIDCodeMap[comPort] = idCode;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowUpdateMessage($"ERROR: Failed while attempting to parse CSV file \"{Path.GetFileName(csvFile)}\": {ex.Message}");
                        m_log.Publish(MessageLevel.Error, nameof(AutoConfigPortScanner), exception: ex);
                        return;
                    }

                    if (comPortIDCodeMap.Count == 0)
                    {
                        ShowUpdateMessage($"No mappings found in CSV import file \"{csvFile}\".{Environment.NewLine}    Verify format: column 1 should be COM port and column 2 should be ID code.");
                        return;
                    }

                    ShowUpdateMessage($"Loaded {comPortIDCodeMap.Count:N0} mappings from CSV import file \"{csvFile}\", starting scan and import...");

                    Ticks startTime = DateTime.UtcNow.Ticks;
                    int mappings = 0;

                    // Save original settings
                    string orgStartComPort = textBoxStartComPort.Text;
                    string orgEndComPort = textBoxEndComPort.Text;
                    string orgStartIDCode = textBoxStartIDCode.Text;
                    string orgEndIDCode = textBoxEndIDCode.Text;

                    SetProgressBarMinMax(0, comPortIDCodeMap.Count);

                    try
                    {
                        // Scan each row in import file
                        foreach (KeyValuePair<int, int> kvp in comPortIDCodeMap)
                        {
                            int comPort = kvp.Key;
                            int idCode = kvp.Value;

                            SetTextBoxText(textBoxStartComPort, comPort.ToString());
                            SetTextBoxText(textBoxEndComPort, comPort.ToString());
                            SetTextBoxText(textBoxStartIDCode, idCode.ToString());
                            SetTextBoxText(textBoxEndIDCode, idCode.ToString());

                            buttonScan_Click(buttonScan, e);
                            m_scanExecutionComplete.Wait();

                            if (m_cancellationTokenSource?.IsCancellationRequested ?? false)
                                break;

                            mappings++;
                            UpdateProgressBar(mappings);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowUpdateMessage($"ERROR: Failed while attempting to scan COM port to ID code mappings in CSV file \"{Path.GetFileName(csvFile)}\": {ex.Message}");
                        m_log.Publish(MessageLevel.Error, nameof(AutoConfigPortScanner), exception: ex);
                    }
                    finally
                    {
                        // Restore original settings
                        SetTextBoxText(textBoxStartComPort, orgStartComPort);
                        SetTextBoxText(textBoxEndComPort, orgEndComPort);
                        SetTextBoxText(textBoxStartIDCode, orgStartIDCode);
                        SetTextBoxText(textBoxEndIDCode, orgEndIDCode);

                        if (m_cancellationTokenSource?.IsCancellationRequested ?? false)
                            ShowUpdateMessage($"{Environment.NewLine}Import canceled after running for {(DateTime.UtcNow.Ticks - startTime).ToElapsedTimeString(3)}. Completed {mappings:N0} mappings from CSV file \"{Path.GetFileName(csvFile)}\" before cancel.");
                        else
                            ShowUpdateMessage($"{Environment.NewLine}Import for {mappings:N0} mappings in CSV file \"{Path.GetFileName(csvFile)}\" completed in {(DateTime.UtcNow.Ticks - startTime).ToElapsedTimeString(3)}");
                    }
                }
                finally
                {
                    SetControlEnabledState(buttonImport, true);
                }
            });
        }

        private void buttonChangeMode_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, $"Are you sure want to change all serial devices' configured modes to \"{(Settings.ControllingConnection ? "controlling" : "listening")}\" connections?", "Change Configured Mode", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string configFile = FilePath.GetAbsolutePath(textBoxSourceConfig.Text);
            (_, string connectionString, string dataProviderString, bool error) = LoadConnectionSettings(configFile);

            if (error)
                return;

            AdoDataConnection connection;

            try
            {
                connection = new AdoDataConnection(connectionString, dataProviderString);
                StringBuilder message = new();

                message.AppendLine();
                message.AppendLine($"Opened database configured in \"{Path.GetFileName(configFile)}\".");
                message.AppendLine();

            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"ERROR: Failed while attempting to open database configured in \"{Path.GetFileName(configFile)}\": {ex.Message}");
                m_log.Publish(MessageLevel.Error, nameof(AutoConfigPortScanner), exception: ex);
                return;
            }

            try
            {
                ShowUpdateMessage($"Changing all serial devices' configured modes to \"{(Settings.ControllingConnection ? "controlling" : "listening")}\" connections...");

                UpdateAllDeviceConfigurationTypes(connection, Settings.ControllingConnection);
                
                ShowUpdateMessage($"{Environment.NewLine}Change complete. Mode is \"{(Settings.ControllingConnection ? "controlling" : "listening")}\".{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"ERROR: Failed while change configured device mode: {ex.Message}");
                m_log.Publish(MessageLevel.Error, nameof(AutoConfigPortScanner), exception: ex);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            m_cancellationTokenSource?.Cancel();
        }

        private void linkLabelClearConsole_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClearUpdateMessages();
        }

        private void radioButtonControlling_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings.ControllingConnection == radioButtonControlling.Checked)
                return;

            Settings.ControllingConnection = radioButtonControlling.Checked;
            ShowAssignedConnectionType();
        }

        private void radioButtonListening_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings.ControllingConnection == !radioButtonListening.Checked)
                return;

            Settings.ControllingConnection = !radioButtonListening.Checked;
            ShowAssignedConnectionType();
        }

        #endregion

        #region [ UI Helper Functions ]

        private static string GetSettingsFileName()
        {
            Environment.SpecialFolder specialFolder = Environment.SpecialFolder.CommonApplicationData;
            string appDataPath = Environment.GetFolderPath(specialFolder);
            return Path.Combine(appDataPath, nameof(AutoConfigPortScanner), "settings.ini");
        }

        private void SetTextBoxText(TextBox textBox, string text)
        {
            if (m_formClosing || textBox is null)
                return;

            if (InvokeRequired)
                BeginInvoke(new Action<TextBox, string>(SetTextBoxText), textBox, text);
            else
                textBox.Text = text;
        }

        private void SetError(object sender, CancelEventArgs e, string message)
        {
            if (m_formClosing || sender is not Control control)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, CancelEventArgs, string>(SetError), sender, e, message);
            }
            else
            {
                e.Cancel = true;

                if (control.Enabled)
                {
                    errorProvider.SetError(control, message);
                    SelectAll(sender, EventArgs.Empty);
                    SetControlEnabledState(buttonScan, false);
                }
                else
                {
                    ClearError(sender);
                }
            }
        }

        private void ClearError(object sender)
        {
            if (m_formClosing || sender is not Control control)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<object>(ClearError), sender);
            }
            else
            {
                errorProvider.SetError(control, "");
                SetControlEnabledState(buttonScan, true);
            }
        }

        private void SelectAll(object sender, EventArgs eventArgs)
        {
            if (!m_formLoaded || sender is not TextBox textBox)
                return;

            textBox.SelectAll();
            textBox.Focus();
        }

        private void ShowUpdateMessage(string message)
        {
            if (m_formClosing)
                return;

            lock (m_appendOutputMessages)
                m_messages.AppendLine(message);

            m_log.Publish(MessageLevel.Info, "StatusMessage", message);
            m_appendOutputMessages.RunOnceAsync();
        }

        private void AppendOutputMessages()
        {
            string messages;

            lock (m_appendOutputMessages)
            {
                messages = m_messages.ToString();
                m_messages.Clear();
            }

            BeginInvoke(new Action(() => textBoxMessageOutput.AppendText($"{messages}")));
        }

        private void ClearUpdateMessages()
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(ClearUpdateMessages));
            }
            else
            {
                lock (textBoxMessageOutput)
                    textBoxMessageOutput.Text = "";
            }
        }

        private void SetControlEnabledState(Control control, bool state)
        {
            if (m_formClosing || control is null)
                return;

            if (InvokeRequired)
                BeginInvoke(new Action<Control, bool>(SetControlEnabledState), control, state);
            else
                control.Enabled = state;
        }

        private void UpdateFeedback(string timeRemaining = null, int? discoveredDevices = null)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, int?>(UpdateFeedback), timeRemaining, discoveredDevices);
            }
            else
            {
                labelFeedback.Text = string.Format(labelFeedback.Tag.ToString(), timeRemaining ?? m_lastTimeRemaining, discoveredDevices ?? m_lastDiscoveredDevices);

                if (timeRemaining is not null)
                    m_lastTimeRemaining = timeRemaining;

                if (discoveredDevices is not null)
                    m_lastDiscoveredDevices = discoveredDevices.Value;
            }
        }

        private void ClearFeedback()
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(ClearFeedback));
            }
            else
            {
                labelFeedback.Text = string.Empty;
                m_lastTimeRemaining = "Calculating...";
                m_lastDiscoveredDevices = 0;
            }
        }

        private void UpdateProgressBar(int value)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(UpdateProgressBar), value);
            }
            else
            {
                if (value < progressBar.Minimum)
                    value = progressBar.Minimum;

                if (value > progressBar.Maximum)
                    progressBar.Maximum = value;

                progressBar.Value = value;
            }
        }

        private void SetProgressBarMinMax(int minimum, int maximum)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<int, int>(SetProgressBarMinMax), minimum, maximum);
            }
            else
            {
                progressBar.Minimum = minimum < 0 ? 0 : minimum;
                progressBar.Maximum = maximum;
            }
        }

        private (Guid nodeID, string connectionString, string dataProviderString, bool error) LoadConnectionSettings(string configFile)
        {
            Guid nodeID = default;
            string connectionString = default;
            string dataProviderString = default;
            bool error = false;

            try
            {
                // Fail if source config file does not exist
                if (!File.Exists(configFile))
                    throw new FileNotFoundException($"Source config file \"{configFile}\" was not found.");

                // Load needed database settings from target config file
                XDocument serviceConfig = XDocument.Load(configFile);

                nodeID = Guid.Parse(serviceConfig
                    .Descendants("systemSettings")
                    .SelectMany(systemSettings => systemSettings.Elements("add"))
                    .Where(element => "NodeID".Equals((string)element.Attribute("name"), StringComparison.OrdinalIgnoreCase))
                    .Select(element => (string)element.Attribute("value"))
                    .FirstOrDefault() ?? Guid.Empty.ToString());

                if (nodeID == Guid.Empty)
                    throw new InvalidOperationException("Failed to find \"NodeID\" setting");

                connectionString = serviceConfig
                    .Descendants("systemSettings")
                    .SelectMany(systemSettings => systemSettings.Elements("add"))
                    .Where(element => "ConnectionString".Equals((string)element.Attribute("name"), StringComparison.OrdinalIgnoreCase))
                    .Select(element => (string)element.Attribute("value"))
                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Failed to find \"ConnectionString\" setting");

                dataProviderString = serviceConfig
                    .Descendants("systemSettings")
                    .SelectMany(systemSettings => systemSettings.Elements("add"))
                    .Where(element => "DataProviderString".Equals((string)element.Attribute("name"), StringComparison.OrdinalIgnoreCase))
                    .Select(element => (string)element.Attribute("value"))
                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(dataProviderString))
                    throw new InvalidOperationException("Failed to find \"DataProviderString\" setting");
            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"ERROR: Failed while attempting to parse settings from \"{Path.GetFileName(configFile)}\": {ex.Message}");
                m_log.Publish(MessageLevel.Error, nameof(AutoConfigPortScanner), exception: ex);
                error = true;
            }

            return (nodeID, connectionString, dataProviderString, error);
        }

        private void ShowAssignedConnectionType()
        {
            if (m_formLoaded)
                ShowUpdateMessage($"{Environment.NewLine}Assigned connection type: {(Settings.ControllingConnection ? "Active" : "Passive")} -> set to add \"{(Settings.ControllingConnection ? "CONFIG FRAME CONTROLLED" : "LISTEN ONLY")}\" connections for device configurations.{Environment.NewLine}");
        }

        #endregion
    }
}
