namespace net.docmaker{
    partial class frmMain {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mainView = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddProject = new System.Windows.Forms.Button();
            this.btnDetailProject = new System.Windows.Forms.Button();
            this.btnChangeProject = new System.Windows.Forms.Button();
            this.btnGetProject = new System.Windows.Forms.Button();
            this.btnDeleteProject = new System.Windows.Forms.Button();
            this.btnPutProject = new System.Windows.Forms.Button();
            this.btnAddTask = new System.Windows.Forms.Button();
            this.btnTaskDetail = new System.Windows.Forms.Button();
            this.btnTaskChange = new System.Windows.Forms.Button();
            this.btnTaskCopy = new System.Windows.Forms.Button();
            this.btnTaskDelete = new System.Windows.Forms.Button();
            this.lblSeparate1 = new System.Windows.Forms.Label();
            this.btnViewFolder = new System.Windows.Forms.Button();
            this.lblSeparate2 = new System.Windows.Forms.Label();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOfficeMaster = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPersonMaster = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTaskMaster = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProgressMaster = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiUtilities = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScreenResolution = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRepository = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiInquiry = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMember = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHomepage = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCancelSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBackToProjects = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsHighLight = new System.Windows.Forms.ToolStrip();
            this.tbx1 = new System.Windows.Forms.ToolStripTextBox();
            this.tsbSearch = new System.Windows.Forms.ToolStripButton();
            this.cmsTabsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCloseTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChange = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangeProject = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteProject = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPutProject = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.mainView.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tsHighLight.SuspendLayout();
            this.cmsTabsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainView
            // 
            this.mainView.AutoScroll = true;
            this.mainView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mainView.Controls.Add(this.splitContainer1);
            this.mainView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainView.Location = new System.Drawing.Point(0, 0);
            this.mainView.Name = "mainView";
            this.mainView.Size = new System.Drawing.Size(1006, 683);
            this.mainView.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvMain);
            this.splitContainer1.Panel1MinSize = 330;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer1.Panel2MinSize = 30;
            this.splitContainer1.Size = new System.Drawing.Size(1006, 683);
            this.splitContainer1.SplitterDistance = 620;
            this.splitContainer1.TabIndex = 9;
            // 
            // dgvMain
            // 
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMain.Location = new System.Drawing.Point(0, 0);
            this.dgvMain.MultiSelect = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.RowTemplate.Height = 21;
            this.dgvMain.Size = new System.Drawing.Size(1006, 620);
            this.dgvMain.TabIndex = 0;
            this.dgvMain.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMain_CellMouseDoubleClick);
            this.dgvMain.SelectionChanged += new System.EventHandler(this.dgvMain_SelectionChanged);
            this.dgvMain.Sorted += new System.EventHandler(this.dgvMain_Sorted);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnAddProject);
            this.flowLayoutPanel1.Controls.Add(this.btnDetailProject);
            this.flowLayoutPanel1.Controls.Add(this.btnChangeProject);
            this.flowLayoutPanel1.Controls.Add(this.btnGetProject);
            this.flowLayoutPanel1.Controls.Add(this.btnDeleteProject);
            this.flowLayoutPanel1.Controls.Add(this.btnPutProject);
            this.flowLayoutPanel1.Controls.Add(this.btnAddTask);
            this.flowLayoutPanel1.Controls.Add(this.btnTaskDetail);
            this.flowLayoutPanel1.Controls.Add(this.btnTaskChange);
            this.flowLayoutPanel1.Controls.Add(this.btnTaskCopy);
            this.flowLayoutPanel1.Controls.Add(this.btnTaskDelete);
            this.flowLayoutPanel1.Controls.Add(this.lblSeparate1);
            this.flowLayoutPanel1.Controls.Add(this.btnViewFolder);
            this.flowLayoutPanel1.Controls.Add(this.lblSeparate2);
            this.flowLayoutPanel1.Controls.Add(this.btnQuit);
            this.flowLayoutPanel1.Controls.Add(this.btnExit);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1006, 55);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // btnAddProject
            // 
            this.btnAddProject.Location = new System.Drawing.Point(3, 3);
            this.btnAddProject.Name = "btnAddProject";
            this.btnAddProject.Size = new System.Drawing.Size(115, 47);
            this.btnAddProject.TabIndex = 1;
            this.btnAddProject.Text = "新規\r\nプロジェクト";
            this.btnAddProject.UseVisualStyleBackColor = true;
            this.btnAddProject.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnDetailProject
            // 
            this.btnDetailProject.Location = new System.Drawing.Point(124, 3);
            this.btnDetailProject.Name = "btnDetailProject";
            this.btnDetailProject.Size = new System.Drawing.Size(115, 47);
            this.btnDetailProject.TabIndex = 2;
            this.btnDetailProject.Text = "プロジェクト\r\n詳細";
            this.btnDetailProject.UseVisualStyleBackColor = true;
            this.btnDetailProject.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnChangeProject
            // 
            this.btnChangeProject.Location = new System.Drawing.Point(245, 3);
            this.btnChangeProject.Name = "btnChangeProject";
            this.btnChangeProject.Size = new System.Drawing.Size(115, 47);
            this.btnChangeProject.TabIndex = 3;
            this.btnChangeProject.Text = "プロジェクト\r\n変更";
            this.btnChangeProject.UseVisualStyleBackColor = true;
            this.btnChangeProject.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnGetProject
            // 
            this.btnGetProject.Location = new System.Drawing.Point(366, 3);
            this.btnGetProject.Name = "btnGetProject";
            this.btnGetProject.Size = new System.Drawing.Size(115, 47);
            this.btnGetProject.TabIndex = 4;
            this.btnGetProject.Text = "プロジェクト\r\n取込";
            this.btnGetProject.UseVisualStyleBackColor = true;
            this.btnGetProject.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnDeleteProject
            // 
            this.btnDeleteProject.Location = new System.Drawing.Point(487, 3);
            this.btnDeleteProject.Name = "btnDeleteProject";
            this.btnDeleteProject.Size = new System.Drawing.Size(115, 47);
            this.btnDeleteProject.TabIndex = 5;
            this.btnDeleteProject.Text = "プロジェクト\r\n削除";
            this.btnDeleteProject.UseVisualStyleBackColor = true;
            this.btnDeleteProject.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnPutProject
            // 
            this.btnPutProject.Location = new System.Drawing.Point(608, 3);
            this.btnPutProject.Name = "btnPutProject";
            this.btnPutProject.Size = new System.Drawing.Size(115, 47);
            this.btnPutProject.TabIndex = 6;
            this.btnPutProject.Text = "プロジェクト\r\n取出";
            this.btnPutProject.UseVisualStyleBackColor = true;
            this.btnPutProject.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnAddTask
            // 
            this.btnAddTask.Location = new System.Drawing.Point(729, 3);
            this.btnAddTask.Name = "btnAddTask";
            this.btnAddTask.Size = new System.Drawing.Size(120, 47);
            this.btnAddTask.TabIndex = 12;
            this.btnAddTask.Text = "タスク追加";
            this.btnAddTask.UseVisualStyleBackColor = true;
            this.btnAddTask.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnTaskDetail
            // 
            this.btnTaskDetail.Location = new System.Drawing.Point(855, 3);
            this.btnTaskDetail.Name = "btnTaskDetail";
            this.btnTaskDetail.Size = new System.Drawing.Size(120, 47);
            this.btnTaskDetail.TabIndex = 13;
            this.btnTaskDetail.Text = "タスク詳細";
            this.btnTaskDetail.UseVisualStyleBackColor = true;
            this.btnTaskDetail.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnTaskChange
            // 
            this.btnTaskChange.Location = new System.Drawing.Point(3, 56);
            this.btnTaskChange.Name = "btnTaskChange";
            this.btnTaskChange.Size = new System.Drawing.Size(120, 47);
            this.btnTaskChange.TabIndex = 14;
            this.btnTaskChange.Text = "タスク変更";
            this.btnTaskChange.UseVisualStyleBackColor = true;
            this.btnTaskChange.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnTaskCopy
            // 
            this.btnTaskCopy.Location = new System.Drawing.Point(129, 56);
            this.btnTaskCopy.Name = "btnTaskCopy";
            this.btnTaskCopy.Size = new System.Drawing.Size(120, 47);
            this.btnTaskCopy.TabIndex = 15;
            this.btnTaskCopy.Text = "タスクコピー";
            this.btnTaskCopy.UseVisualStyleBackColor = true;
            this.btnTaskCopy.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnTaskDelete
            // 
            this.btnTaskDelete.Location = new System.Drawing.Point(255, 56);
            this.btnTaskDelete.Name = "btnTaskDelete";
            this.btnTaskDelete.Size = new System.Drawing.Size(120, 47);
            this.btnTaskDelete.TabIndex = 16;
            this.btnTaskDelete.Text = "タスク削除";
            this.btnTaskDelete.UseVisualStyleBackColor = true;
            this.btnTaskDelete.Click += new System.EventHandler(this.buttons_click);
            // 
            // lblSeparate1
            // 
            this.lblSeparate1.AutoSize = true;
            this.lblSeparate1.Location = new System.Drawing.Point(381, 53);
            this.lblSeparate1.Name = "lblSeparate1";
            this.lblSeparate1.Size = new System.Drawing.Size(9, 12);
            this.lblSeparate1.TabIndex = 10;
            this.lblSeparate1.Text = " ";
            // 
            // btnViewFolder
            // 
            this.btnViewFolder.Location = new System.Drawing.Point(396, 56);
            this.btnViewFolder.Name = "btnViewFolder";
            this.btnViewFolder.Size = new System.Drawing.Size(110, 47);
            this.btnViewFolder.TabIndex = 7;
            this.btnViewFolder.Text = "フォルダ表示";
            this.btnViewFolder.UseVisualStyleBackColor = true;
            this.btnViewFolder.Click += new System.EventHandler(this.buttons_click);
            // 
            // lblSeparate2
            // 
            this.lblSeparate2.AutoSize = true;
            this.lblSeparate2.Location = new System.Drawing.Point(512, 53);
            this.lblSeparate2.Name = "lblSeparate2";
            this.lblSeparate2.Size = new System.Drawing.Size(9, 12);
            this.lblSeparate2.TabIndex = 11;
            this.lblSeparate2.Text = " ";
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(527, 56);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(100, 47);
            this.btnQuit.TabIndex = 9;
            this.btnQuit.Text = "終  了";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.buttons_click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(633, 56);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 47);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "閉じる";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.buttons_click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.editToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSettings,
            this.toolStripSeparator2,
            this.tsmiOfficeMaster,
            this.tsmiPersonMaster,
            this.tsmiTaskMaster,
            this.tsmiProgressMaster,
            this.toolStripSeparator3,
            this.tsmiUtilities,
            this.toolStripSeparator4,
            this.tsmiInquiry,
            this.tsmiMember,
            this.toolStripSeparator5,
            this.tsmiVersion,
            this.tsmiHomepage});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.menuToolStripMenuItem.Text = "メニュー";
            this.menuToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuToolStripMenuItem_DropDownItemClicked);
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.tsmiSettings.Size = new System.Drawing.Size(240, 22);
            this.tsmiSettings.Text = "環境設定";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(237, 6);
            // 
            // tsmiOfficeMaster
            // 
            this.tsmiOfficeMaster.Name = "tsmiOfficeMaster";
            this.tsmiOfficeMaster.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D9)));
            this.tsmiOfficeMaster.Size = new System.Drawing.Size(240, 22);
            this.tsmiOfficeMaster.Text = "事業所マスタを編集";
            // 
            // tsmiPersonMaster
            // 
            this.tsmiPersonMaster.Name = "tsmiPersonMaster";
            this.tsmiPersonMaster.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.tsmiPersonMaster.Size = new System.Drawing.Size(240, 22);
            this.tsmiPersonMaster.Text = "担当者マスタを編集";
            // 
            // tsmiTaskMaster
            // 
            this.tsmiTaskMaster.Name = "tsmiTaskMaster";
            this.tsmiTaskMaster.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.tsmiTaskMaster.Size = new System.Drawing.Size(240, 22);
            this.tsmiTaskMaster.Text = "処理内容マスタを編集";
            // 
            // tsmiProgressMaster
            // 
            this.tsmiProgressMaster.Name = "tsmiProgressMaster";
            this.tsmiProgressMaster.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.tsmiProgressMaster.Size = new System.Drawing.Size(240, 22);
            this.tsmiProgressMaster.Text = "状態マスタを編集";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(237, 6);
            // 
            // tsmiUtilities
            // 
            this.tsmiUtilities.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiScreenResolution,
            this.tsmiRepository});
            this.tsmiUtilities.Name = "tsmiUtilities";
            this.tsmiUtilities.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.tsmiUtilities.Size = new System.Drawing.Size(240, 22);
            this.tsmiUtilities.Text = "画面関係ユーティリティ";
            this.tsmiUtilities.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuToolStripMenuItem_DropDownItemClicked);
            // 
            // tsmiScreenResolution
            // 
            this.tsmiScreenResolution.Name = "tsmiScreenResolution";
            this.tsmiScreenResolution.Size = new System.Drawing.Size(148, 22);
            this.tsmiScreenResolution.Text = "解像度表示";
            // 
            // tsmiRepository
            // 
            this.tsmiRepository.Name = "tsmiRepository";
            this.tsmiRepository.Size = new System.Drawing.Size(148, 22);
            this.tsmiRepository.Text = "リポジトリブラウザ";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(237, 6);
            // 
            // tsmiInquiry
            // 
            this.tsmiInquiry.Name = "tsmiInquiry";
            this.tsmiInquiry.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.tsmiInquiry.Size = new System.Drawing.Size(240, 22);
            this.tsmiInquiry.Text = "質問受付フォーム";
            // 
            // tsmiMember
            // 
            this.tsmiMember.Name = "tsmiMember";
            this.tsmiMember.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tsmiMember.Size = new System.Drawing.Size(240, 22);
            this.tsmiMember.Text = "docmaker.net メンバー検索";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(237, 6);
            // 
            // tsmiVersion
            // 
            this.tsmiVersion.Name = "tsmiVersion";
            this.tsmiVersion.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.tsmiVersion.Size = new System.Drawing.Size(240, 22);
            this.tsmiVersion.Text = "バージョン情報";
            // 
            // tsmiHomepage
            // 
            this.tsmiHomepage.Name = "tsmiHomepage";
            this.tsmiHomepage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.tsmiHomepage.Size = new System.Drawing.Size(240, 22);
            this.tsmiHomepage.Text = "docmaker.netホームページ";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSelectAll,
            this.tsmiCancelSelect,
            this.tsmiCopySelected,
            this.tsmiBackToProjects});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.editToolStripMenuItem.Text = "編集";
            this.editToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.editToolStripMenuItem_DropDownItemClicked);
            // 
            // tsmiSelectAll
            // 
            this.tsmiSelectAll.Name = "tsmiSelectAll";
            this.tsmiSelectAll.Size = new System.Drawing.Size(192, 22);
            this.tsmiSelectAll.Text = "すべて選択する";
            // 
            // tsmiCancelSelect
            // 
            this.tsmiCancelSelect.Name = "tsmiCancelSelect";
            this.tsmiCancelSelect.Size = new System.Drawing.Size(192, 22);
            this.tsmiCancelSelect.Text = "すべての選択を解除する";
            // 
            // tsmiCopySelected
            // 
            this.tsmiCopySelected.Name = "tsmiCopySelected";
            this.tsmiCopySelected.Size = new System.Drawing.Size(192, 22);
            this.tsmiCopySelected.Text = "選択したタスクをコピーする";
            // 
            // tsmiBackToProjects
            // 
            this.tsmiBackToProjects.Name = "tsmiBackToProjects";
            this.tsmiBackToProjects.Size = new System.Drawing.Size(192, 22);
            this.tsmiBackToProjects.Text = "プロジェクトを閉じる";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.closeToolStripMenuItem.Text = "終了";
            // 
            // tsHighLight
            // 
            this.tsHighLight.BackColor = System.Drawing.SystemColors.HighlightText;
            this.tsHighLight.Dock = System.Windows.Forms.DockStyle.None;
            this.tsHighLight.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbx1,
            this.tsbSearch});
            this.tsHighLight.Location = new System.Drawing.Point(715, 1);
            this.tsHighLight.Name = "tsHighLight";
            this.tsHighLight.Size = new System.Drawing.Size(290, 25);
            this.tsHighLight.TabIndex = 3;
            this.tsHighLight.Text = "toolStrip1";
            // 
            // tbx1
            // 
            this.tbx1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbx1.Name = "tbx1";
            this.tbx1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbx1.ShortcutsEnabled = false;
            this.tbx1.Size = new System.Drawing.Size(200, 25);
            // 
            // tsbSearch
            // 
            this.tsbSearch.AutoSize = false;
            this.tsbSearch.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tsbSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSearch.Image = ((System.Drawing.Image)(resources.GetObject("tsbSearch.Image")));
            this.tsbSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSearch.Name = "tsbSearch";
            this.tsbSearch.Size = new System.Drawing.Size(78, 22);
            this.tsbSearch.Text = "検　索";
            this.tsbSearch.Click += new System.EventHandler(this.tsbSearch_Click);
            // 
            // cmsTabsMenu
            // 
            this.cmsTabsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCloseTab,
            this.tsmiChange,
            this.tsmiCopy,
            this.tsmiChangeProject,
            this.tsmiDeleteProject,
            this.tsmiPutProject});
            this.cmsTabsMenu.Name = "cmsTabsMenu";
            this.cmsTabsMenu.Size = new System.Drawing.Size(146, 136);
            this.cmsTabsMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTabsMenu_Opening);
            this.cmsTabsMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsTabsMenu_ItemClicked);
            // 
            // tsmiCloseTab
            // 
            this.tsmiCloseTab.Name = "tsmiCloseTab";
            this.tsmiCloseTab.Size = new System.Drawing.Size(145, 22);
            this.tsmiCloseTab.Text = "タブを閉じる";
            // 
            // tsmiChange
            // 
            this.tsmiChange.Name = "tsmiChange";
            this.tsmiChange.Size = new System.Drawing.Size(145, 22);
            this.tsmiChange.Text = "タスク変更";
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Name = "tsmiCopy";
            this.tsmiCopy.Size = new System.Drawing.Size(145, 22);
            this.tsmiCopy.Text = "タスクコピー";
            // 
            // tsmiChangeProject
            // 
            this.tsmiChangeProject.Name = "tsmiChangeProject";
            this.tsmiChangeProject.Size = new System.Drawing.Size(145, 22);
            this.tsmiChangeProject.Text = "プロジェクト変更";
            // 
            // tsmiDeleteProject
            // 
            this.tsmiDeleteProject.Name = "tsmiDeleteProject";
            this.tsmiDeleteProject.Size = new System.Drawing.Size(145, 22);
            this.tsmiDeleteProject.Text = "プロジェクト削除";
            // 
            // tsmiPutProject
            // 
            this.tsmiPutProject.Name = "tsmiPutProject";
            this.tsmiPutProject.Size = new System.Drawing.Size(145, 22);
            this.tsmiPutProject.Text = "プロジェクト取出";
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1008, 706);
            this.tabControl.TabIndex = 6;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.tsHighLight);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Opacity = 0D;
            this.Text = "docmaker.net Ver.2.0.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.mainView.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tsHighLight.ResumeLayout(false);
            this.tsHighLight.PerformLayout();
            this.cmsTabsMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel mainView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiOfficeMaster;
        private System.Windows.Forms.ToolStripMenuItem tsmiPersonMaster;
        private System.Windows.Forms.ToolStripMenuItem tsmiTaskMaster;
        private System.Windows.Forms.ToolStripMenuItem tsmiProgressMaster;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmiUtilities;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tsmiInquiry;
        private System.Windows.Forms.ToolStripMenuItem tsmiMember;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiVersion;
        private System.Windows.Forms.ToolStripMenuItem tsmiHomepage;
        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.Button btnPutProject;
        private System.Windows.Forms.Button btnDeleteProject;
        private System.Windows.Forms.Button btnGetProject;
        private System.Windows.Forms.Button btnChangeProject;
        private System.Windows.Forms.Button btnDetailProject;
        private System.Windows.Forms.Button btnAddProject;
        private System.Windows.Forms.Button btnViewFolder;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.ToolStrip tsHighLight;
        private System.Windows.Forms.ToolStripTextBox tbx1;
        private System.Windows.Forms.ToolStripButton tsbSearch;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip cmsTabsMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancelSelect;
        private System.Windows.Forms.ToolStripMenuItem tsmiBackToProjects;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Label lblSeparate1;
        private System.Windows.Forms.Label lblSeparate2;
        private System.Windows.Forms.Button btnAddTask;
        private System.Windows.Forms.Button btnTaskDetail;
        private System.Windows.Forms.Button btnTaskChange;
        private System.Windows.Forms.Button btnTaskCopy;
        private System.Windows.Forms.Button btnTaskDelete;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiChange;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopySelected;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeProject;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteProject;
        private System.Windows.Forms.ToolStripMenuItem tsmiPutProject;
        private System.Windows.Forms.ToolStripMenuItem tsmiScreenResolution;
        private System.Windows.Forms.ToolStripMenuItem tsmiRepository;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripMenuItem tsmiCloseTab;

    }
}

