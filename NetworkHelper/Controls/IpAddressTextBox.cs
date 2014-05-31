using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using NetworkHelper.Utilities;

namespace NetworkHelper.Controls
{
    public partial class IpAddressTextBox : UserControl
    {
        #region Instance variables

        private bool _lastIsValid;

        #endregion

        #region Public instance properties

        public event EventHandler<IpAddressTextBoxIsValidChangedEventArgs> IsValidChanged;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", textBoxOctet1.Text, textBoxOctet2.Text, textBoxOctet3.Text, textBoxOctet4.Text);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string[] octets = new string[4];

                    string[] parsedOctets = value.Split(new char[] { '.' }, 4);
                    for (int i = 0; i < parsedOctets.Length; i++)
                    {
                        octets[i] = parsedOctets[i];
                    }

                    textBoxOctet1.Text = octets[0];
                    textBoxOctet2.Text = octets[1];
                    textBoxOctet3.Text = octets[2];
                    textBoxOctet4.Text = octets[3];
                }
                else
                {
                    textBoxOctet1.Text = string.Empty;
                    textBoxOctet2.Text = string.Empty;
                    textBoxOctet3.Text = string.Empty;
                    textBoxOctet4.Text = string.Empty;
                }

                CheckIsValidChanged();
            }
        }

        [Browsable(false)]
        public bool IsValid
        {
            get
            {
                return IpAddressHelper.IsIpAddressValid(Text);
            }
        }

        [Browsable(false)]
        public int FocusedOctet
        {
            get
            {
                int result;

                if (textBoxOctet1.Focused)
                {
                    result = 1;
                }
                else if (textBoxOctet2.Focused)
                {
                    result = 2;
                }
                else if (textBoxOctet3.Focused)
                {
                    result = 3;
                }
                else if (textBoxOctet4.Focused)
                {
                    result = 4;
                }
                else
                {
                    result = 0;
                }

                return result;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        textBoxOctet1.Focus();
                        break;
                    case 2:
                        textBoxOctet2.Focus();
                        break;
                    case 3:
                        textBoxOctet3.Focus();
                        break;
                    case 4:
                        textBoxOctet4.Focus();
                        break;
                }
            }
        }

        #endregion

        #region Form and controls methods and events

        public IpAddressTextBox()
        {
            InitializeComponent();
        }

        private void Controls_EnabledChanged(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            control.BackColor = control.Enabled ? SystemColors.Window : SystemColors.Control;
        }

        private void TextBoxesOctets_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void textBoxOctet1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = ProcessKey(null, textBoxOctet1, textBoxOctet2, e.KeyCode);
        }

        private void textBoxOctet1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = ProcessKey(null, textBoxOctet1, textBoxOctet2, e.KeyChar);
        }

        private void textBoxOctet2_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = ProcessKey(textBoxOctet1, textBoxOctet2, textBoxOctet3, e.KeyCode);
        }

        private void textBoxOctet2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = ProcessKey(textBoxOctet1, textBoxOctet2, textBoxOctet3, e.KeyChar);
        }

        private void textBoxOctet3_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = ProcessKey(textBoxOctet2, textBoxOctet3, textBoxOctet4, e.KeyCode);
        }

        private void textBoxOctet3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = ProcessKey(textBoxOctet2, textBoxOctet3, textBoxOctet4, e.KeyChar);
        }

        private void textBoxOctet4_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = ProcessKey(textBoxOctet3, textBoxOctet4, null, e.KeyCode);
        }

        private void textBoxOctet4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = ProcessKey(textBoxOctet3, textBoxOctet4, null, e.KeyChar);
        }

        private void TextBoxesOctets_KeyUp(object sender, KeyEventArgs e)
        {
            CheckIsValidChanged();
        }

        #endregion

        #region Instance helper methods

        private void CheckIsValidChanged()
        {
            bool lastIsValid = _lastIsValid;
            _lastIsValid = IsValid;
            if (lastIsValid != _lastIsValid)
            {
                OnIsValidChanged();
            }
        }

        private void OnIsValidChanged()
        {
            EventHandler<IpAddressTextBoxIsValidChangedEventArgs> isValidChanged = IsValidChanged;
            if (isValidChanged != null)
            {
                isValidChanged(this, new IpAddressTextBoxIsValidChangedEventArgs(_lastIsValid));
            }
        }

        #endregion

        #region Class helper methods

        private static bool ProcessKey(TextBox previousTextBoxOctet, TextBox currentTextBoxOctet, TextBox nextTextBoxOctet, Keys keyCode)
        {
            bool result = false;

            switch (keyCode)
            {
                case Keys.Left:
                    if (previousTextBoxOctet != null && currentTextBoxOctet.SelectionLength == 0 && currentTextBoxOctet.SelectionStart == 0)
                    {
                        previousTextBoxOctet.Focus();

                        result = true;
                    }
                    break;
                case Keys.Right:
                    if (nextTextBoxOctet != null && currentTextBoxOctet.SelectionLength == 0 && currentTextBoxOctet.SelectionStart == currentTextBoxOctet.Text.Length)
                    {
                        nextTextBoxOctet.Focus();

                        result = true;
                    }
                    break;
            }

            return result;
        }

        private static bool ProcessKey(TextBox previousTextBoxOctet, TextBox currentTextBoxOctet, TextBox nextTextBoxOctet, char keyChar)
        {
            bool result = false;

            if ((nextTextBoxOctet != null && keyChar == '.') || char.IsDigit(keyChar) || keyChar == (int)Keys.Back)
            {
                if (keyChar == '.')
                {
                    if (!string.IsNullOrEmpty(currentTextBoxOctet.Text) && currentTextBoxOctet.Text.Length != currentTextBoxOctet.SelectionLength)
                    {
                        if (IsOctetValid(currentTextBoxOctet.Text))
                        {
                            nextTextBoxOctet.Focus();
                        }
                        else
                        {
                            currentTextBoxOctet.SelectAll();
                        }
                    }

                    result = true;
                }
                else if (currentTextBoxOctet.SelectionLength != currentTextBoxOctet.Text.Length)
                {
                    if (currentTextBoxOctet.Text.Length == 2)
                    {
                        if (keyChar == (int)Keys.Back)
                        {
                            currentTextBoxOctet.Text.Remove(currentTextBoxOctet.Text.Length - 1, 1);
                        }
                        else if (!IsOctetValid(currentTextBoxOctet.Text + keyChar))
                        {
                            currentTextBoxOctet.SelectAll();

                            result = true;
                        }
                        else if (nextTextBoxOctet != null)
                        {
                            nextTextBoxOctet.Focus();
                        }
                    }
                }
                else if (previousTextBoxOctet != null && currentTextBoxOctet.Text.Length == 0 && keyChar == (int)Keys.Back)
                {
                    previousTextBoxOctet.Focus();
                    previousTextBoxOctet.SelectionStart = previousTextBoxOctet.Text.Length;
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        private static bool IsOctetValid(string octet)
        {
            int parsedOctet;
            return int.TryParse(octet, out parsedOctet) && parsedOctet >= 0 && parsedOctet <= 255;
        }

        #endregion
    }

    public sealed class IpAddressTextBoxIsValidChangedEventArgs : EventArgs
    {
        public bool IsValid { get; private set; }

        public IpAddressTextBoxIsValidChangedEventArgs(bool isValid)
        {
            IsValid = isValid;
        }
    }
}