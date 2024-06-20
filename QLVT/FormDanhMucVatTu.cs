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
    public partial class FormDanhMucVatTu : Form
    {
        public FormDanhMucVatTu()
        {
            InitializeComponent();
        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.vattuBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS1);

        }

        private void FormDanhMucVatTu_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS1.Vattu' table. You can move, or remove it, as needed.
            this.vattuTableAdapter.Connection.ConnectionString = Program.conStr;
            this.vattuTableAdapter.Fill(this.dS1.Vattu);

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            ReportDMVattu report = new ReportDMVattu();
            ReportPrintTool print = new ReportPrintTool(report);
            print.ShowPreviewDialog();
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            ReportDMVattu report = new ReportDMVattu();
            try
            {
                if (File.Exists(@"D:\ReportQLVT\ReportDanhMucVatTu.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File ReportDanhMucVatTu.pdf đã tồn tại!\nBạn có muốn ghi đè không?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"D:\ReportQLVT\ReportDanhMucVatTu.pdf");
                        MessageBox.Show("File ReportDanhMucVatTu.pdf đã được ghi thành công tại D:\\ReportQLVT",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    report.ExportToPdf(@"D:\ReportQLVT\ReportDanhMucVatTu.pdf");
                    MessageBox.Show("File ReportDanhMucVatTu.pdf đã được ghi thành công tại D:\\ReportQLVT",
                "Thông báo", MessageBoxButtons.OK);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Vui lòng đóng file ReportDanhMucVatTu.pdf lại trước",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
