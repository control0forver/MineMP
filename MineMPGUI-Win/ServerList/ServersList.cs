using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineMPGUI_Win
{

    public partial class ServersList : Form
    {
        public ServersList()
        {
            InitializeComponent();
        }

        private void ServersList_Shown(object sender, EventArgs e)
        {
            GUIWinMain.Launcher.Hide();
        }
    }
}
