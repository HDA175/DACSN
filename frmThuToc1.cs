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
    public partial class frmThuToc1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=MSI\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True");
        public frmThuToc1()
        {
            InitializeComponent();
        }

        private void btnNhanvien_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnThongTin_Click(object sender, EventArgs e)
        {
            string thongTin =
      "BẢNG GIÁ THU MUA TÓC DÀI\n\n" +
      "Chiều dài tóc\t\tĐơn giá (VNĐ/kg)\n" +
      "--------------------------------------\n" +
      "40 – 49 cm\t\t599,000\n" +
      "50 – 59 cm\t\t699,000\n" +
      "60 – 69 cm\t\t859,000\n" +
      "70 – 79 cm\t\t959,000\n" +
      "80 – 89 cm\t\t1,059,000\n" +
      "90 – 100 cm\t\t1,159,000\n" +
      "\n(Dưới 40 cm không thu mua)";

            MessageBox.Show(thongTin, "Thông tin bảng giá", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txthoantra_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmThuToc1_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void TinhThanhTien()
        {
            if (double.TryParse(txtChieuDai.Text, out double chieuDai) &&
                double.TryParse(txtSk.Text, out double soKy))
            {
                double donGia = 0;

                if (chieuDai >= 40 && chieuDai <= 49)
                    donGia = 599000;
                else if (chieuDai >= 50 && chieuDai <= 59)
                    donGia = 699000;
                else if (chieuDai >= 60 && chieuDai <= 69)
                    donGia = 859000;
                else if (chieuDai >= 70 && chieuDai <= 79)
                    donGia = 959000;
                else if (chieuDai >= 80 && chieuDai <= 89)
                    donGia = 1059000;
                else if (chieuDai >= 90 && chieuDai <= 100)
                    donGia = 1159000;
                else
                    donGia = 0; // Dưới 40 cm không tính tiền

                double thanhTien = soKy * donGia;

                // Giảm 2% nếu tình trạng xấu
                if (cboTinhTrangToc.Text == "Xấu")
                    thanhTien *= 0.98;

                txtThanhtien.Text = thanhTien.ToString("N2");
            }
            else
            {
                txtThanhtien.Text = "";
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*";
            dlgOpen.FilterIndex = 2;
            dlgOpen.Title = "Chọn ảnh minh hoạ cho sản phẩm";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                picAnh.Image = Image.FromFile(dlgOpen.FileName);
            }
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string sql = "INSERT INTO tblThuToc(NgayThu, ChieuDai, TinhTrangSoiToc, SoKy, ThanhTien) " +
                             "VALUES(@NgayThu, @ChieuDai, @TinhTrangSoiToc, @SoKy, @ThanhTien)";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@NgayThu", dtpngaythu.Value);
                cmd.Parameters.AddWithValue("@ChieuDai", txtChieuDai.Text);
                cmd.Parameters.AddWithValue("@TinhTrangSoiToc", cboTinhTrangToc.Text);
                cmd.Parameters.AddWithValue("@SoKy", double.Parse(txtSk.Text));
                cmd.Parameters.AddWithValue("@ThanhTien", decimal.Parse(txtThanhtien.Text.Replace(",", "")));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Đã nhập dữ liệu thành công!", "Thông báo");

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhập: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        private void LoadData()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tblThuToc", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvThuToc.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnXuatPhieu_Click(object sender, EventArgs e)
        {
         MessageBox.Show("Xuất phiếu thu tóc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtChieuDai_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTien();
        }

        private void txtSk_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTien();
        }

        private void cboTinhTrangToc_SelectedIndexChanged(object sender, EventArgs e)
        {
            TinhThanhTien();
        }
    }
}
