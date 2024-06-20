using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.Report
{
    public partial class RP_HoatdongNV : DevExpress.XtraReports.UI.XtraReport
    {
        public RP_HoatdongNV()
        {
            InitializeComponent();
        }

        public RP_HoatdongNV(String maNhanVien, DateTime fromDate, DateTime toDate)
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.conStr;
            this.sqlDataSource1.Queries[0].Parameters[0].Value = maNhanVien;
            this.sqlDataSource1.Queries[0].Parameters[1].Value = fromDate;
            this.sqlDataSource1.Queries[0].Parameters[2].Value = toDate;
            this.sqlDataSource1.Fill();
        }

    }
}
