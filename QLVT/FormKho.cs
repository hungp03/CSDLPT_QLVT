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
    public partial class FormKho : Form
    {
        // Tương tự form nhân viên, định nghĩa các biến vị trí và trạng thái thêm, cùng với stack undo
        int position = 0;
        bool isAdding = false;
        string maCN = "";

        Stack undoStack = new Stack();

        private void ThongBao(string mess)
        {
            MessageBox.Show(mess, "Thông báo", MessageBoxButtons.OK);
        }

        public FormKho()
        {
            InitializeComponent();
        }


        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void khoBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsKho.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS1);

        }

        private void FormKho_Load(object sender, EventArgs e)
        {
          
            dS1.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS1.DatHang' table. You can move, or remove it, as needed.
            this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
            this.datHangTableAdapter.Fill(this.dS1.DatHang);
            // TODO: This line of code loads data into the 'dS1.PhieuXuat' table. You can move, or remove it, as needed.
            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.conStr;
            this.phieuXuatTableAdapter.Fill(this.dS1.PhieuXuat);
            // TODO: This line of code loads data into the 'dS1.PhieuNhap' table. You can move, or remove it, as needed.
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
            this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);

            this.khoTableAdapter.Connection.ConnectionString = Program.conStr;
            this.khoTableAdapter.Fill(this.dS1.Kho);
            //???
            maCN = ((DataRowView)bdsKho[0])["MACN"].ToString();
            cbChiNhanh.DataSource = Program.bindingSource;
            cbChiNhanh.DisplayMember = "TENCN";
            cbChiNhanh.ValueMember = "TENSERVER";
            cbChiNhanh.SelectedIndex = Program.brand;

            // Phân quyền nhóm CONGTY chỉ xem được dữ liệu từ cả 2 chi nhánh
            if (Program.mGroup.Equals("CONGTY"))
            {
                btnThem.Enabled = false;
                btnXoa.Enabled = false;
                btnGhi.Enabled = false;
                btnHoantac.Enabled = false;
                panelControl2.Enabled = false;
            }

            // Phân quyền nhóm CHINHANH - USER không được phép đổi sang chi nhánh khác
            // nhưng được phép thao tác CRUD trong chi nhánh của mình
            if (Program.mGroup.Equals("CHINHANH") || Program.mGroup.Equals("USER"))
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
            if (cbChiNhanh.SelectedIndex != Program.brand)
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
                ThongBao("Lỗi kết nối tới chi nhánh");
            }
            else
            {
                //Đổ data từ DS vào TA
                this.datHangTableAdapter.Connection.ConnectionString = Program.conStr;
                this.datHangTableAdapter.Fill(this.dS1.DatHang);

                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.conStr;
                this.phieuNhapTableAdapter.Fill(this.dS1.PhieuNhap);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.conStr;
                this.phieuXuatTableAdapter.Fill(this.dS1.PhieuXuat);

                this.khoTableAdapter.Connection.ConnectionString = Program.conStr;
                this.khoTableAdapter.Fill(dS1.Kho);
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            position = bdsKho.Position;
            panelControl2.Enabled = true;
            isAdding = true;
            bdsKho.AddNew();
            txtMacn.Text = maCN;
            txtMakho.Enabled = true;
            btnHoantac.Enabled = true;
            btnThoat.Enabled = btnLammoi.Enabled = btnXoa.Enabled = btnThem.Enabled = false;
            khoGridControl.Enabled = false;
            panelControl2.Enabled = true;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bdsKho.Count <= 0)
            {
                btnXoa.Enabled = false;
            }

            if (bdsDatHang.Count > 0)
            {
                ThongBao("Không thể xóa kho hàng do đã có đơn đặt hàng");
                return;
            }

            if (bdsPhieuNhap.Count > 0)
            {
                ThongBao("Không thể xóa kho hàng do đã có phiếu nhập");
                return;
            }

            if (bdsDatHang.Count > 0)
            {
                ThongBao("Không thể xóa kho hàng do đã có phiếu xuất");
                return;
            }

            string undoQuery = "INSERT INTO DBO.KHO( MAKHO,TENKHO,DIACHI,MACN) " +
                " VALUES( N'" + txtMakho.Text + "',N'" +
                        txtTenkho.Text + "',N'" +
                        txtDiachi.Text + "', N'" +
                        txtMacn.Text.Trim() + "' ) ";

            undoStack.Push(undoQuery);

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa kho này?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    position = bdsKho.Position;
                    bdsKho.RemoveCurrent();

                    this.khoTableAdapter.Connection.ConnectionString = Program.conStr;
                    this.khoTableAdapter.Update(this.dS1.Kho);

                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    btnHoantac.Enabled = true;
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Có lỗi xảy ra. Thử lại sau\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.khoTableAdapter.Fill(this.dS1.Kho);
                    bdsKho.Position = position;
                    return;
                }
            }
            // Không xác nhận xóa => bỏ câu hoàn tác
            else
            {
                undoStack.Pop();
            }
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!validateInput())
            {
                return;
            }

            string maKho = txtMakho.Text.Trim();
            DataRowView drv = (DataRowView)bdsKho[bdsKho.Position];
            string tenKhoHang = drv["TENKHO"].ToString();
            string diaChi = drv["DIACHI"].ToString();

            string query = "DECLARE @result int " +
                           "EXEC @result = [dbo].[SP_KiemtraMaKho] @MAKHO = '" + maKho + "' " +
                           "SELECT @result";
            _ = new SqlCommand(query, Program.conn);
            try
            {
                if (Program.myReader != null && !Program.myReader.IsClosed)
                {
                    Program.myReader.Close();
                }

                Program.myReader = Program.ExecSqlDataReader(query);
                if (Program.myReader == null)
                {
                    return;
                }

                Program.myReader.Read();
                int res = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();

                int pointerPosition = bdsKho.Position;
                int khoPosition = bdsKho.Find("MAKHO", txtMakho.Text);
                // Xử lý 4 trường hợp
                /*TH1: result = 1 && pointerPosition != khoPosition->Thêm mới nhưng MAKHO đã tồn tại
                TH2: result = 1 && pointerPosition == khoPosition->Sửa kho đang tồn tại
                TH3: result = 0 && pointerPosition == khoPosition->Thêm mới bình thường
                TH4: result = 0 && pointerPosition != khoPosition->Thêm mới bình thường*/

                //TH1
                if (res == 1 && pointerPosition != khoPosition)
                {
                    ThongBao("Mã kho đã được sử dụng");
                    return;
                }

                //TH2,3,4
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        string undoQuery = "";
                        if (isAdding == true)
                        {
                            undoQuery = "DELETE DBO.KHO WHERE MAKHO = '" + txtMakho.Text.Trim() + "'";
                        }
                        else
                        {
                            undoQuery = "UPDATE DBO.KHO set TENKHO = N'" + tenKhoHang + "', DIACHI = N'" + diaChi + "' WHERE MAKHO = '" + maKho + "'";
                        }
                        undoStack.Push(undoQuery);

                        // Kết thúc chỉnh sửa và cập nhật dữ liệu
                        bdsKho.EndEdit();
                        khoTableAdapter.Update(this.dS1.Kho);

                        isAdding = false;
                        ThongBao("Ghi dữ liệu thành công");

                        btnThem.Enabled = true;
                        btnXoa.Enabled = true;
                        btnLammoi.Enabled = true;
                        btnHoantac.Enabled = true;
                        btnThoat.Enabled = true;
                        khoGridControl.Enabled = true;
                        txtMakho.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        bdsKho.RemoveCurrent();
                        ThongBao("Có lỗi xảy ra:\n" + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ThongBao("Thực thi DB thất bại\n" + ex.Message);
            }
            finally
            {
                if (Program.myReader != null && !Program.myReader.IsClosed)
                {
                    Program.myReader.Close();
                }
            }
        }

        private void btnLammoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.khoTableAdapter.Fill(dS1.Kho);
            }
            catch (Exception ex)
            {
                ThongBao("Lỗi khi làm mới dữ liệu\n" + ex.Message);
                return;
            }
        }
        private bool ValidateInput1(DevExpress.XtraEditors.TextEdit textBox, string fieldName, string pattern, int maxLength)
        {
            if (textBox.Text.Trim() == "")
            {
                ThongBao($"Không bỏ trống {fieldName}");
                textBox.Focus();
                return false;
            }

            if (Regex.IsMatch(textBox.Text.Trim(), pattern) == false)
            {
                ThongBao($"{fieldName} chỉ chấp nhận chữ và số");
                textBox.Focus();
                return false;
            }

            if (textBox.Text.Length > maxLength)
            {
                ThongBao($"{fieldName} không thể lớn hơn {maxLength} kí tự");
                textBox.Focus();
                return false;
            }

            return true;
        }

        private bool validateInput()
        {
            if (!ValidateInput1(txtMakho, "Mã kho hàng", @"^[a-zA-Z0-9]+$", 4)) return false;
            if (!ValidateInput1(txtTenkho, "Tên kho hàng", @"^[\p{L}\p{N}, .!?-]+$", 30)) return false;
            if (!ValidateInput1(txtDiachi, "Địa chỉ kho hàng", @"^[\p{L}\p{N}, .!?-]+$", 100)) return false;

            return true;
        }

        private void btnHoantac_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (isAdding == true && btnThem.Enabled == false)
            {
                isAdding = false;
                btnThem.Enabled = true;
                btnXoa.Enabled = true;
                btnHoantac.Enabled = false;
                btnLammoi.Enabled = true;
                btnThoat.Enabled = true;
                txtMakho.Enabled = false;
                khoGridControl.Enabled = true;
                panelControl2.Enabled = true;

                bdsKho.CancelEdit();

                bdsKho.Position = position;
                return;
            }

            if (undoStack.Count == 0)
            {
                ThongBao("Không có thao tác để khôi phục");
                btnHoantac.Enabled = false;
                return;
            }

            bdsKho.CancelEdit();
            string undoQuery = undoStack.Pop().ToString();
            _ = Program.ExecSqlNonQuery(undoQuery);
            bdsKho.Position = position;
            this.khoTableAdapter.Fill(dS1.Kho);
        }
    }
}
