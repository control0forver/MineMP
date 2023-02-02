namespace MineMPGUI_Win
{
    partial class ServersList
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.listViewItem1 = new MineMPGUI_Win.ServerList.ListViewItem();
            this.listViewItem2 = new MineMPGUI_Win.ServerList.ListViewItem();
            this.listViewItem3 = new MineMPGUI_Win.ServerList.ListViewItem();
            this.listViewItem4 = new MineMPGUI_Win.ServerList.ListViewItem();
            this.listViewItem5 = new MineMPGUI_Win.ServerList.ListViewItem();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.flowLayoutPanel1.Controls.Add(this.listViewItem1);
            this.flowLayoutPanel1.Controls.Add(this.listViewItem2);
            this.flowLayoutPanel1.Controls.Add(this.listViewItem3);
            this.flowLayoutPanel1.Controls.Add(this.listViewItem4);
            this.flowLayoutPanel1.Controls.Add(this.listViewItem5);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // listViewItem1
            // 
            this.listViewItem1.BackColor = System.Drawing.Color.DimGray;
            this.listViewItem1.Location = new System.Drawing.Point(3, 3);
            this.listViewItem1.Name = "listViewItem1";
            this.listViewItem1.Size = new System.Drawing.Size(157, 95);
            this.listViewItem1.TabIndex = 0;
            // 
            // listViewItem2
            // 
            this.listViewItem2.BackColor = System.Drawing.Color.DimGray;
            this.listViewItem2.Location = new System.Drawing.Point(166, 3);
            this.listViewItem2.Name = "listViewItem2";
            this.listViewItem2.Size = new System.Drawing.Size(157, 95);
            this.listViewItem2.TabIndex = 1;
            // 
            // listViewItem3
            // 
            this.listViewItem3.BackColor = System.Drawing.Color.DimGray;
            this.listViewItem3.Location = new System.Drawing.Point(329, 3);
            this.listViewItem3.Name = "listViewItem3";
            this.listViewItem3.Size = new System.Drawing.Size(157, 95);
            this.listViewItem3.TabIndex = 2;
            // 
            // listViewItem4
            // 
            this.listViewItem4.BackColor = System.Drawing.Color.DimGray;
            this.listViewItem4.Location = new System.Drawing.Point(492, 3);
            this.listViewItem4.Name = "listViewItem4";
            this.listViewItem4.Size = new System.Drawing.Size(157, 95);
            this.listViewItem4.TabIndex = 3;
            // 
            // listViewItem5
            // 
            this.listViewItem5.BackColor = System.Drawing.Color.DimGray;
            this.listViewItem5.Location = new System.Drawing.Point(3, 104);
            this.listViewItem5.Name = "listViewItem5";
            this.listViewItem5.Size = new System.Drawing.Size(157, 95);
            this.listViewItem5.TabIndex = 4;
            // 
            // ServersList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ServersList";
            this.Text = "Servers";
            this.Shown += new System.EventHandler(this.ServersList_Shown);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private ServerList.ListViewItem listViewItem1;
        private ServerList.ListViewItem listViewItem2;
        private ServerList.ListViewItem listViewItem3;
        private ServerList.ListViewItem listViewItem4;
        private ServerList.ListViewItem listViewItem5;
    }
}