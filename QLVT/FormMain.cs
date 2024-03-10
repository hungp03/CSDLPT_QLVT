using DevExpress.XtraBars;
using System;
using System.Threading.Tasks;
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
            btnNhanVien.Enabled = true;
        }

        //Dùng hàm này để delay 1s tránh việc máy không phản hồi kịp
        private async void InitializeAsync()
        {
            await Task.Delay(1000); // Chờ 1 giây
            btnLogin.PerformClick(); // Tự động bấm nút btnLogin
        }
        public FormMain()
        {
            InitializeComponent();
            InitializeAsync();
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
            btnNhanVien.Enabled = false;

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
            btnNhanVien.Enabled = false;

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

        private void btnNhanVien_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormNhanVien));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormNhanVien form = new FormNhanVien();
                form.MdiParent = this;
                form.Show();
            }
        }

    }
}
