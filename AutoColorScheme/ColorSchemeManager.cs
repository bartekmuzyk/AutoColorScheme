using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoColorScheme
{
    internal class ColorSchemeManager
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam,
            SendMessageTimeoutFlags fuFlags, uint uTimeout, out IntPtr lpdwResult);

        // Define constants used for the SendMessageTimeout function
        private const uint HWND_BROADCAST = 0xFFFF;
        private const uint WM_SETTINGCHANGE = 0x001A;

        [Flags]
        private enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8
        }

        private readonly RegistryKey key;

        public ColorSchemeManager()
        {
            key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true)
                ?? throw new InvalidOperationException("Personalize key not found.");
        }

        private void BroadcastChange()
        {
            string changeParameter = "ImmersiveColorSet";
            IntPtr lParam = Marshal.StringToHGlobalUni(changeParameter);

            SendMessageTimeout(
                (IntPtr)HWND_BROADCAST,
                WM_SETTINGCHANGE,
                IntPtr.Zero, lParam,
                SendMessageTimeoutFlags.SMTO_ABORTIFHUNG,
                1000,
                out _
            );
        }

        public bool GetDarkMode()
        {
            var value = (int)key.GetValue("SystemUsesLightTheme")!;

            return value == 0;
        }

        public void SetDarkMode(bool enabled)
        {
            var darkModeEnabledInSystem = GetDarkMode();

            if (darkModeEnabledInSystem == enabled) return;

#if DEBUG
            var mode = enabled ? "dark" : "light";
            Debug.WriteLine($"Setting {mode} mode...");
#endif

            var value = enabled ? 0 : 1;
            var inverseValue = enabled ? 1 : 0;

            key.SetValue("AppsUseLightTheme", inverseValue, RegistryValueKind.DWord);
            key.SetValue("SystemUsesLightTheme", inverseValue, RegistryValueKind.DWord);
            BroadcastChange();

#if DEBUG
            Debug.WriteLine("Middle part");
#endif

            key.SetValue("AppsUseLightTheme", value, RegistryValueKind.DWord);
            key.SetValue("SystemUsesLightTheme", value, RegistryValueKind.DWord);
            BroadcastChange();

#if DEBUG
            Debug.WriteLine("Set!");
#endif

            GC.Collect();  // Force garbage collection to prevent memory leaks
            // Need to think how to prevent memory leaks properly
        }
    }
}
