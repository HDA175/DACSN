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
        private List<NhanVien> danhSachNhanVien = new List<NhanVien>();
        DataTable tblNV;
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
            if (dgvNhanVien.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Bạn có muốn sửa thông tin nhân viên này không?",
                                                      "Xác nhận sửa",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int index = dgvNhanVien.CurrentRow.Index;

                    // Cập nhật thông tin trong danh sách
                    string gioiTinh = rbNam.Checked ? "Nam" :
                                      rbNu.Checked ? "Nữ" : "Khác";

                    List<string> caLam = new List<string>();
                    if (cbCa1.Checked) caLam.Add("Ca 1");
                    if (cbCa2.Checked) caLam.Add("Ca 2");
                    if (cbCa3.Checked) caLam.Add("Ca 3");

                    string caLamText = caLam.Count == 3 ? "Cả ngày" : string.Join(", ", caLam);

                    danhSachNhanVien[index] = new NhanVien()
                    {
                        MaNV = txtMaNhanVien.Text,
                        TenNV = txtTenNhanVien.Text,
                        NgaySinh = dtpNgaySinh.Value.ToShortDateString(),
                        GioiTinh = gioiTinh,
                        DiaChi = txtDiaChi.Text,
                        SDT = txtDienThoai.Text,
                        CaLam = caLamText
                    };

                    CapNhatDataGridView();
                    XoaTrang();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dữ liệu nhân viên đã được lưu thành công!", "Thông báo",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?",
                                                      "Xác nhận xóa",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int index = dgvNhanVien.CurrentRow.Index;
                    danhSachNhanVien.RemoveAt(index);
                    CapNhatDataGridView();
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string gioiTinh = rbNam.Checked ? "Nam" :
                             rbNu.Checked ? "Nữ" : "Khác";

            // Xác định ca làm
            List<string> caLam = new List<string>();
            if (cbCa1.Checked) caLam.Add("Ca 1");
            if (cbCa2.Checked) caLam.Add("Ca 2");
            if (cbCa3.Checked) caLam.Add("Ca 3");

            string caLamText = caLam.Count == 3 ? "Cả ngày" : string.Join(", ", caLam);

            // Tạo đối tượng nhân viên
            NhanVien nv = new NhanVien()
            {
                MaNV = txtMaNhanVien.Text,
                TenNV = txtTenNhanVien.Text,
                NgaySinh = dtpNgaySinh.Value.ToShortDateString(),
                GioiTinh = gioiTinh,
                DiaChi = txtDiaChi.Text,
                SDT = txtDienThoai.Text,
                CaLam = caLamText
            };

            // Thêm vào danh sách
            danhSachNhanVien.Add(nv);

            // Cập nhật DataGridView
            CapNhatDataGridView();

            // Xóa dữ liệu nhập
            XoaTrang();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
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

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
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

        private void txtMaNhanVien_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenNhanVien_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDienThoai_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtChucVu_TextChanged(object sender, EventArgs e)
        {

        }

        private void DTPngaysinh_ValueChanged(object sender, EventArgs e)
        {

        }
        public void LoadDataGridView()
        {
            
        }
        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            dgvNhanVien.Columns.Add("STT", "STT");
            dgvNhanVien.Columns.Add("MaNV", "Mã nhân viên");
            dgvNhanVien.Columns.Add("TenNV", "Tên nhân viên");
            dgvNhanVien.Columns.Add("NgaySinh", "Ngày sinh");
            dgvNhanVien.Columns.Add("GioiTinh", "Giới tính");
            dgvNhanVien.Columns.Add("DiaChi", "Địa chỉ");
            dgvNhanVien.Columns.Add("SDT", "Số điện thoại");
            dgvNhanVien.Columns.Add("CaLam", "Ca làm");

            dgvNhanVien.AllowUserToAddRows = false;
        }

        private void dgvNhanVien_Click(object sender, EventArgs e)
        {
           
        }
        private void CapNhatDataGridView()
        {
            dgvNhanVien.Rows.Clear();
            for (int i = 0; i < danhSachNhanVien.Count; i++)
            {
                var nv = danhSachNhanVien[i];
                dgvNhanVien.Rows.Add(i + 1, nv.MaNV, nv.TenNV, nv.NgaySinh, nv.GioiTinh, nv.DiaChi, nv.SDT, nv.CaLam);
            }
        }

        private void XoaTrang()
        {
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtDiaChi.Clear();
            txtDienThoai.Clear();
            rbNam.Checked = true;
            cbCa1.Checked = cbCa2.Checked = cbCa3.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
            txtMaNhanVien.Focus();
        }
        public class NhanVien
        {
            public string MaNV { get; set; }
            public string TenNV { get; set; }
            public string NgaySinh { get; set; }
            public string GioiTinh { get; set; }
            public string DiaChi { get; set; }
            public string SDT { get; set; }
            public string CaLam { get; set; }
        }

        private void txtDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Chỉ được nhập số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < danhSachNhanVien.Count)
            {
                NhanVien nv = danhSachNhanVien[e.RowIndex];

                txtMaNhanVien.Text = nv.MaNV;
                txtTenNhanVien.Text = nv.TenNV;
                txtDiaChi.Text = nv.DiaChi;
                txtDienThoai.Text = nv.SDT;

                // Ngày sinh
                DateTime ngay;
                if (DateTime.TryParse(nv.NgaySinh, out ngay))
                    dtpNgaySinh.Value = ngay;

                // Giới tính
                rbNam.Checked = nv.GioiTinh == "Nam";
                rbNu.Checked = nv.GioiTinh == "Nữ";
                rbKhac.Checked = nv.GioiTinh == "Khác";

                // Ca làm
                cbCa1.Checked = nv.CaLam.Contains("Ca 1");
                cbCa2.Checked = nv.CaLam.Contains("Ca 2");
                cbCa3.Checked = nv.CaLam.Contains("Ca 3");
            }
        }
    }
}
