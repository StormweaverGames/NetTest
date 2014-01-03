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
            this.grb_clientProperties = new System.Windows.Forms.GroupBox();
            this.pnl_colorPicker = new System.Windows.Forms.Panel();
            this.lbl_color = new System.Windows.Forms.Label();
            this.txt_clientName = new System.Windows.Forms.TextBox();
            this.lbl_clientName = new System.Windows.Forms.Label();
            this.cdl_playerColor = new System.Windows.Forms.ColorDialog();
            this.btn_launch = new System.Windows.Forms.Button();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.txt_ip = new System.Windows.Forms.TextBox();
            this.grb_clientProperties.SuspendLayout();
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
            // grb_clientProperties
            // 
            this.grb_clientProperties.Controls.Add(this.txt_ip);
            this.grb_clientProperties.Controls.Add(this.lbl_IP);
            this.grb_clientProperties.Controls.Add(this.pnl_colorPicker);
            this.grb_clientProperties.Controls.Add(this.lbl_color);
            this.grb_clientProperties.Controls.Add(this.txt_clientName);
            this.grb_clientProperties.Controls.Add(this.lbl_clientName);
            this.grb_clientProperties.Location = new System.Drawing.Point(16, 40);
            this.grb_clientProperties.Name = "grb_clientProperties";
            this.grb_clientProperties.Size = new System.Drawing.Size(281, 239);
            this.grb_clientProperties.TabIndex = 2;
            this.grb_clientProperties.TabStop = false;
            this.grb_clientProperties.Text = "Client Properties";
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
            // lbl_IP
            // 
            this.lbl_IP.AutoSize = true;
            this.lbl_IP.Location = new System.Drawing.Point(10, 68);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(94, 13);
            this.lbl_IP.TabIndex = 4;
            this.lbl_IP.Text = "IP (blank for LAN):";
            // 
            // txt_ip
            // 
            this.txt_ip.Location = new System.Drawing.Point(10, 84);
            this.txt_ip.Name = "txt_ip";
            this.txt_ip.Size = new System.Drawing.Size(265, 20);
            this.txt_ip.TabIndex = 5;
            this.txt_ip.Leave += new System.EventHandler(this.txt_ip_Leave);
            // 
            // GameLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 291);
            this.Controls.Add(this.btn_launch);
            this.Controls.Add(this.grb_clientProperties);
            this.Controls.Add(this.cmb_launchType);
            this.Controls.Add(this.lbl_launchType);
            this.Name = "GameLauncher";
            this.Text = "Launcher";
            this.grb_clientProperties.ResumeLayout(false);
            this.grb_clientProperties.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_launchType;
        private System.Windows.Forms.ComboBox cmb_launchType;
        private System.Windows.Forms.GroupBox grb_clientProperties;
        private System.Windows.Forms.Panel pnl_colorPicker;
        private System.Windows.Forms.Label lbl_color;
        private System.Windows.Forms.TextBox txt_clientName;
        private System.Windows.Forms.Label lbl_clientName;
        private System.Windows.Forms.ColorDialog cdl_playerColor;
        private System.Windows.Forms.Button btn_launch;
        private System.Windows.Forms.TextBox txt_ip;
        private System.Windows.Forms.Label lbl_IP;
    }
}