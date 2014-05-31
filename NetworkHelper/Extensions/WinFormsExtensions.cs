using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NetworkHelper.Extensions
{
    public static class WinFormsExtensions
    {
        #region Public API

        public static void InvokeIfRequired<T>(this T control, Action<T> action)
            where T : Control
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => action(control)));
            }
            else
            {
                action(control);
            }
        }

        public static void AppendLine(this TextBoxBase textBox, string text)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            textBox.AppendText(text);
            textBox.AppendText(Environment.NewLine);
        }

        public static void AppendLine(this TextBoxBase textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            textBox.AppendText(Environment.NewLine);
        }

        public static void AppendText(this RichTextBox richTextBox, string text, Color color)
        {
            if (richTextBox == null)
            {
                throw new ArgumentNullException("richTextBox");
            }

            int selectionStart = richTextBox.TextLength;

            richTextBox.AppendText(text);

            int selectionEnd = richTextBox.TextLength;

            richTextBox.Select(selectionStart, selectionEnd - selectionStart);
            richTextBox.SelectionColor = color;
            richTextBox.SelectionLength = 0;
        }

        public static void AppendLine(this RichTextBox richTextBox, string text, Color color)
        {
            richTextBox.AppendText(text, color);
            richTextBox.AppendText(Environment.NewLine);
        }

        public static bool IsHorizontalScrollBarVisible(this Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            return IsScrollBarVisible(control.Handle, true);
        }

        public static bool IsVerticalScrollBarVisible(this Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            return IsScrollBarVisible(control.Handle, false);
        }

        #endregion

        #region Helper functions

        private static bool IsScrollBarVisible(IntPtr controlHandle, bool horizontal)
        {
            bool result;

            int styleConstant = horizontal ? WS_HSCROLL : WS_VSCROLL;
            result = (GetWindowLong(controlHandle, GWL_STYLE) & styleConstant) == styleConstant;

            return result;
        }

        #endregion

        #region Windows API definitions

        [DllImport("user32", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_STYLE = -16;

        private const int WS_HSCROLL = 0x100000;
        private const int WS_VSCROLL = 0x200000;

        #endregion
    }
}