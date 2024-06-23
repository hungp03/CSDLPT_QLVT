using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;

namespace QLVT.Report
{
    public partial class FormHoatDongNhapXuat : Form
    {
        DateTime fromDate;
        DateTime toDate;
        DateTime currentDay;
        string chiNhanh;
        public FormHoatDongNhapXuat()
        {
            InitializeComponent();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if(!valitdateInput()){
                return;
            }
            
            fromDate = dateFromDate.DateTime;
            toDate = dateToDate.DateTime;
            currentDay = DateTime.Now;
            ReportTongHopNhapXuat thnx = new ReportTongHopNhapXuat(fromDate, toDate);
            ReportPrintTool rpt = new ReportPrintTool(thnx);
            thnx.xrLabel7.Text = fromDate.ToString("dd/MM/yyyy");
            thnx.xrLabel4.Text = toDate.ToString("dd/MM/yyyy");
            thnx.xrLabel5.Text = cbChiNhanh.SelectedValue.ToString().Contains("SERVER1") ? " Hồ Chí Minh" : "HÀ NỘI";
            rpt.ShowPreviewDialog();

        }

        private bool valitdateInput()
        {
            if(dateFromDate.EditValue == null)
            {
                MessageBox.Show("Chưa chọn ngày bắt đầu", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            if(dateToDate.EditValue == null)
            {
                MessageBox.Show("Chưa chọn ngày kết thúc", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            if(dateFromDate.DateTime >= dateToDate.DateTime)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!valitdateInput())
            {
                return;
            }
            try
            {
                currentDay = DateTime.Now;
                chiNhanh = cbChiNhanh.SelectedValue.ToString().Contains("SERVER1") ? "HCM" : "HÀ NỘI";
                ReportTongHopNhapXuat reportExportToPDF = new ReportTongHopNhapXuat(fromDate, toDate);
                string savePath = $@"D:\ReportQLVT\ReportTongHopNhapXuat{currentDay.ToString("dd-MM-yyyy")}_chi_nhanh_{chiNhanh}.pdf";
                if (File.Exists($@"{savePath}"))
                {
                    DialogResult dr = MessageBox.Show("Đã có file khác trùng tên với file của Report này", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr != DialogResult.Yes)
                    {
                        reportExportToPDF.ExportToPdf(savePath);
                        MessageBox.Show($"File đã được lưu thành công tại {savePath}", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                }
                else
                {
                    reportExportToPDF.ExportToPdf(savePath);
                    MessageBox.Show($"File đã được lưu thành công tại {savePath}", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception E)
            {
                MessageBox.Show("Lưu file thất bại" +E.Message,
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
            
        }

        private void cbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView" || cbChiNhanh.SelectedValue == null)
            {
                return;
            }
            Program.servername = cbChiNhanh.SelectedValue.ToString();

            // Nếu chọn chi nhánh khác với chi nhánh hiện tại
            if (cbChiNhanh.SelectedIndex != Program.brand)
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

            // kiểm tra kết nối tới server cần truy xuất dữ liệu
            if (Program.connectDB() == 0)
            {
                MessageBox.Show("Lỗi kết nối tới chi nhánh","Thông báo",MessageBoxButtons.OK);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FormHoatDongNhapXuat_Load(object sender, EventArgs e)
        {
            cbChiNhanh.DataSource = Program.bindingSource;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            cbChiNhanh.SelectedIndex = Program.brand;


            //Phân quyền nhóm CHINHANH-USER có thể xem theo chi nhánh của mình
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                cbChiNhanh.Enabled = false;
            }
        }
    }
}
