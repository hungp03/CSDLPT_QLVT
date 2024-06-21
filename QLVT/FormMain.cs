using DevExpress.XtraBars;
using QLVT.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        //Bật, tắt các nút và ribbon
        public void EnableBtn()
        {
            btnTaoTK.Enabled = true;
            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;
            btnNhanVien.Enabled = true;
            btnVatTu.Enabled = true;
            ribbonPage1.Visible = ribbonPage2.Visible = true; 
        }

        private async void InitializeAsync()
        {
            await Task.Delay(1000); // Chờ 1 giây
            btnDangNhap.PerformClick(); // Tự động bấm nút đăng nhập
        }

        public FormMain()
        {
            InitializeComponent();
            InitializeAsync();
        }

       
        //Check xem form đã có chưa, nếu chưa thì mới khởi tạo form
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
            {
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

        private void btnDangNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void btnDangXuat_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            logout();

            btnDangNhap.Enabled = true;
            btnDangXuat.Enabled = false;
            btnNhanVien.Enabled = false;

            Form f = CheckExists(typeof(FormLogin));
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

            Program.formMain.UID.Text = "Mã nhân viên";
            Program.formMain.NAME.Text = "Họ tên";
            Program.formMain.GROUP.Text = "Vai trò";

            Program.username = "";
            Program.mName = "";
            Program.mGroup = "";
        }

        private void btnNhanVien_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormNhanVien));
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

        private void btnVatTu_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormVatTu));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormVatTu form = new FormVatTu();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void btnKho_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormKho));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormKho form = new FormKho();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void btnThoat_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát chứ", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormSupportDSNV));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormSupportDSNV form = new FormSupportDSNV(1);
                form.MdiParent = this;
                form.Show();
            }
        }

        private void btnTaoTK_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormTaoTK));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormTaoTK form = new FormTaoTK();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonPN_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormPhieuNhap));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormPhieuNhap form = new FormPhieuNhap();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonPX_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormPhieuXuat));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormPhieuXuat form = new FormPhieuXuat();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormDanhMucVatTu));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDanhMucVatTu form = new FormDanhMucVatTu();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormChiTietSoLuongTriGiaHangHoa));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormChiTietSoLuongTriGiaHangHoa form = new FormChiTietSoLuongTriGiaHangHoa();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormDonHangKhongPhieuNhap));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDonHangKhongPhieuNhap form = new FormDonHangKhongPhieuNhap();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormDonHangKhongPhieuNhap));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDonHangKhongPhieuNhap form = new FormDonHangKhongPhieuNhap();
                form.MdiParent = this;
                form.Show();
            }
        }
        private void barButtonDH_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormDonDatHang));
            if(f != null)
            {
                f.Activate();
            }
            else
            {
                FormDonDatHang form = new FormDonDatHang();
                form.MdiParent = this;
                form.Show();
            }
        }
        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormHoatdongNV));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormHoatdongNV form = new FormHoatdongNV();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form f = CheckExists(typeof(FormHoatDongNhapXuat));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormHoatDongNhapXuat form = new FormHoatDongNhapXuat();
                form.MdiParent = this;
                form.Show();
            }
        }
    }
    }
