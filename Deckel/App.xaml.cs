using Deckel.Helpers;
using Deckel.NativeMethods;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Violeta.Appearance;
using Application = System.Windows.Application;

namespace Deckel
{
    public partial class App
    {
        private Mutex _isRunning;

        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.Apply(ThemeHelper.AppsUseDarkTheme()
                ? ApplicationTheme.Dark
                : ApplicationTheme.Light);
            SystemEvents.UserPreferenceChanged += (_, _) =>
                ThemeManager.Apply(ThemeHelper.AppsUseDarkTheme() ? ApplicationTheme.Dark : ApplicationTheme.Light);
            UxTheme.ApplyPreferredAppMode();

            _ = TrayIcon.GetInstance();

            base.OnStartup(e);
        }
    }
}
