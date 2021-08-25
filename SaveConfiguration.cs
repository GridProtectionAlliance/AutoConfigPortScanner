//******************************************************************************************************
//  SaveConfiguration.cs - Gbtc
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
using System.Text.RegularExpressions;
using AutoConfigPortScanner.Model;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.Diagnostics;
using GSF.PhasorProtocols;
using GSF.Units.EE;
using Phasor = AutoConfigPortScanner.Model.Phasor;
using SignalType = AutoConfigPortScanner.Model.SignalType;

namespace AutoConfigPortScanner
{
    public static class TableOperationExtensions
    {
        public static Device FindDeviceByComPort(this Device[] devices, ushort comPort)
        {
            string portName = $"COM{comPort}";

            foreach (Device device in devices)
            {
                Dictionary<string, string> settings = device.ConnectionString?.ParseKeyValuePairs();

                if (settings is null)
                    continue;

                if (settings.TryGetValue("port", out string configuredPortName) && configuredPortName.Equals(portName, StringComparison.OrdinalIgnoreCase))
                    return device;
            }

            return null;
        }

        public static void AddNewDevice(this TableOperations<Device> deviceTable, Device device) => 
            deviceTable.AddNewRecord(device);

        public static Device NewDevice(this TableOperations<Device> deviceTable) => 
            deviceTable.NewRecord();

        public static Device QueryDevice(this TableOperations<Device> deviceTable, string acronym) => 
            deviceTable.QueryRecordWhere("Acronym = {0}", acronym) ?? deviceTable.NewDevice();

        public static void UpdateDevice(this TableOperations<Device> deviceTable, Device device) => 
            deviceTable.UpdateRecord(device);

        public static IEnumerable<SignalType> LoadSignalTypes(this TableOperations<SignalType> signalTypeTable, string source) => 
            signalTypeTable.QueryRecordsWhere("Source = {0}", source);

        public static Measurement NewMeasurement(this TableOperations<Measurement> measurementTable) => 
            measurementTable.NewRecord();

        public static Measurement QueryMeasurement(this TableOperations<Measurement> measurementTable, string signalReference) => 
            measurementTable.QueryRecordWhere("SignalReference = {0}", signalReference) ?? measurementTable.NewMeasurement();

        public static void AddNewOrUpdateMeasurement(this TableOperations<Measurement> measurementTable, Measurement measurement) => 
            measurementTable.AddNewOrUpdateRecord(measurement);

        public static IEnumerable<Phasor> QueryPhasorsForDevice(this TableOperations<Phasor> phasorTable, int deviceID) => 
            phasorTable.QueryRecordsWhere("DeviceID = {0}", deviceID).OrderBy(phasor => phasor.SourceIndex);

        public static int DeletePhasorsForDevice(this AdoDataConnection connection, int deviceID) => 
            connection.ExecuteScalar<int>("DELETE FROM Phasor WHERE DeviceID = {0}", deviceID);

        public static Phasor NewPhasor(this TableOperations<Phasor> phasorTable) => 
            phasorTable.NewRecord();

        public static void AddNewPhasor(this TableOperations<Phasor> phasorTable, Phasor phasor) =>
            phasorTable.AddNewRecord(phasor);

        public static Phasor QueryPhasorForDevice(this TableOperations<Phasor> phasorTable, int deviceID, int sourceIndex) => 
            phasorTable.QueryRecordWhere("DeviceID = {0} AND SourceIndex = {1}", deviceID, sourceIndex) ?? phasorTable.NewPhasor();
    }

    partial class MainForm
    {
        // Connection string parameters of system that is controlling COM connection
        private const string ControllingConnectionString = "autoStartDataParsingSequence = true; skipDisableRealTimeData = false; disableRealTimeDataOnStop = false";

        // Connection string parameters of system that is only listening to COM connection
        private const string ListeningConnectionString = "autoStartDataParsingSequence = false; skipDisableRealTimeData = true; disableRealTimeDataOnStop = false";

        private static readonly string[] s_commonVoltageLevels = { "44", "69", "115", "138", "161", "169", "230", "345", "500", "765", "1100" };

        private Dictionary<string, SignalType> m_deviceSignalTypes;
        private Dictionary<string, SignalType> m_phasorSignalTypes;

        // Remove any invalid characters from acronym
        private static string GetCleanAcronym(string acronym) => 
            Regex.Replace(acronym, @"[^A-Z0-9\-!_\.@#\$]", "", RegexOptions.IgnoreCase);

