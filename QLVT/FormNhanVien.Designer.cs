namespace QLVT
{
    partial class FormNhanVien
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label mANVLabel;
            System.Windows.Forms.Label hOLabel;
            System.Windows.Forms.Label dIACHILabel;
            System.Windows.Forms.Label nGAYSINHLabel;
            System.Windows.Forms.Label mACNLabel;
            System.Windows.Forms.Label trangThaiXoaLabel;
            System.Windows.Forms.Label lUONGLabel;
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnThem = new DevExpress.XtraBars.BarButtonItem();
            this.btnXoa = new DevExpress.XtraBars.BarButtonItem();
            this.btnGhi = new DevExpress.XtraBars.BarButtonItem();
            this.btnHoanTac = new DevExpress.XtraBars.BarButtonItem();
            this.btnLamMoi = new DevExpress.XtraBars.BarButtonItem();
            this.btnChuyenCN = new DevExpress.XtraBars.BarButtonItem();
            this.btnThoat = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dS1 = new QLVT.DS1();
            this.bdsNhanVien = new System.Windows.Forms.BindingSource(this.components);
            this.nhanVienTableAdapter = new QLVT.DS1TableAdapters.NhanVienTableAdapter();
            this.tableAdapterManager = new QLVT.DS1TableAdapters.TableAdapterManager();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbChiNhanh = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nhanVienGridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colMANV = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTEN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDIACHI = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNGAYSINH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLUONG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMACN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTrangThaiXoa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelNhapLieu = new DevExpress.XtraEditors.PanelControl();
            this.txtLuong = new DevExpress.XtraEditors.TextEdit();
            this.checkboxTHXoa = new System.Windows.Forms.CheckBox();
            this.txtMacn = new DevExpress.XtraEditors.TextEdit();
            this.deNgaySinh = new DevExpress.XtraEditors.DateEdit();
            this.txtDiaChi = new DevExpress.XtraEditors.TextEdit();
            this.txtTen = new DevExpress.XtraEditors.TextEdit();
            this.txtHo = new DevExpress.XtraEditors.TextEdit();
            this.txtManv = new DevExpress.XtraEditors.TextEdit();
            this.bdsPhieuXuat = new System.Windows.Forms.BindingSource(this.components);
            this.bdsPhieuNhap = new System.Windows.Forms.BindingSource(this.components);
            this.bdsDatHang = new System.Windows.Forms.BindingSource(this.components);
            this.fKDatHangNhanVienBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.phieuXuatTableAdapter = new QLVT.DS1TableAdapters.PhieuXuatTableAdapter();
            this.phieuNhapTableAdapter = new QLVT.DS1TableAdapters.PhieuNhapTableAdapter();
            this.datHangTableAdapter = new QLVT.DS1TableAdapters.DatHangTableAdapter();
            mANVLabel = new System.Windows.Forms.Label();
            hOLabel = new System.Windows.Forms.Label();
            dIACHILabel = new System.Windows.Forms.Label();
            nGAYSINHLabel = new System.Windows.Forms.Label();
            mACNLabel = new System.Windows.Forms.Label();
            trangThaiXoaLabel = new System.Windows.Forms.Label();
            lUONGLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsNhanVien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nhanVienGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelNhapLieu)).BeginInit();
            this.panelNhapLieu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLuong.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMacn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deNgaySinh.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deNgaySinh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiaChi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtManv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPhieuXuat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPhieuNhap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDatHang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKDatHangNhanVienBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // mANVLabel
            // 
            mANVLabel.AutoSize = true;
            mANVLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mANVLabel.Location = new System.Drawing.Point(95, 66);
            mANVLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            mANVLabel.Name = "mANVLabel";
            mANVLabel.Size = new System.Drawing.Size(96, 18);
            mANVLabel.TabIndex = 0;
            mANVLabel.Text = "Mã nhân viên";
            // 
            // hOLabel
            // 
            hOLabel.AutoSize = true;
            hOLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            hOLabel.Location = new System.Drawing.Point(652, 67);
            hOLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            hOLabel.Name = "hOLabel";
            hOLabel.Size = new System.Drawing.Size(52, 18);
            hOLabel.TabIndex = 2;
            hOLabel.Text = "Họ tên";
            hOLabel.Click += new System.EventHandler(this.hOLabel_Click);
            // 
            // dIACHILabel
            // 
            dIACHILabel.AutoSize = true;
            dIACHILabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dIACHILabel.Location = new System.Drawing.Point(649, 155);
            dIACHILabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dIACHILabel.Name = "dIACHILabel";
            dIACHILabel.Size = new System.Drawing.Size(50, 18);
            dIACHILabel.TabIndex = 6;
            dIACHILabel.Text = "Địa chỉ";
            dIACHILabel.Click += new System.EventHandler(this.dIACHILabel_Click);
            // 
            // nGAYSINHLabel
            // 
            nGAYSINHLabel.AutoSize = true;
            nGAYSINHLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            nGAYSINHLabel.Location = new System.Drawing.Point(95, 155);
            nGAYSINHLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            nGAYSINHLabel.Name = "nGAYSINHLabel";
            nGAYSINHLabel.Size = new System.Drawing.Size(72, 18);
            nGAYSINHLabel.TabIndex = 8;
            nGAYSINHLabel.Text = "Ngày sinh";
            // 
            // mACNLabel
            // 
            mACNLabel.AutoSize = true;
            mACNLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mACNLabel.Location = new System.Drawing.Point(1277, 67);
            mACNLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            mACNLabel.Name = "mACNLabel";
            mACNLabel.Size = new System.Drawing.Size(95, 18);
            mACNLabel.TabIndex = 12;
            mACNLabel.Text = "Mã chi nhánh";
            // 
            // trangThaiXoaLabel
            // 
            trangThaiXoaLabel.AutoSize = true;
            trangThaiXoaLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            trangThaiXoaLabel.Location = new System.Drawing.Point(1277, 154);
            trangThaiXoaLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            trangThaiXoaLabel.Name = "trangThaiXoaLabel";
            trangThaiXoaLabel.Size = new System.Drawing.Size(104, 18);
            trangThaiXoaLabel.TabIndex = 14;
            trangThaiXoaLabel.Text = "Trạng thái xóa";
            // 
            // lUONGLabel
            // 
            lUONGLabel.AutoSize = true;
            lUONGLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lUONGLabel.Location = new System.Drawing.Point(95, 291);
            lUONGLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lUONGLabel.Name = "lUONGLabel";
            lUONGLabel.Size = new System.Drawing.Size(48, 18);
            lUONGLabel.TabIndex = 15;
            lUONGLabel.Text = "Lương";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnThem,
            this.btnXoa,
            this.btnGhi,
            this.btnHoanTac,
            this.btnLamMoi,
            this.btnChuyenCN,
            this.btnThoat});
            this.barManager1.MaxItemId = 7;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnThem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnXoa, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnGhi, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnHoanTac, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnLamMoi, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnChuyenCN, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnThoat, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.Text = "Tools";
            // 
            // btnThem
            // 
            this.btnThem.Caption = "Thêm";
            this.btnThem.Id = 0;
            this.btnThem.ImageOptions.SvgImage = global::QLVT.Properties.Resources.actions_addcircled;
            this.btnThem.Name = "btnThem";
            this.btnThem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnThem_ItemClick);
            // 
            // btnXoa
            // 
            this.btnXoa.Caption = "Xóa";
            this.btnXoa.Id = 1;
            this.btnXoa.ImageOptions.SvgImage = global::QLVT.Properties.Resources.actions_trash;
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnXoa_ItemClick);
            // 
            // btnGhi
            // 
            this.btnGhi.Caption = "Ghi";
            this.btnGhi.Id = 2;
            this.btnGhi.ImageOptions.SvgImage = global::QLVT.Properties.Resources.save;
            this.btnGhi.Name = "btnGhi";
            this.btnGhi.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnGhi_ItemClick);
            // 
            // btnHoanTac
            // 
            this.btnHoanTac.Caption = "Hoàn tác";
            this.btnHoanTac.Id = 3;
            this.btnHoanTac.ImageOptions.SvgImage = global::QLVT.Properties.Resources.undo;
            this.btnHoanTac.Name = "btnHoanTac";
            this.btnHoanTac.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHoanTac_ItemClick);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Caption = "Làm mới";
            this.btnLamMoi.Id = 4;
            this.btnLamMoi.ImageOptions.SvgImage = global::QLVT.Properties.Resources.actions_refresh;
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLamMoi_ItemClick);
            // 
            // btnChuyenCN
            // 
            this.btnChuyenCN.Caption = "Chuyển chi nhánh";
            this.btnChuyenCN.Id = 5;
            this.btnChuyenCN.ImageOptions.SvgImage = global::QLVT.Properties.Resources.movepivottable;
            this.btnChuyenCN.Name = "btnChuyenCN";
            this.btnChuyenCN.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChuyenCN_ItemClick);
            // 
            // btnThoat
            // 
            this.btnThoat.Caption = "Thoát";
            this.btnThoat.Id = 6;
            this.btnThoat.ImageOptions.SvgImage = global::QLVT.Properties.Resources.clearheaderandfooter;
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnThoat_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1882, 30);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 753);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1882, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 723);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1882, 30);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 723);
            // 
            // dS1
            // 
            this.dS1.DataSetName = "DS1";
            this.dS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bdsNhanVien
            // 
            this.bdsNhanVien.DataMember = "NhanVien";
            this.bdsNhanVien.DataSource = this.dS1;
            // 
            // nhanVienTableAdapter
            // 
            this.nhanVienTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.ChiNhanhTableAdapter = null;
            this.tableAdapterManager.CTDDHTableAdapter = null;
            this.tableAdapterManager.CTPNTableAdapter = null;
            this.tableAdapterManager.CTPXTableAdapter = null;
            this.tableAdapterManager.DatHangTableAdapter = null;
            this.tableAdapterManager.KhoTableAdapter = null;
            this.tableAdapterManager.NhanVienTableAdapter = this.nhanVienTableAdapter;
            this.tableAdapterManager.PhieuNhapTableAdapter = null;
            this.tableAdapterManager.PhieuXuatTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = QLVT.DS1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.VattuTableAdapter = null;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cbChiNhanh);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1882, 70);
            this.panelControl1.TabIndex = 10;
            // 
            // cbChiNhanh
            // 
            this.cbChiNhanh.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbChiNhanh.FormattingEnabled = true;
            this.cbChiNhanh.Location = new System.Drawing.Point(192, 22);
            this.cbChiNhanh.Margin = new System.Windows.Forms.Padding(4);
            this.cbChiNhanh.Name = "cbChiNhanh";
            this.cbChiNhanh.Size = new System.Drawing.Size(370, 26);
            this.cbChiNhanh.TabIndex = 2;
            this.cbChiNhanh.SelectedIndexChanged += new System.EventHandler(this.cbChiNhanh_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "CHI NHÁNH";
            // 
            // nhanVienGridControl
            // 
            this.nhanVienGridControl.DataSource = this.bdsNhanVien;
            this.nhanVienGridControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.nhanVienGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.nhanVienGridControl.Location = new System.Drawing.Point(0, 100);
            this.nhanVienGridControl.MainView = this.gridView1;
            this.nhanVienGridControl.Margin = new System.Windows.Forms.Padding(4);
            this.nhanVienGridControl.MenuManager = this.barManager1;
            this.nhanVienGridControl.Name = "nhanVienGridControl";
            this.nhanVienGridControl.Size = new System.Drawing.Size(1882, 385);
            this.nhanVienGridControl.TabIndex = 10;
            this.nhanVienGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMANV,
            this.colHO,
            this.colTEN,
            this.colDIACHI,
            this.colNGAYSINH,
            this.colLUONG,
            this.colMACN,
            this.colTrangThaiXoa});
            this.gridView1.DetailHeight = 437;
            this.gridView1.GridControl = this.nhanVienGridControl;
            this.gridView1.Name = "gridView1";
            // 
            // colMANV
            // 
            this.colMANV.Caption = "Mã NV";
            this.colMANV.FieldName = "MANV";
            this.colMANV.MinWidth = 31;
            this.colMANV.Name = "colMANV";
            this.colMANV.Visible = true;
            this.colMANV.VisibleIndex = 0;
            this.colMANV.Width = 117;
            // 
            // colHO
            // 
            this.colHO.Caption = "Họ";
            this.colHO.FieldName = "HO";
            this.colHO.MinWidth = 31;
            this.colHO.Name = "colHO";
            this.colHO.Visible = true;
            this.colHO.VisibleIndex = 1;
            this.colHO.Width = 117;
            // 
            // colTEN
            // 
            this.colTEN.Caption = "Tên";
            this.colTEN.FieldName = "TEN";
            this.colTEN.MinWidth = 31;
            this.colTEN.Name = "colTEN";
            this.colTEN.Visible = true;
            this.colTEN.VisibleIndex = 2;
            this.colTEN.Width = 117;
            // 
            // colDIACHI
            // 
            this.colDIACHI.Caption = "Địa chỉ";
            this.colDIACHI.FieldName = "DIACHI";
            this.colDIACHI.MinWidth = 31;
            this.colDIACHI.Name = "colDIACHI";
            this.colDIACHI.Visible = true;
            this.colDIACHI.VisibleIndex = 3;
            this.colDIACHI.Width = 117;
            // 
            // colNGAYSINH
            // 
            this.colNGAYSINH.Caption = "Ngày sinh";
            this.colNGAYSINH.FieldName = "NGAYSINH";
            this.colNGAYSINH.MinWidth = 31;
            this.colNGAYSINH.Name = "colNGAYSINH";
            this.colNGAYSINH.Visible = true;
            this.colNGAYSINH.VisibleIndex = 4;
            this.colNGAYSINH.Width = 117;
            // 
            // colLUONG
            // 
            this.colLUONG.Caption = "Lương";
            this.colLUONG.FieldName = "LUONG";
            this.colLUONG.MinWidth = 31;
            this.colLUONG.Name = "colLUONG";
            this.colLUONG.Visible = true;
            this.colLUONG.VisibleIndex = 5;
            this.colLUONG.Width = 117;
            // 
            // colMACN
            // 
            this.colMACN.Caption = "Mã chi nhánh";
            this.colMACN.FieldName = "MACN";
            this.colMACN.MinWidth = 31;
            this.colMACN.Name = "colMACN";
            this.colMACN.Visible = true;
            this.colMACN.VisibleIndex = 6;
            this.colMACN.Width = 117;
            // 
            // colTrangThaiXoa
            // 
            this.colTrangThaiXoa.Caption = "Trạng thái";
            this.colTrangThaiXoa.FieldName = "TrangThaiXoa";
            this.colTrangThaiXoa.MinWidth = 31;
            this.colTrangThaiXoa.Name = "colTrangThaiXoa";
            this.colTrangThaiXoa.Visible = true;
            this.colTrangThaiXoa.VisibleIndex = 7;
            this.colTrangThaiXoa.Width = 117;
            // 
            // panelNhapLieu
            // 
            this.panelNhapLieu.Controls.Add(lUONGLabel);
            this.panelNhapLieu.Controls.Add(this.txtLuong);
            this.panelNhapLieu.Controls.Add(trangThaiXoaLabel);
            this.panelNhapLieu.Controls.Add(this.checkboxTHXoa);
            this.panelNhapLieu.Controls.Add(mACNLabel);
            this.panelNhapLieu.Controls.Add(this.txtMacn);
            this.panelNhapLieu.Controls.Add(nGAYSINHLabel);
            this.panelNhapLieu.Controls.Add(this.deNgaySinh);
            this.panelNhapLieu.Controls.Add(dIACHILabel);
            this.panelNhapLieu.Controls.Add(this.txtDiaChi);
            this.panelNhapLieu.Controls.Add(this.txtTen);
            this.panelNhapLieu.Controls.Add(hOLabel);
            this.panelNhapLieu.Controls.Add(this.txtHo);
            this.panelNhapLieu.Controls.Add(mANVLabel);
            this.panelNhapLieu.Controls.Add(this.txtManv);
            this.panelNhapLieu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNhapLieu.Location = new System.Drawing.Point(0, 485);
            this.panelNhapLieu.Margin = new System.Windows.Forms.Padding(4);
            this.panelNhapLieu.Name = "panelNhapLieu";
            this.panelNhapLieu.Size = new System.Drawing.Size(1882, 268);
            this.panelNhapLieu.TabIndex = 11;
            // 
            // txtLuong
            // 
            this.txtLuong.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsNhanVien, "LUONG", true));
            this.txtLuong.Location = new System.Drawing.Point(229, 289);
            this.txtLuong.Margin = new System.Windows.Forms.Padding(4);
            this.txtLuong.MenuManager = this.barManager1;
            this.txtLuong.Name = "txtLuong";
            this.txtLuong.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLuong.Properties.Appearance.Options.UseFont = true;
            this.txtLuong.Properties.DisplayFormat.FormatString = "n0";
            this.txtLuong.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtLuong.Properties.EditFormat.FormatString = "n0";
            this.txtLuong.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtLuong.Size = new System.Drawing.Size(239, 24);
            this.txtLuong.TabIndex = 16;
            // 
            // checkboxTHXoa
            // 
            this.checkboxTHXoa.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.bdsNhanVien, "TrangThaiXoa", true));
            this.checkboxTHXoa.Location = new System.Drawing.Point(1429, 152);
            this.checkboxTHXoa.Margin = new System.Windows.Forms.Padding(4);
            this.checkboxTHXoa.Name = "checkboxTHXoa";
            this.checkboxTHXoa.Size = new System.Drawing.Size(130, 30);
            this.checkboxTHXoa.TabIndex = 15;
            this.checkboxTHXoa.UseVisualStyleBackColor = true;
            // 
            // txtMacn
            // 
            this.txtMacn.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsNhanVien, "MACN", true));
            this.txtMacn.Location = new System.Drawing.Point(1421, 64);
            this.txtMacn.Margin = new System.Windows.Forms.Padding(4);
            this.txtMacn.MenuManager = this.barManager1;
            this.txtMacn.Name = "txtMacn";
            this.txtMacn.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMacn.Properties.Appearance.Options.UseFont = true;
            this.txtMacn.Size = new System.Drawing.Size(156, 24);
            this.txtMacn.TabIndex = 13;
            // 
            // deNgaySinh
            // 
            this.deNgaySinh.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsNhanVien, "NGAYSINH", true));
            this.deNgaySinh.EditValue = null;
            this.deNgaySinh.Location = new System.Drawing.Point(229, 151);
            this.deNgaySinh.Margin = new System.Windows.Forms.Padding(4);
            this.deNgaySinh.MenuManager = this.barManager1;
            this.deNgaySinh.Name = "deNgaySinh";
            this.deNgaySinh.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deNgaySinh.Properties.Appearance.Options.UseFont = true;
            this.deNgaySinh.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deNgaySinh.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deNgaySinh.Size = new System.Drawing.Size(239, 24);
            this.deNgaySinh.TabIndex = 9;
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsNhanVien, "DIACHI", true));
            this.txtDiaChi.Location = new System.Drawing.Point(793, 152);
            this.txtDiaChi.Margin = new System.Windows.Forms.Padding(4);
            this.txtDiaChi.MenuManager = this.barManager1;
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiaChi.Properties.Appearance.Options.UseFont = true;
            this.txtDiaChi.Size = new System.Drawing.Size(398, 24);
            this.txtDiaChi.TabIndex = 7;
            this.txtDiaChi.EditValueChanged += new System.EventHandler(this.txtDiaChi_EditValueChanged);
            // 
            // txtTen
            // 
            this.txtTen.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsNhanVien, "TEN", true));
            this.txtTen.Location = new System.Drawing.Point(972, 63);
            this.txtTen.Margin = new System.Windows.Forms.Padding(4);
            this.txtTen.MenuManager = this.barManager1;
            this.txtTen.Name = "txtTen";
            this.txtTen.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTen.Properties.Appearance.Options.UseFont = true;
            this.txtTen.Size = new System.Drawing.Size(156, 24);
            this.txtTen.TabIndex = 5;
            this.txtTen.EditValueChanged += new System.EventHandler(this.txtTen_EditValueChanged);
            // 
            // txtHo
            // 
            this.txtHo.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsNhanVien, "HO", true));
            this.txtHo.Location = new System.Drawing.Point(793, 63);
            this.txtHo.Margin = new System.Windows.Forms.Padding(4);
            this.txtHo.MenuManager = this.barManager1;
            this.txtHo.Name = "txtHo";
            this.txtHo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHo.Properties.Appearance.Options.UseFont = true;
            this.txtHo.Size = new System.Drawing.Size(156, 24);
            this.txtHo.TabIndex = 3;
            this.txtHo.EditValueChanged += new System.EventHandler(this.txtHo_EditValueChanged);
            // 
            // txtManv
            // 
            this.txtManv.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsNhanVien, "MANV", true));
            this.txtManv.Location = new System.Drawing.Point(229, 62);
            this.txtManv.Margin = new System.Windows.Forms.Padding(4);
            this.txtManv.MenuManager = this.barManager1;
            this.txtManv.Name = "txtManv";
            this.txtManv.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtManv.Properties.Appearance.Options.UseFont = true;
            this.txtManv.Size = new System.Drawing.Size(239, 24);
            this.txtManv.TabIndex = 1;
            // 
            // bdsPhieuXuat
            // 
            this.bdsPhieuXuat.DataMember = "FK_PX_NhanVien";
            this.bdsPhieuXuat.DataSource = this.bdsNhanVien;
            // 
            // bdsPhieuNhap
            // 
            this.bdsPhieuNhap.DataMember = "FK_PhieuNhap_NhanVien";
            this.bdsPhieuNhap.DataSource = this.bdsNhanVien;
            // 
            // bdsDatHang
            // 
            this.bdsDatHang.DataSource = this.fKDatHangNhanVienBindingSource;
            // 
            // fKDatHangNhanVienBindingSource
            // 
            this.fKDatHangNhanVienBindingSource.DataMember = "FK_DatHang_NhanVien";
            this.fKDatHangNhanVienBindingSource.DataSource = this.bdsNhanVien;
            // 
            // phieuXuatTableAdapter
            // 
            this.phieuXuatTableAdapter.ClearBeforeFill = true;
            // 
            // phieuNhapTableAdapter
            // 
            this.phieuNhapTableAdapter.ClearBeforeFill = true;
            // 
            // datHangTableAdapter
            // 
            this.datHangTableAdapter.ClearBeforeFill = true;
            // 
            // FormNhanVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1882, 753);
            this.Controls.Add(this.panelNhapLieu);
            this.Controls.Add(this.nhanVienGridControl);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNhanVien";
            this.Text = "Nhân viên";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormNhanVien_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsNhanVien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nhanVienGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelNhapLieu)).EndInit();
            this.panelNhapLieu.ResumeLayout(false);
            this.panelNhapLieu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLuong.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMacn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deNgaySinh.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deNgaySinh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiaChi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtManv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPhieuXuat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPhieuNhap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDatHang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKDatHangNhanVienBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnThem;
        private DevExpress.XtraBars.BarButtonItem btnXoa;
        private DevExpress.XtraBars.BarButtonItem btnGhi;
        private DevExpress.XtraBars.BarButtonItem btnHoanTac;
        private DevExpress.XtraBars.BarButtonItem btnLamMoi;
        private DevExpress.XtraBars.BarButtonItem btnChuyenCN;
        private DevExpress.XtraBars.BarButtonItem btnThoat;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.BindingSource bdsNhanVien;
        private DS1 dS1;
        private DS1TableAdapters.NhanVienTableAdapter nhanVienTableAdapter;
        private DS1TableAdapters.TableAdapterManager tableAdapterManager;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbChiNhanh;
        private DevExpress.XtraEditors.PanelControl panelNhapLieu;
        private System.Windows.Forms.CheckBox checkboxTHXoa;
        private DevExpress.XtraEditors.TextEdit txtMacn;
        private DevExpress.XtraEditors.DateEdit deNgaySinh;
        private DevExpress.XtraEditors.TextEdit txtDiaChi;
        private DevExpress.XtraEditors.TextEdit txtTen;
        private DevExpress.XtraEditors.TextEdit txtHo;
        private DevExpress.XtraEditors.TextEdit txtManv;
        private DevExpress.XtraGrid.GridControl nhanVienGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colMANV;
        private DevExpress.XtraGrid.Columns.GridColumn colHO;
        private DevExpress.XtraGrid.Columns.GridColumn colTEN;
        private DevExpress.XtraGrid.Columns.GridColumn colDIACHI;
        private DevExpress.XtraGrid.Columns.GridColumn colNGAYSINH;
        private DevExpress.XtraGrid.Columns.GridColumn colLUONG;
        private DevExpress.XtraGrid.Columns.GridColumn colMACN;
        private DevExpress.XtraGrid.Columns.GridColumn colTrangThaiXoa;
        private DevExpress.XtraEditors.TextEdit txtLuong;
        private System.Windows.Forms.BindingSource bdsPhieuXuat;
        private System.Windows.Forms.BindingSource bdsPhieuNhap;
        private System.Windows.Forms.BindingSource bdsDatHang;
        private DS1TableAdapters.PhieuXuatTableAdapter phieuXuatTableAdapter;
        private DS1TableAdapters.PhieuNhapTableAdapter phieuNhapTableAdapter;
        private System.Windows.Forms.BindingSource fKDatHangNhanVienBindingSource;
        private DS1TableAdapters.DatHangTableAdapter datHangTableAdapter;
    }
}