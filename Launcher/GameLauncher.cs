using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Launcher.Properties;

namespace Launcher
{
    public partial class GameLauncher : Form
    {
        string Username;
        Color UserColor;
        string IP = null;

        public GameLauncher()
        {
            InitializeComponent();
            cmb_launchType.SelectedIndex = 0;
            
            Username = Settings.Default.UserName;
            UserColor = Settings.Default.Color;

            txt_clientName.Text = Username;
            pnl_colorPicker.BackColor = UserColor;
        }

        private void pnl_colorPicker_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cdl_playerColor.ShowDialog();

            pnl_colorPicker.BackColor = cdl_playerColor.Color;
            UserColor = cdl_playerColor.Color;
            Settings.Default.Color = UserColor;
            Settings.Default.Save();
        }

        private void txt_clientName_TextChanged(object sender, EventArgs e)
        {
            Username = txt_clientName.Text;
            Settings.Default.UserName = Username;
            Settings.Default.Save();
        }

        private void cmb_launchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_launchType.SelectedIndex == 1)
                grb_clientProperties.Enabled = false;
            else
                grb_clientProperties.Enabled = true;
        }

        private void btn_launch_Click(object sender, EventArgs e)
        {
            switch (cmb_launchType.SelectedIndex)
            {
                case 0:
                    Process.Start(Directory.GetCurrentDirectory() + "/NetTest.exe", string.Format(
                        "{0}/\\{1}/\\{2}/\\{3}/\\{4}", "client", Username, UserColor.R, UserColor.G, UserColor.B));
                    break;
                case 1:
                    Process.Start(Directory.GetCurrentDirectory() + "/NetTest.exe", string.Format(
                        "{0}", "server"));
                    break;
            }
        }

        private void txt_ip_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_ip.Text))
                IP = null;
            else
                IP = txt_ip.Text.Trim();
        }
    }
}
