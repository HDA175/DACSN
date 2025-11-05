using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DACSN
{
    public partial class frmHD : Form
    {
        string username;
        public frmHD(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void dịchVụThuêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThueToc frm = new ThueToc(username);
            frm.ShowDialog();
        }

        private void trảHàngThuêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThueToc2 frm = new ThueToc2();
            frm.ShowDialog();
        }
    }
}
