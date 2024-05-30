using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoColorScheme
{
    internal class StartupManager
    {
        private readonly string appName;

        private RegistryKey? key;

        public bool KeyOpened => key != null;

        public bool IsInStartup
        {
            get
            {
                EnsureOpenedKey();

                return key!.GetValue(appName) != null;
            }
        }

        public StartupManager(string appName)
        {
            this.appName = appName;

            key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        }

        private void EnsureOpenedKey()
        {
            if (key == null)
            {
                throw new InvalidOperationException("Registry key is not opened.");
            }
        }

        public void AddToStartup()
        {
            EnsureOpenedKey();

            key!.SetValue(appName, Application.ExecutablePath);
        }

        public void RemoveFromStartup()
        {
            EnsureOpenedKey();

            key!.DeleteValue(appName, false);
        }
    }
}
