using DevExpress.CodeParser;
using DevExpress.DataProcessing.InMemoryDataProcessor;
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
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormNhanVien : Form
    {
        int position = 0;
        bool isAdding = false;
        string brandId = "";

        //Undo -> dùng để hoàn tác dữ liệu nếu lỡ có thao tác không mong muốn
        Stack undoStack = new Stack();

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        private void ThongBao(string mess)
        {
            MessageBox.Show(mess, "Thông báo", MessageBoxButtons.OK);
        }
        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsNhanVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS1);

        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {

            //Không cần kiểm tra khóa ngoại
            dS1.EnforceConstraints = false;
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.conStr;
            this.nhanVienTableAdapter.Fill(this.dS1.NhanVien);

            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.conStr;
            this.phieuXuatTableAdapter.Fill(this.dS1.PhieuXuat);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
            this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);

            this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
            this.datHangTableAdapter.Fill(this.dS1.DatHang);
            //Bug???
            brandId = ((DataRowView)bdsNhanVien[0])["MACN"].ToString();

            //Lấy thông tin chi nhánh từ form đăng nhập
            cbChiNhanh.DataSource = Program.bindingSource;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            cbChiNhanh.SelectedIndex = Program.brand;

            //Phân quyền nhóm CONGTY chỉ được xem dữ liệu
            if (Program.mGroup == "CONGTY")
            {
                btnThem.Enabled = false;
                btnGhi.Enabled = false;
                btnChuyenCN.Enabled = false;
                btnXoa.Enabled = false;
                btnHoanTac.Enabled = false;
                panelNhapLieu.Enabled = false;
            }

            //Phân quyền nhóm CHINHANH-USER có thể thao tác với dữ liệu
            //Nhưng không được quyền chuyển chi nhánh khác để xem dữ liệu
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                cbChiNhanh.Enabled = false;
                this.txtManv.Enabled = false;
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
                   validateEmployeeAddress(txtDiaChi.Text) &&
                   validateEmptyBirthDate() &&
                   validateEmployeeBirthDate(deNgaySinh.DateTime) &&
                   validateEmployeeSalary(txtLuong.EditValue.ToString())
            && validateEmployeeID(txtCMND.Text); 
        }

        private bool validateEmployeeCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ThongBao("Mã nhân viên không được bỏ trống");
                txtManv.Focus();
                return false;
            }
            if (!Regex.IsMatch(code, @"^[a-zA-Z0-9]+$"))
            {
                ThongBao("Mã nhân viên phải là số");
                txtManv.Focus();
                return false;
            }
            return true;
        }
        
        private bool validateEmployeeName(string lastName, string firstName)
        {
            if (string.IsNullOrEmpty(lastName) || !Regex.IsMatch(lastName, @"^[\p{L}\p{N}, ]+$") || lastName.Length > 40)
            {
                ThongBao("Họ chỉ có chữ cái và khoảng trắng và không thể lớn hơn 40 kí tự");
                txtHo.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(firstName) || !Regex.IsMatch(firstName, @"^[\p{L}\p{N}, ]+$") || firstName.Length > 10)
            {
                ThongBao("Tên chỉ có chữ cái và khoảng trắng và không thể lớn hơn 10 kí tự");
                txtTen.Focus();
                return false;
            }
            return true;
        }

        private bool validateEmployeeAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                ThongBao("Địa chỉ không được bỏ trống");
                txtDiaChi.Focus();
                return false;
            }
            if (!Regex.IsMatch(address, @"^[\p{L}\p{N}, ]+$") || address.Length > 100)
            {
                ThongBao("Địa chỉ chỉ chấp nhận chữ cái, số và khoảng trắng và tối đa 100 kí tự");
                txtDiaChi.Focus();
                return false;
            }

            return true;
        }
        private bool validateEmptyBirthDate()
        {
            if (deNgaySinh.DateTime == DateTime.MinValue)
            {
                ThongBao("Chưa nhập ngày sinh");
                deNgaySinh.Focus();
                return false;
            }
            return true;
        }

        private bool validateEmployeeBirthDate(DateTime birthDate)
        {
            if (CalculateAge(birthDate) < 18)
            {
                ThongBao("Nhân viên chưa đủ 18 tuổi");
                deNgaySinh.Focus();
                return false;
            }
            return true;
        }

        private bool validateEmployeeSalary(string input)
        {
            if (!Decimal.TryParse(input, out decimal salary)) // Kiểm tra xem chuỗi có thể chuyển đổi thành số decimal không
            {
                ThongBao("Vui lòng nhập mức lương là một số.");
                txtLuong.Focus();
                return false;
            }
            else if (salary <= 0 || salary < 4000000) // Kiểm tra mức lương có phải là số dương và lớn hơn hoặc bằng 4.000.000 không
            {
                ThongBao("Mức lương phải là số dương và tối thiểu là 4.000.000 đồng.");
                txtLuong.Focus();
                return false;
            }
            else if (salary > int.MaxValue)
            {
                ThongBao("Mức lương quá lớn, không phù hợp.");
                txtLuong.Focus();
                return false;
            }
            return true;
        }

        private bool validateEmployeeID(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ThongBao("CMND/CCCD không được bỏ trống");
                txtCMND.Focus();
                return false;
            }
            if (!Regex.IsMatch(id, @"^\d{9,12}$"))
            {
                ThongBao("CMND/CCCD phải có từ 9 đến 12 số");
                txtCMND.Focus();
                return false;
            }
            return true;
        }




        private void cbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView" || cbChiNhanh.SelectedValue == null)
            {
                return;
            }
            Program.servername = cbChiNhanh.SelectedValue.ToString();

            // Nếu chọn chi nhánh khác với chi nhánh hiện tại
            if (cbChiNhanh.SelectedIndex != Program.brand)
            {
                // Dùng tài khoản hỗ trợ kết nối để chuẩn bị cho việc login vào chi nhánh khác
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
                // Lấy tài khoản hiện tại đang đăng nhập để đăng nhập
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }
            if (Program.connectDB() == 0)
            {
                ThongBao("Lỗi kết nối tới chi nhánh");
            }
            else
            {
                //Đổ data từ DS vào TA
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.conStr;
                this.nhanVienTableAdapter.Fill(dS1.NhanVien);

                this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
                this.datHangTableAdapter.Fill(this.dS1.DatHang);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.conStr;
                this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.conStr;
                this.phieuXuatTableAdapter.Fill(this.dS1.PhieuXuat);
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnLamMoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                nhanVienTableAdapter.Fill(dS1.NhanVien);
                nhanVienGridControl.Enabled = true;
            }
            catch (Exception ex)
            {
                ThongBao("Lỗi khi làm mới dữ liệu: " + ex.Message);
                return;
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLamMoi.Enabled = false;
            btnChuyenCN.Enabled = false;
            btnThoat.Enabled = false;
            checkboxTHXoa.Checked = false;
            btnHoanTac.Enabled = true;
            nhanVienGridControl.Enabled = false;
            panelNhapLieu.Enabled = true;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string maNV = ((DataRowView)bdsNhanVien[bdsNhanVien.Position])["MANV"].ToString();
            int status = int.Parse(((DataRowView)bdsNhanVien[bdsNhanVien.Position])["TrangThaiXoa"].ToString());
            // Không cho phép xóa tài khoảng đang đăng nhập
            if (maNV == Program.username)
            {
                ThongBao("Không thể xóa tài khoản đang đăng nhập");
                return;
            }

            // Nếu không có nhân viên trong bds, vô hiệu nút xóa
            if (bdsNhanVien.Count == 0)
            {
                btnXoa.Enabled = false;
            }

            // user đã lập phiếu nhập thì không cho xóa
            if (bdsPhieuNhap.Count > 0)
            {
                ThongBao("Không thể xóa tài khoản đã tạo phiếu nhập");
                return;
            }

            // user đã lập phiếu nhập thì không cho xóa
            if (bdsPhieuXuat.Count > 0)
            {
                ThongBao("Không thể xóa tài khoản đã tạo phiếu xuất");
                return;
            }

            // user đã đặt đơn đặt hàng thì không cho xóa
            if (bdsDatHang.Count > 0)
            {
                ThongBao("Không thế xóa tài khoản đã lập đơn đặt hàng");
                return;
            }
            if (status == 1)
            {
                ThongBao("Nhân viên đã bị xóa, đang ở chi nhánh khác");
                return;
            }

            DateTime ngsinh = (DateTime)((DataRowView)bdsNhanVien[bdsNhanVien.Position])["NGAYSINH"];

            //Tạo truy vấn hoàn tác, đưa vào undoStack
            string undoQuery = string.Format("INSERT INTO DBO.NHANVIEN( MANV, CMND,HO,TEN,DIACHI,NGAYSINH,LUONG,MACN)" +
            "VALUES({0},N'{1}',N'{2}',N'{3}',N'{4}',CAST('{5}' AS DATETIME), {6},'{7}')", txtManv.Text, txtCMND.Text, txtHo.Text, txtTen.Text, txtDiaChi.Text, ((DateTime)deNgaySinh.EditValue).ToString("yyyy-MM-dd"), txtLuong.EditValue, txtMacn.Text.Trim());
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
                    this.nhanVienTableAdapter.Update(this.dS1.NhanVien);

                    ThongBao("Xóa thành công ");
                    this.btnHoanTac.Enabled = true;
                }
                catch (Exception ex)
                {
                    ThongBao("Có lỗi khi xóa nhân viên " + ex.Message);
                    this.nhanVienTableAdapter.Update(this.dS1.NhanVien);

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

        private int ExecuteSP_TracuuNV(string maNv)
        {
            string query =
                "DECLARE @result int " +
                "EXEC @result = [dbo].[sp_TraCuuNV] '" +
                maNv + "' " +
                "SELECT @result";

            int result = -1;

            try
            {
                if (Program.myReader != null && !Program.myReader.IsClosed)
                {
                    Program.myReader.Close();
                }

                Program.myReader = Program.ExecSqlDataReader(query);

                // Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return result;
                }

                if (Program.myReader.Read())
                {
                    result = int.Parse(Program.myReader.GetValue(0).ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiểm tra MANV thất bại\n" + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Program.myReader != null && !Program.myReader.IsClosed)
                {
                    Program.myReader.Close();
                }
            }

            return result;
        }


        private int ExecuteSP_TracuuCMND(string cmnd, string maNv)
        {
            string query =
                "DECLARE @result int " +
                "EXEC @result = [dbo].[SP_KiemtraCMND] '" +
                cmnd + "', '" + maNv + "' " +
                "SELECT @result";

            int result = -1;

            try
            {
                if (Program.myReader != null && !Program.myReader.IsClosed)
                {
                    Program.myReader.Close();
                }

                Program.myReader = Program.ExecSqlDataReader(query);

                if (Program.myReader == null)
                {
                    return result;
                }

                if (Program.myReader.Read())
                {
                    result = int.Parse(Program.myReader.GetValue(0).ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiểm tra CMND thất bại\n" + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Program.myReader != null && !Program.myReader.IsClosed)
                {
                    Program.myReader.Close();
                }
            }

            return result;
        }


        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // kiểm tra đầu vào có hợp lệ hay không
            if (validateInput() == false)
            {
                return;
            }

            // Lấy dữ liệu trước khi ghi phục vụ cho việc hoàn tác
            string maNv = txtManv.Text.Trim();
            DataRowView drv = ((DataRowView)bdsNhanVien[bdsNhanVien.Position]);
            string cmnd = drv["CMND"].ToString();
            string ho = drv["HO"].ToString();
            string ten = drv["TEN"].ToString();
            string diaChi = drv["DIACHI"].ToString();
            //Console.WriteLine(drv["NGAYSINH"]);
            DateTime ngaySinh = ((DateTime)drv["NGAYSINH"]);
            string luongstr = drv["LUONG"].ToString();
            //Console.WriteLine(luongstr);
            int luong = int.Parse(luongstr);
            //string maChiNhanh = drv["MACN"].ToString();
            int trangThai = (checkboxTHXoa.Checked == true) ? 1 : 0;

            //Sử dụng kết quả bước trên và vị trí của txtManv => các trường hợp xảy ra
            /*TH1: result = 1 && pointerPosition != nvPosition->Thêm mới nhưng MANV đã tồn tại
            TH2: result = 1 && pointerPosition == nvPosition->Sửa nhân viên đang tồn tại
            TH3: result = 0 && pointerPosition == nvPosition->Thêm mới bình thường
            TH4: result = 0 && pointerPosition != nvPosition->Thêm mới bình thường*/

            int nvResult = ExecuteSP_TracuuNV(maNv);
            int pointerPosition = bdsNhanVien.Position;
            int nvPosition = bdsNhanVien.Find("MANV", txtManv.Text);
           
            if (nvResult != 1 && nvResult != 0)
            {
                ThongBao("Có lỗi trong quá trình xử lý");
                return;
            }
            // TH1
            if (nvResult == 1 && pointerPosition != nvPosition)
            {
                ThongBao("Mã NV đã tồn tại");
                return;
            }
            // TH2,3,4

            // Kiểm tra CMND
            int cmndResult = ExecuteSP_TracuuCMND(txtCMND.Text.Trim(), maNv);
            //Console.WriteLine(cmndResult);
            if(cmndResult != 0 && cmndResult != 1)
            {
                ThongBao("Lỗi kiểm tra CMND");
                return;
            }
            if (cmndResult == 0)
            {
                ThongBao("CMND đã tồn tại");
                return;
            }
            DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                try
                {
                    // Bật lại các nút
                    btnThem.Enabled = true;
                    btnXoa.Enabled = true;
                    btnGhi.Enabled = true;
                    btnLamMoi.Enabled = true;
                    btnHoanTac.Enabled = true;

                    btnLamMoi.Enabled = true;
                    btnChuyenCN.Enabled = true;
                    btnThoat.Enabled = true;

                    this.txtManv.Enabled = false;
                    this.bdsNhanVien.EndEdit();
                    this.nhanVienTableAdapter.Update(this.dS1.NhanVien);
                    this.nhanVienGridControl.Enabled = true;

                    // Lưu truy vấn phục vụ hoàn tác
                    string undoQuery = "";

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
                        undoQuery = "UPDATE DBO.NhanVien " +
                        "SET " +
                        "CMND = N'" + cmnd + "'," +
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
                    ThongBao("Ghi thành công");
                }
                catch (Exception ex)
                {
                    bdsNhanVien.RemoveCurrent();
                    MessageBox.Show("Thất bại. Vui lòng kiểm tra lại!\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHoanTac_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Kiểm tra xem nút thêm đã bấm hay chưa
            if (isAdding == true && btnThem.Enabled == false)
            {
                isAdding = false;

                txtManv.Enabled = false;
                btnThem.Enabled = true;
                btnXoa.Enabled = true;
                btnGhi.Enabled = true;

                btnHoanTac.Enabled = false;
                btnLamMoi.Enabled = true;
                btnChuyenCN.Enabled = true;
                checkboxTHXoa.Checked = false;
                btnThoat.Enabled = true;

                nhanVienGridControl.Enabled = true;
                panelNhapLieu.Enabled = true;

                //Hủy thao tác thêm
                bdsNhanVien.CancelEdit();
                // Quay lại vị trí cũ
                bdsNhanVien.Position = position;
                return;
            }


            // Kiểm tra undoStack trống hay không
            // Trường hợp stack trống thì không thực hiện
            if (undoStack.Count == 0)
            {
                ThongBao("Không có tháo tác để khôi phục");
                btnHoanTac.Enabled = false;
                return;
            }

            // Trường hợp stack còn

            //Hủy thao tác trên bds
            bdsNhanVien.CancelEdit();
            // Tạo một String để lưu truy vấn được lấy ra từ stack
            string undoSql = undoStack.Pop().ToString();
            //Console.WriteLine(undoSql);

            //Nếu lệnh undo là lệnh chuyển chi nhánh
            if (undoSql.Contains("SP_ChuyenCN"))
            {
                try
                {
                    string currentBrand = Program.servername;
                    string newBrand = Program.otherServerName;

                    Program.servername = newBrand;
                    Program.mlogin = Program.remotelogin;
                    Program.password = Program.remotepassword;

                    if (Program.connectDB() == 0)
                    {
                        return;
                    }
                    _ = Program.ExecSqlNonQuery(undoSql);
                    ThongBao("Chuyển nhân viên trở lại thành công");
                    Program.servername = newBrand;
                    Program.mlogin = Program.mloginDN;
                    Program.password = Program.passwordDN;
                }
                catch (Exception ex)
                {
                    ThongBao("Chuyển nhân viên thất bại \n" + ex.Message);
                    return;
                }
            }
            else
            {
                if (Program.connectDB() == 0)
                {
                    return;
                }

                _ = Program.ExecSqlNonQuery(undoSql);

            }
            nhanVienTableAdapter.Fill(this.dS1.NhanVien);
        }

        private void btnChuyenCN_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int currentPosition = bdsNhanVien.Position;
            int deleteStatus = int.Parse(((DataRowView)(bdsNhanVien[currentPosition]))["TrangThaiXoa"].ToString());
            string maNv = ((DataRowView)(bdsNhanVien[currentPosition]))["MANV"].ToString();

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
            btnHoanTac.Enabled = true;
        }
        public void chuyenCN(string chiNhanh)
        {
            Console.WriteLine("Chi nhánh đang chọn: " + chiNhanh);
            int currentPosition = bdsNhanVien.Position;
            string maNhanVien = ((DataRowView)bdsNhanVien[currentPosition])["MANV"].ToString();

            /* if (Program.servername == chiNhanh)
             {
                 MessageBox.Show("Hãy chọn chi nhánh khác chi nhánh bạn đang đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;
             }*/

            // Lưu chi nhánh hiện tại và chi nhánh chuyển tới, tên nhân viên được chuyển
            string currBrand;
            string new_Brand;
            if (chiNhanh.Contains("1"))
            {
                currBrand = "CN2";
                new_Brand = "CN1";
            }
            else if (chiNhanh.Contains("2"))
            {
                currBrand = "CN1";
                new_Brand = "CN2";
            }
            else
            {
                MessageBox.Show("Mã chi nhánh không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Console.WriteLine("Chi nhánh hiện tại: " + currBrand);
            Console.WriteLine("Chi nhánh mới: " + new_Brand);

            // Lưu truy vấn để hoàn tác
            string undoQuery = "EXEC SP_ChuyenCN " + maNhanVien + ",'" + currBrand + "'";
            undoStack.Push(undoQuery);

            // Lấy tên chi nhánh chuyển tới để làm tính năng hoàn tác
            Program.otherServerName = chiNhanh;

            // Thực hiện chức năng chuyển chi nhánh, dùng SP
            string query = "EXEC SP_ChuyenCN " + maNhanVien + ",'" + new_Brand + "'";
            _ = new SqlCommand(query, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);
                // Nếu không có kết quả trả về thì kết thúc
                if (Program.myReader == null)
                {
                    return;
                }
                ThongBao("Chuyển chi nhánh thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra trong quá trình thực thi!\n\n" + ex.Message, "thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            this.nhanVienTableAdapter.Fill(this.dS1.NhanVien);
        }

    }
}
