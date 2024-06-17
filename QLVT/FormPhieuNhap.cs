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
        String cheDo = "PN";
        int position = 0;
        bool isAdding = false;

        //Undo -> dùng để hoàn tác dữ liệu nếu lỡ có thao tác không mong muốn
        Stack undoList = new Stack();
        Stack undoIndex = new Stack();

        BindingSource bds = null;
        GridControl gc = null;
        string type = "";
        private Dictionary<int, Dictionary<string, object>> previousRowDataDict = new Dictionary<int, Dictionary<string, object>>();
        private Dictionary<string, Dictionary<int, Dictionary<string, object>>> previousCTPN = new Dictionary<string, Dictionary<int, Dictionary<string, object>>>();


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
            this.phieuNhapTableAdapter.FillBy(this.dS1.PhieuNhap);

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

        }
        private bool validateInputPhieuNhap()
        {
            return validateMaPhieuNhap(txtMAPN.Text) &&
                validateMaNhanVien(txtMANV.Text) &&
                validateMaKho(txtMAKHO.Text) &&
                validateMasoDDH(txtMAPN.Text,txtMasoDDH.Text);
        }
        private bool validateInputCTPN()
        {
            if (position == -1)
            {
                ThongBao("Vui lòng nhập đầy đủ thông tin");
                return false;
            }
            return validateMaVatTuCTPN(dgvCTPN.Rows[position].Cells[1].Value.ToString()) &&
                validateSoLuongCTPN(txtMasoDDH.Text,dgvCTPN.Rows[position].Cells[1].Value.ToString(),dgvCTPN.Rows[position].Cells[2].Value.ToString()) &&
                validateDonGiaCTPN(dgvCTPN.Rows[position].Cells[3].Value.ToString());
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
            if(maNV != Program.username)
            {
                ThongBao("Bạn không thể tạo phiếu nhập cho người khác");
                hOTENComboBox.Focus();
                return false;
            }
            return true;
        }
        private bool validateMaKho(string maKho)
        {
            if(string.IsNullOrEmpty(maKho))
            {
                ThongBao("Vui lòng chọn kho cho phiếu nhập");
                tENKHOComboBox.Focus();
                return false;
            }
            return true;
        }
        private bool validateMasoDDH(string maPN,string masoDDH)
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
            string query = "DECLARE @result int \r\nEXEC @result = [dbo].[SP_KiemtraDDHPhieuNhap] N'"+masoDDH+"'\r\nSELECT @result";

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
            string query = "DECLARE @result int \r\nEXEC @result = [dbo].[SP_KiemTraVattuCTPN] N'" + masoDDH + "',N'" + maVT + "'\r\nSELECT @result";

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
        private bool validateSoLuongCTPN(string masoDDH,string maVT, string soLuongCTPN)
        {
            if (string.IsNullOrEmpty(soLuongCTPN))
            {
                ThongBao("Không được bỏ trống số lượng");
                dgvCTPN.Focus();
                return false;
            }
            int soLuong;
            if(!int.TryParse(soLuongCTPN,out soLuong) || soLuong <= 0)
            {
                ThongBao("Số lượng vật tư phải là một số nguyên dương");
                dgvCTPN.Focus();
                return false;
            }
            soLuong = int.Parse(soLuongCTPN);
            String query = "declare @result int\r\nexec @result = SP_KiemTraSoluongVattuDDH '"+ masoDDH+"', '" + maVT + "', " + soLuongCTPN + "\r\nselect @result";
            int result;
            // Dùng SP để kiểm tra xem có số lượng vật tư trong phiếu nhập có lớn hơn số lượng tồn của vật tư
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
            String query = "declare @result int\r\nexec @result = sp_KiemTraMaPhieuNhap N'" + maPN + "'\r\nselect @result";

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
                    "EXEC @result = [dbo].[SP_KiemTraCTPN] N'"
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
                this.phieuNhapTableAdapter.FillBy(this.dS1.PhieuNhap);
            }
        }
        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Lấy vị trí của con trỏ
            position = bdsPhieuNhap.Position;
            cheDo = "PN";
            isAdding = true;
            groupBoxPhieuNhap.Enabled = true;

            bdsPhieuNhap.AddNew();
            txtMAPN.Enabled = true;
            dteNGAY.EditValue = DateTime.Now;
            dteNGAY.Enabled = false;
            hOTENComboBox.Enabled = true;
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

            phieuNhapGridControl.Enabled = false;
            groupBoxPhieuNhap.Enabled = true;
            dgvCTPN.Enabled = false;
            contextMenuStripCTPN.Enabled = false ; 
        }
        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Lấy dữ liệu trước khi ghi phục vụ cho việc hoàn tác
            String maPN = txtMAPN.Text.Trim();
            DataRowView dr = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
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
            String maDDH = dr["MasoDDH"].ToString();
            String maNV = dr["MANV"].ToString();
            String maKho = dr["MAKHO"].ToString();
            

            // kiểm tra đầu vào có hợp lệ hay không
            if (validateInputPhieuNhap() == false)
            {
                return;
            }
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
                MessageBox.Show("Mã phiếu nhập đã tồn tại!", "Thông báo", MessageBoxButtons.OK);
                txtMAPN.Focus();
                return;
            }

            
            DialogResult dlr = MessageBox.Show("Bạn có chắc chắn muốn ghi dữ liệu vào cơ sở dữ liệu không?", "Thông báo", 
                MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
            if(dlr == DialogResult.OK) 
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
                    this.phieuNhapTableAdapter.FillBy(this.dS1.PhieuNhap);

                    /*cập nhật lại trạng thái thêm mới cho chắc*/
                    isAdding = false;
                    ThongBao("Ghi thành công.");
                    undoList.Push(undoQuery);
                }
                catch (Exception ex)
                {
                    bdsPhieuNhap.RemoveCurrent();
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.phieuNhapTableAdapter.FillBy(this.dS1.PhieuNhap);

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
            this.Close();
        }
        private void btnLamMoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                phieuNhapTableAdapter.FillBy(this.dS1.PhieuNhap);
                phieuNhapGridControl.Enabled = true;
            }catch(Exception ex)
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
            if(Program.username != maNV)
            {
                ThongBao("Không thể xóa phiếu nhập không phải do mình tạo");
                return;
            }
            if(bdsCTPN.Count > 0)
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
                    this.phieuNhapTableAdapter.FillBy(this.dS1.PhieuNhap);

                    /*Cap nhat lai do ben tren can tao cau truy van nen da dat dangThemMoi = true*/
                    isAdding = false;
                    ThongBao("Xóa phiếu nhập thành công");
                    btnHoanTac.Enabled = true;
                }catch (Exception ex)
                {
                    ThongBao("Lỗi xóa phiếu nhập! " + ex.Message);
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.phieuNhapTableAdapter.Update(this.dS1.PhieuNhap);
                    this.phieuNhapTableAdapter.FillBy(this.dS1.PhieuNhap);
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
            // Thay đổi bật/ tắt các nút chức năng
            btnXoa.Enabled = true;
            btnLamMoi.Enabled = true;
            btnThoat.Enabled = true;

            phieuNhapGridControl.Enabled = true;
            dgvCTPN.Enabled = true;
            

            if (isAdding == true && btnThem.Enabled == false && cheDo.Equals("PN"))
            {
                txtMAPN.Enabled = false;
                dteNGAY.Enabled = true;
                hOTENComboBox.Enabled = true;
                tENKHOComboBox.Enabled = true;
                cbxMASODDH.Enabled = true;

                contextMenuStripCTPN.Enabled = true;
                //Hủy thao tác trên bds
                bdsPhieuNhap.CancelEdit();
                bdsPhieuNhap.RemoveCurrent();
                /* trở về lúc đầu con trỏ đang đứng*/
                bdsPhieuNhap.Position = position;
                return;
            }
            else if(isAdding == true && thêmToolStripMenuItem.Enabled == false && cheDo.Equals("CTPN"))
            {
                contextMenuStripCTPN.Enabled = true;
                thêmToolStripMenuItem.Enabled = true;
                xóaToolStripMenuItem.Enabled = true;
                bdsCTPN.CancelEdit();
                if (bdsCTPN.Count != 0)
                {
                    bdsCTPN.RemoveCurrent();
                }
                bdsCTPN.Position = position;
                this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTPNTableAdapter.Fill(this.dS1.CTPN);
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
            if (cheDo.Equals("PN"))
            {
                bdsPhieuNhap.CancelEdit();
                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                this.phieuNhapTableAdapter.FillBy(this.dS1.PhieuNhap);
                bdsPhieuNhap.Position = position;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(Program.conStr))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        int undoPosition = int.Parse(undoIndex.Pop().ToString());
                        DataRowView dr = (DataRowView)bdsCTPN[undoPosition];
                        if (undoSql.Contains("DELETE") && !undoSql.Contains("INSERT"))
                        {
                            ExecuteSP_CapNhatSoLuongVatTu(dr["MAVT"].ToString().Trim(), int.Parse(dr["SOLUONG"].ToString().Trim()) * (-1));
                        }
                        else if (undoSql.Contains("UPDATE"))
                        {
                            string[] tempSoLuong = undoSql.Split(new string[] { "CAST" }, StringSplitOptions.None)[1].Split(new string[] { "AS" }, StringSplitOptions.None);
                            int preSoluong = int.Parse(tempSoLuong[0].Replace("(", "").Trim());
                            int currSoluong = int.Parse(dr["SOLUONG"].ToString().Trim());
                            string currMAVT = dr["MAVT"].ToString().Trim();
                            ExecuteSP_CapNhatSoLuongVatTu(currMAVT, preSoluong - currSoluong);
                        }
                        else if (!undoSql.Contains("DELETE") && undoSql.Contains("INSERT"))
                        {
                            string[] tempSoLuong = undoSql.Split(new string[] { "CAST" }, StringSplitOptions.None)[1].Split(new string[] { "AS" }, StringSplitOptions.None);
                            string[] tempMAVT = undoSql.Split(new string[] { "MAVT" }, StringSplitOptions.None)[1].Split('\'');
                            int preSoluong = int.Parse(tempSoLuong[0].Replace("(", "").Trim());
                            string preMAVT = tempMAVT[3];

                            ExecuteSP_CapNhatSoLuongVatTu(preMAVT.Trim(), preSoluong);
                        }
                        else
                        {
                            string[] tempSoLuong = undoSql.Split(new string[] { "CAST" }, StringSplitOptions.None)[1].Split(new string[] { "AS" }, StringSplitOptions.None);
                            string[] tempMAVT = undoSql.Split(new string[] { "MAVT" }, StringSplitOptions.None)[2].Split('\'');
                            int preSoluong = int.Parse(tempSoLuong[0].Replace("(", "").Trim());
                            string preMAVT = tempMAVT[3];
                            int currSoluong = int.Parse(dr["SOLUONG"].ToString().Trim());
                            string currMAVT = dr["MAVT"].ToString().Trim();

                            ExecuteSP_CapNhatSoLuongVatTu(currMAVT, currSoluong * (-1));
                            ExecuteSP_CapNhatSoLuongVatTu(preMAVT, preSoluong);
                        }
                        bdsCTPN.CancelEdit();
                        this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                        this.cTPNTableAdapter.Fill(this.dS1.CTPN);
                        bdsCTPN.Position = position;
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        if(transaction != null)
                        {
                            transaction.Rollback();
                        }
                        ThongBao("Có lỗi xảy ra trong quá trình thực hiện không thể hoàn tất việc hoàn tác");
                    }
                    
                }
            }
        }
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Lấy vị trí của con trỏ
            position = bdsCTPN.Position;
            isAdding = true;
            cheDo = "CTPN";

            bdsCTPN.AddNew();


            // Thay đổi bật/ tắt các nút chức năng
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLamMoi.Enabled = false;
            btnChitietPN.Enabled = false;
            btnThoat.Enabled = true;
            btnHoanTac.Enabled = true;
            btnGhi.Enabled = false;


            phieuNhapGridControl.Enabled = false;
            groupBoxPhieuNhap.Enabled = false;
            dgvCTPN.Enabled = true;
            thêmToolStripMenuItem.Enabled = false;
            xóaToolStripMenuItem.Enabled = false;
        }
        private void dgvCTPN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            cheDo = "CTPN";
            DataGridViewRow selectedRow = dgvCTPN.Rows[e.RowIndex];
            Dictionary<string, object>  previousRowData = new Dictionary<string, object>();
            foreach (DataGridViewCell cell in selectedRow.Cells)
            {
                string columnName = dgvCTPN.Columns[cell.ColumnIndex].DataPropertyName;

                // Lưu dữ liệu vào biến previousRowData
                previousRowData[columnName] = cell.Value;
            }
            if (!previousRowDataDict.Keys.Contains(bdsCTPN.Position))
            {
                previousRowDataDict[bdsCTPN.Position] = previousRowData;
            }
            else
            {
                if (string.IsNullOrEmpty(previousRowDataDict[bdsCTPN.Position]["MAVT"].ToString()))
                {
                    previousRowDataDict[bdsCTPN.Position]["MAVT"] = previousRowData["MAVT"];
                }
                if (string.IsNullOrEmpty(previousRowDataDict[bdsCTPN.Position]["DONGIA"].ToString()))
                {
                    previousRowDataDict[bdsCTPN.Position]["DONGIA"] = previousRowData["DONGIA"];
                }
                if (string.IsNullOrEmpty(previousRowDataDict[bdsCTPN.Position]["SOLUONG"].ToString()))
                {
                    previousRowDataDict[bdsCTPN.Position]["SOLUONG"] = previousRowData["SOLUONG"];
                }
            }
            position = bdsCTPN.Position;

        }
        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRowView dr = (DataRowView)bdsCTPN[bdsCTPN.Position];
            String maPN = dr["MAPN"].ToString().Trim();
            String maVT = dr["MAVT"].ToString().Trim();
            int soLuong =Int32.Parse(dr["SOLUONG"].ToString());
            float donGia = float.Parse(dr["DONGIA"].ToString());
            String undoQuery = "INSERT INTO dbo.CTPN(MAPN,MAVT,SOLUONG,DONGIA)\r\n" +
                "VALUES(N'"+maPN+"',N'"+maVT+"',CAST("+soLuong+" AS INT),CAST("+donGia+" AS float))";
            undoList.Push(undoQuery);
            undoIndex.Push(bdsCTPN.Position);
            if (MessageBox.Show("Bạn có muốn xóa chi tiết phiếu nhập này không","Thông báo",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                using (SqlConnection connection = new SqlConnection(Program.conStr))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        position = bdsCTPN.Position;
                        bdsCTPN.RemoveCurrent();
                        ExecuteSP_CapNhatSoLuongVatTu(maVT, soLuong * (-1));
                        this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                        this.cTPNTableAdapter.Update(this.dS1.CTPN);
                        this.cTPNTableAdapter.Fill(this.dS1.CTPN);
                  
                        transaction.Commit();

                        /*Cap nhat lai do ben tren can tao cau truy van nen da dat dangThemMoi = true*/
                        isAdding = false;
                        btnHoanTac.Enabled = true;
                        ThongBao("Xóa chi tiết phiếu nhập thành công");
                    }
                    catch (Exception ex)
                    {
                        if(transaction != null)
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
            else
            {
                undoList.Pop();
                undoIndex.Pop();
            }
        }
        private void groupBoxPhieuNhap_Enter(object sender, EventArgs e)
        {
            cheDo = "PN";
        }
        private void ghiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // kiểm tra đầu vào có hợp lệ hay không
            if (validateInputCTPN() == false)
            {
                return;
            }
            String maPN = txtMAPN.Text.Trim();
            DataRowView checkDr = (DataRowView)bdsCTPN[bdsCTPN.Position];
            int pnResult = ExecuteSP_TracuuCTPN(checkDr["MAPN"].ToString().Trim(), checkDr["MAVT"].ToString().Trim());
            if (pnResult != 1 && pnResult != 0)
            {
                ThongBao("Có lỗi trong quá trình xử lý ghi chi tiết phiếu nhập");
                return;
            }

            if (pnResult == 1)
            {
                if (isAdding)
                {
                    MessageBox.Show("Chi tiết phiếu nhập này đã tồn tại!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    string maVTPre = previousRowDataDict[position]["MAVT"].ToString().Trim();
                    string maVTNext = checkDr["MAVT"].ToString().Trim();
                    if (maVTPre != maVTNext)
                    {
                        MessageBox.Show("Chi tiết phiếu nhập này đã tồn tại!", "Thông báo", MessageBoxButtons.OK);
                        return;
                    }
                }
            }

            String masoDDH = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position])["MasoDDH"].ToString().Trim();
            pnResult = ExecuteSP_TracuuVatTuCTPN(checkDr["MAVT"].ToString().Trim(), masoDDH);
            if (pnResult != 1 && pnResult != 0)
            {
                ThongBao("Có lỗi trong quá trình xử lý ghi chi tiết phiếu nhập");
                return;
            }

            if (pnResult == 0)
            {
                MessageBox.Show("Vui lòng chọn vật tư đã dặt hàng trong chi tiết đơn đặt hàng!", "Thông báo", MessageBoxButtons.OK);
                txtMAPN.Focus();
                return;
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
                        //Lưu truy vấn phục vụ hoàn tác

                        string undoQuery = "";
                        previousCTPN[maPN] = previousRowDataDict;
                        if (isAdding == true)
                        {
                            DataRowView drCTPN = (DataRowView)bdsCTPN[position];
                            undoQuery = "DELETE FROM DBO.CTPN " +
                            "WHERE MAPN = N'" + drCTPN["MAPN"].ToString() + "' " +
                            "AND MAVT = N'" + drCTPN["MAVT"] + "'";
                            ExecuteSP_CapNhatSoLuongVatTu(drCTPN["MAVT"].ToString().Trim(), int.Parse(drCTPN["SOLUONG"].ToString().Trim()));
                        }
                        else
                        {
                            //Lưu dữ liệu để hoàn tác
                            String maPhieuNhap = previousCTPN[maPN][position]["MAPN"].ToString().Trim();
                            String maVT = previousCTPN[maPN][position]["MAVT"].ToString().Trim();
                            int soLuong = int.Parse(previousCTPN[maPN][position]["SOLUONG"].ToString().Trim());


                            DataRowView drCTPN = (DataRowView)bdsCTPN[position];
                            String currMaVT = drCTPN["MAVT"].ToString().Trim();
                            if (maVT.Equals(currMaVT))
                            {
                                undoQuery = "UPDATE dbo.CTPN\r\n" +
                                "SET SOLUONG = CAST(" + previousRowDataDict[position]["SOLUONG"] + " AS INT), DONGIA = CAST(" + previousRowDataDict[position]["DONGIA"] + " AS float)\r\n" +
                                "WHERE MAPN = N'" + maPhieuNhap + "' AND MAVT = N'" + maVT + "'";
                                ExecuteSP_CapNhatSoLuongVatTu(maVT, int.Parse(drCTPN["SOLUONG"].ToString().Trim()) - soLuong);
                            }
                            else
                            {
                                undoQuery = "DELETE FROM [dbo].[CTPN] \r\n" +
                                    "WHERE MAPN = '" + drCTPN["MAPN"].ToString().Trim() + "' AND MAVT = '" + drCTPN["MAVT"].ToString().Trim() + "'\r\n" +
                                    "INSERT INTO [dbo].[CTPN] ([MAPN],[MAVT],[SOLUONG],[DONGIA])\r\n" +
                                    "VALUES('" + maPhieuNhap + "','" + maVT +
                                    "',CAST(" + soLuong +
                                    " AS INT),CAST(" + previousRowDataDict[position]["DONGIA"] + " AS float))";
                                ExecuteSP_CapNhatSoLuongVatTu(drCTPN["MAVT"].ToString().Trim(), int.Parse(drCTPN["SOLUONG"].ToString().Trim()));
                                ExecuteSP_CapNhatSoLuongVatTu(maVT, soLuong * (-1));
                            }
                        }
                        previousRowDataDict.Remove(position);
                        this.bdsCTPN.EndEdit();
                        this.cTPNTableAdapter.Update(this.dS1.CTPN);


                        //Hoàn tất thao tác
                        transaction.Commit();

                        /*cập nhật lại trạng thái thêm mới cho chắc*/
                        isAdding = false;
                        ThongBao("Ghi thành công.");
                        undoList.Push(undoQuery);
                        undoIndex.Push(position);
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
                    finally
                    {

                        // Thay đổi bật/ tắt các nút chức năng
                        btnThem.Enabled = true;
                        btnXoa.Enabled = true;
                        btnGhi.Enabled = true;
                        btnHoanTac.Enabled = true;
                        btnLamMoi.Enabled = true;
                        btnChitietPN.Enabled = true;

                        dgvCTPN.Enabled = true;
                        phieuNhapGridControl.Enabled = true;
                        groupBoxPhieuNhap.Enabled = true;

                        thêmToolStripMenuItem.Enabled = true;
                        xóaToolStripMenuItem.Enabled = true;
                    }
                }
            }
        }
    }
}
