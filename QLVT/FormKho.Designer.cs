namespace QLVT
{
    partial class FormKho
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
            System.Windows.Forms.Label mAKHOLabel;
            System.Windows.Forms.Label tENKHOLabel;
            System.Windows.Forms.Label dIACHILabel;
            System.Windows.Forms.Label mACNLabel;
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnThem = new DevExpress.XtraBars.BarButtonItem();
            this.btnXoa = new DevExpress.XtraBars.BarButtonItem();
            this.btnGhi = new DevExpress.XtraBars.BarButtonItem();
            this.btnHoantac = new DevExpress.XtraBars.BarButtonItem();
            this.btnLammoi = new DevExpress.XtraBars.BarButtonItem();
            this.btnThoat = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbChiNhanh = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dS1 = new QLVT.DS1();
            this.bdsKho = new System.Windows.Forms.BindingSource(this.components);
            this.khoTableAdapter = new QLVT.DS1TableAdapters.KhoTableAdapter();
            this.tableAdapterManager = new QLVT.DS1TableAdapters.TableAdapterManager();
            this.khoGridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colMAKHO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTENKHO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDIACHI = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMACN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtMacn = new DevExpress.XtraEditors.TextEdit();
            this.txtDiachi = new DevExpress.XtraEditors.TextEdit();
            this.txtTenkho = new DevExpress.XtraEditors.TextEdit();
            this.txtMakho = new DevExpress.XtraEditors.TextEdit();
            this.bdsDatHang = new System.Windows.Forms.BindingSource(this.components);
            this.bdsPhieuNhap = new System.Windows.Forms.BindingSource(this.components);
            this.bdsPhieuXuat = new System.Windows.Forms.BindingSource(this.components);
            this.datHangTableAdapter = new QLVT.DS1TableAdapters.DatHangTableAdapter();
            this.phieuNhapTableAdapter = new QLVT.DS1TableAdapters.PhieuNhapTableAdapter();
            this.phieuXuatTableAdapter = new QLVT.DS1TableAdapters.PhieuXuatTableAdapter();
            mAKHOLabel = new System.Windows.Forms.Label();
            tENKHOLabel = new System.Windows.Forms.Label();
            dIACHILabel = new System.Windows.Forms.Label();
            mACNLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsKho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.khoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMacn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiachi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTenkho.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMakho.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDatHang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPhieuNhap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPhieuXuat)).BeginInit();
            this.SuspendLayout();
            // 
            // mAKHOLabel
            // 
            mAKHOLabel.AutoSize = true;
            mAKHOLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mAKHOLabel.Location = new System.Drawing.Point(188, 47);
            mAKHOLabel.Name = "mAKHOLabel";
            mAKHOLabel.Size = new System.Drawing.Size(56, 18);
            mAKHOLabel.TabIndex = 0;
            mAKHOLabel.Text = "Mã kho";
            // 
            // tENKHOLabel
            // 
            tENKHOLabel.AutoSize = true;
            tENKHOLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tENKHOLabel.Location = new System.Drawing.Point(188, 133);
            tENKHOLabel.Name = "tENKHOLabel";
            tENKHOLabel.Size = new System.Drawing.Size(62, 18);
            tENKHOLabel.TabIndex = 2;
            tENKHOLabel.Text = "Tên kho";
            // 
            // dIACHILabel
            // 
            dIACHILabel.AutoSize = true;
            dIACHILabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dIACHILabel.Location = new System.Drawing.Point(706, 133);
            dIACHILabel.Name = "dIACHILabel";
            dIACHILabel.Size = new System.Drawing.Size(50, 18);
            dIACHILabel.TabIndex = 4;
            dIACHILabel.Text = "Địa chỉ";
            // 
            // mACNLabel
            // 
            mACNLabel.AutoSize = true;
            mACNLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mACNLabel.Location = new System.Drawing.Point(706, 47);
            mACNLabel.Name = "mACNLabel";
            mACNLabel.Size = new System.Drawing.Size(95, 18);
            mACNLabel.TabIndex = 6;
            mACNLabel.Text = "Mã chi nhánh";
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
            this.btnHoantac,
            this.btnLammoi,
            this.btnThoat});
            this.barManager1.MaxItemId = 6;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnHoantac, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnLammoi, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
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
            // btnHoantac
            // 
            this.btnHoantac.Caption = "Hoàn tác";
            this.btnHoantac.Id = 3;
            this.btnHoantac.ImageOptions.SvgImage = global::QLVT.Properties.Resources.undo;
            this.btnHoantac.Name = "btnHoantac";
            this.btnHoantac.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHoantac_ItemClick);
            // 
            // btnLammoi
            // 
            this.btnLammoi.Caption = "Làm mới";
            this.btnLammoi.Id = 4;
            this.btnLammoi.ImageOptions.SvgImage = global::QLVT.Properties.Resources.actions_refresh;
            this.btnLammoi.Name = "btnLammoi";
            this.btnLammoi.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLammoi_ItemClick);
            // 
            // btnThoat
            // 
            this.btnThoat.Caption = "Thoát";
            this.btnThoat.Id = 5;
            this.btnThoat.ImageOptions.SvgImage = global::QLVT.Properties.Resources.clearheaderandfooter;
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
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
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cbChiNhanh);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1882, 70);
            this.panelControl1.TabIndex = 4;
            // 
            // cbChiNhanh
            // 
            this.cbChiNhanh.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbChiNhanh.FormattingEnabled = true;
            this.cbChiNhanh.Location = new System.Drawing.Point(143, 21);
            this.cbChiNhanh.Name = "cbChiNhanh";
            this.cbChiNhanh.Size = new System.Drawing.Size(370, 26);
            this.cbChiNhanh.TabIndex = 1;
            this.cbChiNhanh.SelectedIndexChanged += new System.EventHandler(this.cbChiNhanh_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "CHI NHÁNH";
            // 
            // dS1
            // 
            this.dS1.DataSetName = "DS1";
            this.dS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bdsKho
            // 
            this.bdsKho.DataMember = "Kho";
            this.bdsKho.DataSource = this.dS1;
            // 
            // khoTableAdapter
            // 
            this.khoTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.ChiNhanhTableAdapter = null;
            this.tableAdapterManager.CTDDHTableAdapter = null;
            this.tableAdapterManager.CTPNTableAdapter = null;
            this.tableAdapterManager.CTPXTableAdapter = null;
            this.tableAdapterManager.DatHangTableAdapter = null;
            this.tableAdapterManager.KhoTableAdapter = this.khoTableAdapter;
            this.tableAdapterManager.NhanVienTableAdapter = null;
            this.tableAdapterManager.PhieuNhapTableAdapter = null;
            this.tableAdapterManager.PhieuXuatTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = QLVT.DS1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.VattuTableAdapter = null;
            // 
            // khoGridControl
            // 
            this.khoGridControl.DataSource = this.bdsKho;
            this.khoGridControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.khoGridControl.Location = new System.Drawing.Point(0, 100);
            this.khoGridControl.MainView = this.gridView1;
            this.khoGridControl.MenuManager = this.barManager1;
            this.khoGridControl.Name = "khoGridControl";
            this.khoGridControl.Size = new System.Drawing.Size(1882, 385);
            this.khoGridControl.TabIndex = 6;
            this.khoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMAKHO,
            this.colTENKHO,
            this.colDIACHI,
            this.colMACN});
            this.gridView1.GridControl = this.khoGridControl;
            this.gridView1.Name = "gridView1";
            // 
            // colMAKHO
            // 
            this.colMAKHO.Caption = "Mã kho";
            this.colMAKHO.FieldName = "MAKHO";
            this.colMAKHO.MinWidth = 25;
            this.colMAKHO.Name = "colMAKHO";
            this.colMAKHO.Visible = true;
            this.colMAKHO.VisibleIndex = 0;
            this.colMAKHO.Width = 94;
            // 
            // colTENKHO
            // 
            this.colTENKHO.Caption = "Tên kho";
            this.colTENKHO.FieldName = "TENKHO";
            this.colTENKHO.MinWidth = 25;
            this.colTENKHO.Name = "colTENKHO";
            this.colTENKHO.Visible = true;
            this.colTENKHO.VisibleIndex = 1;
            this.colTENKHO.Width = 94;
            // 
            // colDIACHI
            // 
            this.colDIACHI.Caption = "Địa chỉ";
            this.colDIACHI.FieldName = "DIACHI";
            this.colDIACHI.MinWidth = 25;
            this.colDIACHI.Name = "colDIACHI";
            this.colDIACHI.Visible = true;
            this.colDIACHI.VisibleIndex = 2;
            this.colDIACHI.Width = 94;
            // 
            // colMACN
            // 
            this.colMACN.Caption = "Mã chi nhánh";
            this.colMACN.FieldName = "MACN";
            this.colMACN.MinWidth = 25;
            this.colMACN.Name = "colMACN";
            this.colMACN.Visible = true;
            this.colMACN.VisibleIndex = 3;
            this.colMACN.Width = 94;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(mACNLabel);
            this.panelControl2.Controls.Add(this.txtMacn);
            this.panelControl2.Controls.Add(dIACHILabel);
            this.panelControl2.Controls.Add(this.txtDiachi);
            this.panelControl2.Controls.Add(tENKHOLabel);
            this.panelControl2.Controls.Add(this.txtTenkho);
            this.panelControl2.Controls.Add(mAKHOLabel);
            this.panelControl2.Controls.Add(this.txtMakho);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 485);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1882, 268);
            this.panelControl2.TabIndex = 7;
            // 
            // txtMacn
            // 
            this.txtMacn.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsKho, "MACN", true));
            this.txtMacn.Location = new System.Drawing.Point(813, 44);
            this.txtMacn.MenuManager = this.barManager1;
            this.txtMacn.Name = "txtMacn";
            this.txtMacn.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMacn.Properties.Appearance.Options.UseFont = true;
            this.txtMacn.Size = new System.Drawing.Size(204, 24);
            this.txtMacn.TabIndex = 7;
            // 
            // txtDiachi
            // 
            this.txtDiachi.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsKho, "DIACHI", true));
            this.txtDiachi.Location = new System.Drawing.Point(813, 130);
            this.txtDiachi.MenuManager = this.barManager1;
            this.txtDiachi.Name = "txtDiachi";
            this.txtDiachi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiachi.Properties.Appearance.Options.UseFont = true;
            this.txtDiachi.Size = new System.Drawing.Size(204, 24);
            this.txtDiachi.TabIndex = 5;
            // 
            // txtTenkho
            // 
            this.txtTenkho.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsKho, "TENKHO", true));
            this.txtTenkho.Location = new System.Drawing.Point(253, 130);
            this.txtTenkho.MenuManager = this.barManager1;
            this.txtTenkho.Name = "txtTenkho";
            this.txtTenkho.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenkho.Properties.Appearance.Options.UseFont = true;
            this.txtTenkho.Size = new System.Drawing.Size(180, 24);
            this.txtTenkho.TabIndex = 3;
            // 
            // txtMakho
            // 
            this.txtMakho.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bdsKho, "MAKHO", true));
            this.txtMakho.Location = new System.Drawing.Point(253, 44);
            this.txtMakho.MenuManager = this.barManager1;
            this.txtMakho.Name = "txtMakho";
            this.txtMakho.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMakho.Properties.Appearance.Options.UseFont = true;
            this.txtMakho.Size = new System.Drawing.Size(180, 24);
            this.txtMakho.TabIndex = 1;
            // 
            // bdsDatHang
            // 
            this.bdsDatHang.DataMember = "FK_DatHang_Kho";
            this.bdsDatHang.DataSource = this.bdsKho;
            // 
            // bdsPhieuNhap
            // 
            this.bdsPhieuNhap.DataMember = "FK_PhieuNhap_Kho";
            this.bdsPhieuNhap.DataSource = this.bdsKho;
            // 
            // bdsPhieuXuat
            // 
            this.bdsPhieuXuat.DataMember = "FK_PhieuXuat_Kho";
            this.bdsPhieuXuat.DataSource = this.bdsKho;
            // 
            // datHangTableAdapter
            // 
            this.datHangTableAdapter.ClearBeforeFill = true;
            // 
            // phieuNhapTableAdapter
            // 
            this.phieuNhapTableAdapter.ClearBeforeFill = true;
            // 
            // phieuXuatTableAdapter
            // 
            this.phieuXuatTableAdapter.ClearBeforeFill = true;
            // 
            // FormKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1882, 753);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.khoGridControl);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FormKho";
            this.Text = "Kho hàng";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormKho_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsKho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.khoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMacn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiachi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTenkho.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMakho.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDatHang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPhieuNhap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPhieuXuat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnThem;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnXoa;
        private DevExpress.XtraBars.BarButtonItem btnGhi;
        private DevExpress.XtraBars.BarButtonItem btnHoantac;
        private DevExpress.XtraBars.BarButtonItem btnLammoi;
        private DevExpress.XtraBars.BarButtonItem btnThoat;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ComboBox cbChiNhanh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bdsKho;
        private DS1 dS1;
        private DS1TableAdapters.KhoTableAdapter khoTableAdapter;
        private DS1TableAdapters.TableAdapterManager tableAdapterManager;
        private DevExpress.XtraGrid.GridControl khoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.TextEdit txtMakho;
        private DevExpress.XtraGrid.Columns.GridColumn colMAKHO;
        private DevExpress.XtraGrid.Columns.GridColumn colTENKHO;
        private DevExpress.XtraGrid.Columns.GridColumn colDIACHI;
        private DevExpress.XtraGrid.Columns.GridColumn colMACN;
        private DevExpress.XtraEditors.TextEdit txtMacn;
        private DevExpress.XtraEditors.TextEdit txtDiachi;
        private DevExpress.XtraEditors.TextEdit txtTenkho;
        private System.Windows.Forms.BindingSource bdsDatHang;
        private System.Windows.Forms.BindingSource bdsPhieuNhap;
        private System.Windows.Forms.BindingSource bdsPhieuXuat;
        private DS1TableAdapters.DatHangTableAdapter datHangTableAdapter;
        private DS1TableAdapters.PhieuNhapTableAdapter phieuNhapTableAdapter;
        private DS1TableAdapters.PhieuXuatTableAdapter phieuXuatTableAdapter;
    }
}