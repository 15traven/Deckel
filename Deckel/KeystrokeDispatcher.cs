using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Deckel.Helpers;
using Deckel.NativeMethods;

namespace Deckel
{
    internal class KeystrokeDispatcher : IDisposable
    {
        private static KeystrokeDispatcher? _instance;
        private static HashSet<Keys>? _validKeys;
        private GlobalKeyboardHook? _hook;

        protected KeystrokeDispatcher()
        {
            InstallKeyHook(KeyDownEventHandler);

            _validKeys = [
                Keys.Alt, Keys.H
            ];
        }

        private void InstallKeyHook(KeyEventHandler downHandler)
        {
            _hook = GlobalKeyboardHook.GetInstance();

            _hook.KeyDown += downHandler;
        }

        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.H)
            {
                User32.ToggleDesktopIcons();
                TrayIcon.UpdateIcon();
            }
        }

        public void Dispose()
        {
            _hook?.Dispose();
            _hook = null;
        }

        internal static KeystrokeDispatcher GetInstance()
        {
            return _instance ??= new KeystrokeDispatcher();
        }
    }
}
