using DACSN.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDevHtmlRenderer.Adapters;

namespace DACSN
{

    public partial class frmNhanVien : Form
    {
        private string connectionString = @"Data Source=DESKTOP-IGUJF5O\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";
        private DataTable dtNhanVien = new DataTable();
        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Xác nhận
            if (MessageBox.Show("Bạn có chắc muốn cập nhật thông tin nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string ma = txtMaNhanVien.Text.Trim();
            string gioiTinh = GetGioiTinhFromControls();
            string caLam = GetCaLamFromControls();
            int tongGioLam = 0;
            if (!string.IsNullOrWhiteSpace(txtTongGioLam.Text))
            {
                if (!int.TryParse(txtTongGioLam.Text.Trim(), out tongGioLam))
                {
                    MessageBox.Show("Tổng giờ làm phải là số nguyên.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTongGioLam.Focus();
                    return;
                }
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"UPDATE tblNhanVien SET
                                   TenNhanVien=@Ten, GioiTinh=@GioiTinh, DiaChi=@DiaChi, DienThoai=@DienThoai,
                                   NgaySinh=@NgaySinh, CaLam=@CaLam, TongGioLam=@TongGioLam
                                   WHERE MaNhanVien=@Ma";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma", ma);
                        cmd.Parameters.AddWithValue("@Ten", txtTenNhanVien.Text.Trim());
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                        cmd.Parameters.AddWithValue("@DienThoai", txtDienThoai.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                        cmd.Parameters.AddWithValue("@CaLam", caLam);
                        cmd.Parameters.AddWithValue("@TongGioLam", tongGioLam);

                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dữ liệu đã được cập nhật trong cơ sở dữ liệu.", "Lưu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string ma = dgvNhanVien.CurrentRow.Cells["MaNhanVien"].Value.ToString();

            if (MessageBox.Show("Bạn có chắc muốn xóa nhân viên có mã: " + ma + " ?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = "DELETE FROM tblNhanVien WHERE MaNhanVien=@Ma";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma", ma);
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text) || string.IsNullOrWhiteSpace(txtTenNhanVien.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã và Tên nhân viên.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string gioiTinh = GetGioiTinhFromControls();
            string caLam = GetCaLamFromControls();
            int tongGioLam = 0;
            if (!string.IsNullOrWhiteSpace(txtTongGioLam.Text))
            {
                if (!int.TryParse(txtTongGioLam.Text.Trim(), out tongGioLam))
                {
                    MessageBox.Show("Tổng giờ làm phải là số nguyên.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTongGioLam.Focus();
                    return;
                }
            }

            // Thực hiện INSERT bằng using để đảm bảo đóng connection
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"INSERT INTO tblNhanVien
                                   (MaNhanVien, TenNhanVien, GioiTinh, DiaChi, DienThoai, NgaySinh, CaLam, TongGioLam)
                                   VALUES
                                   (@Ma, @Ten, @GioiTinh, @DiaChi, @DienThoai, @NgaySinh, @CaLam, @TongGioLam)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma", txtMaNhanVien.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ten", txtTenNhanVien.Text.Trim());
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                        cmd.Parameters.AddWithValue("@DienThoai", txtDienThoai.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                        cmd.Parameters.AddWithValue("@CaLam", caLam);
                        cmd.Parameters.AddWithValue("@TongGioLam", tongGioLam);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearFields();
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                // Primary key hoặc unique constraint violation
                MessageBox.Show("Mã nhân viên đã tồn tại. Vui lòng sử dụng mã khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhanVien.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadData();
        }
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = "SELECT MaNhanVien, TenNhanVien, NgaySinh, GioiTinh, DiaChi, DienThoai, CaLam, TongGioLam FROM tblNhanVien";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        dtNhanVien.Clear();
                        da.Fill(dtNhanVien);
                        dgvNhanVien.DataSource = dtNhanVien;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
     
        private void txtDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

            txtMaNhanVien.Text = row.Cells["MaNhanVien"].Value?.ToString() ?? "";
            txtTenNhanVien.Text = row.Cells["TenNhanVien"].Value?.ToString() ?? "";
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
            txtDienThoai.Text = row.Cells["DienThoai"].Value?.ToString() ?? "";
            txtTongGioLam.Text = row.Cells["TongGioLam"].Value?.ToString() ?? "";

            // Ngày sinh
            DateTime dt;
            if (DateTime.TryParse(row.Cells["NgaySinh"].Value?.ToString(), out dt))
                dtpNgaySinh.Value = dt;
            else
                dtpNgaySinh.Value = DateTime.Now;

            // Giới tính
            string gt = row.Cells["GioiTinh"].Value?.ToString() ?? "";
            rbNam.Checked = gt == "Nam";
            rbNu.Checked = gt == "Nữ";
            rbKhac.Checked = gt == "Khác";

            // Ca làm: cập nhật checkbox
            string ca = row.Cells["CaLam"].Value?.ToString() ?? "";
            if (ca.Equals("Cả ngày", StringComparison.OrdinalIgnoreCase))
            {
                cbCa1.Checked = cbCa2.Checked = cbCa3.Checked = true;
            }
            else
            {
                cbCa1.Checked = ca.Contains("Ca 1");
                cbCa2.Checked = ca.Contains("Ca 2");
                cbCa3.Checked = ca.Contains("Ca 3");
            }
        }
        private string GetGioiTinhFromControls()
        {
            if (rbNam.Checked) return "Nam";
            if (rbNu.Checked) return "Nữ";
            return "Khác";
        }
        private string GetCaLamFromControls()
        {
            List<string> list = new List<string>();
            if (cbCa1.Checked) list.Add("Ca 1");
            if (cbCa2.Checked) list.Add("Ca 2");
            if (cbCa3.Checked) list.Add("Ca 3");

            if (list.Count == 3) return "Cả ngày";
            return string.Join(", ", list);
        }
        private void ClearFields()
        {
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtDiaChi.Clear();
            txtDienThoai.Clear();
            txtTongGioLam.Clear();
            rbNam.Checked = true; // mặc định
            cbCa1.Checked = cbCa2.Checked = cbCa3.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
            txtMaNhanVien.Focus();
        }

        private void txtTongGioLam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnLuong_Click(object sender, EventArgs e)
        {
            FormBangLuong frm = new FormBangLuong();
            frm.ShowDialog();
        }
    }
}
