using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WebAccessibility
{
    public partial class FrmLeftDock : DockWindow.DockWindow
    {
        public FrmLeftDock()
        {
            InitializeComponent();
        }

        private void trvLinks_DoubleClick(object sender, EventArgs e)
        {
            string fullPath = trvLinks.SelectedNode.FullPath;
            MessageBox.Show(fullPath);
        }
    }
}