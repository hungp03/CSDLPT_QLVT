using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.Report
{
    public partial class ReportDSNV : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportDSNV()
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.conStr;
            this.sqlDataSource1.Fill();
        }

    }
}
