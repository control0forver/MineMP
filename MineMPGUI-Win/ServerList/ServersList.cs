using MineMPGUI_Win.ServerList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineMPGUI_Win
{
    public partial class ServersList : Form
    {
        public List<ServerTerminal> servers = new List<ServerTerminal>();

        public ServersList()
        {
            InitializeComponent();
            this.lab_title.Text = this.Text;
            //Thread.Sleep(1000);
        }

        private void ServersList_Shown(object sender, EventArgs e)
        {
            //Thread.Sleep(1700);
            GUIWinMain.Launcher.Hide();
        }

        private void lab_title_MouseDown(object sender, MouseEventArgs e)
        {
            Utils.Window.MoveWindow(this.Handle);
        }

        private void btn_min_Click(object sender, EventArgs e)
        {
            this.WindowState= FormWindowState.Minimized;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_create_new_Click(object sender, EventArgs e)
        {
            ServerTerminal serverTerminal = new ServerTerminal();
            flowLayoutPanel1.Controls.Add(serverTerminal.MiniView);
            servers.Add(serverTerminal);
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {

        }

        private void ServersList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (servers.Count> 0)
            {
                e.Cancel = true;
                MessageBox.Show("You must stop all servers before closing!");
                return;
            }

            //Environment.Exit(0);
        }

        private void refresher_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < servers.Count; i++)
            {
                ServerTerminal serverTerminal = servers[i];
                if (serverTerminal.MineServer.ShouldDispose)
                {
                    serverTerminal.Close();
                    flowLayoutPanel1.Controls.Remove(serverTerminal.MiniView);
                    servers.Remove(serverTerminal);
                }
            }
        }
    }
}
