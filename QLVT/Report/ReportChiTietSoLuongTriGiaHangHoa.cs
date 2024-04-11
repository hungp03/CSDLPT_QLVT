using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.Report
{
    public partial class ReportChiTietSoLuongTriGiaHangHoa : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportChiTietSoLuongTriGiaHangHoa()
        {
        }
        public ReportChiTietSoLuongTriGiaHangHoa(string vaitro, string loai, DateTime fromDate, DateTime toDate)
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.conStr;
            this.sqlDataSource1.Queries[0].Parameters[0].Value = vaitro;
            this.sqlDataSource1.Queries[0].Parameters[1].Value = loai;
            this.sqlDataSource1.Queries[0].Parameters[2].Value = fromDate;
            this.sqlDataSource1.Queries[0].Parameters[3].Value = toDate;
            this.sqlDataSource1.Fill();
        }
    }
}