        private bool SaveDeviceConfiguration(IConfigurationFrame configFrame, ushort comPort, ushort idCode, ScanParameters scanParams)
        {
            try
            {
                AdoDataConnection connection = scanParams.Connection;
                bool autoStartParsingSequenceForConfig = scanParams.AutoStartParsingSequenceForConfig;
                TableOperations<SignalType> signalTypeTable = new(connection);
                string configConnectionMode = autoStartParsingSequenceForConfig ? ControllingConnectionString : ListeningConnectionString;
                string connectionString = string.Format(ConnectionStringTemplate, comPort, Settings.BaudRate, Settings.Parity, Settings.StopBits, Settings.DataBits, Settings.DtrEnable, Settings.RtsEnable, configConnectionMode);

                ShowUpdateMessage($"{Tab2}Saving \"{configFrame.Cells[0].StationName}\" configuration received on COM{comPort} with ID code {idCode}...");

                m_deviceSignalTypes ??= signalTypeTable.LoadSignalTypes("PMU").ToDictionary(key => key.Acronym, StringComparer.OrdinalIgnoreCase);
                m_phasorSignalTypes ??= signalTypeTable.LoadSignalTypes("Phasor").ToDictionary(key => key.Acronym, StringComparer.OrdinalIgnoreCase);

                SaveDeviceConnection(configFrame, connectionString, comPort, idCode, scanParams);

                return true;
            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"{Tab2}ERROR: Failed while saving \"{configFrame.Cells[0].StationName}\" configuration: {ex.Message}");
                m_log.Publish(MessageLevel.Error, nameof(AutoConfigPortScanner), exception: ex);
                
                return false;
            }
        }

        private void SaveDeviceConnection(IConfigurationFrame configFrame, string connectionString, ushort comPort, ushort idCode, ScanParameters scanParams)
        {
            TableOperations<Device> deviceTable = scanParams.DeviceTable;
            Guid nodeID = scanParams.NodeID;

            ShowUpdateMessage($"{Tab2}Saving device connection...");

            // Query existing device record, creating new one if not found
            Device device = scanParams.Devices.FindDeviceByComPort(comPort) ?? deviceTable.NewDevice();
            Dictionary<string, string> connectionStringMap = connectionString.ParseKeyValuePairs();

            bool autoStartDataParsingSequence = true;
            bool skipDisableRealTimeData = false;

            // Handle connection string parameters that are fields in the device table
            if (connectionStringMap.ContainsKey("autoStartDataParsingSequence"))
            {
                autoStartDataParsingSequence = bool.Parse(connectionStringMap["autoStartDataParsingSequence"]);
                connectionStringMap.Remove("autoStartDataParsingSequence");
                connectionString = connectionStringMap.JoinKeyValuePairs();
            }

            if (connectionStringMap.ContainsKey("skipDisableRealTimeData"))
            {
                skipDisableRealTimeData = bool.Parse(connectionStringMap["skipDisableRealTimeData"]);
                connectionStringMap.Remove("skipDisableRealTimeData");
                connectionString = connectionStringMap.JoinKeyValuePairs();
            }

            IConfigurationCell deviceConfig = configFrame.Cells[0];

            string deviceAcronym = deviceConfig.IDLabel;
            string deviceName = null;

            if (string.IsNullOrWhiteSpace(deviceAcronym) && !string.IsNullOrWhiteSpace(deviceConfig.StationName))
                deviceAcronym = GetCleanAcronym(deviceConfig.StationName.ToUpperInvariant().Replace(" ", "_"));
            else
                throw new InvalidOperationException("Unable to get station name or ID label from device configuration frame");

            if (!string.IsNullOrWhiteSpace(deviceConfig.StationName))
                deviceName = deviceConfig.StationName;

            device.NodeID = nodeID;
            device.Acronym = deviceAcronym;
            device.Name = deviceName ?? deviceAcronym;
            device.ProtocolID = scanParams.IeeeC37_118ProtocolID;
            device.FramesPerSecond = configFrame.FrameRate;
            device.AccessID = idCode;
            device.IsConcentrator = false;
            device.ConnectionString = connectionString;
            device.AutoStartDataParsingSequence = autoStartDataParsingSequence;
            device.SkipDisableRealTimeData = skipDisableRealTimeData;
            device.Enabled = true;

            // Check if this is a new device or an edit to an existing one
            if (device.ID == 0)
            {
                // Add new device record
                deviceTable.AddNewDevice(device);

                // Get newly added device with auto-incremented ID
                Device newDevice = deviceTable.QueryDevice(device.Acronym);

                // Save associated device records
                SaveDeviceRecords(configFrame, newDevice, scanParams);
            }
            else
            {
                // Update existing device record
                deviceTable.UpdateDevice(device);
                
                // Save associated device records
                SaveDeviceRecords(configFrame, device, scanParams);
            }
        }

