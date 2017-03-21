namespace WindowsVpnMonitor
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.labelIp = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelDns = new System.Windows.Forms.Label();
            this.checkBoxViscosity = new System.Windows.Forms.CheckBox();
            this.checkBoxClient = new System.Windows.Forms.CheckBox();
            this.buttonChrome = new System.Windows.Forms.Button();
            this.buttonLog = new System.Windows.Forms.Button();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.checkBoxAutoScroll = new System.Windows.Forms.CheckBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.buttonEditConfig = new System.Windows.Forms.Button();
            this.buttonLoadConfig = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current IP Address";
            // 
            // labelIp
            // 
            this.labelIp.AutoSize = true;
            this.labelIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIp.Location = new System.Drawing.Point(9, 33);
            this.labelIp.Name = "labelIp";
            this.labelIp.Size = new System.Drawing.Size(45, 13);
            this.labelIp.TabIndex = 1;
            this.labelIp.Text = "labelIp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "DNS IP Address";
            // 
            // labelDns
            // 
            this.labelDns.AutoSize = true;
            this.labelDns.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDns.Location = new System.Drawing.Point(9, 80);
            this.labelDns.Name = "labelDns";
            this.labelDns.Size = new System.Drawing.Size(56, 13);
            this.labelDns.TabIndex = 3;
            this.labelDns.Text = "labelDns";
            // 
            // checkBoxViscosity
            // 
            this.checkBoxViscosity.AutoSize = true;
            this.checkBoxViscosity.Location = new System.Drawing.Point(11, 108);
            this.checkBoxViscosity.Name = "checkBoxViscosity";
            this.checkBoxViscosity.Size = new System.Drawing.Size(85, 17);
            this.checkBoxViscosity.TabIndex = 4;
            this.checkBoxViscosity.Text = "VPN monitor";
            this.checkBoxViscosity.UseVisualStyleBackColor = true;
            this.checkBoxViscosity.CheckedChanged += new System.EventHandler(this.checkBoxViscosity_CheckedChanged);
            // 
            // checkBoxUtorrent
            // 
            this.checkBoxClient.AutoSize = true;
            this.checkBoxClient.Location = new System.Drawing.Point(11, 131);
            this.checkBoxClient.Name = "checkBoxUtorrent";
            this.checkBoxClient.Size = new System.Drawing.Size(89, 17);
            this.checkBoxClient.TabIndex = 5;
            this.checkBoxClient.Text = "Client monitor";
            this.checkBoxClient.UseVisualStyleBackColor = true;
            this.checkBoxClient.CheckedChanged += new System.EventHandler(this.checkBoxClient_CheckedChanged);
            // 
            // buttonChrome
            // 
            this.buttonChrome.Location = new System.Drawing.Point(11, 172);
            this.buttonChrome.Name = "buttonChrome";
            this.buttonChrome.Size = new System.Drawing.Size(120, 23);
            this.buttonChrome.TabIndex = 6;
            this.buttonChrome.Text = "Browser as Admin";
            this.buttonChrome.UseVisualStyleBackColor = true;
            this.buttonChrome.Click += new System.EventHandler(this.buttonChrome_Click);
            // 
            // buttonLog
            // 
            this.buttonLog.Location = new System.Drawing.Point(145, 230);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(98, 23);
            this.buttonLog.TabIndex = 7;
            this.buttonLog.Text = "Show Log File";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
            // 
            // listBoxLog
            // 
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.Location = new System.Drawing.Point(145, 12);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(524, 212);
            this.listBoxLog.TabIndex = 8;
            // 
            // checkBoxAutoScroll
            // 
            this.checkBoxAutoScroll.AutoSize = true;
            this.checkBoxAutoScroll.Location = new System.Drawing.Point(580, 234);
            this.checkBoxAutoScroll.Name = "checkBoxAutoScroll";
            this.checkBoxAutoScroll.Size = new System.Drawing.Size(89, 17);
            this.checkBoxAutoScroll.TabIndex = 9;
            this.checkBoxAutoScroll.Text = "Autoscroll log";
            this.checkBoxAutoScroll.UseVisualStyleBackColor = true;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Location = new System.Drawing.Point(258, 230);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(98, 23);
            this.buttonClearLog.TabIndex = 10;
            this.buttonClearLog.Text = "Clear log window";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // buttonEditConfig
            // 
            this.buttonEditConfig.Location = new System.Drawing.Point(11, 201);
            this.buttonEditConfig.Name = "buttonEditConfig";
            this.buttonEditConfig.Size = new System.Drawing.Size(120, 23);
            this.buttonEditConfig.TabIndex = 11;
            this.buttonEditConfig.Text = "Edit Config File";
            this.buttonEditConfig.UseVisualStyleBackColor = true;
            this.buttonEditConfig.Click += new System.EventHandler(this.buttonEditConfig_Click);
            // 
            // buttonLoadConfig
            // 
            this.buttonLoadConfig.Location = new System.Drawing.Point(12, 230);
            this.buttonLoadConfig.Name = "buttonLoadConfig";
            this.buttonLoadConfig.Size = new System.Drawing.Size(119, 23);
            this.buttonLoadConfig.TabIndex = 12;
            this.buttonLoadConfig.Text = "Load New Config";
            this.buttonLoadConfig.UseVisualStyleBackColor = true;
            this.buttonLoadConfig.Click += new System.EventHandler(this.buttonLoadConfig_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 262);
            this.Controls.Add(this.buttonLoadConfig);
            this.Controls.Add(this.buttonEditConfig);
            this.Controls.Add(this.buttonClearLog);
            this.Controls.Add(this.checkBoxAutoScroll);
            this.Controls.Add(this.listBoxLog);
            this.Controls.Add(this.buttonLog);
            this.Controls.Add(this.buttonChrome);
            this.Controls.Add(this.checkBoxClient);
            this.Controls.Add(this.checkBoxViscosity);
            this.Controls.Add(this.labelDns);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelIp);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Windows VPN Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelIp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelDns;
        private System.Windows.Forms.CheckBox checkBoxViscosity;
        private System.Windows.Forms.CheckBox checkBoxClient;
        private System.Windows.Forms.Button buttonChrome;
        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.CheckBox checkBoxAutoScroll;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.Button buttonEditConfig;
        private System.Windows.Forms.Button buttonLoadConfig;
    }
}

