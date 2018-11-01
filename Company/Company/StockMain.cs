using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Company
{
    public partial class StockMain : Form
    {
        public StockMain()
        {
            InitializeComponent();
        }
        // FOR: Application Close
        private void StockMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }


        // FOR: productToolStripMenuItem_Click   ( Open Product Table )
        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products pro = new Products();
            pro.MdiParent=this;
            pro.StartPosition = FormStartPosition.CenterScreen;
            pro.Show();
        }

        private void StockMain_Load(object sender, EventArgs e)
        {

        }
    }
}
