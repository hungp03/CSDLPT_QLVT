using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.Report
{
    public partial class ReportDonHangKhongPhieuNhap : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportDonHangKhongPhieuNhap()
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.conStr;
            this.sqlDataSource1.Fill();
            xrLabel3.Text = "Ngày tạo báo cáo: " + DateTime.Now.ToString("HH:mm dd/MM/yyyy");
            xrLabel4.Text = "Người lập: " + Program.mName;
        }

    }
}
