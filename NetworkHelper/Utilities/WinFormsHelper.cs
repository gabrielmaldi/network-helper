using System;
using System.IO;
using System.Windows.Forms;

namespace NetworkHelper.Utilities
{
    public class WinFormsHelper
    {
        private static Lazy<string> _ExecutableDirectory = new Lazy<string>(() => Path.GetDirectoryName(Application.ExecutablePath));

        public static string ExecutableDirectory
        {
            get
            {
                return _ExecutableDirectory.Value;
            }
        }
    }
}