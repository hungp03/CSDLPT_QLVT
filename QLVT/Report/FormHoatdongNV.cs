using DevExpress.XtraReports.UI;
using QLVT.SubForm;
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
    public partial class FormHoatdongNV : Form
    {
        string manv;
        DateTime fromDate;
        DateTime toDate;
        string tennv;
        string formattedDate;
        DateTime currentDay;
        public FormHoatdongNV()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormChonNV form = new FormChonNV();
            form.ShowDialog();
            txtManv.Text = Program.selectedEmp;
            txtHoten.Text = Program.empName;
        }
        private void ThongBao(string mess)
        {
            MessageBox.Show(mess, "Thông báo", MessageBoxButtons.OK);
        }

        private bool validateInput()
        {
            if (string.IsNullOrEmpty(txtManv.Text.Trim()) || string.IsNullOrEmpty(txtHoten.Text.Trim()))
            {
                    ThongBao("Mã nhân viên và tên đang trống. Vui lòng chọn nhân viên");
                    return false;
            }
            if(dateEdit1.EditValue == null)
            {
                ThongBao("Chưa chọn ngày bắt đầu");
                return false;
            }

            if (dateEdit2.EditValue == null)
            {
                ThongBao("Chưa chọn ngày kết thúc");
                return false;
            }

            if (dateEdit1.DateTime >=  dateEdit2.DateTime)
            {
                ThongBao("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
                return false;
            }

            if (dateEdit1.DateTime > DateTime.Today)
            {
                ThongBao("Ngày bắt đầu không được sau ngày hôm nay");
                return false;
            }



            return true;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!validateInput())
            {
                return;
            }
            manv = txtManv.Text.Trim();
            fromDate = dateEdit1.DateTime;
            toDate = dateEdit2.DateTime;
            tennv = txtHoten.Text.Trim();
            RP_HoatdongNV hdnv = new RP_HoatdongNV(manv, fromDate, toDate);
            hdnv.xrName.Text = "Họ tên nhân viên: " + tennv;
            hdnv.xrDate.Text = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + "đến ngày " + toDate.ToString("dd/MM/yyyy");
            currentDay = DateTime.Now;
            formattedDate = "Ngày tạo báo cáo: " +currentDay.ToString("dd/MM/yyyy");
            hdnv.xrReportDate.Text = formattedDate;
            ReportPrintTool rpt = new ReportPrintTool(hdnv);
            rpt.ShowPreviewDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!validateInput())
            {
                return;
            }
            try
            {
                manv = txtManv.Text.Trim();
                fromDate = dateEdit1.DateTime;
                toDate = dateEdit2.DateTime;
                tennv = txtHoten.Text.Trim();

                RP_HoatdongNV hdnv = new RP_HoatdongNV(manv, fromDate, toDate);
                hdnv.xrName.Text = "Họ tên nhân viên: " + tennv;
                hdnv.xrDate.Text = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + "đến ngày " + toDate.ToString("dd/MM/yyyy");
                currentDay = DateTime.Now;
                formattedDate = "Ngày tạo báo cáo: " + currentDay.ToString("dd/MM/yyyy");
                hdnv.xrReportDate.Text = formattedDate;
                ReportPrintTool rpt = new ReportPrintTool(hdnv);

                if (File.Exists($@"D:\ReportQLVT\ReportHoatDongNhanVien_{manv}.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File report này đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        hdnv.ExportToPdf($@"D:\ReportQLVT\ReportHoatDongNhanVien_{manv}.pdf");
                        MessageBox.Show("File đã được ghi thành công tại ổ D:/ReportQLVT",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    hdnv.ExportToPdf($@"D:\ReportQLVT\ReportHoatDongNhanVien_{manv}.pdf");
                    MessageBox.Show("File đã được ghi thành công tại D:/ReportQLVT",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file report cũ",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }

        }

        private void FormHoatdongNV_Load(object sender, EventArgs e)
        {

        }
    }
}
