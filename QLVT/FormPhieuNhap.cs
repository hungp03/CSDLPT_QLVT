using DevExpress.CodeParser;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormPhieuNhap : Form
    {
        public string makho = "";
        string maChiNhanh = "";
        String brandId = "";
        int position = 0;
        bool isAdding = false;

        //Undo -> dùng để hoàn tác dữ liệu nếu lỡ có thao tác không mong muốn
        Stack undoList = new Stack();
        Stack undoCTPN = new Stack();
        Stack undoCTPNSL = new Stack();

        BindingSource bds = null;
        GridControl gc = null;
        string type = "";

        public FormPhieuNhap()
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
        private void ThongBao(String mess)
        {
            MessageBox.Show(mess, "Thông báo", MessageBoxButtons.OK);
        }
        private void phieuNhapBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPhieuNhap.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS1);
        }
        private void FormPhieuNhap_Load(object sender, EventArgs e)
        {

            //Không cần kiểm tra khóa ngoại
            dS1.EnforceConstraints = false;

            // TODO: This line of code loads data into the 'dS1.Vattu' table. You can move, or remove it, as needed.
            this.vattuTableAdapter.Connection.ConnectionString = Program.conStr;
            this.vattuTableAdapter.Fill(this.dS1.Vattu);
            // TODO: This line of code loads data into the 'dS1.CTPN' table. You can move, or remove it, as needed.
            this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTPNTableAdapter.Fill(this.dS1.CTPN);
            // TODO: This line of code loads data into the 'dS1.DSKHO' table. You can move, or remove it, as needed.
            this.dSKHOTableAdapter.Connection.ConnectionString = Program.conStr;
            this.dSKHOTableAdapter.Fill(this.dS1.DSKHO);
            // TODO: This line of code loads data into the 'dS1.DSNV' table. You can move, or remove it, as needed.
            this.dSNVTableAdapter.Connection.ConnectionString = Program.conStr;
            this.dSNVTableAdapter.Fill(this.dS1.DSNV);
            // TODO: This line of code loads data into the 'dS1.DatHang' table. You can move, or remove it, as needed.
            this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
            this.datHangTableAdapter.Fill(this.dS1.DatHang);

            // TODO: This line of code loads data into the 'dS1.PhieuNhap' table. You can move, or remove it, as needed.
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
            this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);

            cbChiNhanh.DataSource = Program.bindingSource;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            cbChiNhanh.SelectedIndex = Program.brand;

            /*brandId = ((DataRowView)bdsPhieuNhap[0])["MACN"].ToString();*/
            //Phân quyền nhóm CONGTY chỉ được xem dữ liệu
            if (Program.mGroup == "CONGTY")
            {
                btnThem.Enabled = false;
                btnGhi.Enabled = false;
                btnXoa.Enabled = false;
                btnHoanTac.Enabled = false;
                groupBoxPhieuNhap.Enabled = false;
                dgvCTPN.Enabled = false;
                contextMenuStripCTPN.Enabled = false;
            }

            //Phân quyền nhóm CHINHANH-USER có thể thao tác với dữ liệu
            //Nhưng không được quyền chuyển chi nhánh khác để xem dữ liệu
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                cbChiNhanh.Enabled = false;
            }
            if (bdsCTPN.Position < 0)
            {
                ghiToolStripMenuItem.Enabled = false;
                xóaToolStripMenuItem.Enabled = false;
            }
        }
        private bool validateInputPhieuNhap()
        {
            return validateMaPhieuNhap(txtMAPN.Text) &&
                validateMaNhanVien(txtMANV.Text) &&
                validateMaKho(txtMAKHO.Text) &&
                validateMasoDDH(txtMAPN.Text, txtMasoDDH.Text);
        }
        private bool validateInputCTPN()
        {
            if (bdsCTPN.Position == -1)
            {
                ThongBao("Vui lòng nhập đầy đủ thông tin");
                return false;
            }
            DataRowView dr = (DataRowView)bdsCTPN[bdsCTPN.Position];
            string maVT = dr["MAVT"].ToString().Trim();
            string soLuongCTPN = dr["SOLUONG"].ToString().Trim();
            string donGiaCTPN = dr["DONGIA"].ToString().Trim();
            return validateMaVatTuCTPN(maVT) &&
                validateSoLuongCTPN(txtMasoDDH.Text, maVT, soLuongCTPN) &&
                validateDonGiaCTPN(donGiaCTPN);
        }
        private bool validateMaPhieuNhap(string maPN)
        {
            if (string.IsNullOrEmpty(maPN))
            {
                ThongBao("Mã phiếu nhập không được bỏ trống");
                txtMAPN.Focus();
                return false;
            }
            if (!maPN.StartsWith("PN"))
            {
                ThongBao("Mã phiếu nhập phải bắt đầu với PN");
                txtMAPN.Focus();
                return false;
            }
            if (maPN.Length > 8)
            {
                MessageBox.Show("Mã đơn dặt hàng tối đa 8 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtMAPN.Focus();
                return false;
            }

                return true;
        }
        private bool validateMaNhanVien(string maNV)
        {
            if (string.IsNullOrEmpty(maNV))
            {
                ThongBao("Vui lòng chọn nhân viên cho phiếu nhập");
                hOTENComboBox.Focus();
                return false;
            }
            return true;
        }
        private bool validateMaKho(string maKho)
        {
            if (string.IsNullOrEmpty(maKho))
            {
                ThongBao("Vui lòng chọn kho cho phiếu nhập");
                tENKHOComboBox.Focus();
                return false;
            }
            if (maKho.Length > 100)
            {
                MessageBox.Show("Nhà cung cấp chỉ có tối đa 100 kí tự", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        private bool validateMasoDDH(string maPN, string masoDDH)
        {
            if (string.IsNullOrEmpty(masoDDH))
            {
                ThongBao("Vui lòng chọn đơn đặt hàng cho phiếu nhập");
                cbxMASODDH.Focus();
                return false;
            }
            int result = ExecuteSP_TracuuDDHPhieuNhap(masoDDH);
            if (result != 1 && result != 0)
            {
                ThongBao("Có lỗi trong quá trình xử lý");
                cbxMASODDH.Focus();
                return false;
            }
            int viTriConTro = bdsPhieuNhap.Position;
            int viTriMaPN = bdsPhieuNhap.Find("MAPN", maPN);
            if (result == 1 && viTriConTro != viTriMaPN)
            {
                ThongBao("Đã có phiếu nhập cho đơn đặt hàng này!");
                cbxMASODDH.Focus();
                return false;
            }
            return true;
        }
        private int ExecuteSP_TracuuDDHPhieuNhap(string masoDDH)
        {
            string query = "DECLARE @result int \r\nEXEC @result = [dbo].[SP_KiemtraDDHPhieuNhap] N'" + masoDDH + "'\r\nSELECT @result";

            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);

                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiểm tra phiếu nhập thất bại\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return result;
        }
        private int ExecuteSP_TracuuVatTuCTPN(string maVT, string masoDDH)
        {
            string query = "DECLARE @result int \r\nEXEC @result = [dbo].[SP_KiemTraVattuDDH] N'" + masoDDH + "',N'" + maVT + "'\r\nSELECT @result";

            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);

                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiểm tra phiếu nhập thất bại\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return result;
        }
        private bool validateMaVatTuCTPN(string maVT)
        {
            if (string.IsNullOrEmpty(maVT))
            {
                ThongBao("Vui lòng chọn vật tư cho chi tiết phiếu nhập");
                dgvCTPN.Focus();
                return false;
            }
            return true;
        }
        private bool validateSoLuongCTPN(string masoDDH, string maVT, string soLuongCTPN)
        {
            if (string.IsNullOrEmpty(soLuongCTPN))
            {
                ThongBao("Không được bỏ trống số lượng");
                dgvCTPN.Focus();
                return false;
            }
            int soLuong;
            if (!int.TryParse(soLuongCTPN, out soLuong) || soLuong <= 0)
            {
                ThongBao("Số lượng vật tư phải là một số nguyên dương");
                dgvCTPN.Focus();
                return false;
            }
            soLuong = int.Parse(soLuongCTPN);
            String query = "declare @result int\r\nexec @result = SP_KiemTraSoluongVattuDDH '" + masoDDH + "', '" + maVT + "', " + soLuongCTPN + "\r\nselect @result";
            int result;
            // Dùng SP để kiểm tra xem có số lượng vật tư trong phiếu nhập có lớn hơn số lượng của vật tư trong đơn đặt hàng
            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);
                // Nếu không có kết quả thì quay về
                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    result = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            if (result != 1 && result != 0)
            {
                ThongBao("Có lỗi trong quá trình xử lý mã phiếu nhập");
                return false;
            }

            if (result == 0)
            {
                ThongBao("Số lượng vật tư không thể lớn hơn số lượng vật tư trong chi tiết đơn hàng!");
                return false;
            }
            return true;

        }
        private bool validateDonGiaCTPN(string dongiaCTPN)
        {
            if (string.IsNullOrEmpty(dongiaCTPN))
            {
                ThongBao("Không được bỏ trống số đơn giá");
                dgvCTPN.Focus();
                return false;
            }
            float dongia;
            if (!float.TryParse(dongiaCTPN, out dongia) || dongia <= 0)
            {
                ThongBao("Đơn giá vật tư phải > 0");
                dgvCTPN.Focus();
                return false;
            }
            return true;

        }
        private int ExecuteSP_TracuuPhieuNhap(String maPN)
        {
            String query = "declare @result int\r\nexec @result = [SP_KiemTraMaPhieuNhapXuat] 'NHAP', N'" + maPN + "'\r\nselect @result";

            // Dùng SP để kiểm tra xem có nhân viên với mã nv đang tạo
            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);
                // Nếu không có kết quả thì quay về
                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return result;
        }
        private int ExecuteSP_TracuuCTPN(string maPN, string maVT)
        {
            string query =
                    "DECLARE @result int " +
                    "EXEC @result = [dbo].[SP_KiemTraVattuCTPNhapXuat] 'NHAP', N'"
                     + maPN + "', N'" + maVT + "' " +
                    "SELECT @result";
            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);

                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiểm tra Chi tiết phiếu nhập thất bại\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return result;
        }
        private void ExecuteSP_CapNhatSoLuongVatTu(string maVatTu, int soLuong)
        {
            string query = "EXEC SP_CapNhatSoLuongVatTu 'IMPORT','" + maVatTu + "', " + soLuong;
            int n = Program.ExecSqlNonQuery(query);
        }
        //Kiểm tra ràng buộc số lượng tồn của vật tư > 0 
        // param soluongThayDoi biểu thị lượng thay đổi số lượng vật tư khi thao tác CTPN (xóa, chỉnh sửa) 
        private int ExecuteSP_KiemtraSoluongtonVattu(string maVT, int soluongThayDoi)
        {
            string query =
                    "DECLARE @result int " +
                    "EXEC @result = [dbo].[SP_KiemTraSoluongVattu] '" + maVT + "', " + soluongThayDoi +
                    " SELECT @result";
            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);

                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiểm tra Chi tiết phiếu nhập thất bại\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return result;
        }
        /*
        Định nghĩa: Phương thức được sử dụng để lấy mã nhân viên của phiếu nhập trong DB
        Mục đích: Kiểm tra MANV để thực hiện so sánh với người dùng (Program.username) cho phép thêm, xóa, sửa phiếu nhập, CTPN
         */
        private string traCuuMANVPhieuNhap()
        {
            DataRowView dr = ((DataRowView)(bdsPhieuNhap.Current));
            String maPN = dr["MAPN"].ToString().Trim();

            string traCuuMANV = "SELECT MANV FROM DBO.PhieuNhap " +
                                  "WHERE MAPN = '" + maPN + "' ";
            try
            {
                Program.myReader = Program.ExecSqlDataReader(traCuuMANV);
                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            string maNV = Program.myReader.GetValue(0).ToString().Trim();
            Program.myReader.Close();
            return maNV;
        }
        /*
         Định nghĩa: Phương thức được sử dụng để lấy mã vật tư của chi tiết phiếu nhập ở vị trí cụ thể
         Mục đích: so sánh maVT trước và sau khi thay đổi để xác định trường hợp là thêm mới hay chỉnh sửa vật tư trong CTPN
         */
        private string traCuuMAVTCTPN(int vitri)
        {
            DataRowView dr = ((DataRowView)(bdsPhieuNhap.Current));
            String maPN = dr["MAPN"].ToString().Trim();

            string traCuuMAVT = "SELECT MAVT FROM DBO.CTPN " +
                                  "WHERE MAPN = '" + maPN + "' ";
            try
            {
                Program.myReader = Program.ExecSqlDataReader(traCuuMAVT);
                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int currentRow = 0;
            string maVT = "";
            while (Program.myReader.Read())
            {
                if(currentRow == vitri)
                {
                    maVT = Program.myReader.GetValue(0).ToString().Trim();
                    break;
                }
                else
                {
                    currentRow++;
                }
            }
            Program.myReader.Close();
            return maVT;
        }
        /*
         Định nghĩa: Phương thức được sử dụng để lấy số lượng của vật tư trong chi tiết phiếu nhập ở vị trí cụ thể
         Mục đích: so sánh số lượng trước và sau khi thay đổi để cập số lượng vật tự
         */
        private int traCuuSoLuongVattuCTPN(string maPN, string maVT)
        {
            string traCuuSLVT = " SELECT SOLUONG FROM DBO.CTPN " +
                                "WHERE MAPN = '" + maPN + "' " +
                                "AND MAVT = '" + maVT + "' ";
            try
            {
                Program.myReader = Program.ExecSqlDataReader(traCuuSLVT);
                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int soluong = 0;
            if (Program.myReader.Read())
            {
                soluong = int.Parse(Program.myReader.GetValue(0).ToString().Trim());
            }
            Program.myReader.Close();
            return soluong;
        }
        /*
         Định nghĩa: Phương thức được sử dụng để lấy đơn giá của vật tư trong chi tiết phiếu nhập ở vị trí cụ thể
         Mục đích: so sánh đơn giá trước và sau khi thay đổi để cập số lượng vật tự
         */
        private float traCuuDongiaVattuCTPN(string maPN, string maVT)
        {
            string traCuuDGVT = " SELECT DONGIA FROM DBO.CTPN " +
                                "WHERE MAPN = '" + maPN + "' " +
                                "AND MAVT = '" + maVT + "' ";
            try
            {
                Program.myReader = Program.ExecSqlDataReader(traCuuDGVT);
                //Không có kết quả thì kết thúc
                if (Program.myReader == null)
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            float soluong = 0;
            if (Program.myReader.Read())
            {
                soluong = float.Parse(Program.myReader.GetValue(0).ToString().Trim());
            }
            Program.myReader.Close();
            return soluong;
        }
        private void hOTENComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hOTENComboBox.SelectedValue == null) return;
            try
            {
                txtMANV.Text = hOTENComboBox.SelectedValue.ToString();
            }
            catch (Exception) { }
        }
        private void tENKHOComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tENKHOComboBox.SelectedValue == null) return;
            try
            {
                txtMAKHO.Text = tENKHOComboBox.SelectedValue.ToString();
            }
            catch (Exception) { }
        }
        private void cbxMASODDH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxMASODDH.SelectedValue == null) { return; }
            try
            {
                txtMasoDDH.Text = cbxMASODDH.SelectedValue.ToString();
            }
            catch (Exception) { }
        }
        private void cbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            Program.servername = cbChiNhanh.SelectedValue.ToString();

            /*Neu chon sang chi nhanh khac voi chi nhanh hien tai*/
            if (cbChiNhanh.SelectedIndex != Program.brand)
            {
                // Dùng tài khoản hỗ trợ kết nối để chuẩn bị cho việc login vào chi nhánh khác
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            /*Neu chon trung voi chi nhanh dang dang nhap o formDangNhap*/
            else
            {
                // Lấy tài khoản hiện tại đang đăng nhập để đăng nhập
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }
            if (Program.connectDB() == 0)
            {
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                this.vattuTableAdapter.Connection.ConnectionString = Program.conStr;
                this.vattuTableAdapter.Fill(this.dS1.Vattu);
                this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTPNTableAdapter.Fill(this.dS1.CTPN);
                this.dSKHOTableAdapter.Connection.ConnectionString = Program.conStr;
                this.dSKHOTableAdapter.Fill(this.dS1.DSKHO);
                this.dSNVTableAdapter.Connection.ConnectionString = Program.conStr;
                this.dSNVTableAdapter.Fill(this.dS1.DSNV);
                this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
                this.datHangTableAdapter.Fill(this.dS1.DatHang);
                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);
            }
        }
        private void phieuNhapGridControl_Click(object sender, EventArgs e)
        {
            if (bdsCTPN.Count == 0)
            {
                ghiToolStripMenuItem.Enabled = false;
                xóaToolStripMenuItem.Enabled = false;
            }
            else
            {
                ghiToolStripMenuItem.Enabled = true;
                xóaToolStripMenuItem.Enabled = true;
            }
        }
        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Lấy vị trí của con trỏ
            position = bdsPhieuNhap.Position;
            isAdding = true;
            groupBoxPhieuNhap.Enabled = true;

            bdsPhieuNhap.AddNew();
            txtMAPN.Enabled = true;
            txtMAPN.Focus();
            dteNGAY.EditValue = DateTime.Now;
            dteNGAY.Enabled = false;
            /*txtMANV.Text = "";*/
            tENKHOComboBox.Enabled = true;
            /*txtMAKHO.Text = "";*/
            cbxMASODDH.Enabled = true;

            // Thay đổi bật/ tắt các nút chức năng
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLamMoi.Enabled = false;
            btnChitietPN.Enabled = false;
            btnThoat.Enabled = true;
            btnHoanTac.Enabled = true;

            phieuNhapGridControl.Enabled = false;
            groupBoxPhieuNhap.Enabled = true;
            dgvCTPN.Enabled = false;
            contextMenuStripCTPN.Enabled = false;
            hOTENComboBox.SelectedValue = Program.username;
        }
        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int vitriPN = bdsPhieuNhap.Position;
            // Lấy dữ liệu trước khi ghi phục vụ cho việc hoàn tác
            String maPN = txtMAPN.Text.Trim();
            DataRowView dr = ((DataRowView)bdsPhieuNhap[vitriPN]);
            /*DateTime ngayLap = ((DateTime)dr["NGAY"]);*/
            DateTime ngayLap = new DateTime();
            if (dr["NGAY"] != DBNull.Value)
            {
                if (DateTime.TryParse(dr["NGAY"].ToString(), out ngayLap))
                {
                }
                else
                {
                    // Xử lý trường hợp không thể chuyển đổi ngày
                    MessageBox.Show("Giá trị trong cột NGAY không phải là kiểu ngày tháng hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            String maDDH = txtMasoDDH.Text.Trim();
            String maNV = txtMANV.Text.Trim();
            String maKho = dr["MAKHO"].ToString().Trim();
            // kiểm tra đầu vào có hợp lệ hay không
            if (validateInputPhieuNhap() == false)
            {
                return;
            }
            if (isAdding)
            {
                if (maNV != Program.username)
                {
                    ThongBao("Không thể tạo phiếu nhập cho người khác");
                    return;
                }
            }
            else
            {
                string checkMaNV = traCuuMANVPhieuNhap();
                if (checkMaNV != Program.username)
                {
                    ThongBao("Không thể chỉnh sửa phiếu nhập do người khác tạo ra");
                    return;
                }
                else if(checkMaNV == Program.username && maNV != Program.username)
                {
                    ThongBao("Không thể tạo phiếu nhập cho người khác");
                    return;
                }
            }

            //Kiểm tra phiếu nhập có mã này đã tồn tại chưa
            int pnResult = ExecuteSP_TracuuPhieuNhap(maPN);
            //Tìm vị trí con trỏ và vị trí ma phieu nhap
            int viTriConTro = bdsPhieuNhap.Position;
            int viTriMaPN = bdsPhieuNhap.Find("MAPN", maPN);
            if (pnResult != 1 && pnResult != 0)
            {
                ThongBao("Có lỗi trong quá trình xử lý mã phiếu nhập");
                return;
            }
            if (pnResult == 1 && viTriConTro != viTriMaPN)
            {
                ThongBao("Mã phiếu nhập đã tồn tại!");
                txtMAPN.Focus();
                return;
            }
            //Kiểm tra xem khi người dùng đổi đơn đặt hàng trong phiếu nhập thì phiếu nhập đó có chi tiết phiếu nhập chưa
            string traCuuMANV = "SELECT MasoDDH FROM DBO.PhieuNhap " +
                                  "WHERE MAPN = '" + maPN + "' ";
            string checkMaDDH = "";
            try
            {
                Program.myReader = Program.ExecSqlDataReader(traCuuMANV);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if(Program.myReader.Read())
                checkMaDDH = Program.myReader.GetValue(0).ToString().Trim();
            Program.myReader.Close();
            if(checkMaDDH != "" && checkMaDDH != maDDH)
            {
                if (bdsCTPN.Count > 0)
                {
                    ThongBao("Không thể thay đổi đơn đặt hàng cho phiếu nhập này vì nó có chứa chi tiết phiếu nhập");
                    return;
                }
            }

            DialogResult dlr = MessageBox.Show("Bạn có chắc chắn muốn ghi dữ liệu vào cơ sở dữ liệu không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dlr == DialogResult.OK)
            {
                try
                {
                    // Lưu truy vấn phục vụ hoàn tác
                    string undoQuery = "";
                    // Trường hợp thêm phiếu nhập
                    if (isAdding == true)
                    {
                        undoQuery = "DELETE FROM CTPN WHERE MAPN = N'" + maPN + "'\r\nDELETE FROM PhieuNhap WHERE MAPN= N'" + maPN + "'";
                    }
                    // Trường hợp sửa phiếu nhập
                    else
                    {
                        undoQuery = "UPDATE DBO.PhieuNhap " +
                        "SET " +
                        "MANV = N'" + maNV + "'," +
                        "MAKHO = N'" + maKho + "'," +
                        "NGAY = CAST('" + ngayLap.ToString("yyyy-MM-dd") + "' AS DATETIME)," +
                        "MasoDDH = N'" + maDDH + "'" +
                        "WHERE MAPN = N'" + maPN + "'";
                    }
                    this.bdsPhieuNhap.EndEdit();
                    this.phieuNhapTableAdapter.Update(this.dS1.PhieuNhap);
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);
                    bdsPhieuNhap.Position = vitriPN;
                    /*cập nhật lại trạng thái thêm mới cho chắc*/
                    isAdding = false;
                    ThongBao("Ghi thành công.");
                    undoList.Push(undoQuery);
                }
                catch (Exception ex)
                {
                    bdsPhieuNhap.RemoveCurrent();
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);

                    MessageBox.Show("Thất bại. Vui lòng kiểm tra lại!\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    txtMAPN.Enabled = false;
                    dteNGAY.Enabled = true;
                    // Thay đổi bật/ tắt các nút chức năng
                    btnThem.Enabled = true;
                    btnXoa.Enabled = true;
                    btnGhi.Enabled = true;
                    btnHoanTac.Enabled = true;
                    btnLamMoi.Enabled = true;
                    btnThoat.Enabled = true;
                    btnChitietPN.Enabled = true;


                    dgvCTPN.Enabled = true;
                    contextMenuStripCTPN.Enabled = true;

                    phieuNhapGridControl.Enabled = true;
                    groupBoxPhieuNhap.Enabled = true;
                }
            }
        }
        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }
        private void btnLamMoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int vitri = bdsPhieuNhap.Position;
                int vitriCT = bdsCTPN.Position;
                phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);
                phieuNhapGridControl.Enabled = true;
                cTPNTableAdapter.Fill(this.dS1.CTPN);
                bdsPhieuNhap.Position = vitri;
                bdsCTPN.Position = vitriCT;
                if (undoCTPN.Count == 0)
                {
                    hoanTacVatTuToolStripMenuItem.Enabled = false;
                }
                else
                {
                    hoanTacVatTuToolStripMenuItem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ThongBao("Lỗi khi làm mới dữ liệu: " + ex.Message);
                return;
            }
        }
        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRowView dr = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
            DateTime ngayLap = new DateTime();
            if (dr["NGAY"] != DBNull.Value)
            {
                if (DateTime.TryParse(dr["NGAY"].ToString(), out ngayLap))
                {
                }
                else
                {
                    // Xử lý trường hợp không thể chuyển đổi ngày
                    MessageBox.Show("Giá trị trong cột NGAY không phải là kiểu ngày tháng hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            String maDDH = dr["MasoDDH"].ToString();
            String maNV = dr["MANV"].ToString();
            String maPN = dr["MAPN"].ToString().Trim();
            String maKho = dr["MAKHO"].ToString();
            string checkMaNV = traCuuMANVPhieuNhap();

            if (Program.username != checkMaNV)
            {
                ThongBao("Không thể xóa phiếu nhập không phải do mình tạo");
                return;
            }
            if (bdsCTPN.Count > 0)
            {
                ThongBao("Không thể xóa phiếu nhập vì có chi tiết phiếu nhập");
                return;
            }
            String cauTruyVanHoanTac = "INSERT INTO DBO.PHIEUNHAP(MAPN, NGAY, MasoDDH, MANV, MAKHO) " +
                    "VALUES( '" + maPN + "', '" +
                    ngayLap.ToString("yyyy-MM-dd") + "', '" +
                    maDDH + "', '" +
                    maNV + "', '" +
                    maKho + "')";
            undoList.Push(cauTruyVanHoanTac);
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    position = bdsPhieuNhap.Position;
                    bdsPhieuNhap.RemoveCurrent();
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.phieuNhapTableAdapter.Update(this.dS1.PhieuNhap);
                    this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);

                    /*Cap nhat lai do ben tren can tao cau truy van nen da dat dangThemMoi = true*/
                    isAdding = false;
                    ThongBao("Xóa phiếu nhập thành công");
                    btnHoanTac.Enabled = true;
                }
                catch (Exception ex)
                {
                    ThongBao("Lỗi xóa phiếu nhập! " + ex.Message);
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.phieuNhapTableAdapter.Update(this.dS1.PhieuNhap);
                    this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);
                    // Trả lại vị trí cũ của nhân viên xóa bị lỗi
                    bdsPhieuNhap.Position = position;
                    return;
                }
            }
            else
            {
                undoList.Pop();
            }

        }
        private void btnHoanTac_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int vitriPN = bdsPhieuNhap.Position;
            // Thay đổi bật/ tắt các nút chức năng
            btnXoa.Enabled = true;
            btnLamMoi.Enabled = true;
            btnThoat.Enabled = true;

            phieuNhapGridControl.Enabled = true;
            dgvCTPN.Enabled = true;


            if (isAdding == true && btnThem.Enabled == false)
            {
                isAdding = false;
                btnThem.Enabled = true;
                txtMAPN.Enabled = false;
                dteNGAY.Enabled = true;
                tENKHOComboBox.Enabled = true;
                cbxMASODDH.Enabled = true;

                contextMenuStripCTPN.Enabled = true;
                //Hủy thao tác trên bds
                bdsPhieuNhap.CancelEdit();
                if(bdsPhieuNhap.Count != 0)
                {
                    bdsPhieuNhap.RemoveCurrent();
                }
                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);
                /* trở về lúc đầu con trỏ đang đứng*/
                bdsPhieuNhap.Position = position;
                return;
            }
            
            btnThem.Enabled = true;
            // Kiểm tra undoStack trống hay không
            // Trường hợp stack trống thì không thực hiện
            if (undoList.Count == 0)
            {
                ThongBao("Không có tháo tác để khôi phục");
                btnHoanTac.Enabled = false;
                return;
            }

            // Tạo một String để lưu truy vấn được lấy ra từ stack
            String undoSql = undoList.Pop().ToString();

            int n = Program.ExecSqlNonQuery(undoSql);
            bdsPhieuNhap.CancelEdit();
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
            this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);
            bdsPhieuNhap.Position = vitriPN;
        }
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Lấy vị trí của con trỏ
            position = bdsCTPN.Position;
            isAdding = true;
            string maPN = txtMAPN.Text;

            if (maPN.Equals(""))
            {
                ThongBao("Vui lòng chọn hoặc tạo phiếu nhập để thực hiện thêm chi tiết phiếu nhập");
                return;
            }
            string checkMaNV = traCuuMANVPhieuNhap();
            if (checkMaNV != Program.username)
            {
                ThongBao("Không thể thêm chi tiết phiếu nhập do người khác tạo ra");
                return;
            }
            bdsCTPN.AddNew();


            // Thay đổi bật/ tắt các nút chức năng
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLamMoi.Enabled = false;
            btnChitietPN.Enabled = false;
            btnThoat.Enabled = true;
            btnHoanTac.Enabled = false;
            btnGhi.Enabled = false;


            phieuNhapGridControl.Enabled = false;
            groupBoxPhieuNhap.Enabled = false;
            dgvCTPN.Enabled = true;
            thêmToolStripMenuItem.Enabled = false;
            xóaToolStripMenuItem.Enabled = false;
            hoanTacVatTuToolStripMenuItem.Enabled = true;
            ghiToolStripMenuItem.Enabled = true;
        }
        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string checkMaNV = traCuuMANVPhieuNhap();
            if (checkMaNV != Program.username)
            {
                ThongBao("Không thể xóa chi tiết phiếu nhập do người khác tạo ra");
                return;
            }
            DataRowView dr = (DataRowView)bdsCTPN[bdsCTPN.Position];
            string maPN = dr["MAPN"].ToString().Trim();
            String maVT = traCuuMAVTCTPN(bdsCTPN.Position);
            int soLuong = traCuuSoLuongVattuCTPN(maPN,maVT);
            float dongia = traCuuDongiaVattuCTPN(maPN, maVT);
            
            if (MessageBox.Show("Bạn có muốn xóa chi tiết phiếu nhập này không", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                using (SqlConnection connection = new SqlConnection(Program.conStr))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        
                        //Xử lý lỗi ràng buộc SOLUONGTON Vật tư > 0
                        int pnresult = ExecuteSP_KiemtraSoluongtonVattu(maVT, soLuong);
                        if (pnresult == 0)
                        {
                            ThongBao("Không thể xóa chi tiết phiếu nhập này vì vật tư không đủ số lượng tồn");
                            return;
                        }

                        string undoSql = "INSERT INTO DBO.CTPN (MAPN, MAVT, SOLUONG, DONGIA) " +
                                        "VALUES('" + maPN + "', '" +
                                        maVT + "', " +
                                        soLuong + ", " +
                                        dongia + ") ";
                        string undoSL = maVT+":"+soLuong.ToString().Trim();
                        position = bdsCTPN.Position;
                        bdsCTPN.RemoveCurrent();
                        ExecuteSP_CapNhatSoLuongVatTu(maVT, soLuong * (-1));
                        this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                        this.cTPNTableAdapter.Update(this.dS1.CTPN);
                        this.cTPNTableAdapter.Fill(this.dS1.CTPN);

                        transaction.Commit();

                        /*Cap nhat lai do ben tren can tao cau truy van nen da dat dangThemMoi = true*/
                        hoanTacVatTuToolStripMenuItem.Enabled = true;
                        isAdding = false;
                        btnHoanTac.Enabled = true;
                        undoCTPN.Push(undoSql);
                        undoCTPNSL.Push(undoSL);
                        ThongBao("Xóa chi tiết phiếu nhập thành công");
                        if (bdsCTPN.Count == 0)
                        {
                            ghiToolStripMenuItem.Enabled = false;
                            xóaToolStripMenuItem.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        ThongBao("Lỗi xóa chi tiết phiếu nhập! " + ex.Message);
                        this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                        this.cTPNTableAdapter.Update(this.dS1.CTPN);
                        this.cTPNTableAdapter.Fill(this.dS1.CTPN);
                        // Trả lại vị trí cũ của nhân viên xóa bị lỗi
                        bdsCTPN.Position = position;
                        return;
                    }
                }

            }
        }
        private void ghiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string checkMaNV = traCuuMANVPhieuNhap();
            if (checkMaNV != Program.username)
            {
                ThongBao("Không thể chỉnh sửa chi tiết phiếu nhập do người khác tạo ra");
                return;
            }
            // kiểm tra đầu vào có hợp lệ hay không
            if (validateInputCTPN() == false)
            {
                return;
            }

            String maPN = txtMAPN.Text.Trim();
            DataRowView checkDr = (DataRowView)bdsCTPN[bdsCTPN.Position];
             

            String masoDDH = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position])["MasoDDH"].ToString().Trim();
            //Kiểm tra vật tư có trong đơn đặt hàng hay không
            int pnResult = ExecuteSP_TracuuVatTuCTPN(checkDr["MAVT"].ToString().Trim(), masoDDH);
            if (pnResult != 1 && pnResult != 0)
            {
                ThongBao("Có lỗi trong quá trình xử lý ghi chi tiết phiếu nhập");
                return;
            }

            if (pnResult == 0)
            {
                ThongBao("Vui lòng chọn vật tư đã dặt hàng trong chi tiết đơn đặt hàng!");
                return;
            }

            //Kiểm tra phiếu nhập có bị trùng vật tư hay không
            pnResult = ExecuteSP_TracuuCTPN(checkDr["MAPN"].ToString().Trim(), checkDr["MAVT"].ToString().Trim());
            if (pnResult != 1 && pnResult != 0)
            {
                ThongBao("Có lỗi trong quá trình xử lý ghi chi tiết phiếu nhập");
                return;
            }

            if (pnResult == 1)
            {
                //Trường hợp thêm vật tư
                if (isAdding)
                {
                    ThongBao("Chi tiết phiếu nhập này đã tồn tại!");
                    return;
                }
                //Trường hợp chỉnh sửa vật tư kiểm tra xem có thay đổi vật tư trong phiếu không
                else
                {
                    string maVTPre = traCuuMAVTCTPN(bdsCTPN.Position);
                    string maVTNext = checkDr["MAVT"].ToString().Trim();
                    if (maVTPre != maVTNext)
                    {
                        ThongBao("Chi tiết phiếu nhập này đã tồn tại!");
                        return;
                    }
                }
            }
            DialogResult dlr = MessageBox.Show("Bạn có chắc chắn muốn ghi dữ liệu vào cơ sở dữ liệu không?", "Thông báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dlr == DialogResult.OK)
                {
                    using (SqlConnection connection = new SqlConnection(Program.conStr))
                    {
                        connection.Open();
                        // Bắt đầu transaction
                        SqlTransaction transaction = connection.BeginTransaction();

                        try
                        {
                            string undoSql = "";
                            string undoSL = "";
                            btnThem.Enabled = true;
                            btnXoa.Enabled = true;
                            btnGhi.Enabled = true;
                            btnHoanTac.Enabled = true;
                            btnLamMoi.Enabled = true;

                            dgvCTPN.Enabled = true;
                            phieuNhapGridControl.Enabled = true;
                            groupBoxPhieuNhap.Enabled = true;

                            thêmToolStripMenuItem.Enabled = true;
                            xóaToolStripMenuItem.Enabled = true;
                            ghiToolStripMenuItem.Enabled = true;
                            

                            DataRowView drCTPN = (DataRowView)bdsCTPN[bdsCTPN.Position];
                            if (isAdding == true)
                            {
                                string maVT = drCTPN["MAVT"].ToString().Trim();
                                undoSql =
                                "DELETE FROM DBO.CTPN " +
                                "WHERE MAPN = '" + maPN + "' " +
                                "AND MAVT = '" +maVT + "'";
                                undoSL = maVT+":"+drCTPN["SOLUONG"].ToString().Trim();
                                
                                ExecuteSP_CapNhatSoLuongVatTu(maVT, int.Parse(drCTPN["SOLUONG"].ToString().Trim()));
                            }
                            else
                            {
                                string maPhieuNhap = drCTPN["MAPN"].ToString().Trim();
                                //Mã vật tư trước khi thay đổi
                                string maVT = traCuuMAVTCTPN(bdsCTPN.Position);
                                //Số lượng của vật tư trước khi thay đổi    
                                int soLuong = traCuuSoLuongVattuCTPN(maPhieuNhap, maVT);
                                if(soLuong == -1)
                                {
                                    ThongBao("Có lỗi xảy ra trong quá trình thực thi");
                                    if (bdsCTPN.Count == 0)
                                    {
                                        xóaToolStripMenuItem.Enabled = false;
                                        ghiToolStripMenuItem.Enabled = false;
                                    }
                                    return;
                                }

                                String currMaVT = drCTPN["MAVT"].ToString().Trim();
                                if (maVT.Equals(currMaVT))
                                {
                                    int luongThayDoi = Math.Abs(int.Parse(drCTPN["SOLUONG"].ToString().Trim()) - soLuong);

                                    //Xử lý lỗi ràng buộc SOLUONGTON Vật tư > 0
                                    pnResult = ExecuteSP_KiemtraSoluongtonVattu(maVT, luongThayDoi);
                                    if (pnResult == 0)
                                    {
                                        ThongBao("Không thể chỉnh sửa chi tiết phiếu nhập này vì vật tư không đủ số lượng tồn");
                                        if (bdsCTPN.Count == 0)
                                        {
                                            xóaToolStripMenuItem.Enabled = false;
                                            ghiToolStripMenuItem.Enabled = false;
                                        }
                                        return;
                                    }
                                    undoSql =   "UPDATE DBO.CTPN " +
                                                "SET " +
                                                "SOLUONG = " + soLuong + ", " +
                                                "DONGIA = " + traCuuDongiaVattuCTPN(maPhieuNhap, maVT) + " " +
                                                "WHERE MAPN = '" + maPhieuNhap + "' " +
                                                "AND MAVT = '" + maVT + "' ";
                                    int soluongThayDoi = int.Parse(drCTPN["SOLUONG"].ToString().Trim()) - soLuong;
                                    undoSL = maVT+":"+soluongThayDoi.ToString().Trim();
                                    ExecuteSP_CapNhatSoLuongVatTu(maVT, soluongThayDoi);
                                }
                                else
                                {
                                    ExecuteSP_CapNhatSoLuongVatTu(currMaVT, int.Parse(drCTPN["SOLUONG"].ToString().Trim()));

                                    //Xử lý lỗi ràng buộc SOLUONGTON Vật tư > 0
                                    int pnresult = ExecuteSP_KiemtraSoluongtonVattu(maVT, soLuong);
                                    if (pnresult == 0)
                                    {
                                        ThongBao("Không thể chỉnh sửa chi tiết phiếu nhập này vì vật tư không đủ số lượng tồn");
                                        if (bdsCTPN.Count == 0)
                                        {
                                            xóaToolStripMenuItem.Enabled = false;
                                            ghiToolStripMenuItem.Enabled = false;
                                        }
                                        return;
                                    }
                                    ExecuteSP_CapNhatSoLuongVatTu(maVT, soLuong * (-1));
                                    undoSql = "DELETE FROM DBO.CTPN " +
                                            "WHERE MAPN = '" + maPN + "' " +
                                            "AND MAVT = '" + currMaVT + "' "+
                                            "INSERT INTO DBO.CTPN(MAPN,MAVT,SOLUONG,DONGIA) VALUES('"+maPN+"','"+maVT+"',"+soLuong+","+traCuuDongiaVattuCTPN(maPN,maVT)+")";
                                    undoSL = maVT+":"+ soLuong.ToString().Trim() + ";"+currMaVT+":" + drCTPN["SOLUONG"].ToString().Trim();
                                }

                            }
                            this.bdsCTPN.EndEdit();
                            DataRow currentRow = drCTPN.Row;
                            this.cTPNTableAdapter.Update(new DataRow[] {currentRow});
                            //Hoàn tất thao tác
                            transaction.Commit();

                            /*cập nhật lại trạng thái thêm mới cho chắc*/
                            isAdding = false;
                            undoCTPN.Push(undoSql);
                            undoCTPNSL.Push(undoSL);
                            hoanTacVatTuToolStripMenuItem.Enabled = true;
                            ThongBao("Ghi thành công.");
                            this.cTPNTableAdapter.Fill(this.dS1.CTPN);
                            
                        }
                        catch (Exception ex)
                        {
                            // Nếu có lỗi, rollback
                            if (transaction != null)
                            {
                                transaction.Rollback();
                            }

                            bdsCTPN.RemoveCurrent();
                            this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                            this.cTPNTableAdapter.Fill(this.dS1.CTPN);
                            phieuNhapGridControl.Enabled = true;
                            MessageBox.Show("Thất bại. Vui lòng kiểm tra lại!\n" + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
        }
        private void hoanTacVatTuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoCTPN.Count == 0)
            {
                ThongBao("Không còn thao tác để hoàn tác");
                return;
            }
            btnXoa.Enabled = true;
            btnGhi.Enabled = true;
            btnLamMoi.Enabled = true;
            btnThoat.Enabled = true;
            btnHoanTac.Enabled = true;

            phieuNhapGridControl.Enabled = true;
            groupBoxPhieuNhap.Enabled = true;
            dgvCTPN.Enabled = true;
            if (isAdding == true && thêmToolStripMenuItem.Enabled == false)
            {
                isAdding = false;
                btnThem.Enabled = true;
                contextMenuStripCTPN.Enabled = true;
                thêmToolStripMenuItem.Enabled = true;
                xóaToolStripMenuItem.Enabled = true;
                if (undoCTPN.Count == 0)
                    hoanTacVatTuToolStripMenuItem.Enabled = false;
                bdsCTPN.CancelEdit();
                if (bdsCTPN.Count == 0)
                {
                    ghiToolStripMenuItem.Enabled = false;
                    xóaToolStripMenuItem.Enabled = false;
                }
                else if (bdsCTPN.Count != 0)
                {
                    bdsCTPN.RemoveCurrent();
                }
                bdsCTPN.Position = position;
                this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTPNTableAdapter.Fill(this.dS1.CTPN);
                return;
            }

            // Tạo một String để lưu truy vấn được lấy ra từ stack
            String undoSql = undoCTPN.Pop().ToString();
            string undoSL = undoCTPNSL.Pop().ToString();



            using (SqlConnection connection = new SqlConnection(Program.conStr))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    if (undoSql.Contains("DELETE") && !undoSql.Contains("INSERT"))
                    {
                        string maVT = undoSL.Split(':')[0];
                        int soluong = Int32.Parse(undoSL.Split(':')[1]);
                        //Xử lý lỗi ràng buộc SOLUONGTON Vật tư > 0
                        int pnresult = ExecuteSP_KiemtraSoluongtonVattu(maVT, soluong);
                        if (pnresult == 0)
                        {
                            ThongBao("Không thể hoàn tác chi tiết phiếu nhập này vì vật tư không đủ số lượng tồn");
                            return;
                        }
                        ExecuteSP_CapNhatSoLuongVatTu(maVT, soluong * (-1));
                    }
                    else if (undoSql.Contains("UPDATE"))
                    {
                        string maVT = undoSL.Split(':')[0];
                        int soluongThayDoi = Int32.Parse(undoSL.Split(':')[1]);
                        int luongThayDoi = Math.Abs(soluongThayDoi);

                        //Xử lý lỗi ràng buộc SOLUONGTON Vật tư > 0
                        int pnResult = ExecuteSP_KiemtraSoluongtonVattu(maVT, luongThayDoi);
                        if (pnResult == 0)
                        {
                            ThongBao("Không thể hoàn tác chi tiết phiếu nhập này vì vật tư không đủ số lượng tồn");
                            if (bdsCTPN.Count == 0)
                            {
                                xóaToolStripMenuItem.Enabled = false;
                                ghiToolStripMenuItem.Enabled = false;
                            }
                            return;
                        }

                        ExecuteSP_CapNhatSoLuongVatTu(maVT, soluongThayDoi * (-1));
                    }
                    else if (!undoSql.Contains("DELETE") && undoSql.Contains("INSERT"))
                    {
                        string maVT = undoSL.Split(':')[0];
                        int soluong = Int32.Parse(undoSL.Split(':')[1]);
                        ExecuteSP_CapNhatSoLuongVatTu(maVT, soluong);
                    }
                    else
                    {
                        string preVT = undoSL.Split(';')[0];
                        string currVT = undoSL.Split(';')[1];
                        string preMAVT = preVT.Split(':')[0];
                        int preSoluong = Int32.Parse(preVT.Split(':')[1]);
                        string currMAVT = currVT.Split(':')[0];
                        int currSoluong = Int32.Parse(currVT.Split(':')[1]);
                        //Xử lý lỗi ràng buộc SOLUONGTON Vật tư > 0
                        int pnresult = ExecuteSP_KiemtraSoluongtonVattu(currMAVT, currSoluong);
                        if (pnresult == 0)
                        {
                            ThongBao("Không thể hoàn tác chi tiết phiếu nhập này vì vật tư không đủ số lượng tồn");
                            return;
                        }
                        ExecuteSP_CapNhatSoLuongVatTu(currMAVT, currSoluong * (-1));
                        ExecuteSP_CapNhatSoLuongVatTu(preMAVT, preSoluong);
                    }
                    int n = Program.ExecSqlNonQuery(undoSql);
                    bdsCTPN.EndEdit();
                    this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.cTPNTableAdapter.Fill(this.dS1.CTPN);
                    bdsCTPN.Position = position;
                    transaction.Commit();
                    if (undoCTPN.Count == 0)
                    {
                        hoanTacVatTuToolStripMenuItem.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ThongBao("Có lỗi xảy ra trong quá trình thực hiện không thể hoàn tất việc hoàn tác");

                }

            }
        }
    }
}
