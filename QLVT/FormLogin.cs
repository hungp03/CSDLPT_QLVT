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
        private void getDSPhanManh(String cmd)
        {

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);

            da.Fill(dt);

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

            getDSPhanManh("select * from V_DS_PHANMANH");
            cbxChiNhanh.SelectedIndex = 0;
            Program.servername = cbxChiNhanh.SelectedValue.ToString();
        }

      

        private void btnDangNhap_Click(object sender, EventArgs e)
        {

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



            String statement = "EXEC SP_LaythongtinNV '" + Program.mlogin + "'";
            Program.myReader = Program.ExecSqlDataReader(statement);
            if (Program.myReader == null)
                return;
            

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


            Program.formMain.UID.Text = "Mã nhân viên: " + Program.username;
            Program.formMain.NAME.Text = "Tên: " + Program.mName;
            Program.formMain.GROUP.Text = "Vai trò: " + Program.mGroup;

            this.Visible = false;
            Program.formMain.EnableBtn();
        }

        private void cbxChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Program.servername = cbxChiNhanh.SelectedValue.ToString();
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