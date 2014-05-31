using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using NetworkHelper.Classes;
using NetworkHelper.Controls;
using NetworkHelper.Extensions;

namespace NetworkHelper.Forms
{
    public partial class IpAddressInputForm : Form
    {
        #region Instance variables

        private Vpn _vpn;
        private bool _wasAlreadyActivated;
        private bool _isRichTextBoxInformationBeingResizedManually;
        private int _lastRichTextBoxInformationHeight;
        private bool _lastRichTextBoxInformationVerticalScrollBarIsVisible;

        #endregion

        #region Public instance properties

        public event EventHandler<IpAddressInputFormCompletedEventArgs> Completed;

        public string IpAddress { get { return gatewayIpAddressTextBox.Text; } }

        #endregion

        #region Form and controls methods and events

        public IpAddressInputForm(Vpn vpn)
        {
            _vpn = vpn;

            InitializeComponent();
        }

        private void IpAddressInputForm_Load(object sender, EventArgs e)
        {
            Text = string.Format(CultureInfo.InvariantCulture, "Add routes for \"{0}\"", _vpn.Name);
            
            richTextBoxInformation.Text = string.Format(CultureInfo.InvariantCulture, "Enter the gateway of \"{0}\".", _vpn.Name);
            richTextBoxInformation.AppendLine();
            richTextBoxInformation.AppendLine();
            richTextBoxInformation.AppendLine("The following routes will be added:");
            richTextBoxInformation.AppendLine();
            
            foreach (Route route in _vpn.Routes.Where(route => route.IsValid && route.IsEnabled))
            {
                richTextBoxInformation.AppendLine(route.ToString());
            }
        }

        private void IpAddressInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnCompleted();
        }

        private void IpAddressInputForm_Activated(object sender, EventArgs e)
        {
            if (!_wasAlreadyActivated)
            {
                CheckInformationImagePosition();
                gatewayIpAddressTextBox.FocusedOctet = 3;

                _wasAlreadyActivated = true;
            }
        }

        private void richTextBoxInformation_Resize(object sender, EventArgs e)
        {
            CheckInformationImagePosition();

            #region Fix RichTextBox not updating vertical ScrollBar when shrank vertically

            Size size = richTextBoxInformation.Size;

            if (!_isRichTextBoxInformationBeingResizedManually)
            {
                if (_lastRichTextBoxInformationHeight == 0 || _lastRichTextBoxInformationHeight > size.Height)
                {
                    _isRichTextBoxInformationBeingResizedManually = true;

                    size.Height--;
                    richTextBoxInformation.Size = size;
                    size.Height++;
                    richTextBoxInformation.Size = size;

                    _isRichTextBoxInformationBeingResizedManually = false;
                }
            }

            _lastRichTextBoxInformationHeight = size.Height;

            #endregion
        }

        private void gatewayIpAddressTextBox_IsValidChanged(object sender, IpAddressTextBoxIsValidChangedEventArgs e)
        {
            buttonOk.Enabled = e.IsValid;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion

        #region Instance helper methods

        private void OnCompleted()
        {
            EventHandler<IpAddressInputFormCompletedEventArgs> completed = Completed;
            if (completed != null)
            {
                completed(this, new IpAddressInputFormCompletedEventArgs(DialogResult, IpAddress));
            }
        }

        private void CheckInformationImagePosition()
        {
            bool richTextBoxInformationVerticalScrollBarIsVisible = richTextBoxInformation.IsVerticalScrollBarVisible();
            if (richTextBoxInformationVerticalScrollBarIsVisible != _lastRichTextBoxInformationVerticalScrollBarIsVisible)
            {
                _lastRichTextBoxInformationVerticalScrollBarIsVisible = richTextBoxInformationVerticalScrollBarIsVisible;
                pictureBoxInformation.Left = richTextBoxInformation.Right - (richTextBoxInformationVerticalScrollBarIsVisible ? 82 : 66);
            }
        }

        #endregion
    }

    public sealed class IpAddressInputFormCompletedEventArgs : EventArgs
    {
        public DialogResult DialogResult { get; private set; }
        public string IpAddress { get; private set; }

        public IpAddressInputFormCompletedEventArgs(DialogResult dialogResult, string ipAddress)
        {
            DialogResult = dialogResult;
            IpAddress = ipAddress;
        }
    }
}