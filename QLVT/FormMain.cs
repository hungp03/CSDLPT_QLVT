using DevExpress.XtraBars;
using QLVT.SubForm;
using System;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        //Bật, tắt các nút
        public void EnableBtn()
        {
            btnLogin.Enabled = false;
            btnDangXuat.Enabled = true;
        }
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

        //Đăng xuất
        private void logout()
        {
            foreach (Form f in this.MdiChildren)
                f.Dispose();
        }
        private void btnDangXuat_ItemClick(object sender, ItemClickEventArgs e)
        {
            logout();

            btnLogin.Enabled = true;
            btnDangXuat.Enabled = false;

            Form f = this.CheckExists(typeof(FormLogin));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormLogin form = new FormLogin();
                //form.MdiParent = this;
                form.Show();
            }

            Program.formMain.UID.Text = "Mã nhân viên:";
            Program.formMain.NAME.Text = "Họ tên:";
            Program.formMain.GROUP.Text = "Vai trò:";
        }

        private void FormMain_Load(object sender, EventArgs e)
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

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void btnDangXuat_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            logout();
            btnDangXuat.Enabled = false;
            btnLogin.Enabled = true;

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

            //Hiển thị thông tin ở góc màn hình
            Program.formMain.UID.Text = "Mã nhân viên";
            Program.formMain.NAME.Text = "Tên";
            Program.formMain.GROUP.Text = "Vai trò";

            Program.username = "";
            Program.mName = "";
            Program.mGroup = "";
        }
    }
}
