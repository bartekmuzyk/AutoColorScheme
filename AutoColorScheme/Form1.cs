using System.Diagnostics;
using WinFormsDarkMode;

namespace AutoColorScheme
{
    public partial class Form1 : Form
    {
        private readonly ColorSchemeManager colorSchemeManager;

        private bool isDarkMode = false;

        private readonly string defaultBottomLabelText;

        public Form1()
        {
            InitializeComponent();
            colorSchemeManager = new ColorSchemeManager();
            defaultBottomLabelText = bottomLabel.Text;
        }

        private string padZero(int num)
        {
            return num < 10 ? $"0{num}" : num.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DarkMode.StyleComboBox = false;
            DarkMode.AutoDarkModeMica(this);

            for (var i = 0; i < 24; i++)
            {
                var padded = padZero(i);
                var full = $"{padded}:00";
                var half = $"{padded}:30";

                darkSchemeTimeChooser.Items.Add(full);
                darkSchemeTimeChooser.Items.Add(half);
                lightSchemeTimeChooser.Items.Add(full);
                lightSchemeTimeChooser.Items.Add(half);
            }

            var darkTimeIndex = Preferences.Default.dark;
            var lightTimeIndex = Preferences.Default.light;

            darkSchemeTimeChooser.SelectedIndex = darkTimeIndex;
            lightSchemeTimeChooser.SelectedIndex = lightTimeIndex;

            SwitchSchemeIfNeeded();

            lightSchemeTimeChooser.SelectedIndexChanged += LightSchemeTimeChooser_SelectedIndexChanged;
            darkSchemeTimeChooser.SelectedIndexChanged += DarkSchemeTimeChooser_SelectedIndexChanged;
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

        private int GetCurrentTimeIndex()
        {
            var now = DateTime.Now.TimeOfDay;
            return now.Hours * 2 + (now.Minutes >= 30 ? 1 : 0);
        }

        private void SwitchSchemeIfNeeded()
        {
            var currentTimeIndex = GetCurrentTimeIndex();
            var darkTimeIndex = Preferences.Default.dark;
            var lightTimeIndex = Preferences.Default.light;

#if DEBUG
            Debug.WriteLine($"isDarkMode: {isDarkMode}, currentTimeIndex: {currentTimeIndex}, darkTimeIndex: {darkTimeIndex}, lightTimeIndex: {lightTimeIndex}");
#endif

            bottomLabel.Text = "Zmieniam motyw...";
            lightSchemeTimeChooser.Enabled = darkSchemeTimeChooser.Enabled = false;
            Application.DoEvents();
            if (lightTimeIndex < darkTimeIndex)
            {
                if (currentTimeIndex < lightTimeIndex || currentTimeIndex >= darkTimeIndex)
                {
                    if (!isDarkMode)
                    {
                        colorSchemeManager.SetDarkMode(true);
                        isDarkMode = true;
                    }
                }
                else
                {
                    if (isDarkMode)
                    {
                        colorSchemeManager.SetDarkMode(false);
                        isDarkMode = false;
                    }
                }
            }
            else if (darkTimeIndex < lightTimeIndex)
            {
                if (currentTimeIndex < darkTimeIndex || currentTimeIndex >= lightTimeIndex)
                {
                    if (isDarkMode)
                    {
                        colorSchemeManager.SetDarkMode(false);
                        isDarkMode = false;
                    }
                }
                else
                {
                    if (!isDarkMode)
                    {
                        colorSchemeManager.SetDarkMode(true);
                        isDarkMode = true;
                    }
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

        private void Form1_Activated(object? sender, EventArgs e)
        {
            Hide();
            Opacity = 1;
            Activated -= Form1_Activated;
        }
    }
}
