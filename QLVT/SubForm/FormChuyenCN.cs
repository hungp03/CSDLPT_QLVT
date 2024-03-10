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
    public partial class FormChuyenCN : DevExpress.XtraEditors.XtraForm
    {
        public FormChuyenCN()
        {
            InitializeComponent();
        }

        private void FormChuyenCN_Load(object sender, EventArgs e)
        {
            cbxChuyenCN.DataSource = Program.bindingSource.DataSource;
            cbxChuyenCN.DisplayMember = "tencn";
            cbxChuyenCN.ValueMember = "tenserver";
            cbxChuyenCN.SelectedIndex = Program.brand;
        }
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        public delegate void MyDelegate(string chiNhanh);
        public MyDelegate branchTransfer;

        private void button1_Click(object sender, EventArgs e)
        {
            if(cbxChuyenCN.SelectedValue.ToString() == Program.servername)
            {
                MessageBox.Show("Hãy chọn chi nhánh khác", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(cbxChuyenCN.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn chi nhánh", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn chuyển nhân viên này đi ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.OK)
            {
                branchTransfer(cbxChuyenCN.SelectedValue.ToString());
            }

            this.Dispose();
        }
    }
}