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

        // Defaults for Serial Settings
        public const int DefaultBaudRate = 57600;
        public const int DefaultDataBits = 8;
        public const Parity DefaultParity = Parity.None;
        public const StopBits DefaultStopBits = StopBits.One;
        public const bool DefaultDtrEnable = false;
        public const bool DefaultRtsEnable = false;

        // Main Settings
        public int StartComPort { get; set; }                       // On UI
        public int EndComPort { get; set; }                         // On UI
        public int StartIDCode { get; set; }                        // On UI
        public int EndIDCode { get; set; }                          // On UI
        public bool Rescan { get; set; }                            // On UI
        public bool AutoStartParsingSequenceForScan { get; set; }   // On UI
        public bool AutoStartParsingSequenceForConfig { get; set; } // Settings file / command line only
        public string SourceConfig { get; set; }                    // On UI
        public int ResponseTimeout { get; set; }                    // Settings file / command line only
        public int ConfigFrameTimeout { get; set; }                 // Settings file / command line only

        // Serial Settings
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
        public bool DtrEnable { get; set; }
        public bool RtsEnable { get; set; }

        public ReadOnlyCollection<string> LocalSerialPorts { get; }
        public int MinPortNumber { get; }
        public int MaxPortNumber { get; }
        public int TotalSerialPorts { get; }

        public const ushort MinIDCode = 1;
        public const ushort MaxIDCode = ushort.MaxValue;

        private readonly IConfiguration m_configuration;

        public Settings(IConfiguration configuration)
        {
            m_configuration = configuration;

            LocalSerialPorts = Array.AsReadOnly(SerialPort.GetPortNames());
            TotalSerialPorts = LocalSerialPorts.Count;
            SortedSet<ushort> portNumbers = new SortedSet<ushort>(LocalSerialPorts.Select(portName => ushort.Parse(portName.Substring(3))));

            if (portNumbers.Count > 0)
            {
                MinPortNumber = portNumbers.First();
                MaxPortNumber = portNumbers.Last();
            }

            IConfigurationSection mainSettings = m_configuration.GetSection(MainSection);
            StartComPort = int.Parse(mainSettings[nameof(StartComPort)]);
            EndComPort = int.Parse(mainSettings[nameof(EndComPort)]);
            StartIDCode = int.Parse(mainSettings[nameof(StartIDCode)]);
            EndIDCode = int.Parse(mainSettings[nameof(EndIDCode)]);
            Rescan = bool.Parse(mainSettings[nameof(Rescan)]);
            AutoStartParsingSequenceForScan = bool.Parse(mainSettings[nameof(AutoStartParsingSequenceForScan)]);
            AutoStartParsingSequenceForConfig = bool.Parse(mainSettings[nameof(AutoStartParsingSequenceForConfig)]);
            SourceConfig = mainSettings[nameof(SourceConfig)];
            ResponseTimeout = int.Parse(mainSettings[nameof(ResponseTimeout)]);
            ConfigFrameTimeout = int.Parse(mainSettings[nameof(ConfigFrameTimeout)]);

            if (StartComPort == 0)
                StartComPort = MinPortNumber;

            if (EndComPort == 0)
                EndComPort = MaxPortNumber;

            if (StartIDCode == 0)
                StartIDCode = MinIDCode;

            if (EndIDCode == 0)
                EndIDCode = MaxIDCode;

            IConfigurationSection serialSettings = m_configuration.GetSection(SerialSection);
            
            BaudRate = int.Parse(serialSettings[nameof(BaudRate)]);
            DataBits = int.Parse(serialSettings[nameof(DataBits)]);
            
            if (Enum.TryParse<Parity>(serialSettings[nameof(Parity)], out Parity parity))
                Parity = parity;
            
            if (Enum.TryParse<StopBits>(serialSettings[nameof(StopBits)], out StopBits stopBits))
                StopBits = stopBits;
            
            DtrEnable = bool.Parse(serialSettings[nameof(DtrEnable)]);
            RtsEnable = bool.Parse(serialSettings[nameof(RtsEnable)]);
        }

        public IConfiguration Configuration => m_configuration;

        public void Save()
        {
            if (m_configuration is null)
                return;

            // Only need serialize settings that can be changed on UI:
            IConfigurationSection mainSettings = m_configuration.GetSection(MainSection);
            mainSettings[nameof(StartComPort)] = StartComPort.ToString();
            mainSettings[nameof(EndComPort)] = EndComPort.ToString();
            mainSettings[nameof(StartIDCode)] = StartIDCode.ToString();
            mainSettings[nameof(EndIDCode)] = EndIDCode.ToString();
            mainSettings[nameof(Rescan)] = Rescan.ToString();
            mainSettings[nameof(AutoStartParsingSequenceForScan)] = AutoStartParsingSequenceForScan.ToString();
            mainSettings[nameof(SourceConfig)] = SourceConfig;
        }

        public static void ConfigureAppSettings(IAppSettingsBuilder builder)
        {
            // Main configuration settings
            builder.Add($"{MainSection}:{nameof(StartComPort)}", "0", "Defines the starting COM port number for the scan.");
            builder.Add($"{MainSection}:{nameof(EndComPort)}", "0", "Defines the ending COM port number for the scan.");
            builder.Add($"{MainSection}:{nameof(StartIDCode)}", "0", "Defines the starting IEEE C37.118 ID code for the scan.");
            builder.Add($"{MainSection}:{nameof(EndIDCode)}", "0", "Defines the ending IEEE C37.118 ID code for the scan.");
            builder.Add($"{MainSection}:{nameof(Rescan)}", DefaultRescan.ToString(), "Defines the value indicating whether already configured COM ports should be rescanned.");
            builder.Add($"{MainSection}:{nameof(AutoStartParsingSequenceForScan)}", DefaultAutoStartParsingSequenceForScan.ToString(), "Defines the value indicating whether scan should send parsing sequence to start connection.");
            builder.Add($"{MainSection}:{nameof(AutoStartParsingSequenceForConfig)}", DefaultAutoStartParsingSequenceForConfig.ToString(), "Defines the value indicating whether added device configuration should be set to send parsing sequence to start connection.");
            builder.Add($"{MainSection}:{nameof(SourceConfig)}", DefaultSourceConfig, "Defines the source configuration file for host application that contains database connection info.");
            builder.Add($"{MainSection}:{nameof(ResponseTimeout)}", DefaultResponseTimeout.ToString(), "Defines the maximum time, in milliseconds, to wait for a serial response.");
            builder.Add($"{MainSection}:{nameof(ConfigFrameTimeout)}", DefaultConfigFrameTimeout.ToString(), "Defines the maximum time, in milliseconds, to wait for a configuration frame.");

            // Serial configuration settings
            builder.Add($"{SerialSection}:{nameof(BaudRate)}", DefaultBaudRate.ToString(), "Defines the serial baud rate. Standard values: 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, or 256000.");
            builder.Add($"{SerialSection}:{nameof(DataBits)}", DefaultDataBits.ToString(), "Defines the standard length of data bits per byte. Standard values: 5, 6, 7 or 8.");
            builder.Add($"{SerialSection}:{nameof(Parity)}", DefaultParity.ToString(), "Defines the parity-checking protocol. Value is one of: Even, Mark, None, Odd or Space.");
            builder.Add($"{SerialSection}:{nameof(StopBits)}", DefaultStopBits.ToString(), "Defines the standard number of stopbits per byte. Value is one of: None, One, OnePointFive or Two.");
            builder.Add($"{SerialSection}:{nameof(DtrEnable)}", DefaultDtrEnable.ToString(), "Defines the value that enables the Data Terminal Ready (DTR) signal during serial communication.");
            builder.Add($"{SerialSection}:{nameof(RtsEnable)}", DefaultRtsEnable.ToString(), "Defines the value indicating whether the Request to Send (RTS) signal is enabled during serial communication.");
        }

        // Command line overrides
        public static Dictionary<string, string> SwitchMappings => new Dictionary<string, string>
        {
            [$"--{nameof(BaudRate)}"] = $"{SerialSection}:{nameof(BaudRate)}",
            [$"--{nameof(DataBits)}"] = $"{SerialSection}:{nameof(DataBits)}",
            [$"--{nameof(Parity)}"] = $"{SerialSection}:{nameof(Parity)}",
            [$"--{nameof(StopBits)}"] = $"{SerialSection}:{nameof(StopBits)}",
            [$"--{nameof(DtrEnable)}"] = $"{SerialSection}:{nameof(DtrEnable)}",
            [$"--{nameof(RtsEnable)}"] = $"{SerialSection}:{nameof(RtsEnable)}",
            ["-b"] = $"{SerialSection}:{nameof(BaudRate)}",
            ["-d"] = $"{SerialSection}:{nameof(DataBits)}",
            ["-p"] = $"{SerialSection}:{nameof(Parity)}",
            ["-s"] = $"{SerialSection}:{nameof(StopBits)}",
            ["-t"] = $"{SerialSection}:{nameof(DtrEnable)}",
            ["-r"] = $"{SerialSection}:{nameof(RtsEnable)}",
            ["-a"] = $"{MainSection}:{nameof(AutoStartParsingSequenceForConfig)}",
            ["-n"] = $"{MainSection}:{nameof(ResponseTimeout)}",
            ["-c"] = $"{MainSection}:{nameof(ConfigFrameTimeout)}"
        };
    }
}