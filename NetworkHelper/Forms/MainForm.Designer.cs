namespace NetworkHelper.Forms
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.manuallyAddRoutesTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.manuallyRemoveRoutesTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleLogVisibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.autoStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BackColor = System.Drawing.Color.White;
            this.richTextBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Font = new System.Drawing.Font("Arial", 14.25F);
            this.richTextBoxLog.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(942, 423);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.ContextMenuStrip = this.contextMenuStripMain;
            this.notifyIconMain.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconMain.Icon")));
            this.notifyIconMain.Text = "NetworkHelper";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIconMain_MouseUp);
            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manuallyAddRoutesTemplateToolStripMenuItem,
            this.toolStripSeparator1,
            this.manuallyRemoveRoutesTemplateToolStripMenuItem,
            this.toolStripSeparator2,
            this.toggleLogVisibilityToolStripMenuItem,
            this.toolStripSeparator3,
            this.autoStartToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.contextMenuStripMain.Name = "contextMenuStrip1";
            this.contextMenuStripMain.Size = new System.Drawing.Size(225, 138);
            this.contextMenuStripMain.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripMain_Opening);
            this.contextMenuStripMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripMain_ItemClicked);
            // 
            // manuallyAddRoutesTemplateToolStripMenuItem
            // 
            this.manuallyAddRoutesTemplateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("manuallyAddRoutesTemplateToolStripMenuItem.Image")));
            this.manuallyAddRoutesTemplateToolStripMenuItem.Name = "manuallyAddRoutesTemplateToolStripMenuItem";
            this.manuallyAddRoutesTemplateToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.manuallyAddRoutesTemplateToolStripMenuItem.Text = "Add routes for \"VPN Name\"";
            this.manuallyAddRoutesTemplateToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
            this.toolStripSeparator1.Visible = false;
            // 
            // manuallyRemoveRoutesTemplateToolStripMenuItem
            // 
            this.manuallyRemoveRoutesTemplateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("manuallyRemoveRoutesTemplateToolStripMenuItem.Image")));
            this.manuallyRemoveRoutesTemplateToolStripMenuItem.Name = "manuallyRemoveRoutesTemplateToolStripMenuItem";
            this.manuallyRemoveRoutesTemplateToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.manuallyRemoveRoutesTemplateToolStripMenuItem.Text = "Remove routes for \"VPN Name\"";
            this.manuallyRemoveRoutesTemplateToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(221, 6);
            this.toolStripSeparator2.Visible = false;
            // 
            // toggleLogVisibilityToolStripMenuItem
            // 
            this.toggleLogVisibilityToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.toggleLogVisibilityToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("toggleLogVisibilityToolStripMenuItem.Image")));
            this.toggleLogVisibilityToolStripMenuItem.Name = "toggleLogVisibilityToolStripMenuItem";
            this.toggleLogVisibilityToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.toggleLogVisibilityToolStripMenuItem.Tag = "";
            this.toggleLogVisibilityToolStripMenuItem.Text = "Show Log";
            this.toggleLogVisibilityToolStripMenuItem.Click += new System.EventHandler(this.toggleLogVisibilityToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(221, 6);
            // 
            // autoStartToolStripMenuItem
            // 
            this.autoStartToolStripMenuItem.CheckOnClick = true;
            this.autoStartToolStripMenuItem.Name = "autoStartToolStripMenuItem";
            this.autoStartToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.autoStartToolStripMenuItem.Text = "Start at log on";
            this.autoStartToolStripMenuItem.CheckedChanged += new System.EventHandler(this.autoStartToolStripMenuItem_CheckedChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(221, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(942, 423);
            this.Controls.Add(this.richTextBoxLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "NetworkHelper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.contextMenuStripMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem toggleLogVisibilityToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem autoStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manuallyAddRoutesTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manuallyRemoveRoutesTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    }
}

