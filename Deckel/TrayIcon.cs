using Deckel.Helpers;
using Deckel.NativeMethods;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Violeta.Win32;
using Application = System.Windows.Forms.Application;
using System.Diagnostics;

namespace Deckel
{
    internal partial class TrayIcon : IDisposable
    {
        private static TrayIcon? _instance;
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
                        Header = "Opend Desktop in Explorer",
                        Command = new RelayCommand(() => Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
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

            _icon.LeftDown += (sender, e) =>
            {
                User32.ToggleDesktopIcons();
                UpdateIcon();
            };
        }

        private static nint GetTrayIcon()
        {
            return ThemeHelper.SystemUsesDarkTheme()
                ? (User32.IsDesktopIconsVisible() ? Resources.tray_white.Handle : Resources.tray_white_active.Handle)
                : (User32.IsDesktopIconsVisible() ? Resources.tray_black.Handle : Resources.tray_black_active.Handle);
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
            if (_instance != null)
                _instance._icon.Icon = newIcon;
        }
    }
}
