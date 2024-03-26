using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.Report
{
    public partial class RpDSNV : DevExpress.XtraReports.UI.XtraReport
    {
        public RpDSNV()
        {
            InitializeComponent();
            sqlDataSource1.Connection.ConnectionString = Program.conStr;
            sqlDataSource1.Fill();
        }
    }
}
