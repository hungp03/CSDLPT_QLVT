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
    public partial class FormVatTu : Form
    {
        // Tương tự như form nhân viên, khởi tạo các biến position(vị trí trên GC), isAdding
        int position = 0;
        bool isAdding = false;

        //Undo -> dùng để hoàn tác dữ liệu nếu lỡ có thao tác không mong muốn
        Stack undoStack = new Stack();

        private void ThongBao(string mess)
        {
            MessageBox.Show(mess, "Thông báo", MessageBoxButtons.OK);
        }

        public FormVatTu()
        {
            InitializeComponent();
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVatTu.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS1);

        }

        private void FormVatTu_Load(object sender, EventArgs e)
        {
            // Không cần kiểm tra khóa ngoại
            dS1.EnforceConstraints = false;

            this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTDDHTableAdapter.Fill(this.dS1.CTDDH);

            this.cTPXTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTPXTableAdapter.Fill(this.dS1.CTPX);

            this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
            this.cTPNTableAdapter.Fill(this.dS1.CTPN);

            // TODO: This line of code loads data into the 'dS1.Vattu' table. You can move, or remove it, as needed.
            this.vattuTableAdapter.Connection.ConnectionString = Program.conStr;
            this.vattuTableAdapter.Fill(this.dS1.Vattu);

            //Đổ dữ liệu chi nhánh vào cmb (trong hàm lấy dspm ở form login)
            cbChiNhanh.DataSource = Program.bindingSource;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            //Lấy mã chi nhánh hiện tại và để cmb hiển thị mặc định
            cbChiNhanh.SelectedIndex = Program.brand;
            //Console.WriteLine("VT-CN: " + Program.brand);

            // Phân quyền CONGTY chỉ xem dữ liệu
            if (Program.mGroup == "CONGTY")
            {
                btnThem.Enabled = false;
                btnXoa.Enabled = false;
                btnGhi.Enabled = false;
                btnHoantac.Enabled = false;
                panelControl2.Enabled = false;
            }

            // Phân quyền nhóm CHINHANH - USER có thể thao tác
            // trong chi nhánh của mình nhưng không được chuyển chi nhánh khác
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                cbChiNhanh.Enabled = false;
            }

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

            // kiểm tra kết nối tới server cần truy xuất dữ liệu
            if (Program.connectDB() == 0)
            {
                ThongBao("Lỗi kết nối tới chi nhánh");
            }
            else
            {
                // Đổ dữ liệu lấy được từ DataSet vào TA
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTDDHTableAdapter.Fill(dS1.CTDDH);

                this.cTPNTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTPNTableAdapter.Fill(dS1.CTPN);

                this.cTPXTableAdapter.Connection.ConnectionString = Program.conStr;
                this.cTPXTableAdapter.Fill(dS1.CTPX);

                this.vattuTableAdapter.Connection.ConnectionString = Program.conStr;
                this.vattuTableAdapter.Fill(dS1.Vattu);
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            position = bdsVatTu.Position;
            // Mở panel nhập liệu
            panelControl2.Enabled = true;
            // Cập nhật trạng thái đang thêm
            isAdding = true;

            // Tự động nhảy xuống dưới 1 hang trong bds
            bdsVatTu.AddNew();
            // Gắn giá trị mặc định cho số lượng tồn là 1
            txtSLT.Value = 1;

            // Thay đổi bật/tắt các nút
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            txtMaVT.Enabled = true;
            btnGhi.Enabled = true;
            btnHoantac.Enabled = true;
            btnLammoi.Enabled = false;
            btnThoat.Enabled = false;

            // Vô hiệu tạm thời GC
            vattuGridControl.Enabled = false;
        }
        // Kiểm tra đầu vào
        private bool validateInput()
        {
            return validate1(txtMaVT, "Mã VT", 4, @"^[a-zA-Z0-9]+$") &&
                   validate1(txtTenVT, "Tên VT", 30, @"^[\p{L}\p{N}, ]+$") &&
                   validate1(txtDVT, "Đơn vị tính", 15, @"^[\p{L}\p{N}, ]+$") &&
                   checkSLT();
        }

        private bool validate1(DevExpress.XtraEditors.TextEdit txt, string fieldName, int maxLength, string pattern)
        {
            if (txt.Text.Trim() == "")
            {
                ThongBao("Không được bỏ trống " + fieldName);
                txt.Focus();
                return false;
            }

            if (!Regex.IsMatch(txt.Text.Trim(), pattern))
            {
                ThongBao(fieldName + " chỉ chấp nhận chữ, số và khoảng trắng");
                txt.Focus();
                return false;
            }

            if (txt.Text.Length > maxLength)
            {
                ThongBao(fieldName + " không quá " + maxLength + " kí tự");
                txt.Focus();
                return false;
            }

            return true;
        }

        private bool checkSLT()
        {
            if (txtSLT.Value < 0)
            {
                ThongBao("Số lượng tồn phải ít nhất bằng 0");
                txtSLT.Focus();
                return false;
            }

            return true;
        }
        private bool kiemTraVT_CNkhac(string maVT)
        {
            string query = "DECLARE @res int\n" + "EXEC @res = [dbo].[SP_KiemtraVT_CNkhac] @MAVT = '" + maVT + "'\nSELECT @res";
            SqlCommand sqlCommand = new SqlCommand(query, Program.conn);

            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);
                if (Program.myReader == null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Console.WriteLine(ex.Message);
                return true;
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            return (result == 1);
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bdsVatTu.Count == 0)
            {
                btnXoa.Enabled = false;
            }

            if (bdsCTDDH.Count > 0)
            {
                ThongBao("Không thể xóa vật tư đã có trong đơn đặt hàng");
                return;
            }

            if (bdsCTPN.Count > 0)
            {
                ThongBao("Không thể xóa vật tư đã trong phiếu nhập");
                return;
            }

            if (bdsCTPX.Count > 0)
            {
                ThongBao("Không thể xóa vật tư có trong phiếu xuất");
                return;
            }

            string maVT = txtMaVT.Text.Trim();
            if (kiemTraVT_CNkhac(maVT))
            {
                ThongBao("Không thể xóa vật tư được dùng ở chi nhánh khác");
                return;
            }

            // Lưu truy vẫn hoàn tác và đẩy vào undoStack
            string undoQuery = "INSERT INTO DBO.Vattu (MAVT, TENVT, DVT, SOLUONGTON) VALUES('" + maVT + "', N'" + txtTenVT.Text + "', N'" + txtDVT.Text + "', " + txtSLT.Value + ")";
            undoStack.Push(undoQuery);
            if (MessageBox.Show("Bạn có muốn xóa vật tư này không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    position = bdsVatTu.Position;
                    bdsVatTu.RemoveCurrent();

                    this.vattuTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.vattuTableAdapter.Update(this.dS1.Vattu);
                    ThongBao("Đã xóa vật tư");
                    btnHoantac.Enabled = true;
                }
                catch (Exception ex)
                {
                    ThongBao("Lỗi khi xóa vật tư\n" + ex.Message);
                    //Console.WriteLine(ex.Message);
                    this.vattuTableAdapter.Fill(this.dS1.Vattu);
                    bdsVatTu.Position = position;
                    return;
                }
            }
            else
            {
                undoStack.Pop();
            }
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Kiểm tra đầu vào
            if (!validateInput())
            {
                return;
            }

            // Lấy dữ liệu trước khi ghi để đổ vào undoStack
            string maVT = txtMaVT.Text.Trim();
            DataRowView drv = (DataRowView)bdsVatTu[bdsVatTu.Position];
            string tenVT = drv["TENVT"].ToString();
            string dvt = drv["DVT"].ToString();
            string slt = drv["SOLUONGTON"].ToString();

            string query = "declare @res int\n" + "exec @res = SP_KiemtraVT '" + maVT + "'\nselect @res";
            SqlCommand sqlCommand = new SqlCommand(query, Program.conn);

            try
            {
                Program.myReader = Program.ExecSqlDataReader(query);
                if (Program.myReader == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ThongBao("Lỗi thực thi database, hãy thử lại sau.");
                Console.WriteLine(ex.Message);
                return;
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            //Console.WriteLine("VT - My readaer[0] result: " + result);
            Program.myReader.Close();

            //Sử dụng kết quả bước trên và vị trí của txtMavt => các trường hợp xảy ra
            /*TH1: result = 1 && pointerPosition != vtPosition->Thêm mới nhưng MAVT đã tồn tại
            TH2: result = 1 && pointerPosition == vtPosition->Sửa vật tư đang tồn tại
            TH3: result = 0 && pointerPosition == vtPosition->Thêm mới bình thường
            TH4: result = 0 && pointerPosition != vtPosition->Thêm mới bình thường*/

            int pointerPosition = bdsVatTu.Position;
            int vtPosition = bdsVatTu.Find("MAVT", txtMaVT.Text);

            // TH1
            if (result == 1 && pointerPosition != vtPosition)
            {
                ThongBao("Mã VT đã được dùng");
                return;
            }
            else
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        btnThem.Enabled = true;
                        btnXoa.Enabled = true;
                        btnGhi.Enabled = true;
                        btnHoantac.Enabled = true;
                        btnLammoi.Enabled = true;
                        btnThoat.Enabled = true;

                        txtMaVT.Enabled = false;
                        vattuGridControl.Enabled = true;

                        // Thêm thì tạo truy vấn xóa để hoàn tác
                        string undoQuery = "";
                        // Nếu bấm thêm trước khi ghi
                        if (isAdding == true)
                        {
                            undoQuery = "DELETE DBO.Vattu WHERE MAVT = '" + txtMaVT.Text.Trim() + "'";
                        }
                        // Ngược lại, nếu sửa thì tạo câu truy vẫn update lại như cũ
                        else
                        {
                            undoQuery = "UPDATE DBO.Vattu SET " +
                                "TENVT = N'" + tenVT + "'," +
                                "DVT = N'" + dvt + "'," +
                                "SOLUONGTON = " + slt + " " +
                                "WHERE MAVT = '" + maVT + "'";
                        }
                        undoStack.Push(undoQuery);
                        this.bdsVatTu.EndEdit();
                        this.vattuTableAdapter.Update(this.dS1.Vattu);
                        isAdding = false;
                        ThongBao("Đã ghi dữ liệu thành công");
                    }
                    catch (Exception ex)
                    {
                        bdsVatTu.RemoveCurrent();
                        ThongBao("Có lỗi xảy ra, vui lòng kiểm tra Console");
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
            }
        }

        private void btnHoantac_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Nếu nhấn thêm nhưng chưa ghi
            if (isAdding == true && btnThem.Enabled == false)
            {
                isAdding = false; btnThem.Enabled = true;
                txtMaVT.Enabled = false;
                btnXoa.Enabled = true;

                btnHoantac.Enabled = false;
                btnLammoi.Enabled = true;
                btnThoat.Enabled = true;

                // Mở lại GC
                vattuGridControl.Enabled = true;
                panelControl2.Enabled = true;

                // Hủy thao tác thêm trên bds
                bdsVatTu.CancelEdit();

                // Trở về lúc đầu contro đang đứng
                bdsVatTu.Position = position;
                return;
            }

            // Kiểm tra nếu stack undo không còn thì báo cho người dùng
            if (undoStack.Count == 0)
            {
                btnHoantac.Enabled = false;
                ThongBao("Không còn thao tác để khôi phục");
                return;
            }

            // Nếu stack undo không trống thì lấy ra và khôi phục
            bdsVatTu.CancelEdit();
            string undoQuery = undoStack.Pop().ToString();
            Console.WriteLine(undoQuery);

            if (Program.connectDB() == 0)
            {
                return;
            }
            int tmp = Program.ExecSqlNonQuery(undoQuery);
            this.vattuTableAdapter.Fill(this.dS1.Vattu);
        }

        private void btnLammoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.vattuTableAdapter.Fill(this.dS1.Vattu);
                vattuGridControl.Enabled = true;
            }
            catch (Exception ex)
            {
                ThongBao("Có lỗi khi làm mới dữ liệu");
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}
