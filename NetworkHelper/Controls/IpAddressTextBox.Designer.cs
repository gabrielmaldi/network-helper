namespace NetworkHelper.Controls
{
    partial class IpAddressTextBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.textBoxOctet4 = new System.Windows.Forms.TextBox();
            this.labelOctetSeparator3 = new System.Windows.Forms.Label();
            this.textBoxOctet3 = new System.Windows.Forms.TextBox();
            this.labelOctetSeparator2 = new System.Windows.Forms.Label();
            this.textBoxOctet2 = new System.Windows.Forms.TextBox();
            this.labelOctetSeparator1 = new System.Windows.Forms.Label();
            this.textBoxOctet1 = new System.Windows.Forms.TextBox();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.SystemColors.Window;
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Controls.Add(this.textBoxOctet4);
            this.panelMain.Controls.Add(this.labelOctetSeparator3);
            this.panelMain.Controls.Add(this.textBoxOctet3);
            this.panelMain.Controls.Add(this.labelOctetSeparator2);
            this.panelMain.Controls.Add(this.textBoxOctet2);
            this.panelMain.Controls.Add(this.labelOctetSeparator1);
            this.panelMain.Controls.Add(this.textBoxOctet1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(128, 18);
            this.panelMain.TabIndex = 0;
            this.panelMain.EnabledChanged += new System.EventHandler(this.Controls_EnabledChanged);
            // 
            // textBoxOctet4
            // 
            this.textBoxOctet4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOctet4.Location = new System.Drawing.Point(100, 0);
            this.textBoxOctet4.MaxLength = 3;
            this.textBoxOctet4.Name = "textBoxOctet4";
            this.textBoxOctet4.Size = new System.Drawing.Size(20, 13);
            this.textBoxOctet4.TabIndex = 7;
            this.textBoxOctet4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxOctet4.Enter += new System.EventHandler(this.TextBoxesOctets_Enter);
            this.textBoxOctet4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxOctet4_KeyDown);
            this.textBoxOctet4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxOctet4_KeyPress);
            this.textBoxOctet4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxesOctets_KeyUp);
            // 
            // labelOctetSeparator3
            // 
            this.labelOctetSeparator3.Location = new System.Drawing.Point(88, 0);
            this.labelOctetSeparator3.Name = "labelOctetSeparator3";
            this.labelOctetSeparator3.Size = new System.Drawing.Size(8, 13);
            this.labelOctetSeparator3.TabIndex = 6;
            this.labelOctetSeparator3.Text = ".";
            this.labelOctetSeparator3.EnabledChanged += new System.EventHandler(this.Controls_EnabledChanged);
            // 
            // textBoxOctet3
            // 
            this.textBoxOctet3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOctet3.Location = new System.Drawing.Point(64, 0);
            this.textBoxOctet3.MaxLength = 3;
            this.textBoxOctet3.Name = "textBoxOctet3";
            this.textBoxOctet3.Size = new System.Drawing.Size(20, 13);
            this.textBoxOctet3.TabIndex = 5;
            this.textBoxOctet3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxOctet3.Enter += new System.EventHandler(this.TextBoxesOctets_Enter);
            this.textBoxOctet3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxOctet3_KeyDown);
            this.textBoxOctet3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxOctet3_KeyPress);
            this.textBoxOctet3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxesOctets_KeyUp);
            // 
            // labelOctetSeparator2
            // 
            this.labelOctetSeparator2.Location = new System.Drawing.Point(56, 0);
            this.labelOctetSeparator2.Name = "labelOctetSeparator2";
            this.labelOctetSeparator2.Size = new System.Drawing.Size(8, 13);
            this.labelOctetSeparator2.TabIndex = 4;
            this.labelOctetSeparator2.Text = ".";
            this.labelOctetSeparator2.EnabledChanged += new System.EventHandler(this.Controls_EnabledChanged);
            // 
            // textBoxOctet2
            // 
            this.textBoxOctet2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOctet2.Location = new System.Drawing.Point(32, 0);
            this.textBoxOctet2.MaxLength = 3;
            this.textBoxOctet2.Name = "textBoxOctet2";
            this.textBoxOctet2.Size = new System.Drawing.Size(20, 13);
            this.textBoxOctet2.TabIndex = 3;
            this.textBoxOctet2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxOctet2.Enter += new System.EventHandler(this.TextBoxesOctets_Enter);
            this.textBoxOctet2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxOctet2_KeyDown);
            this.textBoxOctet2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxOctet2_KeyPress);
            this.textBoxOctet2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxesOctets_KeyUp);
            // 
            // labelOctetSeparator1
            // 
            this.labelOctetSeparator1.Location = new System.Drawing.Point(24, 0);
            this.labelOctetSeparator1.Name = "labelOctetSeparator1";
            this.labelOctetSeparator1.Size = new System.Drawing.Size(8, 13);
            this.labelOctetSeparator1.TabIndex = 2;
            this.labelOctetSeparator1.Text = ".";
            this.labelOctetSeparator1.EnabledChanged += new System.EventHandler(this.Controls_EnabledChanged);
            // 
            // textBoxOctet1
            // 
            this.textBoxOctet1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOctet1.Location = new System.Drawing.Point(4, 0);
            this.textBoxOctet1.MaxLength = 3;
            this.textBoxOctet1.Name = "textBoxOctet1";
            this.textBoxOctet1.Size = new System.Drawing.Size(20, 13);
            this.textBoxOctet1.TabIndex = 1;
            this.textBoxOctet1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxOctet1.Enter += new System.EventHandler(this.TextBoxesOctets_Enter);
            this.textBoxOctet1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxOctet1_KeyDown);
            this.textBoxOctet1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxOctet1_KeyPress);
            this.textBoxOctet1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxesOctets_KeyUp);
            // 
            // IpAddressTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "IpAddressTextBox";
            this.Size = new System.Drawing.Size(128, 18);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TextBox textBoxOctet1;
        private System.Windows.Forms.Label labelOctetSeparator1;
        private System.Windows.Forms.TextBox textBoxOctet2;
        private System.Windows.Forms.Label labelOctetSeparator2;
        private System.Windows.Forms.TextBox textBoxOctet3;
        private System.Windows.Forms.Label labelOctetSeparator3;
        private System.Windows.Forms.TextBox textBoxOctet4;
    }
}
