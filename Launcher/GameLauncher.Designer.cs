namespace Launcher
{
    partial class GameLauncher
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
            this.lbl_launchType = new System.Windows.Forms.Label();
            this.cmb_launchType = new System.Windows.Forms.ComboBox();
            this.grp_clientProperties = new System.Windows.Forms.GroupBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.lbl_port = new System.Windows.Forms.Label();
            this.txt_ip = new System.Windows.Forms.MaskedTextBox();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.pnl_colorPicker = new System.Windows.Forms.Panel();
            this.lbl_color = new System.Windows.Forms.Label();
            this.txt_clientName = new System.Windows.Forms.TextBox();
            this.lbl_clientName = new System.Windows.Forms.Label();
            this.cdl_playerColor = new System.Windows.Forms.ColorDialog();
            this.btn_launch = new System.Windows.Forms.Button();
            this.grp_serverProperties = new System.Windows.Forms.GroupBox();
            this.nud_serverPort = new System.Windows.Forms.NumericUpDown();
            this.lbl_serverPort = new System.Windows.Forms.Label();
            this.txt_servername = new System.Windows.Forms.TextBox();
            this.lbl_serverName = new System.Windows.Forms.Label();
            this.grp_clientProperties.SuspendLayout();
            this.grp_serverProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_serverPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_launchType
            // 
            this.lbl_launchType.AutoSize = true;
            this.lbl_launchType.Location = new System.Drawing.Point(13, 13);
            this.lbl_launchType.Name = "lbl_launchType";
            this.lbl_launchType.Size = new System.Drawing.Size(66, 13);
            this.lbl_launchType.TabIndex = 0;
            this.lbl_launchType.Text = "Launch das:";
            // 
            // cmb_launchType
            // 
            this.cmb_launchType.FormattingEnabled = true;
            this.cmb_launchType.Items.AddRange(new object[] {
            "client",
            "server"});
            this.cmb_launchType.Location = new System.Drawing.Point(86, 13);
            this.cmb_launchType.Name = "cmb_launchType";
            this.cmb_launchType.Size = new System.Drawing.Size(121, 21);
            this.cmb_launchType.TabIndex = 1;
            this.cmb_launchType.Text = "client";
            this.cmb_launchType.SelectedIndexChanged += new System.EventHandler(this.cmb_launchType_SelectedIndexChanged);
            // 
            // grp_clientProperties
            // 
            this.grp_clientProperties.Controls.Add(this.txt_port);
            this.grp_clientProperties.Controls.Add(this.lbl_port);
            this.grp_clientProperties.Controls.Add(this.txt_ip);
            this.grp_clientProperties.Controls.Add(this.lbl_IP);
            this.grp_clientProperties.Controls.Add(this.pnl_colorPicker);
            this.grp_clientProperties.Controls.Add(this.lbl_color);
            this.grp_clientProperties.Controls.Add(this.txt_clientName);
            this.grp_clientProperties.Controls.Add(this.lbl_clientName);
            this.grp_clientProperties.Location = new System.Drawing.Point(16, 40);
            this.grp_clientProperties.Name = "grp_clientProperties";
            this.grp_clientProperties.Size = new System.Drawing.Size(281, 116);
            this.grp_clientProperties.TabIndex = 2;
            this.grp_clientProperties.TabStop = false;
            this.grp_clientProperties.Text = "Client Properties";
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(168, 84);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(59, 20);
            this.txt_port.TabIndex = 7;
            this.txt_port.Text = "14245";
            this.txt_port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_port_KeyPress);
            // 
            // lbl_port
            // 
            this.lbl_port.AutoSize = true;
            this.lbl_port.Location = new System.Drawing.Point(165, 68);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(26, 13);
            this.lbl_port.TabIndex = 6;
            this.lbl_port.Text = "Port";
            // 
            // txt_ip
            // 
            this.txt_ip.Location = new System.Drawing.Point(13, 84);
            this.txt_ip.Mask = "###.###.###.###";
            this.txt_ip.Name = "txt_ip";
            this.txt_ip.PromptChar = ' ';
            this.txt_ip.Size = new System.Drawing.Size(138, 20);
            this.txt_ip.TabIndex = 5;
            // 
            // lbl_IP
            // 
            this.lbl_IP.AutoSize = true;
            this.lbl_IP.Location = new System.Drawing.Point(10, 68);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(94, 13);
            this.lbl_IP.TabIndex = 4;
            this.lbl_IP.Text = "IP (blank for LAN):";
            // 
            // pnl_colorPicker
            // 
            this.pnl_colorPicker.BackColor = System.Drawing.Color.White;
            this.pnl_colorPicker.Location = new System.Drawing.Point(51, 44);
            this.pnl_colorPicker.Name = "pnl_colorPicker";
            this.pnl_colorPicker.Size = new System.Drawing.Size(100, 17);
            this.pnl_colorPicker.TabIndex = 3;
            this.pnl_colorPicker.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnl_colorPicker_MouseDoubleClick);
            // 
            // lbl_color
            // 
            this.lbl_color.AutoSize = true;
            this.lbl_color.Location = new System.Drawing.Point(10, 48);
            this.lbl_color.Name = "lbl_color";
            this.lbl_color.Size = new System.Drawing.Size(34, 13);
            this.lbl_color.TabIndex = 2;
            this.lbl_color.Text = "Color:";
            // 
            // txt_clientName
            // 
            this.txt_clientName.Location = new System.Drawing.Point(51, 17);
            this.txt_clientName.Name = "txt_clientName";
            this.txt_clientName.Size = new System.Drawing.Size(100, 20);
            this.txt_clientName.TabIndex = 1;
            this.txt_clientName.Text = "Player";
            this.txt_clientName.TextChanged += new System.EventHandler(this.txt_clientName_TextChanged);
            this.txt_clientName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_clientName_KeyPress);
            // 
            // lbl_clientName
            // 
            this.lbl_clientName.AutoSize = true;
            this.lbl_clientName.Location = new System.Drawing.Point(7, 20);
            this.lbl_clientName.Name = "lbl_clientName";
            this.lbl_clientName.Size = new System.Drawing.Size(38, 13);
            this.lbl_clientName.TabIndex = 0;
            this.lbl_clientName.Text = "Name:";
            // 
            // cdl_playerColor
            // 
            this.cdl_playerColor.AllowFullOpen = false;
            this.cdl_playerColor.AnyColor = true;
            this.cdl_playerColor.Color = System.Drawing.Color.White;
            // 
            // btn_launch
            // 
            this.btn_launch.Location = new System.Drawing.Point(222, 8);
            this.btn_launch.Name = "btn_launch";
            this.btn_launch.Size = new System.Drawing.Size(75, 23);
            this.btn_launch.TabIndex = 3;
            this.btn_launch.Text = "Launch";
            this.btn_launch.UseVisualStyleBackColor = true;
            this.btn_launch.Click += new System.EventHandler(this.btn_launch_Click);
            // 
            // grp_serverProperties
            // 
            this.grp_serverProperties.Controls.Add(this.nud_serverPort);
            this.grp_serverProperties.Controls.Add(this.lbl_serverPort);
            this.grp_serverProperties.Controls.Add(this.txt_servername);
            this.grp_serverProperties.Controls.Add(this.lbl_serverName);
            this.grp_serverProperties.Enabled = false;
            this.grp_serverProperties.Location = new System.Drawing.Point(16, 162);
            this.grp_serverProperties.Name = "grp_serverProperties";
            this.grp_serverProperties.Size = new System.Drawing.Size(281, 117);
            this.grp_serverProperties.TabIndex = 4;
            this.grp_serverProperties.TabStop = false;
            this.grp_serverProperties.Text = "Server Properties";
            // 
            // nud_serverPort
            // 
            this.nud_serverPort.Location = new System.Drawing.Point(85, 44);
            this.nud_serverPort.Maximum = new decimal(new int[] {
            14255,
            0,
            0,
            0});
            this.nud_serverPort.Minimum = new decimal(new int[] {
            14245,
            0,
            0,
            0});
            this.nud_serverPort.Name = "nud_serverPort";
            this.nud_serverPort.Size = new System.Drawing.Size(66, 20);
            this.nud_serverPort.TabIndex = 3;
            this.nud_serverPort.Value = new decimal(new int[] {
            14245,
            0,
            0,
            0});
            // 
            // lbl_serverPort
            // 
            this.lbl_serverPort.AutoSize = true;
            this.lbl_serverPort.Location = new System.Drawing.Point(16, 46);
            this.lbl_serverPort.Name = "lbl_serverPort";
            this.lbl_serverPort.Size = new System.Drawing.Size(63, 13);
            this.lbl_serverPort.TabIndex = 2;
            this.lbl_serverPort.Text = "Server Port:";
            // 
            // txt_servername
            // 
            this.txt_servername.Location = new System.Drawing.Point(85, 17);
            this.txt_servername.Name = "txt_servername";
            this.txt_servername.Size = new System.Drawing.Size(190, 20);
            this.txt_servername.TabIndex = 1;
            // 
            // lbl_serverName
            // 
            this.lbl_serverName.AutoSize = true;
            this.lbl_serverName.Location = new System.Drawing.Point(7, 20);
            this.lbl_serverName.Name = "lbl_serverName";
            this.lbl_serverName.Size = new System.Drawing.Size(72, 13);
            this.lbl_serverName.TabIndex = 0;
            this.lbl_serverName.Text = "Server Name:";
            // 
            // GameLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 291);
            this.Controls.Add(this.grp_serverProperties);
            this.Controls.Add(this.btn_launch);
            this.Controls.Add(this.grp_clientProperties);
            this.Controls.Add(this.cmb_launchType);
            this.Controls.Add(this.lbl_launchType);
            this.Name = "GameLauncher";
            this.Text = "Launcher";
            this.grp_clientProperties.ResumeLayout(false);
            this.grp_clientProperties.PerformLayout();
            this.grp_serverProperties.ResumeLayout(false);
            this.grp_serverProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_serverPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_launchType;
        private System.Windows.Forms.ComboBox cmb_launchType;
        private System.Windows.Forms.GroupBox grp_clientProperties;
        private System.Windows.Forms.Panel pnl_colorPicker;
        private System.Windows.Forms.Label lbl_color;
        private System.Windows.Forms.TextBox txt_clientName;
        private System.Windows.Forms.Label lbl_clientName;
        private System.Windows.Forms.ColorDialog cdl_playerColor;
        private System.Windows.Forms.Button btn_launch;
        private System.Windows.Forms.Label lbl_IP;
        private System.Windows.Forms.GroupBox grp_serverProperties;
        private System.Windows.Forms.TextBox txt_servername;
        private System.Windows.Forms.Label lbl_serverName;
        private System.Windows.Forms.Label lbl_serverPort;
        private System.Windows.Forms.NumericUpDown nud_serverPort;
        private System.Windows.Forms.MaskedTextBox txt_ip;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label lbl_port;
    }
}