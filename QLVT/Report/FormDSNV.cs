using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.Report
{
    public partial class FormDSNV : Form
    { 
        private string chinhanh = "";
        public FormDSNV()
        {
            InitializeComponent();
        }

        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.nhanVienBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS1);

        }

        private void FormDSNV_Load(object sender, EventArgs e)
        {
            if (Program.mGroup.Equals("CONGTY"))
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
            dS1.EnforceConstraints = false;
            this.nhanVienTableAdapter.Fill(this.dS1.NhanVien);
            comboBox1.DataSource = Program.bindingSource;
            comboBox1.DisplayMember = "TENCN";
            comboBox1.ValueMember = "TENSERVER";
            comboBox1.SelectedIndex = Program.brand;
            chinhanh = comboBox1.SelectedValue.ToString().Contains("1") ? "TP.HCM" : "Cần Thơ";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue.ToString() == "System.Data.DataRowView" || comboBox1.SelectedValue == null)
            {
                return;
            }
            Program.servername = comboBox1.SelectedValue.ToString();
            if (comboBox1.SelectedIndex != Program.brand)
            {
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
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
                chinhanh = comboBox1.SelectedValue.ToString().Contains("1") ? "TP.HCM" : "Cần Thơ";
                //Console.WriteLine("Đang xem trước chi nhánh: ", chinhanh);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RpDSNV rp = new RpDSNV();
            rp.xrLabel2.Text = chinhanh;

            ReportPrintTool rpt = new ReportPrintTool(rp);
            rpt.ShowPreviewDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // cần đảm bảo có ổ D và folder ReqortQLVT
            try
            {
                RpDSNV rp = new RpDSNV();
                rp.xrLabel2.Text = chinhanh;
                if (File.Exists(@"D:\ReportQLVT\DanhSachNhanVien.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File report đã có! Bạn có muốn ghi đè không?", "Thông báo", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        rp.ExportToPdf(@"D:\ReportQLVT\DanhSachNhanVien.pdf");
                        MessageBox.Show("Đã ghi thành công\nXem tại D:\\ReportQLVT\\DanhSachNhanVien.pdf", "Thành công", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn in DSNV chứ?", "Thông báo", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        rp.ExportToPdf(@"D:\ReportQLVT\DanhSachNhanVien.pdf");
                        MessageBox.Show("Đã ghi thành công\nXem tại D:\\ReportQLVT\\DanhSachNhanVien.pdf", "Thành công", MessageBoxButtons.OK);
                    }
                }
            }
            catch(IOException ex)
            {
                MessageBox.Show("Có thể bạn chưa đóng file report\nHoặc có lỗi xảy ra. Hãy thử lại!",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
