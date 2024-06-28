using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.PivotGrid.OLAP.SchemaEntities;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraRichEdit.API.Native;
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
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormDonDatHang : Form
    {
        // Tương tự như form nhân viên, khởi tạo các biến position(vị trí trên GC), isAdding
        int positionDDH = 0;
        int positionCTDDH = 0;
        bool isAdding = false;

        //Undo -> dùng để hoàn tác dữ liệu nếu lỡ có thao tác không mong muốn
        Stack undoStack = new Stack();
        Stack undoTablePosition = new Stack();
        string cheDo = "";

        public FormDonDatHang()
        {
            InitializeComponent();
        }

        private void datHangBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsDatHang.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS1);

        }

        private void FormDonDatHang_Load(object sender, EventArgs e)
        {
            //Không cần kiểm tra khóa ngoại
            dS1.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS1.DSNV' table. You can move, or remove it, as needed.
            this.dSNVTableAdapter.Connection.ConnectionString = Program.conStr;
            this.dSNVTableAdapter.Fill(this.dS1.DSNV);

            // TODO: This line of code loads data into the 'dS1.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.conStr;
            this.nhanVienTableAdapter.Fill(this.dS1.NhanVien);

            // TODO: This line of code loads data into the 'dS1.Vattu' table. You can move, or remove it, as needed.
            this.vattuTableAdapter.Connection.ConnectionString = Program.conStr;
            this.vattuTableAdapter.Fill(this.dS1.Vattu);

            // TODO: This line of code loads data into the 'dS1.DSKHO' table. You can move, or remove it, as needed.
            this.dSKHOTableAdapter.Connection.ConnectionString = Program.conStr;
            this.dSKHOTableAdapter.Fill(this.dS1.DSKHO);
            // TODO: This line of code loads data into the 'dS1.DatHang' table. You can move, or remove it, as needed.
            this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
            //this.datHangTableAdapter.Fill(this.dS1.DatHang);
            this.datHangTableAdapter.FillBy(this.dS1.DatHang);

            // TODO: This line of code loads data into the 'dS1.CTDDH' table. You can move, or remove it, as needed.
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);


            cbChiNhanh.DataSource = Program.bindingSource;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            //cbChiNhanh.SelectedIndex = Program.brand;
            cbChiNhanh.SelectedIndex = Program.bindingSource.Position;
            hOTENComboBox.Enabled = false;
            if (Program.mGroup == "CONGTY")
            {
                btnThem.Enabled = false;
                btnGhi.Enabled = false;
                btnXoa.Enabled = false;
                btnHoanTac.Enabled = false;
                groupBoxDonDatHang.Enabled = false;
                contextMenuStripDDH.Enabled = false;
                contextMenuStripRowDDH.Enabled = false;
                //Ngăn không cho nhấn xóa data trong từng dòng bằng cách tắt contextMenuTrong dgv
                foreach (DataGridViewRow row in dgvCTDDH.Rows)
                {
                    row.ContextMenuStrip = null;
                }
            }

            //Phân quyền nhóm CHINHANH-USER có thể thao tác với dữ liệu
            //Nhưng không được quyền chuyển chi nhánh khác để xem dữ liệu
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                cbChiNhanh.Enabled = false;
            }
            if (undoStack.Count == 0)
            {
                btnHoanTac.Enabled = false;
            }

        }
        //private void ExecuteSP_TracuuActiveEmployee()
        //{
        //    string chiNhanh = null;
        //    if (Program.brand == 1)
        //    {
        //        chiNhanh = "CN2";
        //    }
        //    else if (Program.brand == 0)
        //    {
        //        chiNhanh = "CN1";
        //    }
        //    string query = "SELECT [HO] + N' ' + [TEN] + N' - ' + CONVERT(NVARCHAR(10), [MANV]) as name,[MANV] as maNV\r\n " +
        //        "FROM [QLVT].[dbo].[NhanVien]\r\n" +
        //        "WHERE [MACN] = N'" + chiNhanh + "' AND [TrangThaiXoa] = 0";
        //    try
        //    {
        //        Program.myReader = Program.ExecSqlDataReader(query);

        //        //Không có kết quả thì kết thúc
        //        if (Program.myReader == null)
        //        {
        //            return;
        //        }

        //        DataTable dt = new DataTable();
        //        dt.Load(Program.myReader); // Load data from reader into DataTable

        //        hOTENComboBox.DataSource = dt; // Set data source for ComboBox directly
        //        //hOTENComboBox.DisplayMember = "FullName"; // Set the property displayed in ComboBox
        //        hOTENComboBox.DisplayMember = "name"; // Lấy cột name trong cột mới tạo trong select
        //        hOTENComboBox.ValueMember = "maNV";
        //        hOTENComboBox.SelectedValue = null;
        //        //hOTENComboBox.SelectedIndex = -1;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    Program.myReader.Close();
        //    return;
        //}

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnLamMoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            positionDDH = bdsDatHang.Position;
            try
            {
                this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
                //this.datHangTableAdapter.Fill(this.dS1.DatHang);
                this.datHangTableAdapter.FillBy(this.dS1.DatHang);
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
                bdsDatHang.Position = positionDDH;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi làm mới dữ liệu", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (positionDDH >= 0)
            {
                bdsDatHang.Position = positionDDH;
            }
        }

        private void tENKHOComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tENKHOComboBox.SelectedValue == null)
            {
                txtMANV.Text = String.Empty;
                return;
            }
            try
            {
                txtMAKHO.Text = tENKHOComboBox.SelectedValue.ToString();
            }
            catch (Exception ex) { }
        }

        private void hOTENComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hOTENComboBox.SelectedValue == null) return;
            try
            {
                txtMANV.Text = hOTENComboBox.SelectedValue.ToString();
            }
            catch (Exception ex) { }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //String nhanVien = hOTENComboBox.SelectedItem.ToString().Trim();
            cheDo = "";
            isAdding = true;
            bdsDatHang.AddNew();
            
            // Thay đổi bật/tắt các nút
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            //txtMaVT.Enabled = true;
            btnGhi.Enabled = true;
            btnHoanTac.Enabled = true;
            btnLamMoi.Enabled = false;
            btnThoat.Enabled = false;

            txtMaDDH.Enabled = true;
            datHangGridControl.Enabled = false;
            dgvCTDDH.Enabled = false;
            contextMenuStripDDH.Enabled = false;

            //Lấy mã nhân viên tự động và ngăn cho nhân viên thêm vào của nhân viên khác
            //hOTENComboBox.DataSource = datHangGridControl;
            int dsnvCBBIndex = dSNVBindingSource.Find("MANV", Program.username);
            hOTENComboBox.SelectedIndex = dsnvCBBIndex;
            hOTENComboBox.Enabled = false ;
            //txtMANV.Text = Text;
            //txtMANV.Enabled = false;


        }
        private bool validateMaDonDatHang(string maDDH)
        {
            if (string.IsNullOrEmpty(maDDH))
            {
                MessageBox.Show("Mã đơn dặt hàng không được bỏ trống", "Thông báo", MessageBoxButtons.OK);
                txtMaDDH.Focus();
                return false;
            }
            if (!maDDH.StartsWith("MDDH"))
            {
                MessageBox.Show("Mã đơn dặt hàng phải bắt đầu với MDDH", "Thông báo", MessageBoxButtons.OK);
                txtMaDDH.Focus();
                return false;
            }
            if (maDDH.Length > 8)
            {
                MessageBox.Show("Mã đơn dặt hàng tối đa 8 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtMaDDH.Focus();
                return false;
            }
            return true;
        }
        private bool validateNhaCungCap(string nhaCC)
        {
            if (string.IsNullOrEmpty(nhaCC))
            {
                MessageBox.Show("Không được bỏ trống tên nhà cung cấp", "Thông báo", MessageBoxButtons.OK);
                txtNhaCC.Focus();
                return false;
            }
            if (!Regex.IsMatch(nhaCC, @"^[\p{L}\p{N} -.]+$"))
            {
                MessageBox.Show("Nhà cung cấp chỉ có chữ cái và số không có kí tự đặc biệt", "Thông báo", MessageBoxButtons.OK);

                txtNhaCC.Focus();
                return false;
            }
            if (nhaCC.Length > 100)
            {
                MessageBox.Show("Nhà cung cấp chỉ có tối đa 100 kí tự", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        private bool validateMaNhanVien(string maNV)
        {
            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cho đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                hOTENComboBox.Focus();
                return false;
            }
            return true;
        }
        private bool validateMaKho(string maKho)
        {
            if (string.IsNullOrEmpty(maKho))
            {
                MessageBox.Show("Vui lòng chọn kho cho đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                tENKHOComboBox.Focus();
                return false;
            }
            return true;
        }
        private bool validateDate(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                MessageBox.Show("Vui lòng chọn ngày cho đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                dateNgay.Focus();
                return false;
            }
            return true;
        }
        private bool validateInputMaDDH()
        {
            return validateMaDonDatHang(txtMaDDH.Text) &&
                validateMaKho(txtMAKHO.Text) &&
                validateMaNhanVien(txtMANV.Text) &&
                validateNhaCungCap(txtNhaCC.Text) &&
                validateDate(dateNgay.Text);
        }
        private bool validateInputCTDDH()
        {
            string MaVTCTDDH = dgvCTDDH.Rows[positionCTDDH].Cells[1].Value?.ToString().Trim() ?? string.Empty;
            string SoLuongCTDDH = dgvCTDDH.Rows[positionCTDDH].Cells[2].Value?.ToString().Trim() ?? string.Empty;
            string DonGiaCTDDH = dgvCTDDH.Rows[positionCTDDH].Cells[3].Value?.ToString().Trim() ?? string.Empty;
            return validateMaVTCTDDH(MaVTCTDDH) &&
            validateSoLuongCTDDH(SoLuongCTDDH) &&
            validateDonGiaCTDDH(DonGiaCTDDH);
            //return validateMaVTCTDDH(dgvCTDDH.Rows[positionCTDDH].Cells[1].Value.ToString().Trim()) &&
            //    validateSoLuongCTDDH(dgvCTDDH.Rows[positionCTDDH].Cells[2].Value.ToString().Trim()) &&
            //    validateDonGiaCTDDH(dgvCTDDH.Rows[positionCTDDH].Cells[3].Value.ToString().Trim());
        }
        private bool validateMaVTCTDDH(string maVT)
        {   
            if (string.IsNullOrEmpty(maVT.Trim()))
            {
                MessageBox.Show("Ô vật tư còn trống", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        private bool validateSoLuongCTDDH(String soLuongString)
        {
            int soLuong;
            bool result = int.TryParse(soLuongString.Trim(), out soLuong);
            if (result == false)
            {
                MessageBox.Show("Số lượng trong chi tiết đơn đặt hàng phải là 1 số","Thông báo", MessageBoxButtons.OK);
            }
            if (soLuong<=0)
            {
                MessageBox.Show("Số lượng trong chi tiết đơn đặt hàng phải >0", "Thông báo", MessageBoxButtons.OK);
                return false;
            }    
            return result;
        }
        private bool validateDonGiaCTDDH(String  donGiaString)
        {
            int donGia;
            bool result = int.TryParse(donGiaString.Trim(), out donGia);
            if (result == false) 
            {
                MessageBox.Show("Đơn giá trong chi tiết đơn hàng không hợp lệ", "Thông báo", MessageBoxButtons.OK);
            }
            if (donGia <= 0)
            {
                MessageBox.Show("Đơn giá trong chi tiết đơn đặt hàng phải >0", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            return result;
        }
        private void datHangGridControl_Click(object sender, EventArgs e)
        {            
            if (bdsDatHang.Count > 0)
            {
                positionDDH = bdsDatHang.Position;
            }
        }

        private void btnHoanTac_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            positionDDH = bdsDatHang.Position;
            dgvCTDDH.Enabled = true;
            thêmToolStripMenuItem.Enabled = true;
            xóaToolStripMenuItem.Enabled = true;
            ghiToolStripMenuItem.Enabled = true;

            contextMenuStripRowDDH.Enabled = true;
            //xóaVậtTưToolStripMenuItem.Enabled=true;


            groupBoxDonDatHang.Enabled = true;
            contextMenuStripDDH.Enabled = true;
            datHangGridControl.Enabled = true;
            if (isAdding == true && cheDo.Equals("CTDDH"))
            {
                cheDo = "";
                isAdding = false; btnThem.Enabled = true;
                btnXoa.Enabled = true;
                btnLamMoi.Enabled = true;
                btnThoat.Enabled = true;
                datHangGridControl.Enabled = true;
                bdsCTDDH.CancelEdit();
                //bdsCTDDH.RemoveCurrent();
                //this.cTDDHTableAdapter.Update(this.dS1.CTDDH);
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
                this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
                //this.datHangTableAdapter.Fill(this.dS1.DatHang);
                this.datHangTableAdapter.FillBy(this.dS1.DatHang);
                btnHoanTac.Enabled = undoStack.Count>0;
                bdsDatHang.Position = positionDDH;
                return;

            }
            // Nếu nhấn thêm nhưng chưa ghi
            if (isAdding == true && btnThem.Enabled == false)
            {
                isAdding = false; btnThem.Enabled = true;
                btnXoa.Enabled = true;
                btnLamMoi.Enabled = true;
                btnThoat.Enabled = true;
                datHangGridControl.Enabled = true;

                // Hủy thao tác thêm trên bds
                bdsDatHang.CancelEdit();
                // Xóa dòng được thêm
                //bdsDatHang.RemoveCurrent();
                if (undoStack.Count == 0)
                {
                    btnHoanTac.Enabled = false;
                    bdsDatHang.Position = positionDDH;
                    return;
                }
                bdsDatHang.Position = positionDDH;
                return;
            }
            object undoQueryExe = undoStack.Pop();
            if(undoQueryExe is string)
            {
                string undoQuery = undoQueryExe as string;
                int _ = Program.ExecSqlNonQuery(undoQuery);
                Console.WriteLine(undoQuery);
            }
            else if(undoQueryExe is List<String>)
            {
                List<String> undoQueryList = undoQueryExe as List<String>;
                foreach(String undoQuery in undoQueryList)
                {
                    int _ = Program.ExecSqlNonQuery(undoQuery);
                    Console.WriteLine(undoQuery);
                }
            }
            if (Program.connectDB() == 0)
            {
                return;
            }
            this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
            //this.datHangTableAdapter.Fill(this.dS1.DatHang);
            this.datHangTableAdapter.FillBy(this.dS1.DatHang);
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
            // Nếu Stack undo hết thì tự động vô hiệu hóa
            if (undoStack.Count == 0)
            {
                btnHoanTac.Enabled = false;
                if (positionDDH >= 0)
                {
                    bdsDatHang.Position = positionDDH;
                }
                return;
            }
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            positionDDH = bdsDatHang.Position;
            cheDo = "";
            if (validateInputMaDDH()==false) 
            {
                return;
            }
            int manv;
            int.TryParse(txtMANV.Text, out manv);
            if (Execute_SPKiemTraTrangThaiNV(manv) == 1) //1 là đã bị xóa, 0 là chưa được sử dụng
            {
                MessageBox.Show("Nhân viên đã bị xóa không thể ghi vào cơ sở dữ liệu", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (dateNgay.DateTime > DateTime.Now)
            {
                MessageBox.Show("Ngày đặt hàng không được phép lớn hơn ngày hôm nay", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            //if(isAdding == false)
            //{
            //    DataRowView drvMaNV = (DataRowView)bdsDatHang[bdsDatHang.Position];
            //    DataRow rowMaNV = drvMaNV.Row;
            //    String checkmaNV = rowMaNV["MANV"].ToString().Trim();
            //    if (txtMANV.Text.Trim() != checkmaNV)
            //    {
            //        MessageBox.Show("Không thể đơn đặt hàng của người khác", "Thông báo", MessageBoxButtons.OK);
            //        return;
            //    }
            //}
            if (isAdding == true)
            {
                if (Execute_SPKiemTraMDDH(txtMaDDH.Text.Trim()) == 1) //1 là đã đc sd,0 là chưa được sử dụng
                {
                    MessageBox.Show("Mã đơn đặt hàng đã được sử dụng", "Thông báo", MessageBoxButtons.OK);
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
                    // Trường hợp thêm đơn đặt hàng
                    if (isAdding == true)
                    {   
                        //hOTENComboBox.Enabled = true;
                        hOTENComboBox.Enabled = false;
                        //Lấy dữ liệu mới để phục vụ xóa dữ liệu mơi nhập vào
                        DataRowView drv = (DataRowView)bdsDatHang[bdsDatHang.Position];
                        DataRow drvRow = drv.Row;
                        String maDDH = drvRow["MasoDDH"].ToString().Trim();
                        undoQuery = "DELETE FROM CTDDH WHERE MasoDDH = N'" + maDDH + "'\r\nDELETE FROM DatHang WHERE MasoDDH= N'" + maDDH + "'";
                    }
                    else
                    {
                        // Lấy dữ liệu trước khi ghi để đổ vào undoStack
                        //DataRowView drv = (DataRowView)bdsDatHang[bdsDatHang.Position];
                        // Dữ liệu mới để cho vào cơ sở dữ liệu
                        //String maNV = txtMANV.Text.Trim();
                        //String nhaCC = txtNhaCC.Text.Trim();
                        //String maDDH = txtMaDDH.Text.Trim();
                        //String maKho = txtMAKHO.Text.Trim();


                        //String maNV = drv["MANV"].ToString().Trim();
                        //String nhaCC = drv["NhaCC"].ToString().Trim();
                        //String maDDH = drv["MasoDDH"].ToString().Trim();
                        //String maKho = drv["MAKHO"].ToString().Trim();
                        //Lấy dữ liệu cũ
                        DataRowView drv = (DataRowView)bdsDatHang.Current;
                        DataRow row = drv.Row;
                        String maNV = row["MANV", DataRowVersion.Original].ToString().Trim();
                        String nhaCC = row["NhaCC", DataRowVersion.Original].ToString().Trim();
                        String maDDH = row["MasoDDH", DataRowVersion.Original].ToString().Trim();
                        String maKho = row["MAKHO", DataRowVersion.Original].ToString().Trim();
                        //Console.WriteLine($"{maNV} {nhaCC} {maDDH} {maKho} {bdsDatHang.Position}");

                        //DataRowView drvMaNV = (DataRowView)bdsDatHang[bdsDatHang.Position];
                        //DataRow rowMaNV = drvMaNV.Row;
                        //String checkmaNV = rowMaNV["MANV"].ToString().Trim();
                        //if (txtMANV.Text.Trim() != checkmaNV)
                        //{
                        //    MessageBox.Show("Không thể sửa mã đơn đặt hàng của người khác", "Thông báo", MessageBoxButtons.OK);
                        //    return;
                        //}
                        bool isSameNhanVien = Program.username.ToString().Trim() == txtMANV.Text.ToString().Trim();
                        if(isSameNhanVien == false)
                        {
                            MessageBox.Show("Không thể cập nhập đơn hàng của nhân viên khác", "Thông báo", MessageBoxButtons.OK);
                            return;
                        }

                        DateTime ngayLap;
                        if (DateTime.TryParse(row["NGAY", DataRowVersion.Original].ToString(), out ngayLap))
                        {
                        }

                        else
                        {
                            // Xử lý trường hợp không thể chuyển đổi ngày
                            MessageBox.Show("Giá trị trong cột NGAY không phải là kiểu ngày tháng hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        undoQuery = $"UPDATE [dbo].[DatHang]\r\n   " +
                            $"SET " +
                            $"[NGAY] = CAST('{ngayLap.ToString("yyyy-MM-dd")}' AS DATETIME)\r\n      ," +
                            $"[NhaCC] = N'{nhaCC}'\r\n      ," +
                            $"[MANV] = CAST('{maNV}' AS INT)\r\n      ," +
                            $"[MAKHO] = '{maKho}'\r\n      " +
                            $"WHERE [MasoDDH] = '{maDDH}'";

                    }
                    undoStack.Push(undoQuery);
                    Console.WriteLine(undoQuery);
                    this.bdsDatHang.EndEdit();
                    this.datHangTableAdapter.Update(this.dS1.DatHang);
                    this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
                    //this.datHangTableAdapter.Fill(this.dS1.DatHang);
                    this.datHangTableAdapter.FillBy(this.dS1.DatHang);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    txtMaDDH.Enabled = false;
                    dateNgay.Enabled = true;
                    // Thay đổi bật/ tắt các nút chức năng
                    btnThem.Enabled = true;
                    btnXoa.Enabled = true;
                    btnGhi.Enabled = true;
                    btnHoanTac.Enabled = true;
                    isAdding = false;
                    btnLamMoi.Enabled = true;
                    //btnLamMoi.Enabled = true;
                    //if (undoStack.Count == 0)
                    //{
                    //    btnHoanTac.Enabled = false;
                    //}
                    btnHoanTac.Enabled = undoStack.Count > 0;
                    btnThoat.Enabled = true;
                    //btn.Enabled = true;


                    dgvCTDDH.Enabled = true;
                    contextMenuStripDDH.Enabled = true;

                    datHangGridControl.Enabled = true;
                    //.Enabled = true;
                    bdsDatHang.Position = positionDDH;
                }
            }

        }


        private void contextMenuStrip1_Opening_1(object sender, CancelEventArgs e)
        {

        }


        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isSameNhanVien = Program.username.ToString().Trim() == txtMANV.Text.ToString().Trim();
            foreach (DataGridViewRow row in dgvCTDDH.Rows)
            {
                row.ContextMenuStrip = null;
                row.ReadOnly = true;
            }
            contextMenuStripRowDDH.Enabled = false ;
            if (isSameNhanVien == false)
            {
                MessageBox.Show("Không thể cập nhập đơn hàng của nhân viên khác", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            //dgvCTDDH.ReadOnly = false;
            isAdding = true;
            cheDo = "CTDDH";
            //Tắt các lựa chọn khi chọn hàng
            foreach (DataGridViewRow row in dgvCTDDH.Rows)
            {
                row.ContextMenuStrip = null;
            }

            bdsCTDDH.AddNew();
            //Console.WriteLine(cheDo);
            positionCTDDH = bdsCTDDH.Position;
            //Console.WriteLine(positionCTDDH);
            String MasoDDH = txtMaDDH.Text.Trim();
            if (Execute_SPKiemTraDDHPhieuNhap(MasoDDH) == 1) //1 tức là không được phép thêm vì MasoDDH đã đc dùng cho với phiếu nhập
            {
                MessageBox.Show("Không sửa được đơn đặt hàng này vì đơn đặt hàng đã được sử dụng cho phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
                return;
            }
            btnLamMoi.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = false;
            btnHoanTac.Enabled=true;
            btnThoat.Enabled=false;
            btnGhi.Enabled=false;
            dgvCTDDH.Enabled = true;
            ghiToolStripMenuItem.Enabled = true;
            if (bdsCTDDH.Position < 0)
            {
                ghiToolStripMenuItem.Enabled = false;
                xóaToolStripMenuItem.Enabled = false;
            }
            datHangGridControl.Enabled = false;
            //dgvCTDDH.Enabled = false;
            groupBoxDonDatHang.Enabled = false;
            //contextMenuStripDDH.Enabled = false;
            thêmToolStripMenuItem.Enabled = false;
            xóaToolStripMenuItem.Enabled = false;

            //contextMenuStripRowDDH.Enabled = false;
            //Application.DoEvents();

        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            positionDDH = bdsDatHang.Position;
            String MasoDDH = txtMaDDH.Text.Trim();
            //MessageBox.Show("Bạn có xóa vật tư này không?","Xác nhận", MessageBoxButtons.YesNoCancel){

            //}
            bool isSameNhanVien = Program.username.ToString().Trim() == txtMANV.Text.ToString().Trim();
            if (isSameNhanVien == false)
            {
                MessageBox.Show("Không thể sửa chi tiết đơn đặt hàng của người khác", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (Execute_SPKiemTraDDHPhieuNhap(MasoDDH) == 1) //1 tức là không được phép xóa vì MasoDDH đã đc dùng cho với phiếu nhập
            {
                MessageBox.Show("Không xóa được đơn đặt hàng này vì đơn đặt hàng đã được sử dụng cho phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            
            
            DialogResult dr = MessageBox.Show("Bạn có muốn xóa hết vật tư", "Xác nhận", MessageBoxButtons.YesNo);
            if(dr == DialogResult.Yes)
            {
                List<String> undoQueryList = new List<String>();
                while (bdsCTDDH.Count > 0)
                {
                    // Extract data from the current row
                    DataRowView currentRow = (DataRowView)bdsCTDDH.Current;
                    string maDDH = currentRow["MasoDDH"].ToString();
                    string maVT = currentRow["MAVT"].ToString();
                    int soLuongCTDDH;
                    int.TryParse(currentRow["SOLUONG"].ToString(), out soLuongCTDDH);
                    int donGiaCTDDH;
                    int.TryParse(currentRow["DONGIA"].ToString(), out donGiaCTDDH);


                    String undoQuery = $"INSERT INTO [dbo].[CTDDH]\r\n           " +
                    $"([MasoDDH]\r\n           ," +
                    $"[MAVT]\r\n           ," +
                    $"[SOLUONG]\r\n           ," +
                    $"[DONGIA]\r\n           )" +
                    $"VALUES\r\n           (" +
                    $"N'{maDDH}'," +
                    $"N'{maVT}'," +
                    $"{soLuongCTDDH}," +
                    $"{donGiaCTDDH}" +
                    ")";
                    undoQueryList.Add(undoQuery);

                    // Remove the current row from the BindingSource
                    bdsCTDDH.RemoveCurrent();
                }
                this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
                this.datHangTableAdapter.Update(this.dS1.DatHang);
                //this.datHangTableAdapter.Fill(this.dS1.DatHang);
                this.datHangTableAdapter.FillBy(this.dS1.DatHang);
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTDDHTableAdapter.Update(this.dS1.CTDDH);
                this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
                undoStack.Push(undoQueryList);
                btnHoanTac.Enabled = undoStack.Count > 0;
                bdsDatHang.Position = positionDDH;
                MessageBox.Show("Vật tư đã được xóa hết", "Thông báo", MessageBoxButtons.OK);
            }
            this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
            this.datHangTableAdapter.Update(this.dS1.DatHang);
            //this.datHangTableAdapter.Fill(this.dS1.DatHang);
            this.datHangTableAdapter.FillBy(this.dS1.DatHang);
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTDDHTableAdapter.Update(this.dS1.CTDDH);
            this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
        }

        private void ghiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            positionDDH = bdsDatHang.Position;
            if (bdsCTDDH.Count == 0)
            {
                MessageBox.Show("Không ghi được vì chưa có thông tin gì để ghi hãy thêm mới ", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            //lẤY DỮ LIỆU TRÊN BẢNG NHÂN VIÊN
            DataRowView drvMaNV = (DataRowView)bdsDatHang[bdsDatHang.Position];
            DataRow rowMaNV = drvMaNV.Row;
            String checkmaNV = rowMaNV["MANV"].ToString().Trim();
            if (Program.username.ToString().Trim() != checkmaNV)
            {
                MessageBox.Show("Không thể sửa mã đơn đặt hàng của người khác", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (validateInputCTDDH() ==false)
            {
                return;
            }
            bool continueRun = true;
            int checkrow = 0;
            while (bdsCTDDH.Count > checkrow)
            {
                //Lấy dữ liệu mới kiểm tra

                DataRowView drvOldRow = (DataRowView)bdsCTDDH[checkrow];
                DataRow row = drvOldRow.Row;
                string MaVTCTDDH = row["MAVT"].ToString().Trim();
                string MaDDH = row["MasoDDH"].ToString().Trim();
                int SoLuongCTDDH;
                int.TryParse(row["SOLUONG"].ToString().Trim(), out SoLuongCTDDH);
                int DonGiaCTDDH;
                int.TryParse(row["DONGIA"].ToString().Trim(), out DonGiaCTDDH);
                
                //bool continueRun = true;
                if ((validateMaVTCTDDH(MaVTCTDDH) &&
                    validateSoLuongCTDDH(SoLuongCTDDH.ToString()) &&
                    validateDonGiaCTDDH(DonGiaCTDDH.ToString())) == false)
                {
                    bdsCTDDH.CancelEdit();
                    positionDDH = bdsDatHang.Position;
                    
                    bdsDatHang.Position = positionDDH;

                    this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
                    //this.datHangTableAdapter.Fill(this.dS1.DatHang);
                    this.datHangTableAdapter.FillBy(this.dS1.DatHang);
                    this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);


                    continueRun = false;
                    break;
                }
                checkrow++;
            }
            if (continueRun == false)
            {
                // TODO: This line of code loads data into the 'dS1.CTDDH' table. You can move, or remove it, as needed.
                
                try {
                    this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
                }catch (Exception ex)
                {

                }
                return;
            }
                //bool isSameNhanVien = Program.username.ToString().Trim() == txtMANV.Text.ToString().Trim();
                //if (isSameNhanVien == false)
                //{
                //    MessageBox.Show("Không thể cập nhập đơn hàng của nhân viên khác", "Thông báo", MessageBoxButtons.OK);
                //    return;
                //}
            String MasoDDH = dgvCTDDH.Rows[positionCTDDH].Cells[0].Value.ToString().Trim();
            if (Execute_SPKiemTraDDHPhieuNhap(MasoDDH) == 1) //1 tức là không được phép sửa vì MasoDDH đã đc dùng cho với phiếu nhập
            {
                MessageBox.Show("Không sửa được đơn đặt hàng này vì đơn đặt hàng đã được sử dụng cho phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
                return;
            }
            for(int i = 0; i < bdsCTDDH.Count; i++)
            {
                dgvCTDDH.Rows[i].ReadOnly = true;
            }
            if (isAdding== true && cheDo.Equals("CTDDH"))
            {
                
                //Giữ lại vài giá trị để bấm nút hoàn tác
                String MaDDH = dgvCTDDH.Rows[positionCTDDH].Cells[0].Value.ToString().Trim();
                String MaVT = dgvCTDDH.Rows[positionCTDDH].Cells[1].Value.ToString().Trim();
                int soLuongCTDDH;
                int.TryParse(dgvCTDDH.Rows[positionCTDDH].Cells[2].Value.ToString().Trim(), out soLuongCTDDH);
                int donGiaCTDDH;
                int.TryParse(dgvCTDDH.Rows[positionCTDDH].Cells[3].Value.ToString().Trim(), out donGiaCTDDH);
                if (Execute_SPKiemTraCTDDH(MaDDH,MaVT)==1) //0 Tức là không thêm được vì không có vật tư trong chính đơn đặt hàng trong đó,1 là thêm được
                {
                    MessageBox.Show("Mã vật tư đã được sử dụng trong đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                cheDo = "";
                String undoQuery = $"DELETE FROM CTDDH WHERE MasoDDH = N'" +
                $"{MaDDH}" +
                $"' AND MAVT=N'" +
                $"{MaVT}" +
                $"'";
                undoStack.Push(undoQuery);
                this.bdsCTDDH.EndEdit();
            }
            else
            {
                //DataRowView drvMaNV = (DataRowView)bdsDatHang[bdsDatHang.Position];
                //DataRow rowMaNV = drvMaNV.Row;
                //String checkmaNV = rowMaNV["MANV"].ToString().Trim();
                //if (txtMANV.Text.Trim() != checkmaNV)
                //{
                //    MessageBox.Show("Không thể sửa chi tiết đơn đặt hàng của người khác", "Thông báo", MessageBoxButtons.OK);
                //    return;
                //}

                List<String> undoQueryList = new List<String>();
                List<String> deleteQueryList = new List<String>();
                int updateRow = 0;
                while (bdsCTDDH.Count > updateRow)
                {
                    //Lấy dữ liệu cũ
                    
                    DataRowView drvOldRow = (DataRowView)bdsCTDDH[updateRow];
                    DataRow row = drvOldRow.Row;
                    string oldMaVT = row["MAVT", DataRowVersion.Original].ToString().Trim();
                    string oldMaDDH = row["MasoDDH", DataRowVersion.Original].ToString().Trim();
                    int oldSoLuongCTDDH;
                    int.TryParse(row["SOLUONG", DataRowVersion.Original].ToString().Trim(), out oldSoLuongCTDDH);
                    int oldDonGiaCTDDH;
                    int.TryParse(row["DONGIA", DataRowVersion.Original].ToString().Trim(), out oldDonGiaCTDDH);
                    
                    String deleteQuery = "DELETE FROM CTDDH WHERE MasoDDH = N'" + oldMaDDH + "' AND MAVT = N'" + oldMaVT + "'";
                    String undoQuery = $"INSERT INTO [dbo].[CTDDH]\r\n           " +
                        $"([MasoDDH]\r\n           ," +
                        $"[MAVT]\r\n           ," +
                        $"[SOLUONG]\r\n           ," +
                        $"[DONGIA])\r\n     " +
                        $"VALUES\r\n           (" +
                        $"N'{oldMaDDH}'," +
                        $"N'{oldMaVT}'," +
                        $"{oldSoLuongCTDDH}," +
                        $"{oldDonGiaCTDDH})";
                    deleteQueryList.Add(deleteQuery);
                    undoQueryList.Add(undoQuery);
                    bdsCTDDH.MoveNext();
                    updateRow++;
                }
                foreach(String query in undoQueryList)
                {
                    deleteQueryList.Add(query);
                }
                undoStack.Push(deleteQueryList);
            }
            this.cTDDHTableAdapter.Update(this.dS1.CTDDH);
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
            btnThem.Enabled = btnXoa.Enabled = btnGhi.Enabled = btnLamMoi.Enabled = btnThoat.Enabled = true;
            
            
            groupBoxDonDatHang.Enabled=true;
            datHangGridControl.Enabled = true;

            //Context menu Strip
            contextMenuStripDDH.Enabled = true;
            thêmToolStripMenuItem.Enabled = true;
            ghiToolStripMenuItem.Enabled = true;
            xóaToolStripMenuItem.Enabled = true;

            contextMenuStripRowDDH.Enabled = true;
            isAdding = false;
            //xóaVậtTưToolStripMenuItem.Enabled= true;
            btnHoanTac.Enabled = undoStack.Count > 0;
            bdsDatHang.Position = positionDDH;
        }
        private int Execute_SPKiemTraCTDDH(String MasoDDH,String MaVT)
        {
            String query = $"DECLARE\t@return_value int\r\n\r\n" +
                $"EXEC\t@return_value = [dbo].[SP_KiemTraCTDDH]\r\n\t\t" +
                $"@MasoDDH = N'{MasoDDH}',\r\n\t\t@MAVT = N'{MaVT}'\r\n\r\n" +
                $"SELECT\t'Return Value' = @return_value\r\n";
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
                MessageBox.Show("Kiểm tra chi tiết đơn đặt hàng thất bại\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return result;
        }
        private int Execute_SPKiemTraTrangThaiNV(int maNV)
        {
            String query = $"DECLARE @Result INT;\r\nEXEC @Result= SP_KiemTraTrangThaiNV'" +
                $"{maNV}" +
                $"'\r\nSELECT @Result";
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
                MessageBox.Show("Kiểm tra trạng thái nhân viên thất bại\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return result;
        }
        private int Execute_SPKiemTraDDHPhieuNhap(String MasoDDH)
        {
            String query = $"DECLARE @Result int;\r\nEXEC @Result = SP_KiemtraDDHPhieuNhap'" +
                $"{MasoDDH}" +
                $"';\r\nSELECT @Result;";
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
                MessageBox.Show("Kiểm tra trạng thái đơn đặt hàng đã được dùng với phiếu xuất hay chưa đã thất bại\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return result;
        }
        private int Execute_SPKiemTraMDDH(String MasoDHH)
        {
            String query = $"DECLARE @Result INT;\r\nEXEC @Result= SP_KiemTraMDDH N'" +
                $"{MasoDHH}" +
                $"' SELECT @Result";
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
                MessageBox.Show("Kiểm tra trạng thái đơn đặt hàng thất bại\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return result;
        }


        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            cheDo = "";
            String MasoDDH = txtMaDDH.Text.Trim();
            if (bdsCTDDH.Count > 0)
            {
                MessageBox.Show("Không xóa được đơn hàng này vì đã được dùng trong chi tiết đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (Execute_SPKiemTraDDHPhieuNhap(MasoDDH) == 1) //1 tức là không được phép xóa vì MasoDDH đã đc dùng cho với phiếu nhập
            {
                MessageBox.Show("Không xóa được đơn đặt hàng này vì đơn đặt hàng đã được sử dụng cho phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            
            DataRowView drv = ((DataRowView)bdsDatHang[bdsDatHang.Position]);

            String MaNV = drv["MANV"].ToString().Trim();
            String NhaCC = drv["NhaCC"].ToString().Trim();
            String MaDDH = drv["MasoDDH"].ToString().Trim();
            String MaKho = drv["MAKHO"].ToString().Trim();
            if (Program.username.ToString().Trim() == MaNV)
            {
                MessageBox.Show("Không thể cập nhập đơn hàng của nhân viên khác", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            DateTime NgayLap = DateTime.Now;
            if (DateTime.TryParse(drv["NGAY"].ToString(), out NgayLap))
            {

            }
            else
            {
                // Xử lý trường hợp không thể chuyển đổi ngày
                MessageBox.Show("Giá trị trong cột NGAY không phải là kiểu ngày tháng hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //if (program.username != manv)
            //{
            //    messagebox.show("không thể xóa đơn dặt hàng không phải do mình tạo ra");
            //    return;
            //}

            String undoQuery = $"INSERT INTO [dbo].[DatHang]([MasoDDH],[NGAY],[NhaCC],[MANV],[MAKHO])" +
                $"VALUES \r\n " +
                $"('{MaDDH}'\r\n           ," +
                $"'{NgayLap}'\r\n           ," +
                $"'{NhaCC}'\r\n           ," +
                $"'{MaNV}'\r\n           ," +
                $"'{MaKho}')";

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    positionDDH = bdsDatHang.Position;
                    bdsDatHang.RemoveCurrent();
                    this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.datHangTableAdapter.Update(this.dS1.DatHang);
                    //this.datHangTableAdapter.Fill(this.dS1.DatHang);
                    this.datHangTableAdapter.FillBy(this.dS1.DatHang);
                    undoStack.Push(undoQuery);
                    MessageBox.Show("Xóa đơn đặt hàng thành công", "Thông báo", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa đơn đặt hàng", "Thông báo" + ex.Message, MessageBoxButtons.OK);
                }

            }
        }

        private void dgvCTDDH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                //Nếu bấm vào cột không phải dữ liệu thì sẽ không làm gì hết
                return;
            }
            positionCTDDH = e.RowIndex;


        }

        private void xóaVậtTưToolStripMenuItem_Click(object sender, EventArgs e)
        {
            positionDDH = bdsDatHang.Position;
            bool isSameNhanVien = Program.username.ToString().Trim() == txtMANV.Text.ToString().Trim();
            if (isSameNhanVien == false)
            {
                MessageBox.Show("Không thể sửa chi tiết đơn đặt hàng của người khác", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            String MaDDH = dgvCTDDH.Rows[positionCTDDH].Cells[0].Value.ToString().Trim();
            String MaVT = dgvCTDDH.Rows[positionCTDDH].Cells[1].Value.ToString().Trim();
            int soLuongCTDDH;
            int.TryParse(dgvCTDDH.Rows[positionCTDDH].Cells[2].Value.ToString().Trim(), out soLuongCTDDH);
            int donGiaCTDDH;
            int.TryParse(dgvCTDDH.Rows[positionCTDDH].Cells[3].Value.ToString().Trim(), out donGiaCTDDH);
            if (Execute_SPKiemTraDDHPhieuNhap(MaDDH) == 1) //1 tức là không được phép xóa vì MasoDDH đã đc dùng cho với phiếu nhập
            {
                MessageBox.Show("Không xóa được đơn đặt hàng này vì đơn đặt hàng đã được sử dụng cho phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            //cheDo = "CTDDH";
            String undoQuery = $"INSERT INTO [dbo].[CTDDH]\r\n           " +
                $"([MasoDDH]\r\n           ," +
                $"[MAVT]\r\n           ," +
                $"[SOLUONG]\r\n           ," +
                $"[DONGIA]\r\n           )" +
                $"VALUES\r\n           (" +
                $"N'{MaDDH}'," +
                $"N'{MaVT}'," +
                $"{soLuongCTDDH}," +
                $"{donGiaCTDDH}" +
                ")";
            bdsCTDDH.RemoveCurrent();
            this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
            this.datHangTableAdapter.Update(this.dS1.DatHang);
            //this.datHangTableAdapter.Fill(this.dS1.DatHang);
            this.datHangTableAdapter.FillBy(this.dS1.DatHang);
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTDDHTableAdapter.Update(this.dS1.CTDDH);
            this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
            undoStack.Push(undoQuery);
            btnHoanTac.Enabled = undoStack.Count>0;
            MessageBox.Show("Xóa đơn đặt hàng thành công", "Thông báo", MessageBoxButtons.OK);
            positionDDH = bdsDatHang.Position;
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
                MessageBox.Show("Lỗi kết nối tới chi nhánh", "Thông báo", MessageBoxButtons.OK);
            }
            this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
            //this.datHangTableAdapter.Fill(this.dS1.DatHang);
            this.datHangTableAdapter.FillBy(this.dS1.DatHang);
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);
            bdsDatHang.Position = positionDDH;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
