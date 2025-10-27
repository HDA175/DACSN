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
using System.Data.SqlClient;
namespace DACSN
{
    public partial class frmDoanhThu : Form
    {
        Function lop = new Function();
        public frmDoanhThu()
        {
            InitializeComponent();
        }
        private void frmDoanhThu_Load(object sender, EventArgs e)
        {
            cb_ThongKe.Items.Clear();
            cb_ThongKe.Items.AddRange(new string[] { "Tất cả", "Ngày", "Tuần", "Tháng", "Năm" });
            cb_ThongKe.SelectedIndex = 0; // Mặc định là "Tất cả"

            // Xóa bỏ các hàm cập nhật không cần thiết và sai logic
            // CapNhatLaiThanhTien(); // BỎ
            // CapNhatTongDoanhThu(); // BỎ
            // LoadData(); // Sẽ được gọi trong sự kiện SelectedIndexChanged

            // Thiết lập định dạng cho cột tiền tệ
            dgv_DoanhThu.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            dgv_DoanhThu.Columns["TongTien"].DefaultCellStyle.FormatProvider = new CultureInfo("vi-VN");
            TinhTongTien();
        }
        private void LoadDataAndCalculateTotal(string dieuKien, List<SqlParameter> parameters = null)
        {
            try
            {
                // Câu lệnh lấy dữ liệu chi tiết cho DataGridView
                string sqlData = "SELECT ct.MaHDBan, hd.NgayBan, SUM(ct.ThanhTien) AS TongTien " +
                                 "FROM tblChiTietHDBan ct JOIN tblHDBan hd ON ct.MaHDBan = hd.MaHDBan " +
                                 "WHERE 1=1 " + dieuKien + " " +
                                 "GROUP BY ct.MaHDBan, hd.NgayBan";

                dgv_DoanhThu.DataSource = lop.LoadDL(sqlData, parameters);

                // Câu lệnh tính tổng doanh thu chính xác từ CSDL theo điều kiện lọc
                string sqlTotal = "SELECT SUM(ct.ThanhTien) " +
                                  "FROM tblChiTietHDBan ct JOIN tblHDBan hd ON ct.MaHDBan = hd.MaHDBan " +
                                  "WHERE 1=1 " + dieuKien;

                object tongObj = lop.LayGT(sqlTotal, parameters);
                if (tongObj != DBNull.Value && tongObj != null)
                {
                    decimal tong = Convert.ToDecimal(tongObj);
                    // Định dạng tiền tệ theo kiểu Việt Nam (dấu chấm ngăn cách hàng nghìn)
                    txt_TongTien.Text = tong.ToString("N0", new CultureInfo("vi-VN")) + " VNĐ";
                }
                else
                {
                    txt_TongTien.Text = "0 VNĐ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            if (dtp_TuNgay.Value.Date > dtp_DenNgay.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xây dựng điều kiện và tham số một cách an toàn
            string dieuKien = "AND hd.NgayBan BETWEEN @TuNgay AND @DenNgay ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TuNgay", dtp_TuNgay.Value.Date),
                new SqlParameter("@DenNgay", dtp_DenNgay.Value.Date.AddDays(1).AddSeconds(-1)) // Lấy hết các giao dịch trong ngày cuối
            };

            string keyword = txt_TuKhoa.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                dieuKien += "AND ct.MaHDBan LIKE @Keyword ";
                parameters.Add(new SqlParameter("@Keyword", "%" + keyword + "%"));
            }

            // Gọi hàm trung tâm để tải dữ liệu
            LoadDataAndCalculateTotal(dieuKien, parameters);
            string sql = "SELECT ...";
            dgv_DoanhThu.DataSource = lop.LoadDL(sql);

            // Gọi hàm tính tổng để cập nhật lại tổng tiền theo kết quả tìm kiếm
            TinhTongTien();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Nút Làm mới
            txt_TuKhoa.Clear();
            dtp_TuNgay.Value = DateTime.Now;
            dtp_DenNgay.Value = DateTime.Now;
            cb_ThongKe.SelectedIndex = 0;
        }

        private void cb_ThongKe_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dieuKien = "";
            var parameters = new List<SqlParameter>();
            DateTime ngayChon = dtp_TuNgay.Value.Date;

            switch (cb_ThongKe.Text)
            {
                case "Ngày":
                    dieuKien = "AND CONVERT(date, hd.NgayBan) = @Ngay";
                    parameters.Add(new SqlParameter("@Ngay", ngayChon));
                    break;
                case "Tuần":
                    // Lấy ngày đầu tuần (Thứ 2)
                    DateTime dauTuan = ngayChon.AddDays(-(int)ngayChon.DayOfWeek + (int)DayOfWeek.Monday);
                    if (ngayChon.DayOfWeek == DayOfWeek.Sunday) dauTuan = dauTuan.AddDays(-7); // Xử lý cho Chủ nhật
                    DateTime cuoiTuan = dauTuan.AddDays(7);
                    dieuKien = "AND hd.NgayBan >= @DauTuan AND hd.NgayBan < @CuoiTuan";
                    parameters.Add(new SqlParameter("@DauTuan", dauTuan));
                    parameters.Add(new SqlParameter("@CuoiTuan", cuoiTuan));
                    break;
                case "Tháng":
                    dieuKien = "AND MONTH(hd.NgayBan) = @Thang AND YEAR(hd.NgayBan) = @Nam";
                    parameters.Add(new SqlParameter("@Thang", ngayChon.Month));
                    parameters.Add(new SqlParameter("@Nam", ngayChon.Year));
                    break;
                case "Năm":
                    dieuKien = "AND YEAR(hd.NgayBan) = @Nam";
                    parameters.Add(new SqlParameter("@Nam", ngayChon.Year));
                    break;
                case "Tất cả":
                default:
                    // Không cần thêm điều kiện
                    break;
            }

            LoadDataAndCalculateTotal(dieuKien, parameters);
        }
        private void TinhTongTien()
        {
            decimal tongTien = 0;

            // Duyệt qua tất cả các dòng trong DataGridView
            foreach (DataGridViewRow row in dgv_DoanhThu.Rows)
            {
                // Kiểm tra để chắc chắn rằng ô không bị rỗng
                if (row.Cells["TongTien"].Value != null)
                {
                    decimal giaTri;
                    // Cố gắng chuyển đổi giá trị của ô sang dạng số (decimal)
                    // Dùng TryParse để tránh lỗi nếu giá trị không phải là số
                    if (decimal.TryParse(row.Cells["TongTien"].Value.ToString(), out giaTri))
                    {
                        // Nếu chuyển đổi thành công, cộng vào tổng
                        tongTien += giaTri;
                    }
                }
            }

            // Định dạng tổng tiền theo kiểu Việt Nam (ví dụ: 1.000.000) và hiển thị
            txt_TongTien.Text = tongTien.ToString("N0", new CultureInfo("vi-VN")) + " VNĐ";
        }

        private void dgv_DoanhThu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
