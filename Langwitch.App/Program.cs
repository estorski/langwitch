﻿using System;
using System.Threading;
using System.Windows.Forms;

namespace Langwitch
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var uniquenessService = new UniquenessService("Langwitch");
            uniquenessService.RunOrIgnore(delegate
            {
                var isExiting = false;

                var configService = new ConfigService();
                configService.ReadFromConfigFile();
                configService.ReadFromCommandLineArguments();

                var startupService = new WinStartupService(configService);

                ILanguageSetterService languageSetterService;
                if (configService.SwitchMethod == SwitchMethod.InputSimulation)
                    languageSetterService = new SimulatorLanguageSetterService();
                else
                    languageSetterService = new MessageLanguageSetterService();

                var overlayService = new OverlayService(configService);
                var languageService = new LanguageService(configService, overlayService, languageSetterService);
                var hotkeyService = new HotkeyService(configService, languageService);
                var trayService = new TrayService(configService)
                {
                    OnExit = delegate { isExiting = true; }
                };

                try
                {

                    hotkeyService.Start();
                    trayService.Start();
                    overlayService.Start();
                    startupService.ResolveStartup();
                    while (true)
                    {
                        Thread.Sleep(10);
                        try
                        {
                            Application.DoEvents();
                            if (isExiting)
                                // Quitting the loop must be just enough
                                break;
                        }
                        catch (Exception ex)
                        {
                            // Do nothing O_o as of yet.
                            break;
                        }
                    }

                }
                finally
                {
                    trayService.Stop();
                    hotkeyService.Stop();
                    overlayService.Stop();
                }
            });
        }

    }
}
