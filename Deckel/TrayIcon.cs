using Deckel.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Violeta.Resources;
using Wpf.Ui.Violeta.Win32;
using Application = System.Windows.Forms.Application;
using System.Reflection;

namespace Deckel
{
    internal partial class TrayIcon : IDisposable
    {
        private static TrayIcon _instance;
        private readonly TrayIconHost _icon;

        private TrayIcon()
        {
            _icon = new TrayIconHost
            {
                Icon = GetTrayIcon(),
                ToolTipText = $"{Application.ProductName} v{Application.ProductVersion.Split('+')[0]}",
                Menu =
                [
                    new TrayMenuItem()
                    {
                        Header = $"v{Application.ProductVersion.Split('+')[0]}",
                        IsEnabled = false
                    },
                    new TraySeparator(),
                    new TrayMenuItem()
                    {
                        Header = "Quit",
                        Command = new RelayCommand(System.Windows.Application.Current.Shutdown)
                    }
                ],
                IsVisible = true
            };
        }

        private static nint GetTrayIcon()
        {
            return ThemeHelper.SystemUsesDarkTheme()
                ? Resources.tray_white.Handle
                : Resources.tray_black.Handle;
        }

        public void Dispose()
        {
            _icon.IsVisible = false;
        }

        public static TrayIcon GetInstance()
        {
            return _instance ??= new TrayIcon();
        }

        public static void UpdateIcon()
        {
            var newIcon = GetTrayIcon();
            _instance._icon.Icon = newIcon;
        }
    }
}
