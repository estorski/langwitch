﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Product.SettingsUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConfigService ConfigService { get; set; }
        private ConfigViewModel ViewModel { get; set; }

        public MainWindow()
        {
            ConfigService = new ConfigService();
            ConfigService.ReadFromConfigFile();

            ViewModel = new ConfigViewModel(ConfigService);
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            DataContext = ViewModel;
            InitializeComponent();
        }

        private void DoOnViewModelChanged()
        {
            // Save configuration
            ConfigService.SaveToFile();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            DoOnViewModelChanged();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Close the running product, and restart it.
            var process = Process.GetProcessesByName("Langwitch").FirstOrDefault();
            if (process == null)
                process = Process.GetProcessesByName("Langwitch.vshost").FirstOrDefault();

            if (process != null)
            {
                process.CloseMainWindow();
                process.WaitForExit(500);
                process.Kill();
                var productLocation = 
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Langwitch.exe");
                Process.Start(new ProcessStartInfo(productLocation));
            }
            this.Close();
        }

        private void HotkeyComposer_Layout_Changed(object sender, RoutedEventArgs e)
        {
            ViewModel.NotifyLayoutSequenceChanged();
        }

        private void HotkeyComposer_Language_Changed(object sender, RoutedEventArgs e)
        {
            ViewModel.NotifyLanguageSequenceChanged();
        }
    }
}
