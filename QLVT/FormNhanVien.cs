using DevExpress.ChartRangeControlClient.Core;
using QLVT.SubForm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
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
            this.nhanVienTableAdapter.Fill(this.dataSet1.NhanVien);

            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.conStr;
            this.phieuXuatTableAdapter.Fill(this.dataSet1.PhieuXuat);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
            this.phieuNhapTableAdapter.Fill(this.dataSet1.PhieuNhap);

            this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
            this.datHangTableAdapter.Fill(this.dataSet1.DatHang);
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
            int age = DateTime.Now.Year - dateOfBirth.Year;
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
            if (string.IsNullOrEmpty(lastName) || !Regex.IsMatch(lastName, @"^[\p{L}\p{N}, ]+$") || lastName.Length > 40)
            {
                MessageBox.Show("Họ chỉ có chữ cái và khoảng trắng và không thể lớn hơn 40 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtHo.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(firstName) || !Regex.IsMatch(firstName, @"^[\p{L}\p{N}, ]+$") || firstName.Length > 10)
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
            if (!Regex.IsMatch(address, @"^[\p{L}\p{N}, ]+$") || address.Length > 100)
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
            this.Close();
        }
        //Nút này có tác dụng đổ dữ liệu mới từ DS vào GC NhanVien
        private void barBtnLammoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                nhanVienTableAdapter.Fill(dataSet1.NhanVien);
                nhanVienGridControl.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }

        private void barBtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Lấy vị trí của con trỏ
            position = bdsNhanVien.Position;
            isAdding = true;
            panelNhapLieu.Enabled = true;

            //Thêm dòng mới bằng hàm AddNew
            bdsNhanVien.AddNew();
            txtMacn.Text = brandId;

            // Thay đổi bật/ tắt các nút chức năng
            txtManv.Enabled = true;
            barBtnThem.Enabled = false;
            barBtnXoa.Enabled = false;
            barBtnLuu.Enabled = true;

            barBtnPhucHoi.Enabled = true;
            barBtnLammoi.Enabled = false;
            barBtnChuyenCN.Enabled = false;
            barBtnThoat.Enabled = false;
            checkboxTHXoa.Checked = false;

            nhanVienGridControl.Enabled = false;
            panelNhapLieu.Enabled = true;
        }

        private void barBtnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Kiểm tra xem nút thêm đã bấm hay chưa
            if (isAdding == true && barBtnThem.Enabled == false)
            {
                isAdding = false;

                txtManv.Enabled = false;
                barBtnThem.Enabled = true;
                barBtnXoa.Enabled = true;
                barBtnLuu.Enabled = true;

                barBtnPhucHoi.Enabled = false;
                barBtnLammoi.Enabled = true;
                barBtnChuyenCN.Enabled = true;
                checkboxTHXoa.Checked = false;
                barBtnThoat.Enabled = true;

                nhanVienGridControl.Enabled = true;
                panelNhapLieu.Enabled = true;

                // Hủy bỏ thao tác trên bds
                bdsNhanVien.CancelEdit();

                //Xóa dòng hiện tại đang được thêm bởi nút thêm
                bdsNhanVien.RemoveCurrent();

                // Quay lại vị trí cũ
                bdsNhanVien.Position = position;
                return;
            }


            // Kiểm tra undoStack trống hay không
            // Trường hợp stack trống thì không thực hiện
            if (undoStack.Count == 0)
            {
                MessageBox.Show("Không có tháo tác để khôi phục", "Thông báo", MessageBoxButtons.OK);
                barBtnPhucHoi.Enabled = false;
                return;
            }

            // Trường hợp stack còn

            //Hủy thao tác trên bds
            bdsNhanVien.CancelEdit();
            // Tạo một String để lưu truy vấn được lấy ra từ stack
            String undoSql = undoStack.Pop().ToString();
            //Console.WriteLine(undoSql);

            //Nếu lệnh undo là lệnh chuyển chi nhánh
            if (undoSql.Contains("SP_ChuyenCN"))
            {
                try
                {
                    String currentBrand = Program.servername;
                    String newBrand = Program.otherServerName;

                    Program.servername = newBrand;
                    Program.mlogin = Program.remotelogin;
                    Program.password = Program.remotepassword;

                    if (Program.connectDB() == 0)
                    {
                        return;
                    }
                    int tmp = Program.ExecSqlNonQuery(undoSql);
                    MessageBox.Show("Chuyển nhân viên trở lại thành công", "Thông báo", MessageBoxButtons.OK);
                    Program.servername = newBrand;
                    Program.mlogin = Program.mloginDN;
                    Program.password = Program.passwordDN;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chuyển nhân viên thất bại \n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    return;
                }
            }
            else
            {
                if (Program.connectDB() == 0)
                {
                    return;
                }
                int tmp = Program.ExecSqlNonQuery(undoSql);

            }
            this.nhanVienTableAdapter.Fill(this.dataSet1.NhanVien);
        }

        private void barBtnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String maNV = ((DataRowView)bdsNhanVien[bdsNhanVien.Position])["MANV"].ToString();

            // Không cho phép xóa tài khoảng đang đăng nhập
            if (maNV == Program.username)
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            // Nếu không có nhân viên trong bds, vô hiệu nút xóa
            if (bdsNhanVien.Count == 0)
            {
                barBtnXoa.Enabled = false;
            }

            // user đã lập phiếu nhập thì không cho xóa
            if (bdsPhieuNhap.Count > 0)
            {
                MessageBox.Show("Không thể xóa tài khoản đã tạo phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            // user đã lập phiếu nhập thì không cho xóa
            if (bdsPhieuXuat.Count > 0)
            {
                MessageBox.Show("Không thể xóa tài khoản đã tạo phiếu xuất", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            // user đã đặt đơn đặt hàng thì không cho xóa
            if (bdsDatHang.Count > 0)
            {
                MessageBox.Show("Không thế xóa tài khoản đã lập đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            int status = checkboxTHXoa.Checked ? 1 : 0;
            DateTime ngsinh = (DateTime)((DataRowView)bdsNhanVien[bdsNhanVien.Position])["NGAYSINH"];

            //Tạo truy vấn hoàn tác, đưa vào undoStack
            String undoQuery = string.Format("INSERT INTO DBO.NHANVIEN( MANV,HO,TEN,DIACHI,NGAYSINH,LUONG,MACN)" +
            "VALUES({0},N'{1}',N'{2}',N'{3}',CAST('{4}' AS DATETIME), {5},'{6}')", txtManv.Text, txtHo.Text, txtTen.Text, txtDiachi.Text, ((DateTime)deNgaySinh.EditValue).ToString("yyyy-MM-dd"), txtLuong.EditValue, txtMacn.Text.Trim());
            /*Console.WriteLine(undoQuery);*/

            undoStack.Push(undoQuery);

            // Xác nhận nếu bấm OK thì xóa
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                // Nút xóa này là xóa hẳn, còn chuyển chi nhánh thì mới cho trạng thái sang 1
                try
                {
                    position = bdsNhanVien.Position;
                    bdsNhanVien.RemoveCurrent();
                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.nhanVienTableAdapter.Update(this.dataSet1.NhanVien);

                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.barBtnPhucHoi.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi xóa nhân viên " + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.nhanVienTableAdapter.Update(this.dataSet1.NhanVien);

                    // Trả lại vị trí cũ của nhân viên xóa bị lỗi
                    bdsNhanVien.Position = position;
                    return;
                }
            }
            else
            {
                undoStack.Pop();
            }
        }

        private void barBtnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // kiểm tra đầu vào có hợp lệ hay không
            if (validateInput() == false)
            {
                return;
            }
            
            // Lấy dữ liệu trước khi ghi phục vụ cho việc hoàn tác
            String maNv = txtManv.Text.Trim();
            DataRowView drv = ((DataRowView)bdsNhanVien[bdsNhanVien.Position]);
            String ho = drv["HO"].ToString();
            String ten = drv["TEN"].ToString();
            String diaChi = drv["DIACHI"].ToString();
            DateTime ngaySinh = ((DateTime)drv["NGAYSINH"]);
            // Console.WriteLine(ngaySinh);

            int luong = int.Parse(drv["LUONG"].ToString());
            String maChiNhanh = drv["MACN"].ToString();
            int trangThai = (checkboxTHXoa.Checked == true) ? 1 : 0;

            string query = "declare @res int\n" + "EXEC @res = SP_TracuuNV '" + maNv + "'\nselect @res";
            SqlCommand sqlCommand = new SqlCommand(query, Program.conn);
            // Dùng SP để kiểm tra xem có nhân viên với mã nv đang tạo
            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);
                // Nếu không có kết quả thì quay về
                if (Program.myReader == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Console.WriteLine(ex.Message);
                return;
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();

            //Sử dụng kết quả bước trên và vị trí của txtManv => các trường hợp xảy ra
            /*TH1: ketQua = 1 && pointerPosition != nvPosition->Thêm mới nhưng MANV đã tồn tại
            TH2: ketQua = 1 && pointerPosition == nvPosition->Sửa nhân viên đang tồn tại
            TH3: ketQua = 0 && pointerPosition == nvPosition->Thêm mới bình thường
            TH4: ketQua = 0 && pointerPosition != nvPosition->Thêm mới bình thường*/

            int pointerPosition = bdsNhanVien.Position;
            int nvPosition = bdsNhanVien.Find("MANV", txtManv.Text);

            // TH1
            if (result == 1 && pointerPosition != nvPosition)
            {
                MessageBox.Show("Mã NV đã tồn tại", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            // TH2,3,4
            else
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        // Bật lại các nút
                        barBtnThem.Enabled = true;
                        barBtnXoa.Enabled = true;
                        barBtnLuu.Enabled = true;
                        barBtnPhucHoi.Enabled = true;


                        barBtnLammoi.Enabled = true;
                        barBtnChuyenCN.Enabled = true;
                        barBtnThoat.Enabled = true;

                        this.txtManv.Enabled = false;
                        this.bdsNhanVien.EndEdit();
                        this.nhanVienTableAdapter.Update(this.dataSet1.NhanVien);
                        this.nhanVienGridControl.Enabled = true;

                        // Lưu truy vấn phục vụ hoàn tác
                        String undoQuery = "";
                        
                        // Trường hợp thêm nhân viên
                        if (isAdding == true)
                        {
                            undoQuery = "" +
                                "DELETE DBO.NHANVIEN " +
                                "WHERE MANV = " + txtManv.Text.Trim();
                        }

                        // Trường hợp sửa nhân viên
                        else
                        {
                            undoQuery ="UPDATE DBO.NhanVien " +
                            "SET " +
                            "HO = N'" + ho + "'," +
                            "TEN = N'" + ten + "'," +
                            "DIACHI = N'" + diaChi + "'," +
                            "NGAYSINH = CAST('" + ngaySinh.ToString("yyyy-MM-dd") + "' AS DATETIME)," +
                            "LUONG = '" + luong + "'," +
                            "TrangThaiXoa = " + trangThai + " " +
                            "WHERE MANV = '" + maNv + "'";
                        }
                        // Console.WriteLine(undoQuery);

                        // Đưa câu truy vấn hoàn tác vào undoList
                        undoStack.Push(undoQuery);

                        // Cập nhật lại trạng thái đang thêm mới
                        isAdding = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        bdsNhanVien.RemoveCurrent();
                        MessageBox.Show("Thất bại. Vui lòng kiểm tra lại!\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void barBtnChuyenCN_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int currentPosition = bdsNhanVien.Position;
            int deleteStatus = int.Parse(((DataRowView)(bdsNhanVien[currentPosition]))["TrangThaiXoa"].ToString());
            String maNv = ((DataRowView)(bdsNhanVien[currentPosition]))["MANV"].ToString();

            //Không cho chuyển chi nhánh của người đang đăng nhập
            if (Program.username == maNv)
            {
                MessageBox.Show("Không thể chuyển người đang đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiểm tra trạng thái xóa, nếu đã xóa thì không chuyển nữa
            if (deleteStatus == 1)
            {
                MessageBox.Show("NV này không còn ở CN này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra form chuyển CN có chưa
            Form f = this.CheckExists(typeof(FormChuyenCN));
            if (f != null)
            {
                f.Activate();
            }
            FormChuyenCN form = new FormChuyenCN();
            form.Show();

            //Chuyển hàm chuyển CN sang form chuyển CN
            form.branchTransfer = new FormChuyenCN.MyDelegate(chuyenCN);
            barBtnPhucHoi.Enabled = true;
        }
        
        public void chuyenCN(String chiNhanh)
        {
            Console.WriteLine("Chi nhánh đang chọn: " + chiNhanh);
            // Nếu chi nhánh đang chọn là chi nhánh hiện tại, thì không cho chuyển
            /*if (chiNhanh == Program.servername)
            {
                MessageBox.Show("Hãy chọn chi nhánh khác", "Thông báo", MessageBoxButtons.OK);
                return;
            }*/
            // Lưu chi nhánh hiện tại và chi nhánh chuyển tới, tên nhân viên được chuyển
            String currentBrand ="";
            String newBrand = "";
            int currentPosition = bdsNhanVien.Position;
            String maNhanVien = ((DataRowView)bdsNhanVien[currentPosition])["MANV"].ToString();

            if (chiNhanh.Contains("1"))
            {
                currentBrand = "CN2";
                newBrand = "CN1";
            }
            else if (chiNhanh.Contains("2"))
            {
                currentBrand = "CN1";
                newBrand = "CN2";
            }
            else
            {
                MessageBox.Show("Mã chi nhánh không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Console.WriteLine("Chi nhánh hiện tại: " + currentBrand);
            Console.WriteLine("Chi nhánh mới: " + newBrand);

            // Lưu truy vấn để hoàn tác
            String undoQuery = "EXEC SP_ChuyenCN " + maNhanVien + ",'" + currentBrand + "'";
            undoStack.Push(undoQuery);

            // Lấy tên chi nhánh chuyển tới để làm tính năng hoàn tác
            Program.otherServerName = chiNhanh;
            Console.WriteLine("Ten server con lai" + Program.otherServerName);

            // Thực hiện chức năng chuyển chi nhánh, dùng SP
            String query = "EXEC SP_ChuyenCN " + maNhanVien + ",'" + newBrand + "'";

            SqlCommand sqlCommand = new SqlCommand(query, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);
                MessageBox.Show("Chuyển chi nhánh thành công", "thông báo", MessageBoxButtons.OK);
                // Nếu không có kết quả trả về thì kết thúc
                if (Program.myReader == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra trong quá trình thực thi!\n\n" + ex.Message, "thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            this.nhanVienTableAdapter.Fill(this.dataSet1.NhanVien);
        }
    }
    
}
