using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DACSN
{
    public partial class ThueToc : Form
    {
        SqlConnection conn;
        SqlDataAdapter adapter;
        DataTable dtThueToc;

        string username;

        public ThueToc(string user)
        {
            InitializeComponent();
            conn = new SqlConnection(@"Data Source=DESKTOP-IGUJF5O\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True");
            username = user;
        }

        private void ThueToc_Load(object sender, EventArgs e)
        {
            txtTenNhanVien.Text = username;
            LoadHang();
            LoadData();
            cboHinhThucThue.Items.AddRange(new string[] { "Giờ", "Ngày", "Tuần", "Tháng" });
        }

        private void LoadHang()
        {
            string sql = "SELECT MaHang, TenHang FROM tblHang";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cboMaHang.DataSource = dt;
            cboMaHang.DisplayMember = "MaHang";
            cboMaHang.ValueMember = "MaHang";
        }

        private void cboMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaHang.SelectedValue != null)
            {
                string sql = "SELECT TenHang FROM tblHang WHERE MaHang = @MaHang";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaHang", cboMaHang.SelectedValue.ToString());
                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();
                if (result != null)
                txtTenHang.Text = result.ToString();
            }
        }

        private void txtTienDatCoc_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtThanhTien.Text, out decimal thanhTien) &&
        decimal.TryParse(txtTienDatCoc.Text, out decimal datCoc))
            {
                txtConLai.Text = (thanhTien - datCoc).ToString("0.00");
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.png;*.jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtHinhAnh.Text = ofd.FileName;
                picAnh.Image = Image.FromFile(ofd.FileName);
                picAnh.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo một dòng mới trong DataTable
                DataRow newRow = dtThueToc.NewRow();

                newRow["MaPhieuThue"] = txtMaPhieuThue.Text;
                newRow["TenNhanVien"] = txtTenNhanVien.Text;
                newRow["HinhAnhSanPham"] = txtHinhAnh.Text;
                newRow["SoDienThoai"] = txtDienThoai.Text;
                newRow["TenKhachHang"] = txtTenKhachHang.Text;
                newRow["DiaChi"] = txtDiaChi.Text;
                newRow["MaHang"] = cboMaHang.Text;
                newRow["TenHang"] = txtTenHang.Text;
                newRow["HinhThucThue"] = cboHinhThucThue.Text;
                newRow["ThoiGianThue"] = txtThoiGianThue.Text;
                newRow["NgayThue"] = dtpNgayThue.Value;
                newRow["NgayTra"] = dtpNgayTra.Value;

                // Tính toán thành tiền, tiền đặt cọc, còn lại
                decimal thanhTien = 0, tienDatCoc = 0;
                decimal.TryParse(txtThanhTien.Text, out thanhTien);
                decimal.TryParse(txtTienDatCoc.Text, out tienDatCoc);
                decimal conLai = thanhTien - tienDatCoc;

                newRow["ThanhTien"] = thanhTien;
                newRow["TienDatCoc"] = tienDatCoc;
                newRow["ConLai"] = conLai;

                // Thêm dòng vào DataTable (đang liên kết với DataGridView)
                dtThueToc.Rows.Add(newRow);

                // Cập nhật lại DataGridView
                dgvThueToc.DataSource = dtThueToc;

                MessageBox.Show("Đã thêm dữ liệu vào danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Câu lệnh SQL phù hợp với cấu trúc bảng
                string sql = @"INSERT INTO tblThueToc 
        (MaPhieuThue, TenNhanVien, HinhAnhSanPham, SoDienThoai, TenKhachHang, DiaChi, 
         MaHang, TenHang, HinhThucThue, ThoiGianThue, NgayThue, NgayTra, 
         ThanhTien, TienDatCoc)
        VALUES 
        (@MaPhieuThue, @TenNhanVien, @HinhAnhSanPham, @SoDienThoai, @TenKhachHang, @DiaChi, 
         @MaHang, @TenHang, @HinhThucThue, @ThoiGianThue, @NgayThue, @NgayTra, 
         @ThanhTien, @TienDatCoc)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                // Các cột nvarchar
                cmd.Parameters.AddWithValue("@MaPhieuThue", txtMaPhieuThue.Text);
                cmd.Parameters.AddWithValue("@TenNhanVien", txtTenNhanVien.Text);
                cmd.Parameters.AddWithValue("@HinhAnhSanPham", txtHinhAnh.Text);
                cmd.Parameters.AddWithValue("@SoDienThoai", txtDienThoai.Text);
                cmd.Parameters.AddWithValue("@TenKhachHang", txtTenKhachHang.Text);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@MaHang", cboMaHang.Text);
                cmd.Parameters.AddWithValue("@TenHang", txtTenHang.Text);
                cmd.Parameters.AddWithValue("@HinhThucThue", cboHinhThucThue.Text);
                cmd.Parameters.AddWithValue("@ThoiGianThue", txtThoiGianThue.Text);

                // Các cột kiểu ngày
                cmd.Parameters.AddWithValue("@NgayThue", dtpNgayThue.Value);
                cmd.Parameters.AddWithValue("@NgayTra", dtpNgayTra.Value);

                // Chuyển đổi kiểu dữ liệu cho decimal
                decimal thanhTien = 0, tienDatCoc = 0;
                decimal.TryParse(txtThanhTien.Text, out thanhTien);
                decimal.TryParse(txtTienDatCoc.Text, out tienDatCoc);

                cmd.Parameters.Add("@ThanhTien", SqlDbType.Decimal).Value = thanhTien;
                cmd.Parameters.Add("@TienDatCoc", SqlDbType.Decimal).Value = tienDatCoc;

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Đã lưu thông tin thuê tóc vào cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        private void LoadData()
        {
            string sql = "SELECT * FROM tblThueToc";
            adapter = new SqlDataAdapter(sql, conn);
            dtThueToc = new DataTable();
            adapter.Fill(dtThueToc);
            dgvThueToc.DataSource = dtThueToc;
        }

        private void dgvThueToc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvThueToc.Rows[e.RowIndex];

                txtMaPhieuThue.Text = row.Cells["MaPhieuThue"].Value?.ToString();
                txtTenNhanVien.Text = row.Cells["TenNhanVien"].Value?.ToString();
                txtHinhAnh.Text = row.Cells["HinhAnhSanPham"].Value?.ToString();
                txtDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtTenKhachHang.Text = row.Cells["TenKhachHang"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                cboMaHang.Text = row.Cells["MaHang"].Value?.ToString();
                txtTenHang.Text = row.Cells["TenHang"].Value?.ToString();
                cboHinhThucThue.Text = row.Cells["HinhThucThue"].Value?.ToString();
                txtThoiGianThue.Text = row.Cells["ThoiGianThue"].Value?.ToString();

                // Xử lý ngày
                if (DateTime.TryParse(row.Cells["NgayThue"].Value?.ToString(), out DateTime ngayThue))
                    dtpNgayThue.Value = ngayThue;
                else
                    dtpNgayThue.Value = DateTime.Now;

                if (DateTime.TryParse(row.Cells["NgayTra"].Value?.ToString(), out DateTime ngayTra))
                    dtpNgayTra.Value = ngayTra;
                else
                    dtpNgayTra.Value = DateTime.Now;

                // Xử lý số (decimal)
                txtThanhTien.Text = row.Cells["ThanhTien"].Value?.ToString();
                txtTienDatCoc.Text = row.Cells["TienDatCoc"].Value?.ToString();
                txtConLai.Text = row.Cells["ConLai"].Value?.ToString();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaPhieuThue.Text))
                {
                    MessageBox.Show("Vui lòng chọn phiếu thuê cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa phiếu thuê này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    string sql = "DELETE FROM tblThueToc WHERE MaPhieuThue = @MaPhieuThue";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaPhieuThue", txtMaPhieuThue.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Đã xóa phiếu thuê thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaPhieuThue.Text))
                {
                    MessageBox.Show("Vui lòng chọn phiếu thuê cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql = @"UPDATE tblThueToc SET 
                        TenNhanVien = @TenNhanVien,
                        HinhAnhSanPham = @HinhAnhSanPham,
                        SoDienThoai = @SoDienThoai,
                        TenKhachHang = @TenKhachHang,
                        DiaChi = @DiaChi,
                        MaHang = @MaHang,
                        TenHang = @TenHang,
                        HinhThucThue = @HinhThucThue,
                        ThoiGianThue = @ThoiGianThue,
                        NgayThue = @NgayThue,
                        NgayTra = @NgayTra,
                        ThanhTien = @ThanhTien,
                        TienDatCoc = @TienDatCoc
                      WHERE MaPhieuThue = @MaPhieuThue";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MaPhieuThue", txtMaPhieuThue.Text);
                cmd.Parameters.AddWithValue("@TenNhanVien", txtTenNhanVien.Text);
                cmd.Parameters.AddWithValue("@HinhAnhSanPham", txtHinhAnh.Text);
                cmd.Parameters.AddWithValue("@SoDienThoai", txtDienThoai.Text);
                cmd.Parameters.AddWithValue("@TenKhachHang", txtTenKhachHang.Text);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@MaHang", cboMaHang.Text);
                cmd.Parameters.AddWithValue("@TenHang", txtTenHang.Text);
                cmd.Parameters.AddWithValue("@HinhThucThue", cboHinhThucThue.Text);
                cmd.Parameters.AddWithValue("@ThoiGianThue", txtThoiGianThue.Text);
                cmd.Parameters.AddWithValue("@NgayThue", dtpNgayThue.Value);
                cmd.Parameters.AddWithValue("@NgayTra", dtpNgayTra.Value);

                // Chuyển kiểu dữ liệu chính xác cho 3 trường số
                decimal thanhTien = 0, tienDatCoc = 0, conLai = 0;
                decimal.TryParse(txtThanhTien.Text, out thanhTien);
                decimal.TryParse(txtTienDatCoc.Text, out tienDatCoc);
                decimal.TryParse(txtConLai.Text, out conLai);

                cmd.Parameters.AddWithValue("@ThanhTien", thanhTien);
                cmd.Parameters.AddWithValue("@TienDatCoc", tienDatCoc);
                cmd.Parameters.AddWithValue("@ConLai", conLai);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Cập nhật thông tin phiếu thuê thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void txtDienThoai_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Ngăn tiếng “beep” khi nhấn Enter

                string sql = "SELECT TenKhach, DiaChi FROM tblKhach WHERE DienThoai = @SDT";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SDT", txtDienThoai.Text);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtTenKhachHang.Text = reader["TenKhach"].ToString();
                    txtDiaChi.Text = reader["DiaChi"].ToString();
                }
                else
                {
                    MessageBox.Show("Chưa có khách hàng này trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                conn.Close();
            }
        }
    } 
}
