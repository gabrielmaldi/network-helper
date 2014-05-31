using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace NetworkHelper.Utilities
{
    public static class SingleApplicationHelper
    {
        #region Constants

        private static readonly Mutex PerUserMutex;
        public static readonly uint WM_NEWINSTANCE;

        static SingleApplicationHelper()
        {
            PerUserMutex = new Mutex(true, GetUniqueIdentifier(WindowsIdentity.GetCurrent().User.Value));
            WM_NEWINSTANCE = RegisterWindowMessage(GetUniqueIdentifier("WM_NEWINSTANCE"));
        }

        #endregion

        #region Public API

        public static void EnsureSingleInstance(Action onSingleInstance)
        {
            if (PerUserMutex.WaitOne(TimeSpan.Zero, true))
            {
                try
                {
                    onSingleInstance();
                }
                finally
                {
                    PerUserMutex.ReleaseMutex();
                }
            }
            else
            {
                PostMessage(HWND_BROADCAST, WM_NEWINSTANCE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        #endregion

        #region Helper functions

        private static string GetUniqueIdentifier(string suffix)
        {
            return string.Format(CultureInfo.InvariantCulture, "NetworkHelper_{{67C1D1B7-1CAA-4227-AB7C-361C8B6501B3}}_{0}", suffix);
        }

        #endregion

        #region Windows API definitions

        private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xFFFF);
        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint RegisterWindowMessage(string lpString);

        #endregion
    }
}