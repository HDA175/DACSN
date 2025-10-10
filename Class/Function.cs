using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DACSN.Class
{
    public class Function
    {
        public static SqlConnection con;
        public static void Connect()
        {
            con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.QLBHConnectionString;
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                MessageBox.Show("Kết nối thành công", "Thông báo");
            }
            else MessageBox.Show("Kết nối thất bại");
        }
        public static void Disconnect()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
                con = null;
            }
        }
        public static DataTable GetDataToTable(string sql)
        {
            DataTable table = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            dap.Fill(table);
            return table;
        }
        public DataTable LoadDL(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(dt);
            return dt;
        }
        public object LayGT(string sql)
        {
            SqlCommand comm = new SqlCommand(sql, con);
            object kq = comm.ExecuteScalar();
            return kq;
        }
        public int ThemXoaSua(string sql)
        {
            SqlCommand comm = new SqlCommand(sql, con);
            int kq = comm.ExecuteNonQuery();
            return kq;
        }
        public static void RunSQL(string sql)
        {
            SqlCommand cmd; //Đối tượng thuộc lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = con; //Gán kết nối
            cmd.CommandText = sql; //Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();//Giải phóng bộ nhớ
            cmd = null;
        }
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            dap.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }
        public static void RunSqlDel(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Function.con;
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Dữ liệu đang được dùng, không thể xoá...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
        }
        public static void FillCombo(string sql, ComboBox cbo, string ma, string ten)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            dap.Fill(table);
            cbo.DataSource = table;
            cbo.ValueMember = ma; //Trường giá trị
            cbo.DisplayMember = ten; //Trường hiển thị
        }
        public static string GetFieldValues(string sql)
        {
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            return ma;
        }

     public string TaoMaTuDong(string tenBang, string tenCotMa, string prefix)
        {
            try
            {
                string sql = $"SELECT {tenCotMa} FROM {tenBang}";
                DataTable dt = LoadDL(sql);

                int maxSo = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string ma = row[tenCotMa].ToString(); // Ví dụ: HD005
                    if (ma.StartsWith(prefix))
                    {
                        string soPart = ma.Substring(prefix.Length);
                        if (int.TryParse(soPart, out int so))
                        {
                            if (so > maxSo)
                                maxSo = so;
                        }
                    }
                }

                maxSo++; // Số kế tiếp
                return prefix + maxSo.ToString("D3"); // Ví dụ: HD006
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo mã tự động: " + ex.Message);
                return prefix + "001"; // Trường hợp lỗi vẫn trả mã khởi tạo
            }
        }
    }
}