        private void SaveDeviceRecords(IConfigurationFrame configFrame, Device device, ScanParameters scanParams)
        {
            AdoDataConnection connection = scanParams.Connection;
            TableOperations<Measurement> measurementTable = new(connection);
            IConfigurationCell cell = configFrame.Cells[0];

            // Add frequency
            SaveFixedMeasurement(m_deviceSignalTypes["FREQ"], device, measurementTable, scanParams, cell.FrequencyDefinition.Label);

            // Add dF/dt
            SaveFixedMeasurement(m_deviceSignalTypes["DFDT"], device, measurementTable, scanParams);

            // Add status flags
            SaveFixedMeasurement(m_deviceSignalTypes["FLAG"], device, measurementTable, scanParams);

            // Add analogs
            SignalType analogSignalType = m_deviceSignalTypes["ALOG"];

            for (int i = 0; i < cell.AnalogDefinitions.Count; i++)
            {
                int index = i + 1;
                IAnalogDefinition analogDefinition = cell.AnalogDefinitions[i];
                string signalReference = $"{device.Acronym}-{analogSignalType.Suffix}{index}";

                // Query existing measurement record for specified signal reference - function will create a new blank measurement record if one does not exist
                Measurement measurement = measurementTable.QueryMeasurement(signalReference);
                string pointTag = scanParams.CreateIndexedPointTag(device.Acronym, analogSignalType.Acronym, index);
                measurement.DeviceID = device.ID;
                measurement.PointTag = pointTag;
                measurement.AlternateTag = analogDefinition.Label;
                measurement.Description = $"{device.Acronym} Analog Value {index} {analogDefinition.AnalogType}: {analogDefinition.Label}";
                measurement.SignalReference = signalReference;
                measurement.SignalTypeID = analogSignalType.ID;
                measurement.Internal = true;
                measurement.Enabled = true;

                measurementTable.AddNewOrUpdateMeasurement(measurement);
            }

            // Add digitals
            SignalType digitalSignalType = m_deviceSignalTypes["DIGI"];

            for (int i = 0; i < cell.DigitalDefinitions.Count; i++)
            {
                int index = i + 1;
                IDigitalDefinition digitialDefinition = cell.DigitalDefinitions[i];
                string signalReference = $"{device.Acronym}-{digitalSignalType.Suffix}{index}";

                // Query existing measurement record for specified signal reference - function will create a new blank measurement record if one does not exist
                Measurement measurement = measurementTable.QueryMeasurement(signalReference);
                string pointTag = scanParams.CreateIndexedPointTag(device.Acronym, digitalSignalType.Acronym, index);
                measurement.DeviceID = device.ID;
                measurement.PointTag = pointTag;
                measurement.AlternateTag = digitialDefinition.Label;
                measurement.Description = $"{device.Acronym} Digital Value {index}: {digitialDefinition.Label}";
                measurement.SignalReference = signalReference;
                measurement.SignalTypeID = digitalSignalType.ID;
                measurement.Internal = true;
                measurement.Enabled = true;

                measurementTable.AddNewOrUpdateMeasurement(measurement);
            }

            // Add phasors
            SaveDevicePhasors(cell, device, measurementTable, scanParams);
        }

        private void SaveFixedMeasurement(SignalType signalType, Device device, TableOperations<Measurement> measurementTable, ScanParameters scanParams, string label = null)
        {
            string signalReference = $"{device.Acronym}-{signalType.Suffix}";

            // Query existing measurement record for specified signal reference - function will create a new blank measurement record if one does not exist
            Measurement measurement = measurementTable.QueryMeasurement(signalReference);
            string pointTag = scanParams.CreatePointTag(device.Acronym, signalType.Acronym);
            measurement.DeviceID = device.ID;
            measurement.PointTag = pointTag;
            measurement.Description = $"{device.Acronym} {signalType.Name}{(string.IsNullOrWhiteSpace(label) ? "" : " - " + label)}";
            measurement.SignalReference = signalReference;
            measurement.SignalTypeID = signalType.ID;
            measurement.Internal = true;
            measurement.Enabled = true;

            measurementTable.AddNewOrUpdateMeasurement(measurement);
        }

