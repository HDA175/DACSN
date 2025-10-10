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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // wire additional events not set in designer
            this.btnExit.Click += btnExit_Click;
            this.guna2Button1.Click += guna2Button1_Click; // Đăng ký
            this.label4.Click += label4_Click; // Quên mật khẩu
            this.Load += Form1_Load;
            this.txtLogin.KeyDown += Input_KeyDown;
            this.txtPassword.KeyDown += Input_KeyDown;
            this.txtPassword.DoubleClick += txtPassword_DoubleClick;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            labelError.Visible = false;
			// Basic validations
            var username = txtLogin.Text.Trim();
            var password = txtPassword.Text;
            var role = guna2ComboBox1.SelectedItem as string;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vui lòng chọn chức danh", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2ComboBox1.DroppedDown = true;
                return;
            }
            if (!chkAgree.Checked)
            {
                MessageBox.Show("Bạn cần đồng ý với điều khoản để tiếp tục", "Điều khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                EnsureConnected();
                EnsureAuthSchema();

                // Compare against table tblTaiKhoan (TenDangNhap, MatKhauHash, ChucDanh)
                using (var cmd = new SqlCommand(@"SELECT TOP 1 TenDangNhap, ChucDanh
FROM tblTaiKhoan
WHERE TenDangNhap = @u AND MatKhauHash = @h AND ChucDanh = @r", Function.con))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@h", ComputeSha256(password));
                    cmd.Parameters.AddWithValue("@r", role);

                    var reader = cmd.ExecuteReader();
                    bool ok = reader.HasRows;
                    reader.Close();

                    if (ok)
                    {
                        // Success: open main form
                        Program.CurrentUsername = username;
                        Program.CurrentRole = role;
                        this.Hide();
                        using (var main = new Trangchu())
                        {
                            main.ShowDialog();
                        }
                        this.Close();
                        return;
                    }
                }

                // If not matched by hash, optionally try plain (in case DB stores plain text)
                using (var cmd2 = new SqlCommand(@"SELECT TOP 1 TenDangNhap, ChucDanh
FROM tblTaiKhoan
WHERE TenDangNhap = @u AND MatKhau = @p AND ChucDanh = @r", Function.con))
                {
                    cmd2.Parameters.AddWithValue("@u", username);
                    cmd2.Parameters.AddWithValue("@p", password);
                    cmd2.Parameters.AddWithValue("@r", role);

                    var reader2 = cmd2.ExecuteReader();
                    bool ok2 = reader2.HasRows;
                    reader2.Close();
                    if (ok2)
                    {
                        Program.CurrentUsername = username;
                        Program.CurrentRole = role;
                        this.Hide();
                        using (var main = new Trangchu())
                        {
                            main.ShowDialog();
                        }
                        this.Close();
                        return;
                    }
                }

                // Failed
                labelError.Visible = true;
                txtPassword.SelectAll();
                txtPassword.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnExit.PerformClick();
            }
        }

        private void txtPassword_DoubleClick(object sender, EventArgs e)
        {
            // Toggle show/hide password on double click
            txtPassword.PasswordChar = txtPassword.PasswordChar == '\0' ? '*' : '\0';
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelError.Visible = false;
            // Populate roles if empty
            if (guna2ComboBox1.Items.Count == 0)
            {
                guna2ComboBox1.Items.Add("Quản trị");
                guna2ComboBox1.Items.Add("Nhân viên");
            }
            if (guna2ComboBox1.Items.Count > 0 && guna2ComboBox1.SelectedIndex < 0)
            {
                guna2ComboBox1.SelectedIndex = 0;
            }

            // Open connection once
            try { EnsureConnected(); EnsureAuthSchema(); } catch { /* ignore here, show later if needed */ }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Open register form
            using (var dk = new Dangky())
            {
                dk.ShowDialog();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ quản trị để đặt lại mật khẩu.", "Quên mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            // Create tblTaiKhoan if missing and seed default admin
            if (!TableExists("tblTaiKhoan"))
            {
                using (var create = new SqlCommand(@"
    CREATE TABLE dbo.tblTaiKhoan (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(100) NOT NULL UNIQUE,
    MatKhauHash NVARCHAR(64) NULL,
    MatKhau NVARCHAR(255) NULL,
    ChucDanh NVARCHAR(100) NOT NULL
);
", Function.con))
                {
                    create.ExecuteNonQuery();
                }
            }

            // Seed default admin if none
            using (var hasAny = new SqlCommand("SELECT TOP 1 1 FROM dbo.tblTaiKhoan", Function.con))
            {
                var any = hasAny.ExecuteScalar();
                if (any == null)
                {
                    using (var seed = new SqlCommand(@"INSERT INTO dbo.tblTaiKhoan (TenDangNhap, MatKhauHash, ChucDanh)
                        VALUES (@u, @h, @r)", Function.con))
                    {
                        seed.Parameters.AddWithValue("@u", "admin");
                        seed.Parameters.AddWithValue("@h", ComputeSha256("admin"));
                        seed.Parameters.AddWithValue("@r", "Quản trị");
                        seed.ExecuteNonQuery();
                    }
                }
            }
        }

        private void Login_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
