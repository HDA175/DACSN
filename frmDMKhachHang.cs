using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DACSN
{
    public partial class frmDMKhachHang : Form
    {
        string connectionString = @"Data Source=DESKTOP-IGUJF5O\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";
        SqlConnection conn;
        SqlDataAdapter adapter;
        DataTable dtKhach;

        public frmDMKhachHang()
        {
            InitializeComponent();
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmDMKhachHang_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectionString);
            LoadData();
            dgvKhachHang.ReadOnly = true;
            dgvKhachHang.AllowUserToAddRows = false;
            dgvKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaKhach.Text == "" || txtTenKhach.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                conn.Open();
                string sql = "INSERT INTO tblKhach VALUES (@Ma, @Ten, @DiaChi, @SDT)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Ma", txtMaKhach.Text);
                cmd.Parameters.AddWithValue("@Ten", txtTenKhach.Text);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@SDT", mtbSDT.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm khách hàng thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm khách hàng: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            LoadData();
            ClearFields();
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaKhach.Text = dgvKhachHang.Rows[e.RowIndex].Cells["MaKhach"].Value.ToString();
                txtTenKhach.Text = dgvKhachHang.Rows[e.RowIndex].Cells["TenKhach"].Value.ToString();
                txtDiaChi.Text = dgvKhachHang.Rows[e.RowIndex].Cells["DiaChi"].Value.ToString();
                mtbSDT.Text = dgvKhachHang.Rows[e.RowIndex].Cells["DienThoai"].Value.ToString();
            }

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow == null) return;

            DialogResult result = MessageBox.Show("Bạn có muốn sửa thông tin khách hàng này không?",
                                                  "Xác nhận sửa",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE tblKhach SET TenKhach=@Ten, DiaChi=@DiaChi, DienThoai=@SDT WHERE MaKhach=@Ma";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Ma", txtMaKhach.Text);
                    cmd.Parameters.AddWithValue("@Ten", txtTenKhach.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@SDT", mtbSDT.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi sửa khách hàng: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                LoadData();
                ClearFields();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow == null) return;

            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này không?",
                                                  "Xác nhận xóa",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    string sql = "DELETE FROM tblKhach WHERE MaKhach=@Ma";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Ma", txtMaKhach.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa khách hàng: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                LoadData();
                ClearFields();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tất cả thay đổi đã được lưu vào CSDL!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadData()
        {
            try
            {
                conn.Open();
                string sql = "SELECT * FROM tblKhach";
                adapter = new SqlDataAdapter(sql, conn);
                dtKhach = new DataTable();
                adapter.Fill(dtKhach);
                dgvKhachHang.DataSource = dtKhach;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void ClearFields()
        {
            txtMaKhach.Clear();
            txtTenKhach.Clear();
            txtDiaChi.Clear();
            mtbSDT.Clear();
            txtMaKhach.Focus();
        }

        private void mtbSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Số điện thoại chỉ được nhập số!", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtRefresh_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
