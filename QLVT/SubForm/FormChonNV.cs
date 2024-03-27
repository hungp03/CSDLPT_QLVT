using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.SubForm
{
    public partial class FormChonNV : DevExpress.XtraEditors.XtraForm
    {
        public FormChonNV()
        {
            InitializeComponent();
        }

        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsNhanVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS1);

        }

        private void FormChonNV_Load(object sender, EventArgs e)
        {
            dS1.EnforceConstraints = false;
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.conStr;
            // TODO: This line of code loads data into the 'dS1.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Fill(this.dS1.NhanVien);

            comboBox1.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            comboBox1.DisplayMember = "TENCN";
            comboBox1.ValueMember = "TENSERVER";
            comboBox1.SelectedIndex = Program.brand;

            if (Program.mGroup == "CONGTY")
            {
                comboBox1.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataRowView drv = ((DataRowView)(bdsNhanVien.Current));
            string maNhanVien = drv["MANV"].ToString().Trim();
            //Console.WriteLine(maNhanVien);
            Program.selectedEmp = maNhanVien;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue.ToString() == "System.Data.DataRowView" || comboBox1.SelectedValue == null)
            {
                return;
            }
            Program.servername = comboBox1.SelectedValue.ToString();

            // Nếu chọn chi nhánh khác với chi nhánh hiện tại
            if (comboBox1.SelectedIndex != Program.brand)
            {
                // Dùng tài khoản hỗ trợ kết nối để chuẩn bị cho việc login vào chi nhánh khác
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
                // Lấy tài khoản hiện tại đang đăng nhập để đăng nhập
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }
            if (Program.connectDB() == 0)
            {
                MessageBox.Show("Lỗi kết nối tới chi nhánh", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                //Đổ data từ DS vào TA
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.conStr;
                this.nhanVienTableAdapter.Fill(dS1.NhanVien);
            }
        }
    }
}