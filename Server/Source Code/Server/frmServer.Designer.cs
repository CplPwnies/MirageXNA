namespace MirageXNA.Core
{
    partial class frmServer
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
            this.tabmain = new System.Windows.Forms.TabControl();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tabPlayers = new System.Windows.Forms.TabPage();
            this.lvwInfo = new System.Windows.Forms.ListView();
            this.chIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabmain.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.tabPlayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabmain
            // 
            this.tabmain.Controls.Add(this.tabLog);
            this.tabmain.Controls.Add(this.tabPlayers);
            this.tabmain.Location = new System.Drawing.Point(12, 12);
            this.tabmain.Name = "tabmain";
            this.tabmain.SelectedIndex = 0;
            this.tabmain.Size = new System.Drawing.Size(575, 173);
            this.tabmain.TabIndex = 1;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.tbLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(567, 147);
            this.tabLog.TabIndex = 0;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.Color.White;
            this.tbLog.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLog.Location = new System.Drawing.Point(6, 6);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(555, 135);
            this.tbLog.TabIndex = 0;
            // 
            // tabPlayers
            // 
            this.tabPlayers.Controls.Add(this.lvwInfo);
            this.tabPlayers.Location = new System.Drawing.Point(4, 22);
            this.tabPlayers.Name = "tabPlayers";
            this.tabPlayers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPlayers.Size = new System.Drawing.Size(567, 147);
            this.tabPlayers.TabIndex = 1;
            this.tabPlayers.Text = "Players";
            this.tabPlayers.UseVisualStyleBackColor = true;
            // 
            // lvwInfo
            // 
            this.lvwInfo.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvwInfo.AllowColumnReorder = true;
            this.lvwInfo.AutoArrange = false;
            this.lvwInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chIndex,
            this.columnHeader2,
            this.columnHeader3});
            this.lvwInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwInfo.FullRowSelect = true;
            this.lvwInfo.GridLines = true;
            this.lvwInfo.Location = new System.Drawing.Point(3, 3);
            this.lvwInfo.MultiSelect = false;
            this.lvwInfo.Name = "lvwInfo";
            this.lvwInfo.Size = new System.Drawing.Size(561, 141);
            this.lvwInfo.TabIndex = 2;
            this.lvwInfo.UseCompatibleStateImageBehavior = false;
            this.lvwInfo.View = System.Windows.Forms.View.Details;
            this.lvwInfo.SelectedIndexChanged += new System.EventHandler(this.lvwInfo_SelectedIndexChanged);
            // 
            // chIndex
            // 
            this.chIndex.Tag = "Index";
            this.chIndex.Text = "Index";
            this.chIndex.Width = 95;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Tag = "Name";
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 96;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Access";
            this.columnHeader3.Width = 98;
            // 
            // frmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 197);
            this.Controls.Add(this.tabmain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmServer_FormClosing);
            this.Load += new System.EventHandler(this.frmServer_Load);
            this.tabmain.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.tabPlayers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabmain;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TabPage tabPlayers;
        public System.Windows.Forms.TextBox tbLog;
        public System.Windows.Forms.ListView lvwInfo;
        private System.Windows.Forms.ColumnHeader chIndex;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;

    }
}