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
        DataTable tblCTHDB;
        public static SqlConnection con;
        public static void Connect()
        {

            con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.QLBanHangConnectionString;
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
            //dap.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }
        //Hàm tạo khóa có dạng: TientoNgaythangnam_giophutgiay
        public static string CreateKey(string tiento)
        {
            string key = tiento;
            string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
            //Ví dụ 07/08/2009
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            key = key + d;
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            //Ví dụ 7:08:03 PM hoặc 7:08:03 AM
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            //Xóa ký tự trắng và PM hoặc AM
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            key = key + t;
            return key;
        }
        //Chuyển đổi từ PM sang dạng 24h
        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
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
            // Kiểm tra nếu đối tượng kết nối là null hoặc đã đóng, thì cố gắng kết nối lại
            if (con == null || con.State != ConnectionState.Open)
            {
                Connect(); // Thử gọi lại Connect()
            }

            // Tiếp tục với việc tạo data adapter và fill table
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            dap.Fill(table);
            // ... các dòng code còn lại
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
        public static string ConvertDateTime(string date)
        {
            // Giả sử người dùng nhập ngày theo định dạng dd/MM/yyyy
            DateTime d = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            // Trả về định dạng yyyy-MM-dd để lưu SQL
            return d.ToString("yyyy-MM-dd");
        }

    }

}