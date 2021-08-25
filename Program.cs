//******************************************************************************************************
//  Program.cs - Gbtc
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
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gemstone.Configuration;
using GSF.Diagnostics;
using GSF.IO;
using Microsoft.Extensions.Configuration;

namespace AutoConfigPortScanner
{
    internal static class Program
    {
        internal static LogPublisher Log { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            string localPath = FilePath.GetAbsolutePath("");
            string logPath = string.Format("{0}{1}Logs{1}", localPath, Path.DirectorySeparatorChar);

            try
            {
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);
            }
            catch
            {
                logPath = localPath;
            }

            Logger.FileWriter.SetPath(logPath);
            Logger.FileWriter.SetLoggingFileCount(10);
            Logger.SuppressFirstChanceExceptionLogMessages();

            // Create a new log publisher instance
            Log = Logger.CreatePublisher(typeof(MainForm), MessageClass.Application);
            
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            IConfiguration configuration = new ConfigurationBuilder()
                .ConfigureGemstoneDefaults(Settings.ConfigureAppSettings, useINI: true)
                .AddCommandLine(args, Settings.SwitchMappings)
                .Build();

            Settings settings = new(configuration);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new MainForm { Settings = settings });
            }
            catch (Exception ex)
            {
                Log.Publish(MessageLevel.Error, "Application", "Unhandled Application Exception", exception: ex);
            }

            try
            {
                settings.Save();
            }
            catch (Exception ex)
            {
                Log.Publish(MessageLevel.Error, "Application", "Save Settings Exception", exception: ex);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Log.Publish(MessageLevel.Error, "Application", "Unhandled AppDomain Exception", exception: ex);
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            foreach (Exception ex in e.Exception.Flatten().InnerExceptions)
                Log.Publish(MessageLevel.Error, "Task", "Unhandled Task Exception", exception: ex);

            e.SetObserved();
        }
    }
}
