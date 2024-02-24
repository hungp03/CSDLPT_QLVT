using DevExpress.Skins;
using DevExpress.UserSkins;
using QLVT.SubForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace QLVT
{
    /*
     * Phải khởi tạo các biến toàn cục ở đây vì khi form chưa khởi chạy, 
     * ta chưa có biến để hoạt động, nếu không có các biến này, khi một Form chưa chạy hoặc tắt đi
     * Form khác sẽ không hiểu các biến đó
     */
    internal static class Program
    {
        public static SqlConnection conn = new SqlConnection();
        public static SqlDataReader myReader;

        //Chuỗi kết nối về server
        public static string conStr = "";
        public static string conPublisher = "Data Source=DESKTOP-53JQKSJ;Initial Catalog=QLVT_DATHANG;User ID=HTKN;Password=123456;TrustServerCertificate=True";

        //Tên server (phân mảnh) kết nối tới
        public static String servername = "";
        public static String username = "";
        public static String mlogin = "";
        public static String password = "";

        //Remote login
        public static String database = "QLVT_DATHANG";
        public static String remotelogin = "HTKN";
        public static String remotepassword = "123456";

        /*Hai biến này dùng để đi từ server này sang server còn lại
         * đi từ server hiện tại sang server 2 => remoteLogin
         * đi từ server 2 về lại server hiện tại => mloginDN
        */
        public static String mloginDN = "";
        public static String passwordDN = "";

        /*mGroup: tên nhóm quyền đang đăng nhập: CONGTY - CHINHANH - USER
         * mName: tên nhân viên đang đăng nhập
         * brand: chi nhánh đang đăng nhập*/
        public static String mGroup = "";
        public static String mName = "";
        public static int brand = 0;

        //BindingSource -> liên kết dữ liệu từ bảng dữ liệu
        public static BindingSource bindingSource = new BindingSource();//bds_dspm
        
        //Các form
        public static FormLogin formLogin;
        public static FormMain formMain;

        /* Hàm ExecSqlDataReader thực hiện câu lệnh mà dữ liệu trả về chỉ dùng để xem
         * không thao tác, chỉnh sửa.
         *
         *  SELECT * FROM view_DSPhanManh
         **********************************************/
        public static SqlDataReader ExecSqlDataReader(String strLenh)
        {
            SqlDataReader myreader;
            SqlCommand sqlcmd = new SqlCommand(strLenh, Program.conn);
            sqlcmd.CommandType = CommandType.Text;
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            try
            {
                myreader = sqlcmd.ExecuteReader(); return myreader;

            }
            catch (SqlException ex)
            {
                Program.conn.Close();
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //Hàm kết nối CSDL
        public static int connectDB()
        {
            if (Program.conn != null && Program.conn.State == ConnectionState.Open)
                Program.conn.Close();
            try
            {
                Console.WriteLine(Program.mlogin);
                Program.conStr = "Data Source=" + Program.servername + ";Initial Catalog=" +
                      Program.database + ";User ID=" +
                      Program.mlogin + ";password=" + Program.password;
                Program.conn.ConnectionString = Program.conStr;
                Program.conn.Open();
                return 1;
            }

            catch (Exception e)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại thông tin đã nhập.\n " + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program.formMain = new FormMain();
            Application.Run(formMain);
        }
        
    }
}