        private void SaveDevicePhasors(IConfigurationCell cell, Device device, TableOperations<Measurement> measurementTable, ScanParameters scanParams)
        {
            bool phaseMatchExact(string phaseLabel, string[] phaseMatches) =>
                phaseMatches.Any(match => phaseLabel.Equals(match, StringComparison.Ordinal));

            bool phaseEndsWith(string phaseLabel, string[] phaseMatches, bool ignoreCase) =>
                phaseMatches.Any(match => phaseLabel.EndsWith(match, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));

            bool phaseStartsWith(string phaseLabel, string[] phaseMatches, bool ignoreCase) =>
                phaseMatches.Any(match => phaseLabel.StartsWith(match, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));

            bool phaseContains(string phaseLabel, string[] phaseMatches, bool ignoreCase) =>
                phaseMatches.Any(match => phaseLabel.IndexOf(match, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) > -1);

            bool phaseMatchHighConfidence(string phaseLabel, string[] containsMatches, string[] endsWithMatches)
            {
                if (phaseEndsWith(phaseLabel, containsMatches, true))
                    return true;

                if (phaseStartsWith(phaseLabel, containsMatches, true))
                    return true;

                foreach (string match in containsMatches.Concat(endsWithMatches))
                {
                    string[] variations = { $" {match}", $"_{match}", $"-{match}", $".{match}" };

                    if (phaseEndsWith(phaseLabel, variations, false))
                        return true;
                }

                foreach (string match in containsMatches)
                {
                    string[] variations = { $" {match} ", $"_{match}_", $"-{match}-", $"-{match}_", $"_{match}-", $".{match}." };

                    if (phaseContains(phaseLabel, variations, false))
                        return true;
                }

                return false;
            }

            char guessPhase(char phase, string phasorLabel)
            {
                if (phaseMatchExact(phasorLabel, new[] { "V1PM", "I1PM" }) || phaseMatchHighConfidence(phasorLabel, new[] { "V1", "VP", "I1", "IP", "VSEQ1", "ISEQ1" }, new[] { "POS", "V1PM", "I1PM", "PS", "PSV", "PSI" }) || phaseEndsWith(phasorLabel, new[] { "+SV", "+SI", "+V", "+I" }, true))
                    return '+';

                if (phaseMatchExact(phasorLabel, new[] { "V0PM", "I0PM", "VZPM", "IZPM" }) || phaseMatchHighConfidence(phasorLabel, new[] { "V0", "I0", "VSEQ0", "ISEQ0" }, new[] { "ZERO", "ZPV", "ZPI", "VSPM", "V0PM", "I0PM", "VZPM", "IZPM", "ZS", "ZSV", "ZSI" }) || phaseEndsWith(phasorLabel, new[] { "0SV", "0SI" }, true))
                    return '0';

                if (phaseMatchExact(phasorLabel, new[] { "VAPM", "IAPM" }) || phaseMatchHighConfidence(phasorLabel, new[] { "VA", "IA" }, new[] { "APV", "API", "VAPM", "IAPM", "AV", "AI" }))
                    return 'A';

                if (phaseMatchExact(phasorLabel, new[] { "VBPM", "IBPM" }) || phaseMatchHighConfidence(phasorLabel, new[] { "VB", "IB" }, new[] { "BPV", "BPI", "VBPM", "IBPM", "BV", "BI" }))
                    return 'B';

                if (phaseMatchExact(phasorLabel, new[] { "VCPM", "ICPM" }) || phaseMatchHighConfidence(phasorLabel, new[] { "VC", "IC" }, new[] { "CPV", "CPI", "VCPM", "ICPM", "CV", "CI" }))
                    return 'C';

                if (phaseMatchExact(phasorLabel, new[] { "VNPM", "INPM" }) || phaseMatchHighConfidence(phasorLabel, new[] { "VN", "IN" }, new[] { "NEUT", "NPV", "NPI", "VNPM", "INPM", "NV", "NI" }))
                    return 'N';

                if (phaseMatchExact(phasorLabel, new[] { "V2PM", "I2PM" }) || phaseMatchHighConfidence(phasorLabel, new[] { "V2", "I2", "VSEQ2", "ISEQ2" }, new[] { "NEG", "-SV", "-SI", "V2PM", "I2PM", "NS", "NSV", "NSI" }))
                    return '-';

                return phase;
            }

            int guessBaseKV(int baseKV, string phasorLabel, string deviceLabel)
            {
                // Check phasor label before device
                foreach (string voltageLevel in s_commonVoltageLevels)
                {
                    if (phasorLabel.IndexOf(voltageLevel, StringComparison.Ordinal) > -1)
                        return int.Parse(voltageLevel);
                }

                foreach (string voltageLevel in s_commonVoltageLevels)
                {
                    if (deviceLabel.IndexOf(voltageLevel, StringComparison.Ordinal) > -1)
                        return int.Parse(voltageLevel);
                }

                return baseKV;
            }

            AdoDataConnection connection = scanParams.Connection;
            TableOperations<Phasor> phasorTable = new(connection);

            // Get phasor signal types
            SignalType iphmSignalType = m_phasorSignalTypes["IPHM"];
            SignalType iphaSignalType = m_phasorSignalTypes["IPHA"];
            SignalType vphmSignalType = m_phasorSignalTypes["VPHM"];
            SignalType vphaSignalType = m_phasorSignalTypes["VPHA"];

            Phasor[] phasors = phasorTable.QueryPhasorsForDevice(device.ID).ToArray();

            bool dropAndAdd = phasors.Length != cell.PhasorDefinitions.Count;

            if (!dropAndAdd)
            {
                // Also do add operation if phasor source index records are not sequential
                if (phasors.Where((phasor, index) => phasor.SourceIndex != index + 1).Any())
                    dropAndAdd = true;
            }

            if (dropAndAdd)
            {
                if (cell.PhasorDefinitions.Count > 0)
                    connection.DeletePhasorsForDevice(device.ID);

                foreach (IPhasorDefinition phasorDefinition in cell.PhasorDefinitions)
                {
                    bool isVoltage = phasorDefinition.PhasorType == PhasorType.Voltage;

                    Phasor phasor = phasorTable.NewPhasor();
                    phasor.DeviceID = device.ID;
                    phasor.Label = phasorDefinition.Label;
                    phasor.Type = isVoltage ? 'V' : 'I';
                    phasor.Phase = guessPhase('+', phasor.Label);
                    phasor.BaseKV = guessBaseKV(500, phasor.Label, string.IsNullOrWhiteSpace(device.Name) ? device.Acronym ?? "" : device.Name);
                    phasor.DestinationPhasorID = null;
                    phasor.SourceIndex = phasorDefinition.Index;

                    phasorTable.AddNewPhasor(phasor);
                    SavePhasorMeasurement(isVoltage ? vphmSignalType : iphmSignalType, device, phasorDefinition, phasor.Phase, phasor.SourceIndex, phasor.BaseKV, measurementTable, scanParams);
                    SavePhasorMeasurement(isVoltage ? vphaSignalType : iphaSignalType, device, phasorDefinition, phasor.Phase, phasor.SourceIndex, phasor.BaseKV, measurementTable, scanParams);
                }
            }
            else
            {
                foreach (IPhasorDefinition phasorDefinition in cell.PhasorDefinitions)
                {
                    bool isVoltage = phasorDefinition.PhasorType == PhasorType.Voltage;

                    Phasor phasor = phasorTable.QueryPhasorForDevice(device.ID, phasorDefinition.Index);
                    phasor.DeviceID = device.ID;
                    phasor.Label = phasorDefinition.Label;
                    phasor.Type = isVoltage ? 'V' : 'I';

                    phasorTable.AddNewPhasor(phasor);
                    SavePhasorMeasurement(isVoltage ? vphmSignalType : iphmSignalType, device, phasorDefinition, phasor.Phase, phasor.SourceIndex, phasor.BaseKV, measurementTable, scanParams);
                    SavePhasorMeasurement(isVoltage ? vphaSignalType : iphaSignalType, device, phasorDefinition, phasor.Phase, phasor.SourceIndex, phasor.BaseKV, measurementTable, scanParams);
                }
            }
        }

        private void SavePhasorMeasurement(SignalType signalType, Device device, IPhasorDefinition phasorDefinition, char phase, int index, int baseKV, TableOperations<Measurement> measurementTable, ScanParameters scanParams)
        {
            string signalReference = $"{device.Acronym}-{signalType.Suffix}{index}";

            // Query existing measurement record for specified signal reference - function will create a new blank measurement record if one does not exist
            Measurement measurement = measurementTable.QueryMeasurement(signalReference);
            string pointTag = scanParams.CreatePhasorPointTag(device.Acronym, signalType.Acronym, phasorDefinition.Label, phase.ToString(), index, baseKV);

            measurement.DeviceID = device.ID;
            measurement.PointTag = pointTag;
            measurement.Description = $"{device.Acronym} {phasorDefinition.Label} {signalType.Name}";
            measurement.PhasorSourceIndex = index;
            measurement.SignalReference = signalReference;
            measurement.SignalTypeID = signalType.ID;
            measurement.Internal = true;
            measurement.Enabled = true;

            measurementTable.AddNewOrUpdateMeasurement(measurement);
        }
    }
}