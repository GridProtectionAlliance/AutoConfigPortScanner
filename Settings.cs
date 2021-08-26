//******************************************************************************************************
//  Settings.cs - Gbtc
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
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using Gemstone.Configuration.AppSettings;
using Microsoft.Extensions.Configuration;

namespace AutoConfigPortScanner
{
    public class Settings
    {
        public const string MainSection = "Main";
        public const string SerialSection = "Serial";

        // Defaults for Main Settings
        public const bool DefaultRescan = false;
        public const bool DefaultAutoStartParsingSequenceForScan = true;
        public const bool DefaultAutoStartParsingSequenceForConfig = true;
        public const string DefaultSourceConfig = @"C:\Program Files\SIEGate\SIEGate.exe.config";
        public const int DefaultResponseTimeout = 2000;
        public const int DefaultConfigFrameTimeout = 60000;
        public const int DefaultDisableDataDelay = 1000;

        // Defaults for Serial Settings
        public const int DefaultBaudRate = 57600;
        public const int DefaultDataBits = 8;
        public const Parity DefaultParity = Parity.None;
        public const StopBits DefaultStopBits = StopBits.One;
        public const bool DefaultDtrEnable = false;
        public const bool DefaultRtsEnable = false;

        // Main Settings
        public bool AutoScan { get; set; }                          // Settings file / command line only
        public bool AutoRemoveIDs { get; set; }                     // Settings file / command line only
        public ushort StartComPort { get; set; }                    // On UI
        public ushort EndComPort { get; set; }                      // On UI
        public ushort[] ComPorts { get; set; }                      // On UI
        public ushort StartIDCode { get; set; }                     // On UI
        public ushort EndIDCode { get; set; }                       // On UI
        public ushort[] IDCodes { get; set; }                       // On UI
        public bool Rescan { get; set; }                            // On UI
        public bool AutoStartParsingSequenceForScan { get; set; }   // On UI
        public bool AutoStartParsingSequenceForConfig { get; set; } // Settings file / command line only
        public string SourceConfig { get; set; }                    // On UI
        public int ResponseTimeout { get; set; }                    // Settings file / command line only
        public int ConfigFrameTimeout { get; set; }                 // Settings file / command line only
        public int DisableDataDelay { get; set; }                   // Settings file / command line only

        // Serial Settings
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
        public bool DtrEnable { get; set; }
        public bool RtsEnable { get; set; }

        public ReadOnlyCollection<string> LocalSerialPorts { get; }
        public ushort MinPortNumber { get; }
        public ushort MaxPortNumber { get; }
        public int TotalSerialPorts { get; }

        public const ushort MinIDCode = 1;
        public const ushort MaxIDCode = ushort.MaxValue;

        public Settings(IConfiguration configuration)
        {
            Configuration = configuration;

            LocalSerialPorts = Array.AsReadOnly(SerialPort.GetPortNames());
            TotalSerialPorts = LocalSerialPorts.Count;
            SortedSet<ushort> portNumbers = new(LocalSerialPorts.Select(portName => ushort.Parse(portName.Substring(3))));

            if (portNumbers.Count > 0)
            {
                MinPortNumber = portNumbers.First();
                MaxPortNumber = portNumbers.Last();
            }

            IConfigurationSection mainSettings = Configuration.GetSection(MainSection);

            AutoScan = bool.Parse(mainSettings[nameof(AutoScan)]);
            AutoRemoveIDs = bool.Parse(mainSettings[nameof(AutoRemoveIDs)]);
            StartComPort = ushort.Parse(mainSettings[nameof(StartComPort)]);
            EndComPort = ushort.Parse(mainSettings[nameof(EndComPort)]);
            ComPorts = ParseUniqueUInt16Values(mainSettings[nameof(ComPorts)]);
            StartIDCode = ushort.Parse(mainSettings[nameof(StartIDCode)]);
            EndIDCode = ushort.Parse(mainSettings[nameof(EndIDCode)]);
            IDCodes = ParseUniqueUInt16Values(mainSettings[nameof(IDCodes)]);
            Rescan = bool.Parse(mainSettings[nameof(Rescan)]);
            AutoStartParsingSequenceForScan = bool.Parse(mainSettings[nameof(AutoStartParsingSequenceForScan)]);
            AutoStartParsingSequenceForConfig = bool.Parse(mainSettings[nameof(AutoStartParsingSequenceForConfig)]);
            SourceConfig = mainSettings[nameof(SourceConfig)];
            ResponseTimeout = int.Parse(mainSettings[nameof(ResponseTimeout)]);
            ConfigFrameTimeout = int.Parse(mainSettings[nameof(ConfigFrameTimeout)]);
            DisableDataDelay = int.Parse(mainSettings[nameof(DisableDataDelay)]);

            if (StartComPort == 0)
                StartComPort = MinPortNumber;

            if (EndComPort == 0)
                EndComPort = MaxPortNumber;

            if (StartIDCode == 0)
                StartIDCode = MinIDCode;

            if (EndIDCode == 0)
                EndIDCode = MaxIDCode;

            IConfigurationSection serialSettings = Configuration.GetSection(SerialSection);
            
            BaudRate = int.Parse(serialSettings[nameof(BaudRate)]);
            DataBits = int.Parse(serialSettings[nameof(DataBits)]);
            
            if (Enum.TryParse(serialSettings[nameof(Parity)], out Parity parity))
                Parity = parity;
            
            if (Enum.TryParse(serialSettings[nameof(StopBits)], out StopBits stopBits))
                StopBits = stopBits;
            
            DtrEnable = bool.Parse(serialSettings[nameof(DtrEnable)]);
            RtsEnable = bool.Parse(serialSettings[nameof(RtsEnable)]);

            if (ResponseTimeout <= 0)
                ResponseTimeout = DefaultResponseTimeout;

            if (ConfigFrameTimeout <= 0)
                ConfigFrameTimeout = DefaultConfigFrameTimeout;

            if (DisableDataDelay <= 0)
                DisableDataDelay = DefaultDisableDataDelay;

            if (ResponseTimeout < DisableDataDelay)
                ResponseTimeout = DisableDataDelay + 500;
        }

