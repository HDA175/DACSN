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
    public partial class ThueToc2 : Form
    {
        SqlConnection conn;
        SqlDataAdapter adapter;
        DataTable dtTraHang;
        string connectionString = @"Data Source=DESKTOP-IGUJF5O\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

        public ThueToc2()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void ThueToc2_Load(object sender, EventArgs e)
        {
            LoadMaPhieuThue();
            LoadTinhTrangTra();
            LoadData();
        }

        private void LoadMaPhieuThue()
        {
            cboMaPhieuThue.Items.Clear();
            string sql = "SELECT MaPhieuThue FROM tblThueToc";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    cboMaPhieuThue.Items.Add(rd["MaPhieuThue"].ToString());
                }
                conn.Close();
            }
        }

        private void LoadTinhTrangTra()
        {
            cboTinhTrangTra.Items.Clear();
            cboTinhTrangTra.Items.AddRange(new string[]
            {
        "Bình thường",
        "Hư nhẹ",
        "Mất phụ kiện",
        "Hư hỏng nặng",
        "Khách báo lỗi"
            });
        }

        private void cboMaPhieuThue_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM tblThueToc WHERE MaPhieuThue = @MaPhieuThue";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaPhieuThue", cboMaPhieuThue.Text);
                conn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    txtTenNhanVien.Text = rd["TenNhanVien"].ToString();
                    txtDienThoai.Text = rd["SoDienThoai"].ToString();
                    txtTenKhachHang.Text = rd["TenKhachHang"].ToString();
                    txtDiaChi.Text = rd["DiaChi"].ToString();
                    txtMaHang.Text = rd["MaHang"].ToString();
                    txtTenHang.Text = rd["TenHang"].ToString();
                    dtpNgayThue.Value = Convert.ToDateTime(rd["NgayThue"]);
                    dtpNgayTra.Value = Convert.ToDateTime(rd["NgayTra"]);
                    txtPhiThue.Text = rd["ThanhTien"].ToString();
                }
                conn.Close();
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtPathHinhAnh.Text = ofd.FileName;
                picAnh.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void TinhTongTien()
        {
            decimal phiThue = 0, phiTreHen = 0, phiPhatSinh = 0;

            decimal.TryParse(txtPhiThue.Text, out phiThue);
            decimal.TryParse(txtPhiTreHen.Text, out phiTreHen);
            decimal.TryParse(txtPhiPhatSinh.Text, out phiPhatSinh);

            txtTongTien.Text = (phiThue + phiTreHen + phiPhatSinh).ToString("0.00");
        }
        private void txtPhiTreHen_TextChanged(object sender, EventArgs e) => TinhTongTien();
        private void txtPhiPhatSinh_TextChanged(object sender, EventArgs e) => TinhTongTien();

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"INSERT INTO tblTraToc
            (MaPhieuTra, MaPhieuThue, TenNhanVien, SoDienThoai, TenKhachHang, DiaChi, MaHang, TenHang,
             NgayThue, NgayTra, PhiThue, PhiTreHen, PhiPhatSinh, TinhTrangTra, GhiChu, HinhAnhSanPham)
            VALUES
            (@MaPhieuTra, @MaPhieuThue, @TenNhanVien, @SoDienThoai, @TenKhachHang, @DiaChi, @MaHang, @TenHang,
             @NgayThue, @NgayTra, @PhiThue, @PhiTreHen, @PhiPhatSinh, @TinhTrangTra, @GhiChu, @HinhAnhSanPham)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhieuTra", txtMaPhieuTra.Text);
                    cmd.Parameters.AddWithValue("@MaPhieuThue", cboMaPhieuThue.Text);
                    cmd.Parameters.AddWithValue("@TenNhanVien", txtTenNhanVien.Text);
                    cmd.Parameters.AddWithValue("@SoDienThoai", txtDienThoai.Text);
                    cmd.Parameters.AddWithValue("@TenKhachHang", txtTenKhachHang.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@MaHang", txtMaHang.Text);
                    cmd.Parameters.AddWithValue("@TenHang", txtTenHang.Text);
                    cmd.Parameters.AddWithValue("@NgayThue", dtpNgayThue.Value);
                    cmd.Parameters.AddWithValue("@NgayTra", dtpNgayTra.Value);
                    cmd.Parameters.AddWithValue("@PhiThue", string.IsNullOrWhiteSpace(txtPhiThue.Text) ? 0 : Convert.ToDecimal(txtPhiThue.Text));
                    cmd.Parameters.AddWithValue("@PhiTreHen", string.IsNullOrWhiteSpace(txtPhiTreHen.Text) ? 0 : Convert.ToDecimal(txtPhiTreHen.Text));
                    cmd.Parameters.AddWithValue("@PhiPhatSinh", string.IsNullOrWhiteSpace(txtPhiPhatSinh.Text) ? 0 : Convert.ToDecimal(txtPhiPhatSinh.Text));
                    cmd.Parameters.AddWithValue("@TinhTrangTra", cboTinhTrangTra.Text);
                    cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
                    cmd.Parameters.AddWithValue("@HinhAnhSanPham", txtPathHinhAnh.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Thêm dữ liệu thành công!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message);
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void LoadData()
        {
            string sql = "SELECT * FROM tblTraToc";
            adapter = new SqlDataAdapter(sql, conn);
            dtTraHang = new DataTable();
            adapter.Fill(dtTraHang);
            dgvTraHang.DataSource = dtTraHang;
        }

        private void dgvTraHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTraHang.Rows[e.RowIndex];
                txtMaPhieuTra.Text = row.Cells["MaPhieuTra"].Value.ToString();
                cboMaPhieuThue.Text = row.Cells["MaPhieuThue"].Value.ToString();
                txtTenNhanVien.Text = row.Cells["TenNhanVien"].Value.ToString();
                txtDienThoai.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtTenKhachHang.Text = row.Cells["TenKhachHang"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtMaHang.Text = row.Cells["MaHang"].Value.ToString();
                txtTenHang.Text = row.Cells["TenHang"].Value.ToString();
                dtpNgayThue.Value = Convert.ToDateTime(row.Cells["NgayThue"].Value);
                dtpNgayTra.Value = Convert.ToDateTime(row.Cells["NgayTra"].Value);
                txtPhiThue.Text = row.Cells["PhiThue"].Value.ToString();
                txtPhiTreHen.Text = row.Cells["PhiTreHen"].Value.ToString();
                txtPhiPhatSinh.Text = row.Cells["PhiPhatSinh"].Value.ToString();
                txtTongTien.Text = row.Cells["TongTien"].Value.ToString();
                cboTinhTrangTra.Text = row.Cells["TinhTrangTra"].Value.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value.ToString();
                txtPathHinhAnh.Text = row.Cells["HinhAnhSanPham"].Value.ToString();

                if (System.IO.File.Exists(txtPathHinhAnh.Text))
                    picAnh.Image = Image.FromFile(txtPathHinhAnh.Text);
                else
                    picAnh.Image = null;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql = @"UPDATE tblTraToc SET
                   MaPhieuThue=@MaPhieuThue, TenNhanVien=@TenNhanVien, SoDienThoai=@SoDienThoai,
                   TenKhachHang=@TenKhachHang, DiaChi=@DiaChi, MaHang=@MaHang, TenHang=@TenHang,
                   NgayThue=@NgayThue, NgayTra=@NgayTra, PhiThue=@PhiThue, PhiTreHen=@PhiTreHen,
                   PhiPhatSinh=@PhiPhatSinh, TongTien=@TongTien, TinhTrangTra=@TinhTrangTra,
                   GhiChu=@GhiChu, HinhAnhSanPham=@HinhAnhSanPham
                   WHERE MaPhieuTra=@MaPhieuTra";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaPhieuTra", txtMaPhieuTra.Text);
                cmd.Parameters.AddWithValue("@MaPhieuThue", cboMaPhieuThue.Text);
                cmd.Parameters.AddWithValue("@TenNhanVien", txtTenNhanVien.Text);
                cmd.Parameters.AddWithValue("@SoDienThoai", txtDienThoai.Text);
                cmd.Parameters.AddWithValue("@TenKhachHang", txtTenKhachHang.Text);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@MaHang", txtMaHang.Text);
                cmd.Parameters.AddWithValue("@TenHang", txtTenHang.Text);
                cmd.Parameters.AddWithValue("@NgayThue", dtpNgayThue.Value);
                cmd.Parameters.AddWithValue("@NgayTra", dtpNgayTra.Value);
                cmd.Parameters.AddWithValue("@PhiThue", string.IsNullOrWhiteSpace(txtPhiThue.Text) ? 0 : Convert.ToDecimal(txtPhiThue.Text));
                cmd.Parameters.AddWithValue("@PhiTreHen", string.IsNullOrWhiteSpace(txtPhiTreHen.Text) ? 0 : Convert.ToDecimal(txtPhiTreHen.Text));
                cmd.Parameters.AddWithValue("@PhiPhatSinh", string.IsNullOrWhiteSpace(txtPhiPhatSinh.Text) ? 0 : Convert.ToDecimal(txtPhiPhatSinh.Text));
                cmd.Parameters.AddWithValue("@TongTien", txtTongTien.Text);
                cmd.Parameters.AddWithValue("@TinhTrangTra", cboTinhTrangTra.Text);
                cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
                cmd.Parameters.AddWithValue("@HinhAnhSanPham", txtPathHinhAnh.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            MessageBox.Show("Cập nhật thành công!");
            LoadData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa phiếu này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = "DELETE FROM tblTraToc WHERE MaPhieuTra = @MaPhieuTra";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhieuTra", txtMaPhieuTra.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Xóa thành công!");
                LoadData();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming Soon!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
