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
    public partial class FormChiTietSoLuongTriGiaHangHoa : Form
    {
        private string vaitro;
        private string loaiPhieu;
        private DateTime fromDate;
        private DateTime toDate;
        public FormChiTietSoLuongTriGiaHangHoa()
        {
            InitializeComponent();
        }
        private bool valitdateInput()
        {

            if (dteTuNgay.EditValue == null)
            {
                MessageBox.Show("Chưa chọn ngày bắt đầu", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            if (dteToiNgay.EditValue == null)
            {
                MessageBox.Show("Chưa chọn ngày kết thúc", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            if (dteTuNgay.DateTime >= dteToiNgay.DateTime)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (!valitdateInput())
            {
                return;
            }
            vaitro = Program.mGroup;
            loaiPhieu = (cmbLoaiPhieu.SelectedItem.ToString() == "NHAP") ? "NHAP":"XUAT";
            fromDate = dteTuNgay.DateTime;
            toDate = dteToiNgay.DateTime;
            ReportChiTietSoLuongTriGiaHangHoa report = new ReportChiTietSoLuongTriGiaHangHoa(vaitro,loaiPhieu,fromDate,toDate);
            ReportPrintTool print = new ReportPrintTool(report);
            report.txtLoaiPhieu.Text = loaiPhieu;
            report.txtTuNgay.Text = fromDate.ToString("dd-MM-yyyy");
            report.txtToiNgay.Text = toDate.ToString("dd-MM-yyyy");
            print.ShowPreviewDialog();
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            vaitro = Program.mGroup;
            loaiPhieu = (cmbLoaiPhieu.SelectedItem.ToString() == "NHAP") ? "NHAP" : "XUAT";
            fromDate = dteTuNgay.DateTime;
            toDate = dteToiNgay.DateTime;
            ReportChiTietSoLuongTriGiaHangHoa report = new ReportChiTietSoLuongTriGiaHangHoa(vaitro, loaiPhieu, fromDate, toDate);
            report.txtLoaiPhieu.Text = loaiPhieu;
            report.txtTuNgay.Text = fromDate.ToString("dd-MM-yyyy");
            report.txtToiNgay.Text = toDate.ToString("dd-MM-yyyy");
            try
            {
                if (File.Exists(@"D:\ReportQLVT\ReportChiTietSoLuongTriGiaHangHoa.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File đã tồn tại!\\nBạn có muốn ghi đè không?","Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"D:\ReportQLVT\ReportChiTietSoLuongTriGiaHangHoa.pdf");
                        MessageBox.Show("File ReportChiTietSoLuongTriGiaHangHoa.pdf đã được ghi thành công tại D:\\ReportQLVT",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    report.ExportToPdf(@"D:\ReportQLVT\ReportChiTietSoLuongTriGiaHangHoa.pdf");
                    MessageBox.Show("File ReportChiTietSoLuongTriGiaHangHoa.pdf đã được ghi thành công tại D:\\ReportQLVT",
                "Thông báo", MessageBoxButtons.OK);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Vui lòng đóng file ReportChiTietSoLuongTriGiaHangHoa.pdf lại trước",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }
        private void FormChiTietSoLuongTriGiaHangHoa_Load(object sender, EventArgs e)
        {
            this.cmbLoaiPhieu.SelectedIndex = 0;
            this.dteTuNgay.EditValue = "01/01/2024";
            DateTime temp = DateTime.Today.Date;
            this.dteToiNgay.EditValue = temp;
        }
    }
}
