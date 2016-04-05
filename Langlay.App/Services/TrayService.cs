﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Product.Common;

namespace Product
{
    public class TrayService
    {
        private IConfigService ConfigService { get; set; }
        private ISettingsService SettingsService { get; set; }
        private ContextMenu ContextMenu { get; set; }
        private NotifyIcon Icon { get; set; }
        private bool IsStarted { get; set; }

        public Action OnExit { get; set; }

        public TrayService(IConfigService configService, ISettingsService settingsService)
        {
            ConfigService = configService;
            SettingsService = settingsService;
        }

        public void Start()
        {
            if (!IsStarted)
            {
                IsStarted = true;
                ContextMenu = new ContextMenu(new[]
                {
                    new MenuItem("Settings", delegate { SettingsService.ShowSettings(); }),
                    new MenuItem("-"),
                    new MenuItem("Quit", delegate { if (OnExit != null) OnExit(); })
                });
                Icon = new NotifyIcon()
                {
                    Text = Application.ProductName,
                    Icon = new Icon(typeof(Program), "Keyboard-Filled-2-16.ico"),
                    Visible = true,
                    ContextMenu = ContextMenu
                };
            }
        }

        public void Stop()
        {
            if (IsStarted)
            {
                IsStarted = false;
                if (ContextMenu != null)
                    ContextMenu.Dispose();
                if (Icon != null)
                    Icon.Dispose();
            }
        }
    }
}