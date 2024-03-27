using DevExpress.XtraEditors;
using QLVT.SubForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormTaoTK : DevExpress.XtraEditors.XtraForm
    {
        private string taiKhoan = "";
        private string matKhau = "";
        private string maNhanVien = "";
        private string vaiTro = "";

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

            if (string.IsNullOrWhiteSpace(txtTendangnhap1.Text))
            {
                MessageBox.Show("Thiếu mật khẩu", "Thông báo", MessageBoxButtons.OK);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMatkhau.Text))
            {
                MessageBox.Show("Thiếu mật khẩu xác nhận", "Thông báo", MessageBoxButtons.OK);
                return false;
            }

            if (txtTendangnhap1.Text != txtMatkhau.Text)
            {
                MessageBox.Show("Mật khẩu không khớp với mật khẩu xác nhận", "Thông báo", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void FormTaoTK_Load(object sender, EventArgs e)
        {
            if (Program.mGroup.Equals("CONGTY"))
            {
                vaiTro = "CONGTY";
                rdCN.Enabled = false;
                rdUser.Enabled = false;
            }
            else
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

            taiKhoan = txtXacnhanMK.Text.Trim();
            matKhau = txtTendangnhap1.Text.Trim();
            maNhanVien = Program.selectedEmp;
            vaiTro = (rdCN.Checked == true) ? "CHINHANH" : "USER";
            Console.WriteLine(taiKhoan + " - " + matKhau + " - " + maNhanVien + " - " + vaiTro);

        }

        private void button3_Click(object sender, EventArgs e)
        {
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