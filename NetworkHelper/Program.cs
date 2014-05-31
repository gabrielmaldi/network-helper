using System;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using NetworkHelper.Forms;
using NetworkHelper.Utilities;

namespace NetworkHelper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        private static void Main()
        {
            SingleApplicationHelper.EnsureSingleInstance(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.ApplicationExit += Application_ApplicationExit;
                Application.ThreadException += Application_ThreadException;
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                Application.Run(new MainForm());
            });
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Logger.Instance.Log(LogLevel.Info, "NetworkHelper exited.");
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                Logger.Instance.Log(LogLevel.Fatal, "UI thread unhandled exception: ", e.Exception);
                Logger.Instance.Log(LogLevel.Info, "NetworkHelper exited.");
            }
            finally
            {
                Environment.Exit(1);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Logger.Instance.Log(LogLevel.Fatal, "Non-UI thread unhandled exception: ", (Exception)e.ExceptionObject);
                Logger.Instance.Log(LogLevel.Info, "NetworkHelper exited.");
            }
            finally
            {
                Environment.Exit(1);
            }
        }
    }
}