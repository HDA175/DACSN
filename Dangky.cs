using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DACSN.Class;
using System.Security.Cryptography;

namespace DACSN
{
    public partial class Dangky : Form
    {
        public Dangky()
        {
            InitializeComponent();
            this.Load += Dangky_Load;
            this.guna2Button1.Click += guna2Button1_Click; // Đăng ký
            this.btnExit.Click += BtnExit_Click;
            this.txtLogin.KeyDown += Input_KeyDown;
            this.txtPassword.KeyDown += Input_KeyDown;
            this.guna2TextBox1.KeyDown += Input_KeyDown; // confirm
        }

        private void Dangky_Load(object sender, EventArgs e)
        {
            try { EnsureConnected(); EnsureAuthSchema(); } catch { }
            if (this.guna2ComboBox1.Items.Count == 0)
            {
                this.guna2ComboBox1.Items.Add("Quản trị");
                this.guna2ComboBox1.Items.Add("Nhân viên");
            }
            if (this.guna2ComboBox1.SelectedIndex < 0 && this.guna2ComboBox1.Items.Count > 0)
            {
                this.guna2ComboBox1.SelectedIndex = 1; // mặc định Nhân viên
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button1.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var username = txtLogin.Text.Trim();
            var password = txtPassword.Text;
            var confirm  = guna2TextBox1.Text;
            var role = guna2ComboBox1.SelectedItem as string;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tài khoản", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }
            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp", "Sai xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2TextBox1.Focus();
                return;
            }

            try
            {
                EnsureConnected();
                EnsureAuthSchema();

                // Kiểm tra trùng tài khoản
                using (var check = new SqlCommand("SELECT 1 FROM dbo.tblTaiKhoan WHERE TenDangNhap=@u", Function.con))
                {
                    check.Parameters.AddWithValue("@u", username);
                    var exists = check.ExecuteScalar();
                    if (exists != null)
                    {
                        MessageBox.Show("Tài khoản đã tồn tại", "Trùng tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtLogin.Focus();
                        txtLogin.SelectAll();
                        return;
                    }
                }

                if (string.IsNullOrEmpty(role))
                {
                    MessageBox.Show("Vui lòng chọn chức danh", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    guna2ComboBox1.DroppedDown = true;
                    return;
                }

                // Lưu tài khoản mới theo chức danh đã chọn
                using (var ins = new SqlCommand(@"INSERT INTO dbo.tblTaiKhoan (TenDangNhap, MatKhauHash, ChucDanh)
VALUES (@u, @h, @r)", Function.con))
                {
                    ins.Parameters.AddWithValue("@u", username);
                    ins.Parameters.AddWithValue("@h", ComputeSha256(password));
                    ins.Parameters.AddWithValue("@r", role);
                    ins.ExecuteNonQuery();
                }

                MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void EnsureConnected()
        {
            if (Function.con == null || Function.con.State != System.Data.ConnectionState.Open)
            {
                Function.Connect();
            }
        }

        private static string ComputeSha256(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);
                var sb = new StringBuilder(hash.Length * 2);
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private static bool TableExists(string tableName)
        {
            using (var cmd = new SqlCommand(@"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @t", Function.con))
            {
                cmd.Parameters.AddWithValue("@t", tableName);
                var o = cmd.ExecuteScalar();
                return o != null;
            }
        }

        private static void EnsureAuthSchema()
        {
            if (!TableExists("tblTaiKhoan"))
            {
                using (var create = new SqlCommand(@"CREATE TABLE dbo.tblTaiKhoan (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(100) NOT NULL UNIQUE,
    MatKhauHash NVARCHAR(64) NULL,
    MatKhau NVARCHAR(255) NULL,
    ChucDanh NVARCHAR(100) NOT NULL);", Function.con))
                {
                    create.ExecuteNonQuery();
                }
            }
        }
    }
}
