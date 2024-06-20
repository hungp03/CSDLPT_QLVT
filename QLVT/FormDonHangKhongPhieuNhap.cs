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
    public partial class FormDonHangKhongPhieuNhap : Form
    {
        private string chiNhanh;
        public FormDonHangKhongPhieuNhap()
        {
            InitializeComponent();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            ReportDonHangKhongPhieuNhap report = new ReportDonHangKhongPhieuNhap();
            ReportPrintTool print = new ReportPrintTool(report);
            report.txtChiNhanh.Text = chiNhanh.ToUpper();
            print.ShowPreviewDialog();
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            ReportDonHangKhongPhieuNhap report = new ReportDonHangKhongPhieuNhap();
            report.txtChiNhanh.Text = chiNhanh.ToUpper();
            try
            {
                if (File.Exists(@"D:\ReportQLVT\ReportDonHangKhongPhieuNhap.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File ReportDonHangKhongPhieuNhap.pdf đã tồn tại!\nBạn có muốn ghi đè không?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"D:\ReportQLVT\ReportDonHangKhongPhieuNhap.pdf");
                        MessageBox.Show("File ReportDonHangKhongPhieuNhap.pdf đã được ghi thành công tại D:\\ReportQLVT",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    report.ExportToPdf(@"D:\ReportQLVT\ReportDonHangKhongPhieuNhap.pdf");
                    MessageBox.Show("File ReportDonHangKhongPhieuNhap.pdf đã được ghi thành công tại D:\\ReportQLVT",
                "Thông báo", MessageBoxButtons.OK);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Vui lòng đóng file ReportDonHangKhongPhieuNhap.pdf lại trước",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }

        private void FormDonHangKhongPhieuNhap_Load(object sender, EventArgs e)
        {
            cbChiNhanh.DataSource = Program.bindingSource;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            cbChiNhanh.SelectedIndex = Program.brand;

            /*brandId = ((DataRowView)bdsPhieuNhap[0])["MACN"].ToString();*/
            //Phân quyền nhóm CONGTY chỉ được xem dữ liệu
            if (Program.mGroup == "CONGTY")
            {
                cbChiNhanh.Enabled = true;
            }

            //Phân quyền nhóm CHINHANH-USER có thể thao tác với dữ liệu
            //Nhưng không được quyền chuyển chi nhánh khác để xem dữ liệu
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                cbChiNhanh.Enabled = false;
            }
            if (cbChiNhanh.SelectedValue != null)
                chiNhanh = cbChiNhanh.Text.Split('-')[1].Trim();
            else chiNhanh = "";
        }
        private void cbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            Program.servername = cbChiNhanh.SelectedValue.ToString();

            /*Neu chon sang chi nhanh khac voi chi nhanh hien tai*/
            if (cbChiNhanh.SelectedIndex != Program.brand)
            {
                // Dùng tài khoản hỗ trợ kết nối để chuẩn bị cho việc login vào chi nhánh khác
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            /*Neu chon trung voi chi nhanh dang dang nhap o formDangNhap*/
            else
            {
                // Lấy tài khoản hiện tại đang đăng nhập để đăng nhập
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }
            if (Program.connectDB() == 0)
            {
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại", "Thông báo", MessageBoxButtons.OK);
            }
            if (cbChiNhanh.SelectedValue != null)
                chiNhanh = cbChiNhanh.Text.Split('-')[1].Trim();
            else chiNhanh = "";
        }
    }
}
