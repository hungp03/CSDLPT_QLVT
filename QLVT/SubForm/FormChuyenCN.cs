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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FormChuyenCN_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Program.bindingSource.DataSource;
            // Sao chép bds từ form đăng nhập
            comboBox1.DisplayMember = "TENCN";
            comboBox1.ValueMember = "TENSERVER";
            comboBox1.SelectedIndex = Program.brand;
        }
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        /*Trong ngôn ngữ lập trình C#
         * delegate là một kiểu dữ liệu đặc biệt 
         * cho phép bạn tạo ra một biến tham chiếu đến một phương thức.
         * Delegate cho phép bạn truyền phương thức
         * như một tham số cho một phương thức khác hoặc lưu trữ nó như một biến.

          Một delegate định nghĩa kiểu của một phương thức, 
        bao gồm kiểu trả về của phương thức và danh sách các tham số của phương thức.
        Khi bạn khai báo một biến delegate, bạn đang tạo ra một tham chiếu 
        đến một phương thức có cùng kiểu dữ liệu với delegate.
        */
        public delegate void MyDelegate(string chiNhanh);
        public MyDelegate branchTransfer;

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null && comboBox1.SelectedValue.ToString() == Program.servername)
            {
                MessageBox.Show("Hãy chọn chi nhánh khác", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(comboBox1.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn chi nhánh", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            Console.WriteLine(comboBox1.Text.Trim());

            // Kiểm tra xem branchTransfer đã được gán chưa
            if (branchTransfer != null)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn chuyển nhân viên này đi ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.OK)
                {
                    Console.WriteLine("TRANSFER:" + branchTransfer);
                    branchTransfer(comboBox1.SelectedValue.ToString());
                }
            }
            else
            {
                MessageBox.Show("Delegate chưa được khởi tạo");
                return;
            }

            this.Dispose();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}