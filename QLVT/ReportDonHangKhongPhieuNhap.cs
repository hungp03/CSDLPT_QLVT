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
        }

    }
}
