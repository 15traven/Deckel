using Microsoft.Win32;

namespace Deckel.Helpers
{
    public static class ThemeHelper
    {
        public static bool AppsUseDarkTheme()
        {
            var value = Registry.GetValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                        "AppsUseLightTheme", 1);

            return value != null && (int)value == 0;
        }

        public static bool SystemUsesDarkTheme()
        {
            var value = Registry.GetValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                        "SystemUsesLightTheme", 0);

            return value == null || (int)value == 0;
        }
    }
}
