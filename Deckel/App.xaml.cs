using Deckel.Helpers;
using Deckel.NativeMethods;
using Microsoft.Win32;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Violeta.Appearance;

namespace Deckel
{
    public partial class App
    {
        private Mutex? _isRunning;

        protected override void OnStartup(StartupEventArgs e)
        {
            AutoStartHelper.CreateAutoStartTask();

            ThemeManager.Apply(ThemeHelper.AppsUseDarkTheme()
                ? ApplicationTheme.Dark
                : ApplicationTheme.Light);
            SystemEvents.UserPreferenceChanged += (_, _) =>
            {
                ThemeManager.Apply(ThemeHelper.AppsUseDarkTheme() ? ApplicationTheme.Dark : ApplicationTheme.Light);
                TrayIcon.UpdateIcon();
            };
            UxTheme.ApplyPreferredAppMode();

            _ = TrayIcon.GetInstance();

            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!EnsureFirstInstance())
            {
                Shutdown();
                return;
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _isRunning?.ReleaseMutex();
            TrayIcon.GetInstance().Dispose();
        }

        private bool EnsureFirstInstance()
        {
            _isRunning = new Mutex(true, "Deckel.App.Mutex", out bool isFirst);
            if (isFirst)
                return true;

            System.Windows.Forms.MessageBox.Show("Deckel is already running", "Failed to open Deckel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }
    }
}
