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
    public partial class frmHDBanChiTiet : Form
    {
        Function lop = new Function();
        string maHD = "";
        string maHCu = "";
        public frmHDBanChiTiet()
        {
            InitializeComponent();
            maHD = lop.TaoMaTuDong("tblHDBan", "MaHD", "HD");

            string ngay = DateTime.Now.ToString("yyyy-MM-dd");
            string gio = DateTime.Now.ToString("HH:mm:ss");

            // Kiểm tra trước khi thêm
            string sqlCheck = $"SELECT COUNT(*) FROM tblHDBan WHERE MaHD = N'{maHD}'";
            int kq = Convert.ToInt32(lop.LayGT(sqlCheck));
            if (kq == 0)
            {
                string sqlInsert = $"INSERT INTO tblHDBan (MaHDBan, NgayBan, GioBan, TongTien) VALUES (N'{maHD}', '{ngay}', '{gio}', 0)";
                lop.ThemXoaSua(sqlInsert);
            }
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
