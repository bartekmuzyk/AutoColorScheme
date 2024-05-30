using Microsoft.Toolkit.Uwp.Notifications;

namespace AutoColorScheme
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.ApplicationExit += Application_ApplicationExit;
            Application.Run(new Form1());
        }

        private static void Application_ApplicationExit(object? sender, EventArgs e)
        {
            ToastNotificationManagerCompat.Uninstall();
        }
    }
}