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
namespace DACSN
{
    public partial class frmHangHoa : Form
    {
        DataTable tblHang; //Bảng hàng
        public frmHangHoa()
        {
            InitializeComponent();
        }

        private void frmHangHoa_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLBHDataSet.tblChatLieu' table. You can move, or remove it, as needed.
            this.tblChatLieuTableAdapter.Fill(this.qLBHDataSet.tblChatLieu);
            string sql;
            sql = "SELECT * from tblChatLieu";
            txtMaHang.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            LoadDataGridView();
            Function.FillCombo(sql, cboMaChatLieu, "MaChatLieu", "TenChatLieu");
            this.tblChatLieuTableAdapter.Fill(this.qLBHDataSet.tblChatLieu);
            cboMaChatLieu.SelectedIndex = -1;
            ResetValues();
        }
        private void ResetValues()
        {
            txtMaHang.Text = "";
            txtTenHang.Text = "";
            cboMaChatLieu.Text = ""; 
            txtSoLuong.Text = "0";
            txtDonGiaNhap.Text = "0";
            txtDonGiaBan.Text = "0";
            txtSoLuong.Enabled = true;
            txtDonGiaNhap.Enabled = false;
            txtDonGiaBan.Enabled = false;
            txtAnh.Text = "";
            picAnh.Image = null;
            txtGhichu.Text = "";
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * from tblHang";
            tblHang = Function.GetDataToTable(sql);
            dgvHang.DataSource = tblHang;

            // Kiểm tra nếu có cột trước khi set (tránh index out)
            if (tblHang.Columns.Count >= 8)
            {
                // Sử dụng tên cột thay vì index để tránh lỗi thứ tự
                dgvHang.Columns["MaHang"].HeaderText = "Mã hàng";
                dgvHang.Columns["TenHang"].HeaderText = "Tên hàng";
                dgvHang.Columns["MaChatLieu"].HeaderText = "Chất liệu";
                dgvHang.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvHang.Columns["MauSac"].HeaderText = "Màu sắc";
                dgvHang.Columns["KieuDang"].HeaderText = "Kiểu dáng";
                dgvHang.Columns["KieuDeToc"].HeaderText = "Kiểu đế tóc";
                dgvHang.Columns["DonGiaNhap"].HeaderText = "Đơn giá nhập";
                dgvHang.Columns["DonGiaBan"].HeaderText = "Đơn giá bán";
                dgvHang.Columns["Anh"].HeaderText = "Ảnh";
                dgvHang.Columns["Ghichu"].HeaderText = "Ghi chú";
               

                // Set width tương ứng (dùng tên cột)
                dgvHang.Columns["MaHang"].Width = 80;
                dgvHang.Columns["TenHang"].Width = 140;
                dgvHang.Columns["MaChatLieu"].Width = 80;
                dgvHang.Columns["SoLuong"].Width = 80;
                dgvHang.Columns["MauSac"].Width = 80;
                dgvHang.Columns["KieuDang"].Width = 80;
                dgvHang.Columns["KieuDeToc"].Width = 80;
                dgvHang.Columns["DonGiaNhap"].Width = 100;
                dgvHang.Columns["DonGiaBan"].Width = 100;
                dgvHang.Columns["Anh"].Width = 200;
                dgvHang.Columns["Ghichu"].Width = 300;
            }
            else
            {
                // Log lỗi hoặc thông báo nếu thiếu cột (tùy chọn)
                MessageBox.Show("Bảng tblHang thiếu cột. Vui lòng kiểm tra database.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dgvHang.AllowUserToAddRows = false;
            dgvHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  // Bỏ qua nếu click header

            string MaChatLieu;
            string sql;

            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }

            if (tblHang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtMaHang.Text = dgvHang.CurrentRow.Cells["MaHang"].Value.ToString();
            txtTenHang.Text = dgvHang.CurrentRow.Cells["TenHang"].Value.ToString();
            MaChatLieu = dgvHang.CurrentRow.Cells["MaChatLieu"].Value.ToString();
            sql = "SELECT TenChatLieu FROM tblChatLieu WHERE MaChatLieu=N'" + MaChatLieu + "'";
            cboMaChatLieu.Text = Function.GetFieldValues(sql);
            txtSoLuong.Text = dgvHang.CurrentRow.Cells["SoLuong"].Value.ToString();
            txtDonGiaNhap.Text = dgvHang.CurrentRow.Cells["DonGiaNhap"].Value.ToString();
            txtDonGiaBan.Text = dgvHang.CurrentRow.Cells["DonGiaBan"].Value.ToString();
            sql = "SELECT Anh FROM tblHang WHERE MaHang=N'" + txtMaHang.Text + "'";
            txtAnh.Text = Function.GetFieldValues(sql);
            picAnh.Image = Image.FromFile(txtAnh.Text);
            sql = "SELECT Ghichu FROM tblHang WHERE MaHang = N'" + txtMaHang.Text + "'";
            txtGhichu.Text = Function.GetFieldValues(sql);
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValues();
            txtMaHang.Enabled = true;
            txtMaHang.Focus();
            txtSoLuong.Enabled = true;
            txtDonGiaNhap.Enabled = true;
            txtDonGiaBan.Enabled = true;
        }

        
            private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }
            if (txtTenHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenHang.Focus();
                return;
            }
            if (cboMaChatLieu.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaChatLieu.Focus();
                return;
            }
            if (txtAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn ảnh minh hoạ cho hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnOpen.Focus();
                return;
            }

            // Kiểm tra trùng mã hàng
            sql = "SELECT MaHang FROM tblHang WHERE MaHang=N'" + txtMaHang.Text.Trim() + "'";
            if (Function.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã tồn tại, bạn phải chọn mã hàng khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }

             sql = "INSERT INTO tblHang(MaHang, TenHang, MaChatLieu, SoLuong, DonGiaNhap, DonGiaBan, Anh, Ghichu, MauSac, KieuDang, KieuDeToc) " +
                  "VALUES(@maHang, @tenHang, @maChatLieu, @soLuong, @donGiaNhap, @donGiaBan, @anh, @ghiChu, @mauSac, @kieuDang, @kieuDeToc)";

            // 2. Tạo một mảng các tham số SqlParameter
            SqlParameter[] parameters = new SqlParameter[]
{
    new SqlParameter("@maHang", txtMaHang.Text.Trim()),
    new SqlParameter("@tenHang", txtTenHang.Text.Trim()),
    new SqlParameter("@maChatLieu", cboMaChatLieu.SelectedValue.ToString()),
    new SqlParameter("@soLuong", Convert.ToInt32(txtSoLuong.Text.Trim())),
    new SqlParameter("@donGiaNhap", Convert.ToDouble(txtDonGiaNhap.Text.Trim())),
    new SqlParameter("@donGiaBan", Convert.ToDouble(txtDonGiaBan.Text.Trim())),
    new SqlParameter("@anh", txtAnh.Text.Trim()),
    new SqlParameter("@ghiChu", txtGhichu.Text.Trim()),
    new SqlParameter("@mauSac", txtMauSac.Text.Trim()),
    new SqlParameter("@kieuDang", txtKieuDang.Text.Trim()),
    new SqlParameter("@kieuDeToc", txtKieuDe.Text.Trim())
};

            try
            {
                // 3. Gọi hàm RunSQL mới và truyền cả câu lệnh và mảng tham số vào
                Function.RunSQL(sql, parameters);

                LoadDataGridView();
                MessageBox.Show("Thêm hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hàng hóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaHang.Enabled = false;
        }

        

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblHang.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaHang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }
            if (txtTenHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenHang.Focus();
                return;
            }
            if (cboMaChatLieu.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaChatLieu.Focus();
                return;
            }
            if (txtAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải ảnh minh hoạ cho hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAnh.Focus();
                return;
            }
            sql = "UPDATE tblHang SET TenHang=N'" + txtTenHang.Text.Trim().ToString() +
                "',MaChatLieu=N'" + cboMaChatLieu.SelectedValue.ToString() +
                "',SoLuong=" + txtSoLuong.Text +
                "',MauSac=N'" + txtMauSac.Text +
                "',KieuDang=N'" + txtKieuDang.Text +
                "',KieuDeToc=N'" + txtKieuDe.Text +
                ",Anh='" + txtAnh.Text + "',Ghichu=N'" + txtGhichu.Text + "' WHERE MaHang=N'" + txtMaHang.Text + "'";
            Function.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnBoQua.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblHang.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaHang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE tblHang WHERE MaHang=N'" + txtMaHang.Text + "'";
                Function.RunSqlDel(sql);
                LoadDataGridView();
                ResetValues();
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnThem.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaHang.Enabled = false;
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
                txtAnh.Text = dlgOpen.FileName;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string sql;
            if ((txtMaHang.Text == "") && (txtTenHang.Text == "") && (cboMaChatLieu.Text == ""))
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from tblHang WHERE 1=1";
            if (txtMaHang.Text != "")
                sql += " AND MaHang LIKE N'%" + txtMaHang.Text + "%'";
            if (txtTenHang.Text != "")
                sql += " AND TenHang LIKE N'%" + txtTenHang.Text + "%'";
            if (cboMaChatLieu.Text != "")
                sql += " AND MaChatLieu LIKE N'%" + cboMaChatLieu.SelectedValue + "%'";
            tblHang = Function.GetDataToTable(sql);
            if (tblHang.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblHang.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvHang.DataSource = tblHang;
            ResetValues();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmChatLieu frmChatLieu = new frmChatLieu();
            frmChatLieu.ShowDialog();
        }

        private void toolHangTon_Click(object sender, EventArgs e)
        {
            frmHangTonKho frmHangTonKho = new frmHangTonKho();
            frmHangTonKho.ShowDialog();
        }

        private void btnHienThi_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT MaHang,TenHang,MaChatLieu,SoLuong,DonGiaNhap,DonGiaBan,Anh,Ghichu,MauSac,KieuDang,KieuDeToc FROM tblHang";
            tblHang = Function.GetDataToTable(sql);
            dgvHang.DataSource = tblHang;
        }

        private void cboMaChatLieu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.tblChatLieuTableAdapter.FillBy(this.qLBHDataSet.tblChatLieu);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void cáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sảnPhẩmMuaỞShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCaNhanHoa frmCaNhanHoa = new frmCaNhanHoa();
            frmCaNhanHoa.ShowDialog();
        }

        private void sảnPhẩmBênNgoàiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCaNhanHoa2 frmCaNhanHoa2 = new frmCaNhanHoa2();
            frmCaNhanHoa2.ShowDialog();
        }

        private void hàngĐặtThiếtKếRiêngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCaNhanHoa3 frmCaNhanHoa3 = new frmCaNhanHoa3();
            frmCaNhanHoa3.ShowDialog();
        }
    }
}
