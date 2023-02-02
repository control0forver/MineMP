using Microsoft.VisualBasic.Devices;
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
    public partial class ListViewItem : UserControl
    {
        Graphics graph;
        public ListViewItem()
        {
            InitializeComponent();
            graph = this.CreateGraphics();
            graph.Clear(this.BackColor);
        }

        private void ListViewItem_MouseEnter(object sender, EventArgs e)
        {
            ControlPaint.DrawBorder(graph, this.ClientRectangle,
            Color.White, 2, ButtonBorderStyle.Solid,
　　　　　  Color.White, 2, ButtonBorderStyle.Solid,
　　　　　  Color.White, 2, ButtonBorderStyle.Solid,
　　　　　  Color.White, 2, ButtonBorderStyle.Solid);
        }

        private void ListViewItem_MouseDown(object sender, MouseEventArgs e)
        {
            ControlPaint.DrawBorder(graph, this.ClientRectangle,
            Color.LightGray, 2, ButtonBorderStyle.Solid,
            Color.LightGray, 2, ButtonBorderStyle.Solid,
            Color.LightGray, 2, ButtonBorderStyle.Solid,
            Color.LightGray, 2, ButtonBorderStyle.Solid);
        }

        private void ListViewItem_MouseUp(object sender, MouseEventArgs e)
        {
            ControlPaint.DrawBorder(graph, this.ClientRectangle,
            Color.White, 2, ButtonBorderStyle.Solid,
            Color.White, 2, ButtonBorderStyle.Solid,
            Color.White, 2, ButtonBorderStyle.Solid,
            Color.White, 2, ButtonBorderStyle.Solid);
        }

        private void ListViewItem_MouseLeave(object sender, EventArgs e)
        {
            if (this.RectangleToScreen(this.ClientRectangle).Contains(MousePosition))
                return;
            graph.Clear(this.BackColor);
        }
    }
}
