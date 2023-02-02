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
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();

            BackGround.Image = Image.FromStream(new MemoryStream(MineMP.CoreResc.VersionH));
        }

        private void Launcher_Shown(object sender, EventArgs e)
        {
            new Thread(() => {
                new ServersList().ShowDialog();
                GUIWinMain.Launcher.Close();
            }).Start();
        }
    }
}
