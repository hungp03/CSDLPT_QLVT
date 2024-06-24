using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraEditors;
using QLVT.SubForm;
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
    public partial class FormTaoTK : DevExpress.XtraEditors.XtraForm
    {
        private string loginName = "";
        private string password = "";
        private string userID = "";
        private string role = "";

        public FormTaoTK()
        {
            InitializeComponent();
        }

        private bool validateInput()
        {
            if (string.IsNullOrWhiteSpace(txtManv.Text))
            {
                MessageBox.Show("Thiếu mã nhân viên", "Thông báo", MessageBoxButtons.OK);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTendangnhap.Text.Trim()))
            {
                MessageBox.Show("Thiếu tên đăng nhập", "Thông báo", MessageBoxButtons.OK);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMatkhau.Text.Trim()))
            {
                MessageBox.Show("Thiếu mật khẩu", "Thông báo", MessageBoxButtons.OK);
                return false;
            }

            if (txtXacnhanMK.Text.Trim() != txtMatkhau.Text.Trim())
            {
                MessageBox.Show("Mật khẩu không khớp với mật khẩu xác nhận", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            if (Program.mGroup.Equals("CHINHANH") && !rdCN.Checked && !rdUser.Checked)
            {
                MessageBox.Show("Chưa chọn vai trò nhân viên", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void FormTaoTK_Load(object sender, EventArgs e)
        {
            if (Program.mGroup.Equals("CONGTY"))
            {
                role = "CONGTY";
                rdCN.Enabled = false;
                rdUser.Enabled = false;
                label5.Visible = false;
                rdCN.Visible = false;
                rdUser.Visible = false;
            }
            else if (Program.mGroup.Equals("CHINHANH"))
            {
                rdCN.Enabled = true;
                rdUser.Enabled = true;
            }
        }

        private void btnTaoTK_Click(object sender, EventArgs e)
        {
            if (!validateInput())
            {
                return;
            }
            loginName = txtTendangnhap.Text.Trim();
            password = txtMatkhau.Text.Trim();
            userID = Program.selectedEmp;
            if ((!rdCN.Checked && !rdUser.Checked) || Program.mGroup.Equals("CONGTY"))
            {
                role = "CONGTY";
            }
            else
            {
                role = rdCN.Checked ? "CHINHANH" : "USER";
            }
            /*Console.WriteLine("ĐANG TẠO TK:" + loginName + " - " + password + " - " + userID + " - " + role);
            Console.WriteLine(Program.servername);*/

            string sqlQuery = "EXEC SP_TAOTK '" + loginName + "' , '" + password + "', '"+ userID + "', '" + role + "'";
            _ = new SqlCommand(sqlQuery, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(sqlQuery);
                if (Program.myReader == null) {
                    return;
                }
                MessageBox.Show("Tạo TK thành công\nNV: " + userID + " - ROLE: " + role +"\nTK: " + loginName + " - PASS: " + password , "Thông báo", MessageBoxButtons.OK);
                Program.myReader.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 15118)
                    MessageBox.Show("Password bạn nhập không thỏa chính sách bảo mật của Windows Server vì còn đơn giản", "Thông báo", MessageBoxButtons.OK);
                else
                    MessageBox.Show("\nTài khoản bạn nhập đã có trong Server. Bạn nhập lại tài khoản khác.", "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.createAcc = true;
            FormChonNV form = new FormChonNV();
            form.ShowDialog();
            txtManv.Text = Program.selectedEmp;
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}