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
    public partial class frmHangTonKho : Form
    {
        Function Function = new Function();
        public frmHangTonKho()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNhanvien_Click(object sender, EventArgs e)
        {

        }

        private void btnThanhtoan_Click(object sender, EventArgs e)
        {

        }

        private void btnDKKH_Click(object sender, EventArgs e)
        {

        }

        private void btnTTKH_Click(object sender, EventArgs e)
        {

        }

        private void btnDichvu_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void txt_TenH_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_CapNhap_Click(object sender, EventArgs e)
        {

        }

        private void dgv_TonKho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cb_MaH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MaH.SelectedIndex == -1 || cb_MaH.SelectedValue == null || cb_MaH.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            string mah = cb_MaH.SelectedValue.ToString();

            // Hiển thị tên hàng
            string sqlTen = $"SELECT TenHang FROM tblHang WHERE MaHang = N'{mah}'";
            object ten = Function.LayGT(sqlTen);
            txt_TenH.Text = ten?.ToString();

            // Cập nhật lại dữ liệu DataGridView
            string sql = $@"
        SELECT 
            h.MaHang, h.TenHang, h.SoLuong AS [Số lượng tồn],
            ISNULL(SUM(ct.SoLuong), 0) AS [Số lượng đã bán],
            CASE 
                WHEN h.SoLuong - ISNULL(SUM(ct.SoLuong), 0) < 0 THEN 0
                ELSE h.SoLuong - ISNULL(SUM(ct.SoLuong), 0)
            END AS [Số lượng còn lại]
        FROM tblHang h
        LEFT JOIN tblChiTietHDBan ct ON h.MaHang = ct.MaHDBan
        WHERE h.MaHang = N'{mah}'
        GROUP BY h.MaHang, h.TenHang, h.SoLuong";

            dgv_TonKho.DataSource = Function.LoadDL(sql);
        }
        private DataTable GetThongKeTatCa()
        {
            string sql = @"
        SELECT 
            h.MaHang, h.TenHang, h.SoLuong AS [Số lượng tồn],
            ISNULL(SUM(ct.SoLuong), 0) AS [Số lượng đã bán],
            CASE 
                WHEN h.SoLuong - ISNULL(SUM(ct.SoLuong), 0) < 0 THEN 0
                ELSE h.SoLuong - ISNULL(SUM(ct.SoLuong), 0)
            END AS [Số lượng còn lại]
        FROM tblHang h
        LEFT JOIN tblChiTietHDBan ct ON h.MaHang = ct.MaHDBan
        GROUP BY h.MaHang, h.TenHang, h.SoLuong";

            return Function.LoadDL(sql);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void dtp_DenNgay_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtp_TuNgay_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btn_Tim_Click(object sender, EventArgs e)
        {
            string tuKhoa = txt_Tim.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa) && !dtp_TuNgay.Checked && !dtp_DenNgay.Checked)
            {
                MessageBox.Show("Vui lòng nhập từ khoá hoặc chọn thời gian để tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dieuKien = "";

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                dieuKien += $@" AND (
            h.MaHang LIKE N'%{tuKhoa}%' OR
            h.TenHang LIKE N'%{tuKhoa}%' OR
            CAST(h.SoLuong AS NVARCHAR) LIKE N'%{tuKhoa}%'
        )";
            }

            string ngayBanDK = "";
            if (dtp_TuNgay.Checked && dtp_DenNgay.Checked)
                ngayBanDK = $"AND hd.NgayBan BETWEEN '{dtp_TuNgay.Value:yyyy-MM-dd}' AND '{dtp_DenNgay.Value:yyyy-MM-dd}'";
            else if (dtp_TuNgay.Checked)
                ngayBanDK = $"AND hd.NgayBan >= '{dtp_TuNgay.Value:yyyy-MM-dd}'";
            else if (dtp_DenNgay.Checked)
                ngayBanDK = $"AND hd.NgayBan <= '{dtp_DenNgay.Value:yyyy-MM-dd}'";

            string sql = $@"
        SELECT 
            h.MaHang, h.TenHang, h.SoLuong AS [Số lượng tồn],
            ISNULL(SUM(ct.SoLuong), 0) AS [Số lượng đã bán],
            CASE 
                WHEN h.SoLuong - ISNULL(SUM(ct.SoLuong), 0) < 0 THEN 0
                ELSE h.SoLuong - ISNULL(SUM(ct.SoLuong), 0)
            END AS [Số lượng còn lại]
        FROM tblHang h
        LEFT JOIN tblChiTietHDBan ct ON h.MaHang = ct.MaHDBan
        LEFT JOIN tblHDBan hd ON ct.MaHDBan = hd.MaHDBan
        WHERE 1=1 {dieuKien} {ngayBanDK}
        GROUP BY h.MaHang, h.TenHang, h.SoLuong";

            dgv_TonKho.DataSource = Function.LoadDL(sql);
        }

        private void txt_Tim_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void frmHangTonKho_Load(object sender, EventArgs e)
        {

            string sql = "SELECT * FROM tblHang";
            DataTable dt = Function.LoadDL(sql);
            cb_MaH.DataSource = dt;
            cb_MaH.DisplayMember = "MaHang";
            cb_MaH.ValueMember = "MaHang";
            cb_MaH.SelectedIndex = -1;

            dgv_TonKho.DataSource = GetThongKeTatCa();
            dgv_TonKho.CellDoubleClick -= dgv_TonKho_CellDoubleClick;
            dgv_TonKho.CellDoubleClick += dgv_TonKho_CellDoubleClick;

        }

        private void dgv_TonKho_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgv_TonKho.Rows.Count)
            {
                DataGridViewRow row = dgv_TonKho.Rows[e.RowIndex];

                // Lấy mã hàng từ dòng được click
                object mahObj = row.Cells["MaHang"]?.Value;
                if (mahObj != null)
                {
                    string mah = mahObj.ToString();

                    // Truy vấn mã hóa đơn gần nhất có chứa mã hàng đó
                    string sql = $"SELECT TOP 1 MaHDBan FROM tblChiTietHDBan WHERE MaHDBan = N'{mah}' ORDER BY MaHDBan DESC";
                    object mahdObj = Function.LayGT(sql);

                    if (mahdObj != null)
                    {
                        string mahd = mahdObj.ToString();
                        frmTimSanPham frmTimSanPham = new frmTimSanPham();
                         frmTimSanPham.ShowDialog();

                        // Gọi lại hàm load nếu muốn cập nhật thống kê tồn kho sau khi chỉnh sửa
                        dgv_TonKho.DataSource = GetThongKeTatCa();
                    }
                    else
                    {
                        MessageBox.Show("Sản phẩm này chưa có trong hóa đơn nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