        public IConfiguration Configuration { get; }

        public void Save()
        {
            if (Configuration is null)
                return;

            // Only need serialize settings that can be changed on UI:
            IConfigurationSection mainSettings = Configuration.GetSection(MainSection);

            mainSettings[nameof(StartComPort)] = StartComPort.ToString();
            mainSettings[nameof(EndComPort)] = EndComPort.ToString();
            mainSettings[nameof(ComPorts)] = string.Join(",", ComPorts);
            mainSettings[nameof(StartIDCode)] = StartIDCode.ToString();
            mainSettings[nameof(EndIDCode)] = EndIDCode.ToString();
            mainSettings[nameof(IDCodes)] = string.Join(",", IDCodes);
            mainSettings[nameof(Rescan)] = Rescan.ToString();
            mainSettings[nameof(AutoStartParsingSequenceForScan)] = AutoStartParsingSequenceForScan.ToString();
            mainSettings[nameof(SourceConfig)] = SourceConfig;
        }

        public static void ConfigureAppSettings(IAppSettingsBuilder builder)
        {
            // Main configuration settings
            builder.Add($"{MainSection}:{nameof(AutoScan)}", "false", "Defines the value indicating if tool should start scan automatically on start.");
            builder.Add($"{MainSection}:{nameof(AutoRemoveIDs)}", "false", "Defines the value indicating if tool should auto-remove ID codes from list as they are completed.");
            builder.Add($"{MainSection}:{nameof(StartComPort)}", "0", "Defines the starting COM port number for the scan.");
            builder.Add($"{MainSection}:{nameof(EndComPort)}", "0", "Defines the ending COM port number for the scan.");
            builder.Add($"{MainSection}:{nameof(ComPorts)}", "", "Defines the comma separated list of serial COM ports to scan (overrides start/end range).");
            builder.Add($"{MainSection}:{nameof(StartIDCode)}", "0", "Defines the starting IEEE C37.118 ID code for the scan.");
            builder.Add($"{MainSection}:{nameof(EndIDCode)}", "0", "Defines the ending IEEE C37.118 ID code for the scan.");
            builder.Add($"{MainSection}:{nameof(IDCodes)}", "", "Defines the comma separated list of IEEE C37.118 ID codes to scan (overrides start/end range).");
            builder.Add($"{MainSection}:{nameof(Rescan)}", DefaultRescan.ToString(), "Defines the value indicating whether already configured COM ports should be rescanned.");
            builder.Add($"{MainSection}:{nameof(AutoStartParsingSequenceForScan)}", DefaultAutoStartParsingSequenceForScan.ToString(), "Defines the value indicating whether scan should send parsing sequence to start connection.");
            builder.Add($"{MainSection}:{nameof(AutoStartParsingSequenceForConfig)}", DefaultAutoStartParsingSequenceForConfig.ToString(), "Defines the value indicating whether added device configuration should be set to send parsing sequence to start connection.");
            builder.Add($"{MainSection}:{nameof(SourceConfig)}", DefaultSourceConfig, "Defines the source configuration file for host application that contains database connection info.");
            builder.Add($"{MainSection}:{nameof(ResponseTimeout)}", DefaultResponseTimeout.ToString(), "Defines the maximum time, in milliseconds, to wait for a serial response.");
            builder.Add($"{MainSection}:{nameof(ConfigFrameTimeout)}", DefaultConfigFrameTimeout.ToString(), "Defines the maximum time, in milliseconds, to wait for a configuration frame.");
            builder.Add($"{MainSection}:{nameof(DisableDataDelay)}", DefaultDisableDataDelay.ToString(), "Defined the delay time, in milliseconds, to wait after sending the DisableRealTimeData command to a device.");

            // Serial configuration settings
            builder.Add($"{SerialSection}:{nameof(BaudRate)}", DefaultBaudRate.ToString(), "Defines the serial baud rate. Standard values: 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, or 256000.");
            builder.Add($"{SerialSection}:{nameof(DataBits)}", DefaultDataBits.ToString(), "Defines the standard length of data bits per byte. Standard values: 5, 6, 7 or 8.");
            builder.Add($"{SerialSection}:{nameof(Parity)}", DefaultParity.ToString(), "Defines the parity-checking protocol. Value is one of: Even, Mark, None, Odd or Space.");
            builder.Add($"{SerialSection}:{nameof(StopBits)}", DefaultStopBits.ToString(), "Defines the standard number of stopbits per byte. Value is one of: None, One, OnePointFive or Two.");
            builder.Add($"{SerialSection}:{nameof(DtrEnable)}", DefaultDtrEnable.ToString(), "Defines the value that enables the Data Terminal Ready (DTR) signal during serial communication.");
            builder.Add($"{SerialSection}:{nameof(RtsEnable)}", DefaultRtsEnable.ToString(), "Defines the value indicating whether the Request to Send (RTS) signal is enabled during serial communication.");
        }

