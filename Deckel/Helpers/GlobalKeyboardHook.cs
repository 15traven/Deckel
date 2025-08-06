using Deckel.NativeMethods;
using System;
using System.Windows.Forms;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;

namespace Deckel.Helpers
{
    internal class GlobalKeyboardHook : IDisposable
    {
        private static GlobalKeyboardHook? _instance;

        private User32.KeyboardHookProc? _callback;
        private nint _hHook = IntPtr.Zero;

        internal event KeyEventHandler KeyDown;
        internal event KeyEventHandler KeyUp;

        protected GlobalKeyboardHook()
        {
            Hook();
        }

        private void Hook()
        {
            _callback = HookProc;

            var hInstance = Kernel32.LoadLibrary("user32.dll");
            _hHook = User32.SetWindowsHookEx(User32.WH_KEYBOARD_LL, _callback, hInstance, 0);
        }

        private void Unhook()
        {
            if (_callback == null) return;

            User32.UnhookWindowsHookEx(_hHook);
            _callback = null;
        }

        private int HookProc(int code, int wParam, ref User32.KeyboardHookStruct lParam)
        {
            if (code < 0)
                return User32.CallNextHookEx(_hHook, code, wParam, ref lParam);

            if (IsWindowsKeyPressed())
                return User32.CallNextHookEx(_hHook, code, wParam, ref lParam);

            var key = (Keys)lParam.vkCode;
            key = AddModifiers(key);

            var kea = new KeyEventArgs(key);

            if (wParam == User32.WM_KEYDOWN || wParam == User32.WM_SYSKEYDOWN)
                KeyDown?.Invoke(this, kea);
            else if (wParam == User32.WM_KEYUP || wParam == User32.WM_SYSKEYUP)
                KeyUp?.Invoke(this, kea);

            return kea.Handled ? 1 : User32.CallNextHookEx(_hHook, code, wParam, ref lParam);
        }
        private bool IsWindowsKeyPressed()
        {
            return Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin);
        }

        private Keys AddModifiers(Keys key)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0) key = key | Keys.Control;

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0) key = key | Keys.Shift;

            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0) key = key | Keys.Alt;

            return key;
        }

        internal static GlobalKeyboardHook GetInstance()
        {
            return _instance ??= new GlobalKeyboardHook();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Unhook();
        }
    }
}
