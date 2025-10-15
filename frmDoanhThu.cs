using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using DACSN.Class;
namespace DACSN
{
    public partial class frmDoanhThu : Form
    {
        Function lop = new Function();
        public frmDoanhThu()
        {
            InitializeComponent();
        }
        private void TinhTongTien()
        {
            decimal tong = 0;
            foreach (DataGridViewRow row in dgv_DoanhThu.Rows)
            {
                if (row.Cells["TongTien"].Value != null)
                {
                    decimal gia;
                    if (decimal.TryParse(row.Cells["TongTien"].Value.ToString(), out gia))
                        tong += gia;
                }
            }
            txt_TongTien.Text = tong.ToString("N0", CultureInfo.InvariantCulture);
        }
        private void frmDoanhThu_Load(object sender, EventArgs e)
        {
            cb_ThongKe.Items.Clear();
            cb_ThongKe.Items.AddRange(new string[] { "Ngày", "Tuần", "Tháng", "Năm" });

            if (cb_ThongKe.Items.Count > 0)
                cb_ThongKe.SelectedIndex = 0;

            LoadData();
            CapNhatLaiThanhTien();
            CapNhatTongDoanhThu();
            dgv_DoanhThu.Columns["TongTien"].DefaultCellStyle.Format = "#,##0 VNĐ";

        }
        void LoadData()
        {
            string sql = "SELECT ct.MaHDBan, hd.NgayBan, SUM(ct.ThanhTien) AS TongTien " +
                         "FROM tblChiTietHDBan ct JOIN tblHDBan hd ON ct.MaHDBan = hd.MaHDBan " +
                         "GROUP BY ct.MaHDBan, hd.NgayBan";
            dgv_DoanhThu.DataSource = lop.LoadDL(sql);
            TinhTongTien();
        }
        private void CapNhatLaiThanhTien()
        {
            string sql = @"
        UPDATE tblChiTietHDBan
        SET ThanhTien = SoLuong * 
            (SELECT DonGiaBan FROM tblHang WHERE tblHang.MaHang = tblChiTietHDBan.MaHDBan)";
            Function lop = new Function();
            lop.ThemXoaSua(sql);
        }
        private void CapNhatTongDoanhThu()
        {
            string sql = "SELECT SUM(ThanhTien) FROM tblChiTietHDBan";
            Function lop = new Function();
            object tongObj = lop.LayGT(sql);

            if (tongObj != DBNull.Value && tongObj != null)
            {
                double tong = Convert.ToDouble(tongObj);
                txt_TongTien.Text = tong.ToString("N0") + " VNĐ";
            }
            else
            {
                txt_TongTien.Text = "0 VNĐ";
            }
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_Tim_Click(object sender, EventArgs e)
        {

            string keyword = txt_TuKhoa.Text.Trim();
            DateTime tuNgayDate = dtp_TuNgay.Value.Date;
            DateTime denNgayDate = dtp_DenNgay.Value.Date;

            if (tuNgayDate > denNgayDate)
            {
                MessageBox.Show("Từ ngày không được lớn hơn đến ngày.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tuNgay = tuNgayDate.ToString("yyyy-MM-dd");
            string denNgay = denNgayDate.ToString("yyyy-MM-dd");

            string dkKeyword = "";
            if (!string.IsNullOrEmpty(keyword))
            {
                dkKeyword = $"AND (ct.MaHDBan LIKE N'%{keyword}%' OR ct.ThanhTien LIKE N'%{keyword}%') ";
            }

            string sql = "SELECT ct.MaHDBan, hd.NgayBan, SUM(ct.ThanhTien) AS TongTien " +
                         "FROM tblChiTietHDBan ct JOIN tblHDBan hd ON ct.MaHDBan = hd.MaHDBan " +
                         $"WHERE hd.NgayBan BETWEEN '{tuNgay}' AND '{denNgay}' {dkKeyword} " +
                         "GROUP BY ct.MaHDBan, hd.NgayBan";

            dgv_DoanhThu.DataSource = lop.LoadDL(sql);
            TinhTongTien();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            txt_TuKhoa.Clear();
            cb_ThongKe.SelectedIndex = 0;
            LoadData();
        }

        private void cb_ThongKe_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dk = "";
            string tuNgay = dtp_TuNgay.Value.ToString("yyyy-MM-dd");
            string denNgay = dtp_DenNgay.Value.ToString("yyyy-MM-dd");

            if (cb_ThongKe.Text == "Ngày")
            {
                dk = $"AND hd.NgayBan = '{tuNgay}'";
            }
            else if (cb_ThongKe.Text == "Tuần")
            {
                DateTime tu = dtp_TuNgay.Value.Date;
                DateTime den = tu.AddDays(7);
                dk = $"AND hd.NgayBan >= '{tu:yyyy-MM-dd}' AND hd.NgayBan < '{den:yyyy-MM-dd}'";
            }
            else if (cb_ThongKe.Text == "Tháng")
            {
                dk = $"AND MONTH(hd.NgayBan) = '{dtp_TuNgay.Value.Month}' AND YEAR(hd.NgayBan) = '{dtp_TuNgay.Value.Year}'";
            }
            else if (cb_ThongKe.Text == "Năm")
            {
                dk = $"AND YEAR(hd.NgayBan) = '{dtp_TuNgay.Value.Year}'";
            }

            string sql = "SELECT ct.MaHDBan, hd.NgayBan, SUM(ct.Thanhtien) AS TongTien " +
                         "FROM tblChiTietHDBan ct JOIN tblHDBan hd ON ct.MaHDBan = hd.MaHDBan " +
                         "WHERE 1=1 " + dk +
                         " GROUP BY ct.MaHDBan, hd.NgayBan";

            dgv_DoanhThu.DataSource = lop.LoadDL(sql);
            TinhTongTien(); 
        }

        private void dgv_DoanhThu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}
