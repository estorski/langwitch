﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using Product.Common;

namespace Product
{
    internal static class Program
    {
        private static bool IsAppInitialized { get; set; }

        private static void InitializeApp()
        {
            if (!IsAppInitialized)
            {
                IsAppInitialized = true;

                // We must let Windows know that our app is DPI aware, so
                // the sizes and coordinates don't get scaled behind the scene
                Win32.SetProcessDPIAware();

                // Make the app react properly to external events
                Application.AddMessageFilter(new AppMessageFilter
                {
                    OnClose = delegate { Application.Exit(); },
                    OnRestart = delegate { Application.Restart(); }
                });
            }
        }

        [STAThread]
        private static void Main()
        {
            try
            {
                var appRunnerService = new AppRunnerService();
                var configService = appRunnerService.ReadConfig();

                var uniquenessService = new UniquenessService(
                    Application.ProductName, configService.DoForceThisInstance,
                    delegate { ProcessUtils.StopOtherMainApp(); });
                uniquenessService.Run(delegate
                {
                    InitializeApp();
                    appRunnerService.RunTheConfig(configService);
                });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
#if DEBUG
                MessageBox.Show(ex.ToString());
#endif

                // In the release environment we don't show exceptions up
            }
        }
    }
}