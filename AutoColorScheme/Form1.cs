using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using WinFormsDarkMode;

namespace AutoColorScheme
{
    public partial class Form1 : Form
    {
        private readonly ColorSchemeManager colorSchemeManager;

        private readonly StartupManager startupManager;

        private bool isDarkMode;

        private readonly string defaultBottomLabelText;

        private enum ThemeSwitch
        {
            GoDark,
            GoLight,
            NoChange
        }

        public Form1()
        {
            InitializeComponent();
            colorSchemeManager = new ColorSchemeManager();
            startupManager = new StartupManager("Auto Color Scheme");

            isDarkMode = colorSchemeManager.GetDarkMode();
            defaultBottomLabelText = bottomLabel.Text;
        }

        private void AddToStartup()
        {
            try
            {
                startupManager.AddToStartup();
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#else
                File.WriteAllText("error.log", e.ToString());
                var result = MessageBox.Show($"Nie uda³o siê dodaæ aplikacji do autostartu:\n{e.Message}\n\nWiêcej informacji w pliku error.log w folderze aplikacji.", "B³¹d dodawania do autostartu", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

                if (result == DialogResult.Retry)
                {
                    AddToStartup();
                }
#endif
            }
        }

        private void RemoveFromStartup()
        {
            try
            {
                startupManager.RemoveFromStartup();
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#else
                File.WriteAllText("error.log", e.ToString());
                var result = MessageBox.Show($"Nie uda³o siê usun¹æ aplikacji z autostartu:\n{e.Message}\n\nWiêcej informacji w pliku error.log w folderze aplikacji.", "B³¹d usuwania z autostartu", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

                if (result == DialogResult.Retry)
                {
                    AddToStartup();
                }
#endif
            }
        }

        private void Form1_Activated(object? sender, EventArgs e)
        {
            Hide();
            Opacity = 1;
            Activated -= Form1_Activated;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DarkMode.StyleComboBox = false;
            DarkMode.AutoDarkModeMica(this);

            for (var i = 0; i < 24; i++)
            {
                var times = new string[] { $"{i:D2}:00", $"{i:D2}:30" };

                darkSchemeTimeChooser.Items.AddRange(times);
                lightSchemeTimeChooser.Items.AddRange(times);
            }

            darkSchemeTimeChooser.SelectedIndex = Preferences.Default.dark;
            lightSchemeTimeChooser.SelectedIndex = Preferences.Default.light;

            lightSchemeTimeChooser.SelectedIndexChanged += LightSchemeTimeChooser_SelectedIndexChanged;
            darkSchemeTimeChooser.SelectedIndexChanged += DarkSchemeTimeChooser_SelectedIndexChanged;

            SwitchSchemeIfNeeded();

            if (startupManager.KeyOpened)
            {
                autoStartCheckBox.Checked = startupManager.IsInStartup;
            }
            else
            {
                autoStartCheckBox.Enabled = false;
            }
        }

        private void DarkSchemeTimeChooser_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var self = (ComboBox)sender!;
            Preferences.Default.dark = self.SelectedIndex;
            Preferences.Default.Save();

            SwitchSchemeIfNeeded();
        }

        private void LightSchemeTimeChooser_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var self = (ComboBox)sender!;
            Preferences.Default.light = self.SelectedIndex;
            Preferences.Default.Save();

            SwitchSchemeIfNeeded();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            var self = (Form)sender;
            self.Hide();
        }

        private ThemeSwitch CalculateThemeSwitch()
        {
            var currentTimeIndex = TimeIndex.TimeToTimeIndex(DateTime.Now.TimeOfDay);
            var darkTimeIndex = Preferences.Default.dark;
            var lightTimeIndex = Preferences.Default.light;

            if (lightTimeIndex < darkTimeIndex)
            {
                return (currentTimeIndex < lightTimeIndex || currentTimeIndex >= darkTimeIndex) ? ThemeSwitch.GoDark : ThemeSwitch.GoLight;
            }
            else if (darkTimeIndex < lightTimeIndex)
            {
                return (currentTimeIndex < darkTimeIndex || currentTimeIndex >= lightTimeIndex) ? ThemeSwitch.GoLight : ThemeSwitch.GoDark;
            }

            return ThemeSwitch.NoChange;
        }

        private void ShowSwitchThemeToast(ThemeSwitch themeSwitch)
        {
            if (themeSwitch == ThemeSwitch.NoChange) return;

            var which = themeSwitch switch
            {
                ThemeSwitch.GoDark => "ciemny",
                ThemeSwitch.GoLight => "jasny",
                _ => ""
            };

            new ToastContentBuilder()
                .AddText($"Pora na {which} motyw.")
                .AddText(themeSwitch switch
                {
                    ThemeSwitch.GoDark => $"Minê³a {TimeIndex.TimeIndexToTime(Preferences.Default.dark)}, wiêc czas zgasiæ œwiat³a!",
                    ThemeSwitch.GoLight => $"Minê³a {TimeIndex.TimeIndexToTime(Preferences.Default.light)}, wiêc czas na wiêcej œwiat³a!",
                    _ => ""
                })
                .Show();
        }

        private void SwitchSchemeIfNeeded()
        {
            bottomLabel.Text = "Zmieniam motyw...";
            lightSchemeTimeChooser.Enabled = darkSchemeTimeChooser.Enabled = false;
            Application.DoEvents();  // Force UI to refresh

            var themeSwitch = CalculateThemeSwitch();

            // Check if the theme to switch to is different from the current one
            if ((themeSwitch == ThemeSwitch.GoDark && !isDarkMode) || (themeSwitch == ThemeSwitch.GoLight && isDarkMode))
            {
                ShowSwitchThemeToast(themeSwitch);

                switch (themeSwitch)
                {
                    case ThemeSwitch.GoDark:
                        colorSchemeManager.SetDarkMode(true);
                        isDarkMode = true;
                        break;
                    case ThemeSwitch.GoLight:
                        colorSchemeManager.SetDarkMode(false);
                        isDarkMode = false;
                        break;
                }
            }

            bottomLabel.Text = defaultBottomLabelText;
            lightSchemeTimeChooser.Enabled = darkSchemeTimeChooser.Enabled = true;
        }

        private void schemeChangeTimer_Tick(object sender, EventArgs e)
        {
            SwitchSchemeIfNeeded();
        }

        private void DoEverythingToFocusTheWindow()
        {
            WindowState = FormWindowState.Normal;
            BringToFront();
            TopMost = true;
            Focus();
            TopMost = false;
        }

        private void tray_Click(object sender, EventArgs e)
        {
            Show();
            DoEverythingToFocusTheWindow();
        }

        private void autoStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var self = (CheckBox)sender;

            if (self.Checked)
            {
                AddToStartup();
            }
            else
            {
                RemoveFromStartup();
            }
        }
    }
}
