using MineMP;
using MineMPShell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MineMP.ConsoleBuffer;

namespace MineMPGUI_Win.ServerList
{
    public partial class ServerTerminal : Form
    {
        public MineServer MineServer = new MineServer();
        public ListViewItem MiniView = new ListViewItem();

        private void RunMineShell()
        {
            new Shell().RunShell(ref MineServer);
        }

        private void RunServer()
        {
            listBox.Items.Add("[Info]Starting Server");

            MineServer.Start();

            listBox.Items.Add("[Info]Server Started");

            listBox.Items.Add("[Info]Starting Shell");

            RunMineShell();
        }

        private void SetupCore()
        {
            listBox.Items.Add("[Info]Setup Core...");

            MineServer.ConsoleBuffer.ClearBuffer();
            MineServer.ConsoleBuffer.ErrorBufferAppended += ConsoleBuffer_ErrorBufferAppended;
            MineServer.ConsoleBuffer.InfoBufferAppended += ConsoleBuffer_InfoBufferAppended;
            MineServer.ConsoleBuffer.WarnBufferAppended += ConsoleBuffer_WarnBufferAppended;
            MineServer.ConsoleBuffer.DebugBufferAppended += ConsoleBuffer_DebugBufferAppended;
            MineServer.ConsoleBuffer.ControlSymbolBufferPushed += ConsoleBuffer_ControlSymbolBufferPushed;
            MineServer.ConsoleBuffer.ReadingInputLine += ConsoleBuffer_ReadingInputLine;
            MineServer.ConsoleBuffer.ReadingInputLinePeeking += ConsoleBuffer_ReadingInputLinePeeking;
            MineServer.Init();

        }

        // Ignore
        private void ConsoleBuffer_ReadingInputLinePeeking()
        {
        }

        private void ConsoleBuffer_ReadingInputLine()
        {
            this.btn_enter.Enabled = true;
        }

        private void ConsoleBuffer_ControlSymbolBufferPushed(ConsoleBuffer.ConsoleControlSymbolBufferPushedEventArgs e)
        {
            switch (e.BufferPushed)
            {
                default: return;

                case ControlSymbols.ClearScreen:
                    listBox.Items.Clear();
                    break;
            }

            return;
        }

        private void ConsoleBuffer_DebugBufferAppended(ConsoleBuffer.ConsoleBufferAppendEventArgs e)
        {
            listBox.Items.Add("magenta|" + e.BufferAppended);
        }

        private void ConsoleBuffer_WarnBufferAppended(ConsoleBuffer.ConsoleBufferAppendEventArgs e)
        {
            listBox.Items.Add("dark_yellow|" + e.BufferAppended);
        }

        private void ConsoleBuffer_InfoBufferAppended(ConsoleBuffer.ConsoleBufferAppendEventArgs e)
        {
            listBox.Items.Add("dark_gray|" + e.BufferAppended);
        }

        private void ConsoleBuffer_ErrorBufferAppended(ConsoleBuffer.ConsoleBufferAppendEventArgs e)
        {
            listBox.Items.Add("red|" + e.BufferAppended);
        }


        private void MiniView_Click(object? sender, EventArgs e)
        {
            if (MineServer.ShouldDispose)
                return;

            this.Show();
        }

        public ServerTerminal()
        {
            InitializeComponent();
            listBox.Items.Clear();
            MiniView.Click += MiniView_Click;

            new Thread(() =>
            {
                // Welcome
                listBox.Items.Add("Hello, MineMP!\r\n");
                listBox.Items.Add("[Press Enter to Continue]\r\n");


                // Continued

                // Start Server
                SetupCore();
                //RunServer();
                //

                // We Finished
                listBox.Items.Add("MineMP Finished");
                listBox.Items.Add("Terminal will be disposed soon.");
            }).Start();

        }

        private void btn_enter_Click(object sender, EventArgs e)
        {
            this.btn_enter.Enabled = false;
            MineServer.ConsoleBuffer.MakeInputLine(textBox1.Text);
            this.textBox1.Text = "";
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color back_color = Color.Black;
                string str = listBox.Items[e.Index].ToString();

                if (str.Contains('|'))
                {
                    switch (str.Substring(0, str.IndexOf('|')))
                    {
                        default:
                            back_color = Color.Black;
                            break;

                        case "magenta":
                            back_color = Color.Magenta;
                            break;
                        case "dark_yellow":
                            back_color = Color.FromArgb(246, 190, 0);
                            break;
                        case "dark_gray":
                            back_color = Color.DarkGray;
                            break;
                        case "red":
                            back_color = Color.Red;
                            break;
                    }

                    str = str.Remove(0, str.IndexOf('|')+1);
                }

                RectangleF rf = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Graphics.MeasureString(str,e.Font).Width, e.Font.Height * str.Split("\n").Length);
                e.Graphics.FillRectangle(new SolidBrush(back_color), rf);
                e.Graphics.DrawString(str, e.Font, new SolidBrush(e.ForeColor), rf, StringFormat.GenericDefault);
            }
        }
        
        private void ServerTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MineServer.ShouldDispose)
                return;

            e.Cancel = true;
            this.Hide();
        }

        private void listBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = listBox.Font.Height * listBox.Items[e.Index].ToString().Split("\n").Length;
        }
    }
}
