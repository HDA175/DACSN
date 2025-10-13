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

namespace DACSN
{
    public partial class frmDMKhachHang : Form
    {
        DataTable dtKhachHang = new DataTable();
        int index = -1;
        public frmDMKhachHang()
        {
            InitializeComponent();
        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void mtbDienThoai_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmDMKhachHang_Load(object sender, EventArgs e)
        {
            dgvKhachHang.Columns.Add("STT", "STT");
            dtKhachHang.Columns.Add("Mã KH");
            dtKhachHang.Columns.Add("Tên KH");
            dtKhachHang.Columns.Add("SĐT");
            dtKhachHang.Columns.Add("Địa chỉ");
            dgvKhachHang.DataSource = dtKhachHang;
            dgvKhachHang.AllowUserToAddRows = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaKhach.Text == "" || txtTenKhach.Text == "" || mtbDienThoai.Text == "" || txtDiaChi.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                return;
            }

            foreach (char c in mtbDienThoai.Text)
            {
                if (!char.IsDigit(c))
                {
                    MessageBox.Show("Số điện thoại chỉ được chứa số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mtbDienThoai.Focus();
                    return;
                }
            }

            dtKhachHang.Rows.Add(txtMaKhach.Text, txtTenKhach.Text, mtbDienThoai.Text, txtDiaChi.Text);

            txtMaKhach.Clear();
            txtTenKhach.Clear();
            mtbDienThoai.Clear();
            txtDiaChi.Clear();
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                index = e.RowIndex;
                txtMaKhach.Text = dgvKhachHang.Rows[index].Cells[0].Value.ToString();
                txtTenKhach.Text = dgvKhachHang.Rows[index].Cells[1].Value.ToString();
                mtbDienThoai.Text = dgvKhachHang.Rows[index].Cells[2].Value.ToString();
                txtDiaChi.Text = dgvKhachHang.Rows[index].Cells[3].Value.ToString();
            }

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (index < 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa!", "Thông báo");
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn sửa khách hàng này?",
                                              "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                // Cập nhật lại thông tin trong DataTable
                dtKhachHang.Rows[index]["Mã KH"] = txtMaKhach.Text;
                dtKhachHang.Rows[index]["Tên KH"] = txtTenKhach.Text;
                dtKhachHang.Rows[index]["SĐT"] = mtbDienThoai.Text;
                dtKhachHang.Rows[index]["Địa chỉ"] = txtDiaChi.Text;
                MessageBox.Show("Đã cập nhật thông tin khách hàng!", "Thông báo");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (index < 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo");
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?",
                                              "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                dtKhachHang.Rows[index].Delete();
                txtMaKhach.Clear();
                txtTenKhach.Clear();
                mtbDienThoai.Clear();
                txtDiaChi.Clear();
                index = -1;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string path = @"C:\KhachHang.csv";

            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (DataRow row in dtKhachHang.Rows)
                {
                    sw.WriteLine(string.Join(",", row.ItemArray));
                }
            }

            MessageBox.Show("Đã lưu thông tin khách hàng vào file C:\\KhachHang.csv", "Thông báo");
        }

        private void dgvKhachHang_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < dgvKhachHang.Rows.Count; i++)
            {
                dgvKhachHang.Rows[i].Cells["STT"].Value = i + 1;
            }
        }

        private void dgvKhachHang_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            for (int i = 0; i < dgvKhachHang.Rows.Count; i++)
            {
                dgvKhachHang.Rows[i].Cells["STT"].Value = i + 1;
            }
        }
    }
}
