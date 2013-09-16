/*--------------------------------------------
 * 
 *   docmaker.net Ver.2
 *
 *   Producer: Hiroyuki Kawaguchi (Hiro KAWAGUCHI Labo.) 
 *   Author:Kenji Takanashi (HL Will Co.,Ltd.)
 *   Since:2013/10/1
 *   
 *--------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using net.docmaker.Classes;
using ShellLib;


namespace net.docmaker {
    //サブウィンドウを閉じたときの次のステップ
    internal enum NextStep : int {
        SPLASH,
        PROJECT,
        TASK
    }

    public partial class frmMain : Form {
        private LogManager log;         //ログ管理
        private EnvData env_data;       //環境情報
        private TabControl.TabPageCollection tabs;  //タブコレクション
        private SvnManager svn;         //Subversion管理
        private DataSet ds_main;        //データ取り込み用
        private frmSubWindow frm_sub;   //サブウインドウ
        private NextStep next_step;     //サブウインドウを閉じた際の、次の処理
        private string current_project; //現在の選択されているプロジェクト名
        private bool search_flag;       //検索ボタン管理用
        private bool is_multiple;       //タスクリスト画面で、複数選択されているかどうかのフラグ

        public frmMain() {
            InitializeComponent();
        }

        //初期化
        private void frmMain_Load(object sender, EventArgs e) {
            this.Visible = false;
            dgvMain.ReadOnly = true;
            dgvMain.ContextMenuStrip = cmsTabsMenu;
            current_project = "";
            search_flag = false;
            is_multiple = false;
            tabs = new TabControl.TabPageCollection(tabControl);

            log = new LogManager(true);
            log.Write("dmpn Start--- " + DateTime.Now.ToShortDateString());

            log.Write("getEnvData " + DateTime.Now);
            env_data = new EnvData(new Documents("", ""), new Project("", false, "", false), new Master("", false, ""), this.Left, this.Top, this.Width, this.Height);
            GetEnvData();
            if (env_data.WindowLocation.X == 0 && env_data.WindowLocation.Y == 0) InitLocationAndSize();
            
            this.Location = env_data.WindowLocation;
            this.Size = env_data.WindowSize;

            //メニューバーは作成しない
            //log.Write("CreateUserMenuBar2");
            log.Write("OpenIndex " + DateTime.Now);
            log.Close();
            if (env_data.Documents.Path.Length == 0) {
                Documents doc = env_data.Documents;
                doc.Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\docmaker.net2\index.xml";
                env_data.Documents = doc;
                Project pro = env_data.Project;
                pro.Folder=Path.GetDirectoryName(doc.Path)+@"\";
                env_data.Project = pro;
                env_data.SaveEnvData();
                if (!File.Exists(doc.Path)) {
                    next_step = NextStep.SPLASH;
                    MessageBox.Show("最初に環境設定をする必要があります。\n環境設定画面を表示します。\n You should set the environment first.", "警告", MessageBoxButtons.OK);
                    frm_sub = new frmSubWindow(env_data, log, OpenWindowType.ENV_SETTINGS);
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                } else
                    StartApplication();
            } else {
                StartApplication();
            }
        }

        //プロパティ
        #region Properties
        #endregion


        //メソッド
        #region Methods

        //環境設定ファイルの取得
        private void GetEnvData() {
            
            XmlSerializer seri = new XmlSerializer(typeof(EnvData));
            FileStream fs;

            if (File.Exists(EnvData.ENV_FILE_NAME)) {
                fs = new FileStream(EnvData.ENV_FILE_NAME, FileMode.Open);
                try {
                    env_data = (EnvData)seri.Deserialize(fs);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } else {
                fs = new FileStream(EnvData.ENV_FILE_NAME, FileMode.Create);
                seri.Serialize(fs, env_data);
            }
            fs.Close();
        }

        //アプリケーションスタート
        private void StartApplication() {
            log.Write("StartApplicatoinBegin");
            log.Write("Step1");
            log.Write("Step2");
            log.Write("Step3");
            log.Close();
            DocTab tab=new DocTab(new TabInfo(false,"",""));
            tab.Controls.Add(this.mainView);
            tab.Location = new System.Drawing.Point(4, 22);
            tab.Name = "ProjectList";
            tab.Padding = new System.Windows.Forms.Padding(3);
            tab.Size = new System.Drawing.Size(1000, 678);
            tab.TabIndex = 0;
            tab.Text = "プロジェクト一覧";
            tab.UseVisualStyleBackColor = true;
            tabs.Add(tab);
            tabControl.SelectedIndexChanged+=new EventHandler(tabControl_SelectedIndexChanged);
            tabControl.ContextMenuStrip = cmsTabsMenu;

            frmSplash splash = new frmSplash();
            splash.FormClosed += new FormClosedEventHandler(frmSplash_Closed);
            splash.Show();
        }

        //ウインドウの位置とサイズの初期化
        private void InitLocationAndSize() {
            this.Width = 1024;
            this.Height = 768;
            this.Top=Screen.GetBounds(this).Height / 2 - (this.Height / 2);
            this.Left=Screen.GetBounds(this).Width / 2 - (this.Width / 2);
            env_data.WindowLocation = this.Location;
            env_data.WindowSize = this.Size;
            env_data.SaveEnvData();

        }

        //DataGridViewの初期化
        private void InitDatagridView(DataView dv) {
            if (dgvMain.Columns.Count == 0) return;
            while (dgvMain.Columns.Count != 0) {
                dgvMain.Columns.RemoveAt(dgvMain.Columns.Count-1);
            }
            dgvMain.DataSource=null;
        }

        //タブの追加
        private void AddTab(DataRow row) {
            string strSelectFolderName = row[2].ToString();
            if (strSelectFolderName.Length == 0) return;
            string id = row[0].ToString();
            current_project =row[1].ToString();
            env_data.Project = new Project(
                env_data.Project.Folder,
                env_data.Project.TortoiseSVN,
                env_data.Project.URL,
                env_data.Project.DropBox,
                strSelectFolderName);
            int tab_index = 0;
            if (IsExistTab(id, out tab_index))
                tabControl.SelectedTab = tabs[tab_index];
            else {
                tabs.Add(new DocTab(new TabInfo(true, id, strSelectFolderName), current_project));
                tabControl.SelectedTab = tabs[tabs.Count - 1];
            }
        }

        //タブの削除
        private void DeleteTab(int tab_index) {
            tabs.RemoveAt(tab_index);
            tabControl.SelectedTab = tabs[tabs.Count - 1];
        }

        //すべてのタスクタブの削除
        private void ClearTabs() {
            while (tabs.Count != 1) {
                tabs.RemoveAt(tabs.Count - 1);
            }
            tabs[0].Show();
        }

        //タスクタブが存在するかどうか
        private bool IsExistTab(string id,out int tab_index){
            tab_index=-1;
            foreach (DocTab tab in tabs){
                if (tab.TabInfo.Id == id) { tab_index = tabs.IndexOf(tab); return true; }
            }
            return false;
        }

        //プロジェクト一覧の表示
        private void ShowProjectData() {
            dgvMain.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            dgvMain.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
            dgvMain.MultiSelect = false;
            ds_main = new DataSet();
            if (Path.GetExtension(env_data.Documents.Path) == ".xml" && File.Exists(env_data.Documents.Path)) {
                try {
                    ds_main.ReadXml(env_data.Documents.Path);
                } catch (Exception e) {
                    MessageBox.Show(e.Message);
                    return;
                }
                DataView dv=new DataView(ds_main.Tables["ProjectData"].Copy());
                if (search_flag) {
                    if (tbx1.Text != "") dv.RowFilter = "案件名 Like '%"+tbx1.Text+"%'";
                }
                search_flag = false;
                InitDatagridView(dv);
                dgvMain.DataSource = dv;
                dgvMain.Columns.RemoveAt(2);
                int[] widths = { 120, 405, 80, 85, 85, 100, 60 };
                for (int i = 0; i < dgvMain.Columns.Count; i++) {
                    dgvMain.Columns[i].Width = widths[i];
                }
                dgvMain.Rows[0].Selected = true;
                SetTaskRowsColor();
                SetButtons();
            } else {
                DialogResult result;
                result = MessageBox.Show("index.xmlファイルが見つかりません", "警告", MessageBoxButtons.OK,MessageBoxIcon.Error);
                if (result == DialogResult.OK) {
                    if (frm_sub == null) {
                        frm_sub = new frmSubWindow(env_data, log,OpenWindowType.ENV_SETTINGS);
                        frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    }
                    next_step = NextStep.PROJECT;
                    frm_sub.Show();
                    this.Enabled = false;
                }
            }
        }

        //タスク一覧の表示
        private void ShowDetail() {
            dgvMain.MultiSelect = true;
            log.Write("ShowDetailBEGIN");
            log.Write("Step1");
            log.Close();
            if (env_data.Project.SelectedFolder == "") {
                MessageBox.Show("プロジェクトが選択されていません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string task = env_data.Project.Folder + env_data.Project.SelectedFolder + @"\task.xml";
            if (!File.Exists(task)) {
                string excel = task.Replace(".xml", ".xls");
                if (!File.Exists(excel)) {
                    MessageBox.Show("タスクマスタが見つかりません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabs[tabs.Count - 1].Controls.Remove(mainView);
                    DeleteTab(tabs.Count - 1);
                    tabs[0].Controls.Add(mainView);
                    tabControl.SelectedIndex = 0;
                    ShowProjectData();
                    return;
                } else {
                    XMLConverter xml = new XMLConverter(XmlType.TaskList, excel);
                    xml.Convert();
                }
            }

            if (env_data.Project.TortoiseSVN) {
                if (svn == null) svn = new SvnManager(env_data);
                if (task == "")
                    svn.TortoiseSvn("Project", "checkout", env_data.Project.Folder + @"\" + env_data.Project.SelectedFolder, env_data.Project.URL + env_data.Project.SelectedFolder);
                else
                    svn.TortoiseSvn("Project", "update", env_data.Project.Folder + @"\" + env_data.Project.SelectedFolder, "");
            }
            log.Write("Step2");
            log.Close();
            ds_main = new DataSet();
            ds_main.ReadXml(task);
            DataView dv = new DataView(ds_main.Tables["Task"].Copy());
            InitDatagridView(dv);
            dgvMain.DataSource = dv;
            DataGridViewTextBoxColumn dvc = new DataGridViewTextBoxColumn();
            dvc.ValueType = Type.GetType("System.Datetime");
            dvc.Name = "最終更新日";
            dgvMain.Columns.Insert(5, dvc);
            int[] widths = {60, 345, 80, 60, 80, 100, 85, 85, 60 };
            while (dgvMain.Columns.Count > 9) { dgvMain.Columns.RemoveAt(dgvMain.Columns.Count - 1); }
            for (int i = 0; i < dgvMain.Columns.Count; i++) {
                dgvMain.Columns[i].Width = widths[i];
                if (i == 3) {
                    dgvMain.Columns[i].HeaderText="様式";
                    foreach (DataGridViewRow dr in dgvMain.Rows) {
                        if (dr.Cells[i].Value != null)
                            dr.Cells[i].Value = Path.GetExtension(dr.Cells[i].Value.ToString());
                    }
                } else if (i == 5) {
                    string f_name;
                    foreach (DataGridViewRow dr in dgvMain.Rows) {
                        if (dr.Cells[3].Value == null) continue;
                        f_name = env_data.Project.Folder + env_data.Project.SelectedFolder + @"\" + ds_main.Tables["Task"].Rows[dr.Index][3];
                        if (File.Exists(f_name))
                            dr.Cells[i].Value = File.GetLastAccessTime(f_name).ToString(@"yyyy/MM/dd");
                    }
                }
            }
            dgvMain.Rows[0].Selected = true;
            SetTaskRowsColor();
            SetButtons();
            log.Write("ShowDetailEND");
            log.Close();
            
        }

        //ボタンのセット
        private void SetButtons() {
            bool flag = false;
            if (tabControl.SelectedIndex==0) flag = true;
            tsHighLight.Visible = flag;
            editToolStripMenuItem.Visible = !flag;

            btnAddProject.Visible = flag;
            btnDetailProject.Visible = flag;
            btnChangeProject.Visible = flag;
            btnGetProject.Visible = flag;
            btnDeleteProject.Visible = flag;
            btnPutProject.Visible = flag;
            btnQuit.Visible = flag;

            btnAddTask.Visible = !flag;
            btnTaskDetail.Visible = !flag;
            btnTaskChange.Visible = !flag;
            btnTaskCopy.Visible = !flag;
            btnTaskDelete.Visible = !flag;
            btnExit.Visible = !flag;

            tsmiCloseTab.Visible = false;
            tsmiChange.Visible =is_multiple? false:!flag;
            tsmiCopy.Visible = !flag;

            tsmiChangeProject.Visible = flag;
            tsmiDeleteProject.Visible = flag;
            tsmiPutProject.Visible = flag;

            tsHighLight.Left = this.Width - tsHighLight.Width-17;
        }

        //DataGridViewのカラー設定
        private void SetTaskRowsColor() {
            foreach (DataGridViewRow dr in dgvMain.Rows) {
                if (dr.Cells[0].Value == null) continue;

                if (tabControl.SelectedIndex != 0) {
                    dr.Cells[8].Style.ForeColor = Color.White;
                    if (dr.Cells[8].Value != null) {
                        if (dr.Cells[8].Value.ToString() == "処理済") {
                            dr.Cells[8].Style.BackColor = Color.Blue;
                        } else if (dr.Cells[8].Value.ToString() == "作業中") {
                            dr.Cells[8].Style.BackColor = Color.Green;
                        } else if (dr.Cells[8].Value.ToString() == "未着手") {
                            dr.Cells[8].Style.BackColor = Color.Red;
                        } else
                            dr.Cells[8].Style.BackColor = Color.Gray;
                    }
                } else {
                    dr.Cells[6].Style.ForeColor = Color.White;
                    if (dr.Cells[6].Value != null) {
                        if (dr.Cells[6].Value.ToString() == "処理済") {
                            dr.Cells[6].Style.BackColor = Color.Blue;
                        } else if (dr.Cells[6].Value.ToString() == "作業中") {
                            dr.Cells[6].Style.BackColor = Color.Green;
                        } else if (dr.Cells[6].Value.ToString() == "未着手") {
                            dr.Cells[6].Style.BackColor = Color.Red;
                        } else
                            dr.Cells[6].Style.BackColor = Color.Gray;
                    }
                }
            }
        }

        //タスク情報の保存
        private void SaveData() {
            if (tabControl.SelectedIndex>0) {
                string folder = env_data.Project.Folder + env_data.Project.SelectedFolder + @"\";
                string task = "task.xml";
                StreamWriter sw=new StreamWriter(folder+task,false);
                lock(ds_main){
                    ds_main.WriteXml(sw,XmlWriteMode.WriteSchema);
                }
                sw.Close();
                ShowDetail();
            }
        
        }

        //外部ファイルの呼び出し
        private void CallWindow(int row_index) {
            string f_name = ds_main.Tables["Task"].Rows[row_index][3].ToString();
            string strTaskPath = env_data.Project.Folder + env_data.Project.SelectedFolder + @"\";
            string app_name;
            string ext;
            Process p;
            if (f_name.Length == 0) {
                Random rnd = new Random();
                string rnd_str = rnd.Next(100).ToString("D2");
                f_name = DateTime.Now.ToString("yyyyMMddhhmmss") +rnd_str + ".txt";
                if (!File.Exists(strTaskPath + f_name)) {
                    StreamWriter sw = File.CreateText(strTaskPath + f_name);
                    sw.Close();

                }
                ds_main.Tables["Task"].Rows[row_index][3] = f_name;
                SaveData();
            }
            if (!File.Exists(strTaskPath + f_name)) {
                if (Path.GetExtension(f_name) == ".txt" || Path.GetExtension(f_name) == ".csv" || Path.GetExtension(f_name) == ".xml")
                    File.Create(strTaskPath + f_name);
                else {
                    MessageBox.Show("ファイルが見つかりません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            ext = Path.GetExtension(f_name);
            if (ext == ".txt" && env_data.Documents.Editor.Length > 5) {
                app_name = env_data.Documents.Editor;
            } else {
                RegistryManager reg = new RegistryManager();
                app_name = reg.GetApplicationFullPathByExtension(ext);
                if (app_name == "") {
                    app_name = reg.GetApplicationFileByExtension(ext);
                    if (app_name == "") {
                        try {
                            p = Process.Start(strTaskPath + f_name);
                        } catch (Exception ex) {
                            throw new Exception(ex.Message);
                        }
                        return;
                    }
                }
            }
            
            try {
                p = Process.Start(app_name, strTaskPath + f_name);
            } catch (Exception ex) {
                try {
                    p = Process.Start(strTaskPath + f_name);
                } catch {
                    throw ex;
                }
            }
        }

        //タスクのコピー
        private void CopyTask(int row_index,ref List<int> row_indexes) {
            DataRow dr;
            DialogResult result;

            dr = ds_main.Tables["Task"].Rows[row_index];
            if (dr[3].ToString() != "") {
                result = MessageBox.Show("タスク「"+dr[1].ToString()+"」には同名のファイルがあります。自動的に重複しないファイル名を付与しますか？",
                                        "重複ファイルの確認",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Exclamation);
            } else
                result = DialogResult.No;
            string strNewFileName;
            if (result == DialogResult.Yes) {
                Random rnd = new Random();
                string rnd_str = rnd.Next(100).ToString("D2");
                strNewFileName = DateTime.Now.ToString("yyyyMMddhhmmss") +
                                rnd_str +
                                Path.GetExtension(dr[3].ToString());
                File.Copy(env_data.Project.Folder + env_data.Project.SelectedFolder + @"\" + dr[3].ToString(),
                          env_data.Project.Folder + env_data.Project.SelectedFolder + @"\" + strNewFileName);
            } else {
                strNewFileName = dr[3].ToString();
            }
            DataRow new_row = ds_main.Tables["Task"].NewRow();
            int idx = 0;
            foreach (object cell_value in dr.ItemArray) {
                switch (idx) {
                    case 1:
                        new_row[idx] = "(コピー)" + cell_value.ToString();
                        break;
                    case 3:
                        new_row[idx] = strNewFileName;
                        break;
                    case 5:
                        new_row[idx] = DateTime.Now.ToString("yyyy/MM/dd");
                        break;
                    case 6:
                        new_row[idx] = "";
                        break;
                    case 7:
                        new_row[idx] = "未着手";
                        break;
                    default:
                        new_row[idx] = (dr[idx] == null) ? "" : cell_value.ToString();
                        break;
                }
                idx++;
            }
            ds_main.Tables["Task"].Rows.InsertAt(new_row, row_index + 1);
            ds_main.WriteXml(env_data.Project.Folder + env_data.Project.SelectedFolder + @"\task.xml", XmlWriteMode.WriteSchema);
            row_indexes.Add(ds_main.Tables["Task"].Rows.IndexOf(new_row));
        }

        //タスクの削除
        private void DeleteTask(int row_index) {
            DataRow dr;
            DialogResult result;
            dr = ds_main.Tables["Task"].Rows[row_index];
            
            result = MessageBox.Show("タスク「"+dr[1].ToString()+"」を削除しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (result == DialogResult.Yes) {
                if (dr[3].ToString() != "") {
                    result = MessageBox.Show("ファイルの実体も一緒に削除しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Yes) {
                        try {
                            File.Delete(env_data.Project.Folder + env_data.Project.SelectedFolder + @"\" + dr[3].ToString());
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                ds_main.Tables["Task"].Rows.RemoveAt(row_index);
                ds_main.WriteXml(env_data.Project.Folder + env_data.Project.SelectedFolder + @"\task.xml", XmlWriteMode.WriteSchema);
            }
        
        }

        private DataRow GetDataRowByDataGridViewRow(string table,DataGridViewRow vrow) {
            DataRow[] rows;
            string col2=table=="ProjectData"? "案件名":"タスク名";
            string query = "項番='" + vrow.Cells[0].Value.ToString() +
                           "' AND " + col2 + "='" + vrow.Cells[1].Value.ToString() + "'";
            rows = ds_main.Tables[table].Select(query);
            if (rows.Length == 0)
                return null;
            else
                return rows[0];
        }

        private TabInfo GetTabInfoByDataRow(DataRow row) {
            foreach (DocTab tab in tabs){
                if (tab.TabInfo.Id==row[0].ToString() && tab.Text==row[1].ToString()) return tab.TabInfo;
            }
            return new TabInfo();
        }

        private int GetRowIndexByDataViewRow(DataGridViewRow vrow) {
            if (vrow.Cells[0].Value == null) return -1;
            DataRow[] rows=ds_main.Tables["Task"].Select("項番='" + vrow.Cells[0].Value.ToString()+
                                                        "' AND タスク名='"+vrow.Cells[1].Value.ToString()+"'");

            if (rows.Length == 0)
                return -1;
            else
                return ds_main.Tables["Task"].Rows.IndexOf(rows[0]);
        }

        //プロジェクトファイルのエクスポート
        private void ExportProjectData() {
            if (dgvMain.SelectedRows.Count == 0) return;
            if (dgvMain.SelectedRows[0].Cells[0].Value == null) return;
            DataRow row = GetDataRowByDataGridViewRow("ProjectData",dgvMain.SelectedRows[0]);
            if (row == null) return;
            DialogResult result = MessageBox.Show("プロジェクト「" + row[1].ToString() + "」の基本情報を\n外部ファイルに出力してよいですか？", "プロジェクト情報ファイル出力の確認", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                log.Write("ExportProjectDataBEGIN");
                DataSet export_data = new DataSet("docmaker.net");
                DataTable dt = export_data.Tables.Add("ProjectData");

                dt.Columns.Add("項番",Type.GetType("System.String"));
                dt.Columns.Add("案件名", Type.GetType("System.String"));
                dt.Columns.Add("フォルダ名", Type.GetType("System.String"));
                dt.Columns.Add("担当者名", Type.GetType("System.String"));
                dt.Columns.Add("作成日", Type.GetType("System.String"));
                dt.Columns.Add("期限", Type.GetType("System.String"));
                dt.Columns.Add("次回更新日", Type.GetType("System.String"));
                dt.Columns.Add("状態", Type.GetType("System.String"));

                dt.Rows.Add(row.ItemArray);

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = row[1].ToString() + ".dprx";
                sfd.Filter = "docmakerPM プロジェクト情報(*.dprx)|*.dprx";
                sfd.Title = "プロジェクト情報を保存";
                result = sfd.ShowDialog();
                if (result == DialogResult.OK) {
                    export_data.WriteXml(sfd.OpenFile(), XmlWriteMode.WriteSchema);
                }
                MessageBox.Show("プロジェクト情報を出力しました", "確認", MessageBoxButtons.OK);
                log.Write("ExportProjectDataEND");
                log.Close();
            }
        }

        //デフォルトのブラウザの取得
        private string GetDefaultBrowser() {
            RegistryManager reg = new RegistryManager();
            return reg.GetApplicationFullPathByExtension(".html");
        }

        //アプリケーションの終了
        private void CloseApplication() {
            if (MessageBox.Show("アプリケーションを終了してよろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;
            SvnManager svn = new SvnManager(env_data);
            svn.TortoiseSvn("Project", "commit", env_data.Project.Folder, "");
            this.Close();
        }
        #endregion


        //イベント処理
        #region Events
        
        //サブウインドウを閉じた際の処理
        private void frmSubWindow_Closed(object sender, EventArgs e) {
            this.Enabled = true;
            if (frm_sub.DialogResult == DialogResult.Cancel && env_data.Documents.Path.Length == 0) this.Close();
            if (frm_sub.DialogResult == DialogResult.Cancel) return;
            switch (frm_sub.OpenWindowType) {
                case OpenWindowType.ENV_SETTINGS:
                    GetEnvData();
                    if (next_step == NextStep.PROJECT) {
                        ShowProjectData();
                    } else if (next_step == NextStep.TASK)
                        ShowDetail();
                    else if (next_step == NextStep.SPLASH) {
                        StartApplication();
                    }
                    break;
                case OpenWindowType.ADD_TASK:
                case OpenWindowType.CHANGE_TASK:
                    ShowDetail();
                    break;
                case OpenWindowType.CREATE_PROJECT_LIST:
                    GetEnvData();
                    ShowProjectData();
                    break;
                case OpenWindowType.GYOUMU_PACK:
                    GetEnvData();
                    break;
                case OpenWindowType.CREATE_PROJECT:
                case OpenWindowType.PROJECT_DATA:
                case OpenWindowType.GET_PROJECT:
                case OpenWindowType.DELETE_PROJECT:
                    ShowProjectData();
                    break;
            }
        }

        //スプラッシュウインドウを閉じた際の慮利
        private void frmSplash_Closed(object sender, EventArgs e) {
            this.Opacity = 1;
            this.Visible = true;
            log.Write("Step5");
            log.Write("startApplicatonEnd");
            log.Close();
            ShowProjectData();
        }

        //DataGridViewをダブルクリックした際の処理
        private void dgvMain_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            DataGridView view = (DataGridView)sender;
            DataRow row;
            view.ClearSelection();
            view.Rows[e.RowIndex].Selected = true;
            if (tabControl.SelectedIndex == 0) {    //プロジェクト一覧の場合
                row = GetDataRowByDataGridViewRow("ProjectData", view.SelectedRows[0]);
                if (row == null) return;
                AddTab(row);
            } else {                            //タスク一覧の場合
                log.Write("ShowFileBegin");
                log.Write("Step1");
                log.Close();
                row = GetDataRowByDataGridViewRow("Task", view.SelectedRows[0]);
                if (row == null) return;
                CallWindow(ds_main.Tables["Task"].Rows.IndexOf(row));
                if (!view.Rows[e.RowIndex].Selected) {
                    view.ClearSelection();
                    view.Rows[e.RowIndex].Selected = true;
                }
            }


        }

        //DataGridViewの選択が変更された場合の処理
        private void dgvMain_SelectionChanged(object sender, EventArgs e) {
            if (dgvMain.SelectedCells.Count > 0) {
                foreach (DataGridViewCell cell in dgvMain.SelectedCells) {
                    if (dgvMain.SelectedRows.Contains(dgvMain.Rows[cell.RowIndex])) continue;
                    dgvMain.Rows[cell.RowIndex].Selected = true;
                }
            }
            is_multiple = dgvMain.SelectedRows.Count == 1 ? false : true;
        }

        //DataGridViewのデータが並び替えられた際の処理
        private void dgvMain_Sorted(object sender, EventArgs e) {
            SetTaskRowsColor();
        }

        //選択しているタブが変更された際の処理
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e) {
            TabControl tbc = (TabControl)sender;
            DocTab tab =(DocTab) tabs[tbc.SelectedIndex];
            foreach (DocTab tb in tabs) {
                if (tb.Controls.Contains(mainView)) { tb.Controls.Remove(mainView); break; }
            }
            tab.Controls.Add(mainView);
            current_project = tab.Name;
            Project pj = env_data.Project;
            pj.SelectedFolder = tab.TabInfo.SelectedFolder;
            env_data.Project = pj;

            if (tabs.IndexOf(tab) == 0)
                ShowProjectData();
            else
                ShowDetail();
           
        }

        //コンテキストメニュー（右クリックメニュー）が表示された際の処理
        private void cmsTabsMenu_Opening(object sender, CancelEventArgs e) {
            ContextMenuStrip cms = (ContextMenuStrip)sender;
            if (cms.SourceControl.Name != "dgvMain" && tabControl.SelectedIndex != 0) {
                tsmiCloseTab.Visible = true;
                tsmiChange.Visible = false;
                tsmiCopy.Visible =false;
            }else if(cms.SourceControl.Name!="dgvMain" && tabControl.SelectedIndex==0){
                e.Cancel = true;
            } else {
                SetButtons();
            }
        }

        //コンテキストメニュー(右クリックメニュー）がクリックされた際の処理
        private void cmsTabsMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            switch (e.ClickedItem.Name) {
                //タブを閉じる
                case "tsmiCloseTab":
                    if (MessageBox.Show("現在のタブを閉じますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        DeleteTab(tabControl.SelectedIndex);
                    break;
                //タスク詳細
                case "tsmiDetail":
                    btnTaskDetail.PerformClick();
                    break;
                //タスク変更
                case "tsmiChange":
                    btnTaskChange.PerformClick();
                    break;
                //タスクコピー
                case "tsmiCopy":
                    btnTaskCopy.PerformClick();
                    break;
                //プロジェクト変更
                case "tsmiChangeProject":
                    btnChangeProject.PerformClick();
                    break;
                //プロジェクト削除
                case "tsmiDeleteProject":
                    btnDeleteProject.PerformClick();
                    break;
                //プロジェクト取り出し
                case "tsmiPutProject":
                    btnPutProject.PerformClick();
                    break;
            }
        }

        //メニューバーの終了をクリックした際の処理
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            if (e.ClickedItem.Name == "closeToolStripMenuItem") {
                CloseApplication();
            }
        }

        //メニューバーの「メニュー」下アイテムをクリックした際の処理
        private void menuToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            DialogResult result;
            switch (e.ClickedItem.Name) {
                //環境設定
                case "tsmiSettings":
                    if (tabs.Count>1)
                        result = MessageBox.Show("環境設定を開く前に開いているプロジェクトを閉じる必要があります。よろしいですか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    else
                        result = DialogResult.Yes;
                    if (result == DialogResult.Yes) {
                        ClearTabs();
                        next_step = NextStep.PROJECT;
                        frm_sub = new frmSubWindow(env_data, log, OpenWindowType.ENV_SETTINGS);
                        frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                        frm_sub.Show();
                        this.Enabled = false;
                    }  
                    break;
                //事業所マスタを編集
                case "tsmiOfficeMaster":
                    try {
                        frm_sub = new frmSubWindow(env_data, log, OpenWindowType.OFFICE_MASTER);
                    } catch (Exception ex) {
                        if (MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) {
                            break;
                        }
                    }
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                    break;
                //担当者マスタを編集
                case "tsmiPersonMaster":
                    try {
                        frm_sub = new frmSubWindow(env_data, log, OpenWindowType.PERSON_MASTER);
                    } catch (Exception ex) {
                        if (MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) {
                            break;
                        }
                    }
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                    break;
                //処理内容マスタを編集
                case "tsmiTaskMaster":
                    try {
                        frm_sub = new frmSubWindow(env_data, log, OpenWindowType.CATEGORY_MASTER);
                    } catch (Exception ex) {
                        if (MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) {
                            break;
                        }
                    }
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                    break;
                //状態マスタを編集
                case "tsmiProgressMaster":
                    try {
                        frm_sub = new frmSubWindow(env_data, log, OpenWindowType.STATUS_MASTER);
                    } catch (Exception ex) {
                        if (MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) {
                            break;
                        }
                    }
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                    break;
                //画面関係ユーティリティ
                case "tsmiUtilities":
                    break;
                //画面解像度
                case "tsmiScreenResolution":
                    MessageBox.Show("画面の解像度：" + Screen.PrimaryScreen.Bounds.Width + "×" + Screen.PrimaryScreen.Bounds.Height,"画面の解像度");
                    break;
                //リポジトリ
                case "tsmiRepository":
                    SvnManager svn = new SvnManager(env_data);
                    svn.TortoiseSvn("Project", "repobrowser", env_data.Project.Folder, env_data.Project.URL);
                    break;
                //質問受付フォーム
                case "tsmiInquiry":
                    string strMyURL = "http://www.docmaker.net/contact_us.html";
                    Process.Start(GetDefaultBrowser(), strMyURL);
                    break;
                //docmaker.net メンバー検索
                case "tsmiMember":
                    string strMyURL2 = "http://www.docmaker.net/member/";
                    Process.Start(GetDefaultBrowser(), strMyURL2);
                    break;
                //バージョン情報
                case "tsmiVersion":
                    frm_sub = new frmSubWindow(env_data, log, OpenWindowType.VERSION);
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                    break;
                //docmaker.netホームページ
                case "tsmiHomepage":
                    string strMyURL3 = "http://www.docmaker.net/";
                    Process.Start(GetDefaultBrowser(), strMyURL3);
                    break;
            }
        }

        //メニューバーの「編集」下アイテムをクリックした際の処理
        private void editToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            DialogResult result;
            switch (e.ClickedItem.Name) {
                //すべて選択する
                case "tsmiSelectAll":
                    foreach (DataGridViewRow dr in dgvMain.Rows){
                        dr.Selected = true; ;
                    }
                    break;
                //すべての選択を解除する
                case "tsmiCancelSelect":    
                    foreach (DataGridViewRow dr in dgvMain.Rows) {
                        dr.Selected=false;
                    }
                    break;
                //選択したタスクをコピーする
                case "tsmiCopySelected":
                    btnTaskCopy.PerformClick();
                    break;
                //プロジェクトを閉じる
                case "tsmiBackToProjects":
                     result = MessageBox.Show("現在のプロジェクトを閉じますが、よろしいですか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Yes) {
                        ClearTabs();
                        ShowProjectData();
                    }                    
                    break;
            }
        }

        //DataGridView下のボタンをクリックした際の処理
        private void buttons_click(object sender, EventArgs e) {
            Button btn = (Button)sender;
            DialogResult result;
            Project pj;
            DataRow row;
            int vrow_index;
            switch (btn.Name) {
                //新規プロジェクト
                case "btnAddProject":
                    log.Write("addProjectBEGIN");
                    log.Write("step1");

                    ShellBrowseForFolderDialog fbdProject = new ShellBrowseForFolderDialog();
                    fbdProject.RootType = ShellBrowseForFolderDialog.RootTypeOptions.ByPath;
                    fbdProject.RootPath = env_data.Master.Folder;
                    fbdProject.DetailsFlags = ShellBrowseForFolderDialog.BrowseInfoFlag.BIF_NONEWFOLDERBUTTON;
                    fbdProject.Title = "業務パックフォルダを選択してください";
                    try {
                        if (!fbdProject.ShowDialog()) return;
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string master = fbdProject.FullName + @"\master.xml";
                    if (!File.Exists(master)) {
                        string xls = fbdProject.FullName + @"\master.xls";
                        if (!File.Exists(xls)) {
                            MessageBox.Show("このフォルダは業務パックフォルダではありません");
                            log.Close();
                            return;
                        }
                        XMLConverter xml;
                        try {
                            xml = new XMLConverter(XmlType.Master, xls);
                            xml.Convert();
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message,"エラー",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }
                    }
                    log.Write("Step2");
                    log.Close();
                    Master env_master = env_data.Master;
                    env_master.SelectedFolder = fbdProject.DisplayName;
                    env_data.Master = env_master;
                    frm_sub = new frmSubWindow(env_data, log, OpenWindowType.CREATE_PROJECT);
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                    log.Write("addProjectEnd");
                    log.Close();
                    break;
                //プロジェクト詳細、プロジェクト変更、プロジェクト削除
                case "btnDetailProject":
                case "btnChangeProject":
                case "btnDeleteProject":
                    if (dgvMain.SelectedRows.Count == 0 || dgvMain.SelectedRows[0].Cells[0].Value == null) return;
                    row = GetDataRowByDataGridViewRow("ProjectData", dgvMain.SelectedRows[0]);
                    if (row == null) return;
                    pj = env_data.Project;
                    pj.SelectedFolder = row[2].ToString();
                    env_data.Project = pj;
                    if (btn.Name == "btnDetailProject")
                        AddTab(row);
                    else if (btn.Name == "btnChangeProject") {
                        frm_sub = new frmSubWindow(env_data, log, OpenWindowType.PROJECT_DATA);
                        frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                        frm_sub.Show();
                        this.Enabled = false;
                    } else if (btn.Name == "btnDeleteProject") {
                        frm_sub = new frmSubWindow(env_data, log, OpenWindowType.DELETE_PROJECT);
                        frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                        frm_sub.Show();
                    }
                    break;
                //プロジェクト取り込み
                case "btnGetProject":
                    frm_sub = new frmSubWindow(env_data, log, OpenWindowType.GET_PROJECT);
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                    break;
                //プロジェクト取り出し
                case "btnPutProject":
                    if (dgvMain.SelectedRows.Count == 0) return;
                    if (env_data.Project.TortoiseSVN || env_data.Project.DropBox)
                        ExportProjectData();
                    else
                        MessageBox.Show("この機能は、TortoiseSVNあるいはDropBoxでプロジェクト共有している場合のみ有効です", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                //フォルダ表示
                case "btnViewFolder":
                    if (tabControl.SelectedIndex == 0) {
                        if (dgvMain.SelectedRows.Count == 0 || dgvMain.SelectedRows[0].Cells[0].Value == null) return;
                        string folder = dgvMain.SelectedRows[0].Cells[0].Value.ToString();
                        if (folder == "") return;
                        if (!Directory.Exists(env_data.Project.Folder + folder)) return;
                        Process.Start("explorer.exe", "/e /select," + env_data.Project.Folder + folder);
                    } else
                        Process.Start("explorer.exe", "/e /select," + env_data.Project.Folder + env_data.Project.SelectedFolder);
                    break;
                //終了
                case "btnQuit":
                    CloseApplication();
                    break;
                //タスク追加
                case "btnAddTask":
                    frm_sub = new frmSubWindow(env_data, log, OpenWindowType.ADD_TASK);
                    frm_sub.TaskDataSet = ds_main;
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                    break;
                //タスク詳細
                case "btnTaskDetail":
                    if (dgvMain.SelectedRows.Count == 0) return;
                    vrow_index = dgvMain.SelectedRows[0].Index;
                    if (vrow_index < 0 || vrow_index >= ds_main.Tables["Task"].Rows.Count) return;
                    log.Write("ShowFileBegin");
                    log.Write("Step1");
                    log.Close();
                    row = GetDataRowByDataGridViewRow("Task", dgvMain.Rows[vrow_index]);
                    if (row == null) return;
                    CallWindow(ds_main.Tables["Task"].Rows.IndexOf(row));
                    break;
                //タスク変更
                case "btnTaskChange":
                    if (dgvMain.SelectedRows.Count == 0) return;

                    if (dgvMain.SelectedRows[0].Index < 0 || dgvMain.SelectedRows[0].Index >= ds_main.Tables["Task"].Rows.Count) return;
                    frm_sub = new frmSubWindow(env_data, log, OpenWindowType.CHANGE_TASK);
                    frm_sub.TaskDataSet = ds_main;
                    frm_sub.CurrentTask = ds_main.Tables["Task"].Rows[dgvMain.SelectedRows[0].Index];
                    frm_sub.CurrentTaskRowIndex = dgvMain.SelectedRows[0].Index;
                    frm_sub.FormClosed += new FormClosedEventHandler(frmSubWindow_Closed);
                    frm_sub.Show();
                    this.Enabled = false;
                    break;
                //タスクコピー
                case "btnTaskCopy":
                    int row_index=0;
                    List<int> row_indexes = new List<int>();
                    DataGridViewRow[] rows=new DataGridViewRow[dgvMain.SelectedRows.Count];
                    dgvMain.SelectedRows.CopyTo(rows,0);
                    Array.Sort(rows,delegate(DataGridViewRow row1, DataGridViewRow row2) { return dgvMain.Rows.IndexOf(row1).CompareTo(dgvMain.Rows.IndexOf(row2)); });
                    foreach (DataGridViewRow dr in rows) {
                        Console.WriteLine(dr.Cells[0].Value.ToString());
                        row_index = GetRowIndexByDataViewRow(dr);
                        if (row_index < 0) return;
                        CopyTask(row_index,ref row_indexes);
                    }
                    ShowDetail();
                    dgvMain.ClearSelection();
                    foreach (int idx in row_indexes) {
                        dgvMain.Rows[idx].Selected = true;
                    }
                    break;
                //タスク削除
                case "btnTaskDelete":
                    row_index =0;
                    foreach (DataGridViewRow dr in dgvMain.SelectedRows) {
                        row_index = GetRowIndexByDataViewRow(dr);
                        DeleteTask(row_index);
                    }
                    ShowDetail();
                    break;
                //閉じる
                case "btnExit":
                    result = MessageBox.Show("現在のプロジェクトを閉じますが、よろしいですか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Yes) {
                        tabs.RemoveAt(tabControl.SelectedIndex);
                        ShowProjectData();
                        tabControl.SelectedIndex = 0;
                    }
                    break;
            }
        }

        //検索ボタンのクリック処理
        private void tsbSearch_Click(object sender, EventArgs e) {
            ToolStripButton button = (ToolStripButton)sender;
            button.Checked = !button.Checked;
            if (button.Checked) search_flag = true;
            ShowProjectData();
        }

        //フォームが閉じる際の処理
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            while (tabs.Count > 1) {
                tabs.RemoveAt(tabs.Count - 1);
            
            }
            if (this.Width < 516 || this.Height < 384 || this.Top < 0 || this.Left < 0) InitLocationAndSize();
        }
        #endregion     

    }
}
