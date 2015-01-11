using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MirageXNA.Globals;
namespace MirageXNA.Core
{
    public partial class frmServer : Form
    {
        public frmServer()
        {
            InitializeComponent();
        }

        private void frmServer_Load(object sender, EventArgs e)
        {

        }

        private void lvwInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Exit Loop Thread //
            Static.ServerRunning = false;
            GC.Collect();
            Application.Exit();
        }
    }
}
