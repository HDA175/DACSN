using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DACSN.Class;

namespace DACSN
{
    public partial class frmCaNhanHoa : Form
    {
        public frmCaNhanHoa()
        {
            InitializeComponent();
        }

        private void frmCaNhanHoa_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLBHDataSet1.tblHang' table. You can move, or remove it, as needed.
            this.tblHangTableAdapter.Fill(this.qLBHDataSet1.tblHang);

        }

        private void cboMaNhanVien_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaSPGoc.Text == "")
            {
                txtTenSPGoc.Text = "";
            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select TenHang  from tblHang where MaHang = N'" + cboMaSPGoc.SelectedValue + "'";
            txtTenSPGoc.Text = Function.GetFieldValues(str);
        }
    }
}
