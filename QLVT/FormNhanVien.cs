using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormNhanVien : Form
    {
        int position = 0;
        bool isAdding = false;
        String brandId = "";

        //Undo -> dùng để hoàn tác dữ liệu nếu lỡ có thao tác không mong muốn
        Stack undoStack = new Stack();
        
        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsNhanVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);

        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            //Không cần kiểm tra khóa ngoại
            dataSet1.EnforceConstraints = false;
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.conStr;
            // TODO: This line of code loads data into the 'dataSet1.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Fill(this.dataSet1.NhanVien);
            //Bug???
            brandId = ((DataRowView)bdsNhanVien[0])["MACN"].ToString();

            //Lấy thông tin chi nhánh từ form đăng nhập
            cbxChiNhanh.DataSource = Program.bindingSource;
            cbxChiNhanh.DisplayMember = "TENCN";
            cbxChiNhanh.ValueMember = "TENSERVER";
            cbxChiNhanh.SelectedIndex = Program.brand;

            //Phân quyền nhóm CONGTY chỉ được xem dữ liệu
            if (Program.mGroup == "CONGTY")
            {
                cbxChiNhanh.Enabled = true;
                barBtnThem.Enabled = false;
                barBtnSua.Enabled = false;
                barBtnLuu.Enabled = false;
                barBtnChuyenCN.Enabled = false;
                barBtnXoa.Enabled = false;
                barBtnPhucHoi.Enabled = false;
                panelNhapLieu.Enabled = false;
            }
            
            //Phân quyền nhóm CHINHANH-USER có thể thao tác với dữ liệu
            //Nhưng không được quyền chuyển chi nhánh khác để xem dữ liệu
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                cbxChiNhanh.Enabled = false;
                this.barBtnThem.Enabled = true;
                this.barBtnSua.Enabled = true;
                this.barBtnLuu.Enabled = true;
                this.barBtnChuyenCN.Enabled = true;
                this.barBtnXoa.Enabled = true;
                this.barBtnPhucHoi.Enabled = true;
                this.txtManv.Enabled = false;
                this.panelNhapLieu.Enabled = true;
            }

        }
        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

        private bool validateInput()
        {
            return validateEmployeeCode(txtManv.Text) &&
                   validateEmployeeName(txtHo.Text, txtTen.Text) &&
                   validateEmployeeAddress(txtDiachi.Text) &&
                   validateEmployeeBirthDate(deNgaySinh.DateTime) &&
                   validateEmployeeSalary(Convert.ToDecimal(txtLuong.EditValue));
        }

        private bool validateEmployeeCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Mã nhân viên không được trống", "Thông báo", MessageBoxButtons.OK);
                txtManv.Focus();
                return false;
            }
            if (!Regex.IsMatch(code, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Mã nhân viên phải là số", "Thông báo", MessageBoxButtons.OK);
                txtManv.Focus();
                return false;
            }
            return true;
        }

        private bool validateEmployeeName(string lastName, string firstName)
        {
            if (string.IsNullOrEmpty(lastName) || !Regex.IsMatch(lastName, @"^[A-Za-z ]+$") || lastName.Length > 40)
            {
                MessageBox.Show("Họ chỉ có chữ cái và khoảng trắng và không thể lớn hơn 40 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtHo.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(firstName) || !Regex.IsMatch(firstName, @"^[a-zA-Z ]+$") || firstName.Length > 10)
            {
                MessageBox.Show("Tên chỉ có chữ cái và khoảng trắng và không thể lớn hơn 10 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtTen.Focus();
                return false;
            }
            return true;
        }

        private bool validateEmployeeAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Địa chỉ không được bỏ trống", "Thông báo", MessageBoxButtons.OK);
                txtDiachi.Focus();
                return false;
            }
            if (!Regex.IsMatch(address, @"^[a-zA-Z0-9, ]+$") || address.Length > 100)
            {
                MessageBox.Show("Địa chỉ chỉ chấp nhận chữ cái, số và khoảng trắng và tối đa 100 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtDiachi.Focus();
                return false;
            }
            return true;
        }

        private bool validateEmployeeBirthDate(DateTime birthDate)
        {
            if (CalculateAge(birthDate) < 18)
            {
                MessageBox.Show("Nhân viên chưa đủ 18 tuổi", "Thông báo", MessageBoxButtons.OK);
                deNgaySinh.Focus();
                return false;
            }
            return true;
        }

        private bool validateEmployeeSalary(decimal salary)
        {
            if (salary < 4000000 || salary == 0)
            {
                MessageBox.Show("Mức lương không thể bỏ trống và tối thiểu là 4.000.000 đồng", "Thông báo", MessageBoxButtons.OK);
                txtLuong.Focus();
                return false;
            }
            return true;
        }

        private void hOTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void hOLabel_Click(object sender, EventArgs e)
        {

        }

        private void tENTextBox_TextChanged(object sender, EventArgs e)
        {

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView" || cbxChiNhanh.SelectedValue == null)
            {
                return;
            }
            Program.servername = cbxChiNhanh.SelectedValue.ToString();
            if (cbxChiNhanh.SelectedIndex != Program.brand)
            {
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }
            if (Program.connectDB() == 0)
            {
                MessageBox.Show("Lỗi kết nối tới chi nhánh", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                //Đổ data từ DS vào GC
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.conStr;
                this.nhanVienTableAdapter.Fill(dataSet1.NhanVien);
            }
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
        //Nút này có tác dụng đổ dữ liệu mới từ DS vào GC NhanVien
        private void barBtnLammoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                nhanVienTableAdapter.Fill(dataSet1.NhanVien);
                nhanVienGridControl.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }

        private void barBtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           /* position = bdsNhanVien.Position;
            isAdding = true;

            bdsNhanVien.AddNew();
            txtChiNhanh.Text = brandId;*/

        }
    }
}
