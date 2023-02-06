using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineMPGUI_Win.ServerList
{
    public partial class MakeServer : Form
    {
        public MakeServer()
        {
            InitializeComponent();
        }

        private bool confirmed = false;

        public bool MakeDialog(out string path) {
            this.ShowDialog();
            path = textBox1.Text.Trim();
            return confirmed;
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != string.Empty)
            {
                confirmed = true;
                this.Close();
            }
            else
                MessageBox.Show("Path should not be empty!");
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            confirmed= false;
            this.Close();
        }
    }
}
