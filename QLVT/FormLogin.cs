using DevExpress.XtraEditors;
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

namespace QLVT
{
    public partial class FormLogin : DevExpress.XtraEditors.XtraForm
    {   
        private SqlConnection conn = new SqlConnection();

        //Hàm lấy danh sách phân mảnh (trừ phân mảnh tra cứu)
        private void getDSPhanManh(String cmd)
        {
            //Kiểm tra trạng thái kết nối, nếu đóng thì mở lại
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //DataTable: lưu trữ dữ liệu dưới dạng Table
            DataTable dt = new DataTable();
            //SqlDataAdapter dùng để lấy và ghi dữ liệu vào MSSQL, đổ dữ liệu vào DataTable, DataSet...
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            //Đổ dữ liệu vào dt
            da.Fill(dt);

            //Đóng kết nối
            conn.Close();
            Program.bindingSource.DataSource = dt;


            cbxChiNhanh.DataSource = Program.bindingSource;
            cbxChiNhanh.DisplayMember = "TENCN";
            cbxChiNhanh.ValueMember = "TENSERVER";
        }

        private int ketNoiDBGoc()
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
            try
            {
                conn.ConnectionString = Program.conPublisher;
                conn.Open();
                return 1;
            }

            catch (Exception e)
            {
                MessageBox.Show("1Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại user name và password.\n " + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            if (ketNoiDBGoc() == 0)
            {
                return;
            }
            //Lấy danh sách phân mảnh (chi nhánh), lưu vào comboBox
            getDSPhanManh("select * from V_DS_PHANMANH");
            cbxChiNhanh.SelectedIndex = 0;
            Program.servername = cbxChiNhanh.SelectedValue.ToString();
        }

      

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            //Kiểm tra thông tin đã nhập hay chưa
            if (txtTaiKhoan.Text.Trim() == "" || txtMatKhau.Text.Trim() == "")
            {
                MessageBox.Show("Tài khoản & mật khẩu không thể bỏ trống", "Thông Báo", MessageBoxButtons.OK);
                return;
            }
            

            Program.mlogin = txtTaiKhoan.Text.Trim();
            Program.password = txtMatKhau.Text.Trim();
            if (Program.connectDB() == 0)
                return;

            Program.brand = cbxChiNhanh.SelectedIndex;
            Program.mloginDN = Program.mlogin   ;
            Program.passwordDN = Program.password;


            //Lấy thông tin người dùng qua SP
            String statement = "EXEC SP_LaythongtinNV '" + Program.mlogin + "'";
            Program.myReader = Program.ExecSqlDataReader(statement);
            if (Program.myReader == null)
                return;
            
            //Đọc kết quả vào myReader
            Program.myReader.Read();

            Program.username = Program.myReader.GetString(0);
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Tài khoản này không có quyền truy cập \n Hãy thử tài khoản khác", "Thông Báo", MessageBoxButtons.OK);
            }
            Program.mName = Program.myReader.GetString(1);
            Program.mGroup = Program.myReader.GetString(2);

            Program.myReader.Close();
            Program.conn.Close();

            //Hiển thị thông tin ở góc màn hình
            Program.formMain.UID.Text = "Mã nhân viên: " + Program.username;
            Program.formMain.NAME.Text = "Tên: " + Program.mName;
            Program.formMain.GROUP.Text = "Vai trò: " + Program.mGroup;

            //Khi đăng nhập thành công, ẩn form này đi
            this.Visible = false;
            Program.formMain.EnableBtn();
        }

        private void cbxChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Program.servername = cbxChiNhanh.SelectedValue.ToString();
                //Console.WriteLine(cbxChiNhanh.SelectedValue.ToString());
            }
            catch (Exception)
            {

            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}