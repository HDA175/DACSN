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
    public partial class Trangchu : Form
    {
        public Trangchu()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Trangchu_Load(object sender, EventArgs e)
        {
            Class.Function.Connect();
            // Disable features by role
            try
            {
                if (!string.IsNullOrEmpty(Program.CurrentRole))
                {
                    // If role is "Nhân viên" then disable employee management button
                    var isNhanVien = string.Equals(Program.CurrentRole, "Nhân viên", StringComparison.OrdinalIgnoreCase);
                    // btnTTKH is labeled "Nhân viên" in designer
                    btnTTKH.Enabled = !isNhanVien;
                }
            }
            catch { }
        }

        private void btnHang_Click(object sender, EventArgs e)
        {
            frmHangHoa h = new frmHangHoa();
            h.ShowDialog();

        }

        private void btnDKKH_Click(object sender, EventArgs e)
        {
            frmDMKhachHang frmDMKhachHang = new frmDMKhachHang();       
               frmDMKhachHang.ShowDialog();
        }

        private void btNv_Click(object sender, EventArgs e)
        {
            frmNhanVien frmNhanVien = new frmNhanVien();
            frmNhanVien.ShowDialog();
        }

        private void btnThanhtoan_Click(object sender, EventArgs e)
        {
            frmHoaDonBan frmHoaDonBan = new frmHoaDonBan(); 
            frmHoaDonBan.ShowDialog();
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            frmTimSanPham frmTimSanPham = new frmTimSanPham();  
            frmTimSanPham.ShowDialog();
        }

        private void btnDoanhThu_Click(object sender, EventArgs e)
        {
            frmDoanhThu frm = new frmDoanhThu();
            frm.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
