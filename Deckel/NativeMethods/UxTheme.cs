using Deckel.Helpers;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace Deckel.NativeMethods
{
    internal static class UxTheme
    {
        [DllImport("uxtheme.dll", EntryPoint = "#132", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool ShouldAppsUseDarkMode();

        [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);

        [DllImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void FlushMenuThemes();

        [DllImport("uxtheme.dll", EntryPoint = "#138", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool ShouldSystemUseDarkMode();

        public enum PreferredAppMode : int { Default, AllowDark, ForceDark, ForceLight, Max };

        public static void ApplyPreferredAppMode()
        {
            if (Environment.OSVersion.Version.Build >= 18362)
            {
                if (ThemeHelper.SystemUsesDarkTheme())
                {
                    SetPreferredAppMode(PreferredAppMode.ForceDark);
                    FlushMenuThemes();
                }

                SystemEvents.UserPreferenceChanged += (_, _) =>
                {
                    if (ThemeHelper.SystemUsesDarkTheme())
                    {
                        SetPreferredAppMode(PreferredAppMode.ForceDark);
                        FlushMenuThemes();
                    }
                    else
                    {
                        SetPreferredAppMode(PreferredAppMode.ForceLight);
                        FlushMenuThemes();
                    }
                };
            }
        }
    }
}