        // Command line overrides
        public static Dictionary<string, string> SwitchMappings => new()
        {
            [$"--{nameof(BaudRate)}"] = $"{SerialSection}:{nameof(BaudRate)}",
            [$"--{nameof(DataBits)}"] = $"{SerialSection}:{nameof(DataBits)}",
            [$"--{nameof(Parity)}"] = $"{SerialSection}:{nameof(Parity)}",
            [$"--{nameof(StopBits)}"] = $"{SerialSection}:{nameof(StopBits)}",
            [$"--{nameof(DtrEnable)}"] = $"{SerialSection}:{nameof(DtrEnable)}",
            [$"--{nameof(RtsEnable)}"] = $"{SerialSection}:{nameof(RtsEnable)}",
            [$"--{nameof(AutoScan)}"] = $"{MainSection}:{nameof(AutoScan)}",
            [$"--{nameof(AutoRemoveIDs)}"] = $"{MainSection}:{nameof(AutoRemoveIDs)}",
            [$"--{nameof(StartComPort)}"] = $"{MainSection}:{nameof(StartComPort)}",
            [$"--{nameof(EndComPort)}"] = $"{MainSection}:{nameof(EndComPort)}",
            ["--AutoStartParsingSequence"] = $"{MainSection}:{nameof(AutoStartParsingSequenceForConfig)}",
            [$"--{nameof(ResponseTimeout)}"] = $"{MainSection}:{nameof(ResponseTimeout)}",
            [$"--{nameof(ConfigFrameTimeout)}"] = $"{MainSection}:{nameof(ConfigFrameTimeout)}",
            [$"--{nameof(DisableDataDelay)}"] = $"{MainSection}:{nameof(DisableDataDelay)}",
            ["-b"] = $"{SerialSection}:{nameof(BaudRate)}",
            ["-d"] = $"{SerialSection}:{nameof(DataBits)}",
            ["-p"] = $"{SerialSection}:{nameof(Parity)}",
            ["-s"] = $"{SerialSection}:{nameof(StopBits)}",
            ["-t"] = $"{SerialSection}:{nameof(DtrEnable)}",
            ["-r"] = $"{SerialSection}:{nameof(RtsEnable)}",
            ["-x"] = $"{MainSection}:{nameof(AutoScan)}",
            ["-i"] = $"{MainSection}:{nameof(AutoRemoveIDs)}",
            ["-a"] = $"{MainSection}:{nameof(AutoStartParsingSequenceForConfig)}",
            ["-n"] = $"{MainSection}:{nameof(ResponseTimeout)}",
            ["-c"] = $"{MainSection}:{nameof(ConfigFrameTimeout)}",
            ["-w"] = $"{MainSection}:{nameof(DisableDataDelay)}"
        };

        public static ushort[] ParseUniqueUInt16Values(string itemList) =>
            itemList.Split(new[] { ",", ";", "\r\n", "\n", "\t", " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => (success: ushort.TryParse(item.Trim(), out ushort value), value))
                .Where(result => result.success)
                .Select(result => result.value)
                .Distinct()
                .ToArray();
    }
}