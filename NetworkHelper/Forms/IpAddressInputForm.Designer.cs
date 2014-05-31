namespace NetworkHelper.Forms
{
    partial class IpAddressInputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IpAddressInputForm));
            this.richTextBoxInformation = new System.Windows.Forms.RichTextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.pictureBoxInformation = new System.Windows.Forms.PictureBox();
            this.panelInformationContainer = new System.Windows.Forms.Panel();
            this.gatewayIpAddressTextBox = new NetworkHelper.Controls.IpAddressTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInformation)).BeginInit();
            this.panelInformationContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxInformation
            // 
            this.richTextBoxInformation.BackColor = System.Drawing.Color.White;
            this.richTextBoxInformation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxInformation.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxInformation.ForeColor = System.Drawing.SystemColors.WindowText;
            this.richTextBoxInformation.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxInformation.Name = "richTextBoxInformation";
            this.richTextBoxInformation.ReadOnly = true;
            this.richTextBoxInformation.Size = new System.Drawing.Size(584, 227);
            this.richTextBoxInformation.TabIndex = 0;
            this.richTextBoxInformation.TabStop = false;
            this.richTextBoxInformation.Text = resources.GetString("richTextBoxInformation.Text");
            this.richTextBoxInformation.Resize += new System.EventHandler(this.richTextBoxInformation_Resize);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Enabled = false;
            this.buttonOk.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOk.Image = ((System.Drawing.Image)(resources.GetObject("buttonOk.Image")));
            this.buttonOk.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonOk.Location = new System.Drawing.Point(264, 240);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(148, 48);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Add routes";
            this.buttonOk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("Arial", 14.25F);
            this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCancel.Location = new System.Drawing.Point(424, 240);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(148, 48);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // pictureBoxInformation
            // 
            this.pictureBoxInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxInformation.BackColor = System.Drawing.Color.White;
            this.pictureBoxInformation.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxInformation.Image")));
            this.pictureBoxInformation.Location = new System.Drawing.Point(518, 3);
            this.pictureBoxInformation.Name = "pictureBoxInformation";
            this.pictureBoxInformation.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxInformation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxInformation.TabIndex = 5;
            this.pictureBoxInformation.TabStop = false;
            // 
            // panelInformationContainer
            // 
            this.panelInformationContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInformationContainer.BackColor = System.Drawing.Color.White;
            this.panelInformationContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInformationContainer.Controls.Add(this.pictureBoxInformation);
            this.panelInformationContainer.Controls.Add(this.richTextBoxInformation);
            this.panelInformationContainer.Location = new System.Drawing.Point(-1, -1);
            this.panelInformationContainer.Name = "panelInformationContainer";
            this.panelInformationContainer.Size = new System.Drawing.Size(586, 229);
            this.panelInformationContainer.TabIndex = 6;
            // 
            // gatewayIpAddressTextBox
            // 
            this.gatewayIpAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gatewayIpAddressTextBox.FocusedOctet = 0;
            this.gatewayIpAddressTextBox.Font = new System.Drawing.Font("Arial", 14.25F);
            this.gatewayIpAddressTextBox.Location = new System.Drawing.Point(12, 252);
            this.gatewayIpAddressTextBox.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.gatewayIpAddressTextBox.Name = "gatewayIpAddressTextBox";
            this.gatewayIpAddressTextBox.Size = new System.Drawing.Size(232, 24);
            this.gatewayIpAddressTextBox.TabIndex = 4;
            this.gatewayIpAddressTextBox.Text = "192.168..";
            this.gatewayIpAddressTextBox.IsValidChanged += new System.EventHandler<NetworkHelper.Controls.IpAddressTextBoxIsValidChangedEventArgs>(this.gatewayIpAddressTextBox_IsValidChanged);
            // 
            // IpAddressInputForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(584, 301);
            this.Controls.Add(this.panelInformationContainer);
            this.Controls.Add(this.gatewayIpAddressTextBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(592, 328);
            this.Name = "IpAddressInputForm";
            this.Text = "Add routes for \"VPN Name\"";
            this.Activated += new System.EventHandler(this.IpAddressInputForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IpAddressInputForm_FormClosing);
            this.Load += new System.EventHandler(this.IpAddressInputForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInformation)).EndInit();
            this.panelInformationContainer.ResumeLayout(false);
            this.panelInformationContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxInformation;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private Controls.IpAddressTextBox gatewayIpAddressTextBox;
        private System.Windows.Forms.PictureBox pictureBoxInformation;
        private System.Windows.Forms.Panel panelInformationContainer;


    }
}