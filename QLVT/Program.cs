using QLVT.Report;
using QLVT.SubForm;

//using QLVT.SubForm;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

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
        //Gắn cờ để theo dõi form chọn nv
        public static bool createAcc = false;

        //Chuỗi kết nối về server
        public static string conStr = "";
        public static string conPublisher = "Data Source=HUNG-PC;Initial Catalog=QLVT;User ID=HTKN;Password=123456;TrustServerCertificate=True";
        //public static string conPublisher = "Data Source=LAPTOP-PUD7RCFK;Initial Catalog=QLVT;Integrated Security=True;User ID=HTKN;Password=123456";


        //Tên server (phân mảnh) kết nối tới
        public static string servername = "";
        public static string username = "";
        public static string mlogin = "";
        public static String password = "";

        //Tên server (chi nhánh) còn lại
        public static string otherServerName = "";
        //nhân viên đang được chọn cho việc tạo tài khoản
        public static string selectedEmp = "";
        public static string empName = "";

        //Remote login
        public static string database = "QLVT";
        public static string remotelogin = "HTKN";
        public static string remotepassword = "123456";
            
        /*Hai biến này dùng để đi từ server này sang server còn lại
         * đi từ server hiện tại sang server 2 => remoteLogin
         * đi từ server 2 về lại server hiện tại => mloginDN
        */
        public static string mloginDN = "";
        public static string passwordDN = "";

        /*mGroup: tên nhóm quyền đang đăng nhập: CONGTY - CHINHANH - USER
         * mName: tên nhân viên đang đăng nhập
         * brand: chi nhánh đang đăng nhập*/
        public static string mGroup = "";
        public static string mName = "";
        public static int brand = 0;

        //BindingSource -> liên kết dữ liệu từ bảng dữ liệu
        public static BindingSource bindingSource = new BindingSource();

        //Các form
        public static FormLogin formLogin;
        public static FormMain formMain;
        public static FormNhanVien formNhanVien;
        public static FormVatTu formVatTu;
        public static FormKho formKho;
        public static FormTaoTK formTaoTK;
        public static FormPhieuNhap formPhieuNhap;

        //subform
        public static FormChonNV formChonNV;
        public static FormChuyenCN formChuyenCN;

        //report form
        public static FormSupportDSNV formSupport;
        public static FormDanhMucVatTu formDanhMucVatTu;

        /* Hàm ExecSqlDataReader thực hiện câu lệnh mà dữ liệu trả về chỉ dùng để xem
         * không thao tác, chỉnh sửa
         */
        public static SqlDataReader ExecSqlDataReader(string strLenh)
        {
            SqlDataReader myreader;
            SqlCommand sqlcmd = new SqlCommand(strLenh, Program.conn);
            sqlcmd.CommandType = CommandType.Text;
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            try
            {
                myreader = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myreader;
            }
            catch (SqlException ex)
            {
                Program.conn.Close();
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        // Hàm thực hiện truy xuất dữ liệu, có thể CRUD thoải mái
        public static DataTable ExecSqlDataTable(String strLenh)
        {
            DataTable dt = new DataTable();
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strLenh, conn);
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        //Hàm cập nhật SP và không trả về dữ liệu
        public static int ExecSqlNonQuery(String strLenh)
        {
            SqlCommand sqlCmd = new SqlCommand(strLenh, conn);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandTimeout = 300; //5p
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            try
            {
                sqlCmd.ExecuteNonQuery();
                conn.Close();
                return 0;
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Error converting data type varchar to int"))
                    MessageBox.Show("Bạn format Cell lại các cột dữ liệu");
                else MessageBox.Show(ex.Message);
                conn.Close();
                return ex.State;
            }
        }
        //Hàm kết nối CSDL
        public static int connectDB()
        {
            if (Program.conn != null && Program.conn.State == ConnectionState.Open)
                Program.conn.Close();
            try
            {
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