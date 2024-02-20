using QLVT.SubForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FormMain()
        {
            InitializeComponent();
        }

        //Check xem form đã có chưa, nếu chưa thì mới khởi tạo form
        private Form CheckExists(Type ftype) {
            foreach (Form f in this.MdiChildren) { 
            if (ftype == f.GetType())
                {
                    return f;
                }
            }
            return null;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormLogin));
            if (f != null)
            {
                f.Activate();        
            }
            else
            {
                FormLogin form = new FormLogin();
                form.Show();
            }
        }
    }
}
