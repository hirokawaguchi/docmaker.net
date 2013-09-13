/*--------------------------------------------
 * 
 *   DocMeker Ver2
 *   
 *   Author:Kenji Takanashi (HL Will Co.,Ltd.)
 *   Since:2013/10/1
 *   
 *--------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using net.docmaker.Classes;

namespace net.docmaker {
    //サブウインドウの種類
    internal enum OpenWindowType : int {
        ENV_SETTINGS,           //環境設定
        ADD_TASK,               //タスク追加
        CHANGE_TASK,            //タスク情報変更
        CREATE_PROJECT,         //プロジェクト作成
        PROJECT_DATA,           //プロジェクト情報変更
        GET_PROJECT,            //プロジェクト取り込み
        DELETE_PROJECT,         //プロジェクト削除
        CREATE_PROJECT_LIST,    //新規プロジェクトリスト作成
        GYOUMU_PACK,            //業務パックフォルダ設定
        OFFICE_MASTER,          //事業所マスタ設定
        PERSON_MASTER,          //担当者マスタ設定
        CATEGORY_MASTER,        //処理内容マスタ設定
        STATUS_MASTER,          //状態マスタ設定
        VERSION                 //バージョン情報
    }

    //サブウインドウ
    internal partial class frmSubWindow : Form {
        private LogManager log;             //ログ管理
        private EnvData env;                //環境設定
        private string folder;              //プロジェクトフォルダ
        private string master_file;         //マスタファイル
        private string project_no;          //項番
        private OpenWindowType c_window;    //開かれているウインドウの種類
        private DataSet data_set;           //データ格納用
        private DataRow current_task;       //選択されているタスクの情報
        private int current_task_index;     //選択されているタスクのインデックス
        private int wizard_step;           //ウイザードのステップ番号

        //サブウインドウの初期化
        internal frmSubWindow(EnvData en, LogManager lg, OpenWindowType c_window) {
            InitializeComponent();

            this.env = en;
            this.log = lg;
            this.folder = env.Project.Folder;
            Init(c_window);
        }

        //プロパティ
        #region Properties
        internal DataSet TaskDataSet { set { data_set = value; } }
        internal OpenWindowType OpenWindowType { get { return c_window; } }
        internal DataRow CurrentTask {
            set {
                current_task = value;
                tbx01.Text = current_task[0].ToString();
                tbx02.Text = current_task[1].ToString();
                cmb01.Text = current_task[2].ToString();
                lbl01.Text = current_task[3].ToString() == "" ? "" : env.Project.Folder + env.Project.SelectedFolder + @"\" + current_task[3].ToString();
                cmb02.Text = current_task[4].ToString();
                tbxTaskStartDate.Text = current_task[5].ToString();
                dtpTaskStartDate.Value = ChangeToDateTime(current_task[5].ToString());
                tbxEndDate.Text = current_task[6].ToString();
                if (tbxEndDate.Text!="") dtpTaskEndDate.Value = ChangeToDateTime(current_task[6].ToString());
                cmb03.Text = current_task[7].ToString();
            }
        }
        internal int CurrentTaskRowIndex { set { current_task_index = value; } }
        #endregion

        //メソッド
        #region Methods

        //初期化
        private void Init(OpenWindowType c_window) {
            this.c_window = c_window;
            DataTable dt;
            switch (c_window) {
                //環境設定
                case OpenWindowType.ENV_SETTINGS:
                    pnlTaskGet.Visible = false;
                    pnlProjectData.Visible = false;
                    pnlMasterList.Visible = false;
                    pnlMaster.Visible = false;
                    pnlGetProject.Visible = false;
                    pnlDeleteProject.Visible = false;
                    pnlVersion.Visible = false;
                    pnlWizard.Visible = false;
                    pnlWizard02.Visible = false;
                    pnlWizard03.Visible = false;
                    pnlWizard10.Visible = false;
                    pnlWizard20.Visible = false;
                    tbx0101.Text = env.Documents.Path;
                    tbx0201.Text = folder;
                    tbx0301.Text = env.Master.Folder;
                    checkBox1.Checked = env.Project.TortoiseSVN;
                    checkBox2.Checked = env.Master.TortoiseSVN;
                    checkBox3.Checked = env.Project.DropBox;
                    tbx0202.Text = env.Project.URL;
                    tbx0302.Text = env.Master.URL;
                    txtEditor.Text = env.Documents.Editor;
                    if (env.Documents.Editor == "") cbxEditor.Checked = true;

                    RegistryManager reg = new RegistryManager();
                    string txt = reg.GetApplicationFileByExtension(".txt");
                    if (txt.Length == 0) txt = "なし";
                    cbxEditor.Text = String.Format("既定のエディタを使用する （{0}）", txt);
                    break;
                //タスク追加
                case OpenWindowType.ADD_TASK:
                    this.Text = "タスク追加";
                    data_set = new DataSet();
                    pnlTaskGet.Visible = true;
                    tbxTaskStartDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    tbxTaskEndDate.Text = "";
                    SetComboList();

                    break;
                //タスク情報変更
                case OpenWindowType.CHANGE_TASK:
                    this.Text = "タスク情報変更";
                    label8.Text = "タスク情報変更";
                    label9.Text = "タスクの情報を下記のとおり変更します。";
                    btnChange.Text = "変更";
                    btnDeleteFile.Visible = true;
                    pnlTaskGet.Visible = true;
                    SetComboList();
                    break;
                //プロジェクトの作成
                case OpenWindowType.CREATE_PROJECT:
                    wizard_step = 0;
                    this.Text = "プロジェクト作成ウィザード";
                    lblWizardTitle.Text = "申請プロジェクトについて";
                    pnlWizard.Visible = true;
                    if (env.Master.Folder.Substring(env.Master.Folder.Length - 1) != @"\") {
                        Master master = env.Master;
                        master.Folder += @"\";
                        env.Master = master;
                        env.SaveEnvData();
                    }
                    master_file = env.Master.Folder + env.Master.SelectedFolder + @"\master.xml";
                    data_set = new DataSet();
                    data_set.ReadXml(master_file, XmlReadMode.ReadSchema);
                    dt = data_set.Tables["申請一覧表"];
                    foreach (DataRow dr in dt.Rows) {
                        cmbWizard0301.Items.Add(dr[0].ToString());
                    }
                    data_set.ReadXml(env.Documents.Path);
                    dt = data_set.Tables["担当者"];
                    foreach (DataRow dr in dt.Rows) {
                        cmbWizard0302.Items.Add(dr[0].ToString());
                    }
                    break;
                //プロジェクト情報の変更
                case OpenWindowType.PROJECT_DATA:
                    this.Text = "プロジェクト情報変更";
                    pnlProjectData.Visible = true;
                    InitProjectData();
                    break;
                //プロジェクト取り込み
                case OpenWindowType.GET_PROJECT:
                    this.Text = "取り込み元プロジェクト一覧";
                    pnlGetProject.Visible = true;
                    data_set = new DataSet();
                    data_set.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
                    CreateListViewItem();
                    break;
                //プロジェクト削除
                case OpenWindowType.DELETE_PROJECT:
                    this.Text = "プロジェクト情報削除";
                    pnlDeleteProject.Visible = true;
                    data_set = new DataSet();
                    data_set.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
                    DataRow del_dr = data_set.Tables["ProjectData"].Select("フォルダ名=" + env.Project.SelectedFolder)[0];
                    lblProjectNo.Text = del_dr[0].ToString();
                    lblProjectName.Text = del_dr[1].ToString();
                    break;
                //プロジェクト一覧作成
                case OpenWindowType.CREATE_PROJECT_LIST:
                    wizard_step = 0;
                    this.Text = "プロジェクト一覧作成ウィザード";
                    lblWizardTitle.Text = "プロジェクト一覧について";
                    lblWizard01.Text = "docmaker.netでは自分が管理する\n申請案件を一覧にしたファイルを\n「プロジェクト一覧」と呼びます。";
                    lblWizard01.Text += "\n\nこのウィザードでは、新規に空のプロジェクト\n一覧を作成します。";
                    pnlWizard.Visible = true;
                    break;
                //業務パックフォルダ変更
                case OpenWindowType.GYOUMU_PACK:
                    wizard_step = 0;
                    this.Text = "業務パックフォルダ設定ウイザード";
                    lblWizardTitle.Text = "業務パックについて";
                    lblWizard01.Text = "docmaker.net Project Managerでは申請に\n必要な様式、手順をしるしたファイルをまとめた";
                    lblWizard01.Text += "\nものを「業務パック」と呼びます。";
                    lblWizard01.Text += "\n\nこのウイザードでは、お使いのパソコンに業務\nパックを保存するフォルダを設定します。";
                    lblWizard01.Text += "\n\n併せて、インターネット経由で最新の業務\nパックに更新する設定も行うことができます。";
                    tbxWizard02Master.Text = env.Master.Folder;
                    cbxWizard03Master.Checked = env.Master.TortoiseSVN;
                    tbxWizard03Master.Text = env.Master.URL;
                    pnlWizard.Visible = true;
                    break;
                //事業所マスタ変更
                case OpenWindowType.OFFICE_MASTER:
                    this.Text = "事業所マスタ";
                    data_set = new DataSet();
                    try {
                        data_set.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
                    } catch (Exception ex) {
                        throw ex;
                    }
                    if (data_set.Tables.Contains("事業所")) {
                        dt = data_set.Tables["事業所"];
                        if (dt.Rows.Count > 0) {
                            tbxOfficeName.Text = dt.Rows[0][0] == null ? "" : dt.Rows[0][0].ToString();
                            tbxOfficeZip.Text = dt.Rows[0][1] == null ? "" : dt.Rows[0][1].ToString();
                            tbxOfficeAddress.Text = dt.Rows[0][2] == null ? "" : dt.Rows[0][2].ToString();
                            tbxOfficeTel.Text = dt.Rows[0][3] == null ? "" : dt.Rows[0][3].ToString();
                            tbxOfficeFax.Text = dt.Rows[0][4] == null ? "" : dt.Rows[0][4].ToString();
                        } else {
                            dt.Rows.Add(dt.NewRow());
                        }
                    } else {
                        data_set.Tables.Add("事業所");
                        dt = data_set.Tables["事業所"];
                        dt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Zip", Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Address", Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("TEL", Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("FAX", Type.GetType("System.String")));
                        dt.Rows.Add(dt.NewRow());
                    }
                    pnlMaster.Visible = true;
                    break;
                //担当者マスタ変更
                case OpenWindowType.PERSON_MASTER:
                    this.Text = "担当者マスタ";
                    lblOfficeMasterTitle.Text = "担当者マスタ";
                    data_set = new DataSet();
                    try {
                        data_set.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
                    } catch (Exception ex) {
                        throw ex;
                    }
                    foreach (DataRow dr in data_set.Tables["担当者"].Rows) {
                        lviewMaster.Items.Add(new ListViewItem(dr[0].ToString()));
                    }
                    pnlMaster.Visible = true;
                    pnlMasterList.Visible = true;
                    break;
                //処理内容マスタ変更
                case OpenWindowType.CATEGORY_MASTER:
                    this.Text = "処理内容マスタ";
                    lblOfficeMasterTitle.Text = "処理内容マスタ";
                    lblMasterList.Text = "処理内容を選択して、もう一度クリックすると名前を編集できます。";
                    data_set = new DataSet();
                    try {
                        data_set.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
                    } catch (Exception ex) {
                        throw ex;
                    }
                    foreach (DataRow dr in data_set.Tables["タスク種別"].Rows) {
                        lviewMaster.Items.Add(new ListViewItem(dr[0].ToString()));
                    }
                    pnlMaster.Visible = true;
                    pnlMasterList.Visible = true;
                    break;
                //状態マスタ変更
                case OpenWindowType.STATUS_MASTER:
                    this.Text = "状態マスタ";
                    lblOfficeMasterTitle.Text = "状態マスタ";
                    lblMasterList.Text = "状態を選択して、もう一度クリックすると名前を編集できます。";
                    data_set = new DataSet();
                    try {
                        data_set.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
                    } catch (Exception ex) {
                        throw ex;
                    }
                    foreach (DataRow dr in data_set.Tables["状態"].Rows) {
                        lviewMaster.Items.Add(new ListViewItem(dr[0].ToString()));
                    }
                    pnlMaster.Visible = true;
                    pnlMasterList.Visible = true;
                    break;
                //バージョン情報表示
                case OpenWindowType.VERSION:
                    this.Text = "バージョン情報";
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    lblVersion.Text = "Version "
                        + assembly.GetName().Version.Major + "."
                        + assembly.GetName().Version.Minor + "."
                        + assembly.GetName().Version.Build;
                    pnlVersion.Visible = true;
                    break;
            }
        }

        //環境設定ファイルの保存
        private bool SaveEnvData() {
            bool flag = true;
            DialogResult result = MessageBox.Show("この環境設定を保存しますか", "環境設定保存の確認", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                string ext = Path.GetExtension(tbx0101.Text);
                if (ext == "" || !File.Exists(tbx0101.Text)) {
                    flag = false;
                    MessageBox.Show("プロジェクト一覧ファイルが見つかりません", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } else if (ext == ".xls") {
                    string xml = "index.xml";
                    string dir = Directory.GetParent(tbx0101.Text).FullName + "\\";
                    if (!File.Exists(dir + xml)) {
                        try {
                            XMLConverter conv = new XMLConverter(XmlType.Index, tbx0101.Text);
                            conv.Convert();
                            tbx0101.Text = tbx0101.Text.Replace(".xls", ".xml");
                        } catch (Exception ex) {
                            flag = false;
                            MessageBox.Show(ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return flag;
                        }

                    }
                }

                if (!Directory.Exists(tbx0201.Text)) {
                    flag = false;
                    MessageBox.Show("プロジェクト保存フォルダが見つかりません", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (tbx0202.Text.Length > 6 && tbx0202.Text.Substring(0, 4) != "http") {
                    flag = false;
                    MessageBox.Show("プロジェクトのURLが正しくありません", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!cbxEditor.Checked && !File.Exists(txtEditor.Text)) {
                    flag = false;
                    MessageBox.Show("テキストエディタが見つかりません");
                }
                if (!Directory.Exists(tbx0301.Text)) {
                    flag = false;
                    MessageBox.Show("業務パックフォルダが見つかりません", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (tbx0302.Text.Length > 6 && tbx0302.Text.Substring(0, 4) != "http") {
                    flag = false;
                    MessageBox.Show("業務パックのURLが正しくありません", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (flag) {
                    if (cbxEditor.Checked) txtEditor.Text = "";
                    env.Documents = new Documents(tbx0101.Text, txtEditor.Text);
                    env.Project = new Project(tbx0201.Text, checkBox1.Checked, tbx0202.Text, checkBox3.Checked);
                    env.Master = new Master(tbx0301.Text, checkBox2.Checked, tbx0302.Text);
                    env.SaveEnvData();
                }
            }
            return flag;
        }

        //タスクファイルの保存
        private bool SaveTask(int row_index) {
            bool flag = true;
            if (lbl01.Text == "削除") {
                File.Delete(env.Project.Folder + env.Project.SelectedFolder + @"\" + current_task[3].ToString());
                lbl01.Text = "";
            } else if (lbl01.Text != "") {
                string copy_file = env.Project.Folder + env.Project.SelectedFolder + @"\" + Path.GetFileName(lbl01.Text);
                if (!File.Exists(lbl01.Text)) {
                    flag = false;
                    MessageBox.Show("ファイルが見つかりません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    DialogResult result;
                    if (c_window == OpenWindowType.ADD_TASK || c_window==OpenWindowType.CHANGE_TASK) {
                        if (File.Exists(copy_file)) {
                            result = MessageBox.Show("このタスク中に同名のファイルがあります。\n自動的に重複しないファイル名を付与しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes) {
                                Random rnd = new Random();
                                string rnd_str = rnd.Next(100).ToString("D2");
                                copy_file = Path.GetDirectoryName(copy_file) + @"\" + DateTime.Now.ToString("yyyyMMddhhmmss") + rnd_str + Path.GetExtension(lbl01.Text);
                            }

                        } else
                            result = DialogResult.Yes;
                        if (result == DialogResult.Yes) {
                            File.Copy(lbl01.Text, copy_file);
                            lbl01.Text = copy_file;
                        }
                    }
                }
            }
            if (tbx01.Text == "") {
                flag = false;
                MessageBox.Show("項番が正しくありません", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (tbx02.Text == "") {
                flag = false;
                MessageBox.Show("タスク名が正しくありません", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (flag) {
                DataRow dr;
                if (row_index == -1)
                    dr = data_set.Tables["Task"].NewRow();
                else
                    dr = data_set.Tables["Task"].Rows[row_index];

                dr[0] = tbx01.Text;
                dr[1] = tbx02.Text;
                dr[2] = cmb01.SelectedIndex > -1 ? cmb01.Items[cmb01.SelectedIndex] : "";
                dr[3] = lbl01.Text == "" ? "" : Path.GetFileName(lbl01.Text);
                dr[4] = cmb02.SelectedIndex > -1 ? cmb02.Items[cmb02.SelectedIndex] : "";
                dr[5] = tbxTaskStartDate.Text;
                dr[6] =tbxTaskEndDate.Text;
                dr[7] = cmb03.SelectedIndex > -1 ? cmb03.Items[cmb03.SelectedIndex] : "";
                if (dr.ItemArray.Length > 8) {
                    for (int i = 8; i < dr.ItemArray.Length; i++) {
                        dr[i] = 9;
                    }
                }
                if (row_index == -1)
                    data_set.Tables["Task"].Rows.Add(dr);
                data_set.WriteXml(folder + env.Project.SelectedFolder + @"\Task.xml", XmlWriteMode.WriteSchema);
            }
            return flag;
        }

        //プロジェクトファイルの保存
        private bool SaveProject() {
            bool flag = true;
            if (tbxProjectName.Text == "") {
                flag = false;
                MessageBox.Show("プロジェクト名が設定されていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                if (c_window == OpenWindowType.PROJECT_DATA) {
                    data_set.Tables["ProjectData"].Select("項番='" + project_no + "'")[0][1] = tbxProjectName.Text;
                    data_set.Tables["ProjectData"].Select("項番='" + project_no + "'")[0][3] = cmbPerson.Text;
                    data_set.Tables["ProjectData"].Select("項番='" + project_no + "'")[0][4] = tbxStartDate.Text;
                    data_set.Tables["ProjectData"].Select("項番='" + project_no + "'")[0][5] = tbxEndDate.Text;
                    data_set.Tables["ProjectData"].Select("項番='" + project_no + "'")[0][6] = tbxRenewalDate.Text;
                    data_set.Tables["ProjectData"].Select("項番='" + project_no + "'")[0][7] = cmbStatus.Text;
                } else if (c_window == OpenWindowType.GET_PROJECT) {
                    Project pj = env.Project;
                    if (tbxProjectFolder.Text.IndexOf(".dpr") == -1) {
                        Random rnd = new Random();
                        pj.SelectedFolder = DateTime.Now.ToString("yyyyMMddhhmmss") + rnd.Next(100).ToString("d2");
                        env.Project = pj;
                        env.SaveEnvData();

                        Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(tbxProjectFolder.Text, pj.Folder + pj.SelectedFolder, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);
                        if (File.Exists(pj.Folder + pj.SelectedFolder + @"\.svn")) File.Delete(pj.Folder + pj.SelectedFolder + @"\.svn");
                        SvnManager svn = new SvnManager(env);
                        svn.TortoiseSvn("Project", "add", pj.Folder + pj.SelectedFolder, "");
                    } else {
                        try {
                            pj.SelectedFolder = tbxProjectFolder.Text.Split(';')[1];
                            env.Project = pj;
                            env.SaveEnvData();
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        if (data_set.Tables["ProjectData"].Select("項番=" + pj.SelectedFolder).Length > 0) {
                            MessageBox.Show("すでにリスト中に同じ項番のプロジェクトが存在します。\nプロジェクト取込を中止しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    if (tbxProjectName.Text == "") {
                        MessageBox.Show("プロジェクト名を入力してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    } else {
                        DataRow dr = data_set.Tables["ProjectData"].NewRow();
                        dr[0] = env.Project.SelectedFolder;
                        dr[1] = tbxProjectName.Text;
                        dr[2] = env.Project.SelectedFolder;
                        dr[3] = cmbPerson.Text;
                        dr[4] = tbxStartDate.Text;
                        dr[5] = tbxEndDate.Text;
                        dr[6] = tbxRenewalDate.Text;
                        dr[7] = cmbStatus.Text;
                        data_set.Tables["ProjectData"].Rows.InsertAt(dr, 0);
                    }
                }
                data_set.WriteXml(env.Documents.Path, XmlWriteMode.WriteSchema);
            }
            return flag;
        }

        //事業所マスタの保存
        private void SaveOfficeMaster() {
            DataRow dr = data_set.Tables["事業所"].Rows[0];
            dr[0] = tbxOfficeName.Text;
            dr[1] = tbxOfficeZip.Text;
            dr[2] = tbxOfficeAddress.Text;
            dr[3] = tbxOfficeTel.Text;
            dr[4] = tbxOfficeFax.Text;
            data_set.WriteXml(env.Documents.Path, XmlWriteMode.WriteSchema);
        }

        //担当者マスタの保存
        private void SavePersonMaster() {
            data_set.Tables["担当者"].Rows.Clear();
            DataRow dr;
            foreach (ListViewItem item in lviewMaster.Items) {
                dr = data_set.Tables["担当者"].NewRow();
                dr[0] = item.Text;
                data_set.Tables["担当者"].Rows.Add(dr);
            }
            data_set.WriteXml(env.Documents.Path, XmlWriteMode.WriteSchema);
        }

        //処理内容マスタの保存
        private void SaveCategoryMaster() {
            data_set.Tables["タスク種別"].Rows.Clear();
            DataRow dr;
            foreach (ListViewItem item in lviewMaster.Items) {
                dr = data_set.Tables["タスク種別"].NewRow();
                dr[0] = item.Text;
                data_set.Tables["タスク種別"].Rows.Add(dr);
            }
            data_set.WriteXml(env.Documents.Path, XmlWriteMode.WriteSchema);
        }

        //状態マスタの保存
        private void SaveStatusMaster() {
            data_set.Tables["状態"].Rows.Clear();
            DataRow dr;
            foreach (ListViewItem item in lviewMaster.Items) {
                dr = data_set.Tables["状態"].NewRow();
                dr[0] = item.Text;
                data_set.Tables["状態"].Rows.Add(dr);
            }
            data_set.WriteXml(env.Documents.Path, XmlWriteMode.WriteSchema);
        }

        //コンボボックスのリスト表示
        private void SetComboList() {
            DataSet ds = new DataSet();
            ds.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
            DataTable tb = ds.Tables["タスク種別"];
            if (tb != null && tb.Rows.Count > 0) {
                foreach (DataRow dr in tb.Rows) {
                    cmb01.Items.Add(dr[0].ToString());
                }
            }
            tb = ds.Tables["担当者"];
            if (tb != null && tb.Rows.Count > 0) {
                foreach (DataRow dr in tb.Rows) {
                    cmb02.Items.Add(dr[0].ToString());
                }
            }
            tb = ds.Tables["状態"];
            if (tb != null && tb.Rows.Count > 0) {
                foreach (DataRow dr in tb.Rows) {
                    cmb03.Items.Add(dr[0].ToString());
                }
            }
        }

        //プロジェクトの追加
        private void AddProject() {
            log.Write("AddProjectBegin");
            log.Write("Step0");

            data_set = new DataSet();
            data_set.ReadXml(master_file, XmlReadMode.ReadSchema);
            Random rnd = new Random();
            Project pj = env.Project;
            log.Write("Step1");
            pj.SelectedFolder = DateTime.Now.ToString("yyyyMMddhhmmss") + rnd.Next(100).ToString("d2");
            env.Project = pj;
            env.SaveEnvData();
            Directory.CreateDirectory(env.Project.Folder + pj.SelectedFolder);
            log.Write("Step2");
            string strApplicationName = data_set.Tables["申請一覧表"].Rows[cmbWizard0301.SelectedIndex][1].ToString();
            DataTable dt_tasks = data_set.Tables[strApplicationName];
            DataView dv = new DataView(dt_tasks);
            string filter = "";
            string tag = "";
            int idx = 1;
            ComboBox cmb;
            //オプションの情報から、master.xmlをフィルタリングする
            foreach (DataRow dr in data_set.Tables["オプション一覧"].Rows) {
                if (tbcWizard10.TabPages[dr[2].ToString()] == null) { idx++; continue; }
                cmb = (ComboBox)tbcWizard10.TabPages[dr[2].ToString()].Controls[dr[2].ToString()];
                filter += filter.Length > 0 ? " AND " : "";
                tag = tbcWizard10.TabPages[dr[2].ToString()].Tag.ToString();
                filter += "[" +tag  + "] IN(" + (cmb.SelectedIndex + 1).ToString() + ",9) ";
                idx++;
            }
            dv.RowFilter = filter;
            log.Write("Step3");
            //フィルタリングした情報を元にTaskテーブルを作成する
            DataSet ds_new_task = new DataSet();
            ds_new_task.Tables.Add(dv.ToTable());
            ds_new_task.Tables[0].TableName = "Task";
            for (int i = 0; i < ds_new_task.Tables["Task"].Rows.Count; i++) {
                ds_new_task.Tables["Task"].Rows[i][0] = (i + 1) * 10;
                if (ds_new_task.Tables["Task"].Rows[i][4].ToString() == "未定") ds_new_task.Tables["Task"].Rows[i][4] = cmbWizard0302.Text;
            }
            ds_new_task.Tables.Add(CreateNewCompanyTable());
            ds_new_task.WriteXml(env.Project.Folder + pj.SelectedFolder + @"\task.xml", XmlWriteMode.WriteSchema);

            log.Write("Step4");
            log.Write("Step5");

            foreach (DataRow dr in ds_new_task.Tables["Task"].Rows) {
                if (dr[3] != null && dr[3].ToString() != "" && !File.Exists(env.Project.Folder + pj.SelectedFolder + @"\" + dr[3].ToString()))
                    File.Copy(Path.GetDirectoryName(master_file) + @"\" + dr[3].ToString(), env.Project.Folder + pj.SelectedFolder + @"\" + dr[3].ToString());
            }
            SvnManager svn = new SvnManager(env);
            svn.TortoiseSvn("Project", "add", env.Project.Folder + pj.SelectedFolder, "");

            //index.xmlへ追加したプロジェクトの情報を追加する
            log.Write("Step6");
            DataSet ds_index = new DataSet();
            ds_index.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
            DataRow dr_new = ds_index.Tables["ProjectData"].NewRow();
            dr_new[0] = pj.SelectedFolder;
            dr_new[1] = tbxWizard0201.Text + "-" + cmbWizard0301.Text;
            dr_new[2] = pj.SelectedFolder;
            dr_new[3] = cmbWizard0302.Text;
            dr_new[4] = DateTime.Now.ToString("yyyy/MM/dd");
            dr_new[5] = "";
            dr_new[6] = "";
            dr_new[7] = "作業中";
            ds_index.Tables["ProjectData"].Rows.InsertAt(dr_new, 0);
            ds_index.WriteXml(env.Documents.Path, XmlWriteMode.WriteSchema);

            log.Write("Step7");
            log.Write("Step8");
            log.Write("AddProjectEnd");
            log.Close();
            this.Close();
        }

        //新規プロジェクトリストの作成
        private void CreateIndex() {
            log.Write("CreateIndexBEGIN");
            data_set = new DataSet();
            if (File.Exists(env.Documents.Path))
                data_set.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
            else {
                XMLConverter conv = new XMLConverter(XmlType.Index, "");
                data_set = conv.CreateNewDataSet();
            }
            DataSet new_index = data_set.Clone();
            new_index.Tables["ProjectData"].Rows.Clear();
            DataRow dr;
            string[] persons = new string[] { tbxWizard0201.Text, "スタッフ1", "スタッフ2", "未定", "申請者" };
            int i;
            for (i = 0; i < persons.Length; i++) {
                dr = new_index.Tables["担当者"].NewRow();
                dr[0] = persons[i];
                new_index.Tables["担当者"].Rows.Add(dr);
            }
            string[] statuses = new string[] { "作業中", "処理済", "未着手" };
            for (i = 0; i < statuses.Length; i++) {
                dr = new_index.Tables["状態"].NewRow();
                dr[0] = statuses[i];
                new_index.Tables["状態"].Rows.Add(dr);
            }
            string[] kinds = new string[] { "書類作成", "準備書類", "行動" };
            for (i = 0; i < kinds.Length; i++) {
                dr = new_index.Tables["タスク種別"].NewRow();
                dr[0] = kinds[i];
                new_index.Tables["タスク種別"].Rows.Add(dr);
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "プロジェクト一覧を保存";
            sfd.FileName ="index.xml";
            sfd.Filter = "docmakerPM プロジェクト一覧|index.xml";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK) {
                new_index.WriteXml(sfd.FileName, XmlWriteMode.WriteSchema);
                Documents documents = env.Documents;
                documents.Path = sfd.FileName;
                env.Documents = documents;
                env.SaveEnvData();
            }

            log.Write("CreateIndexEND");
            log.Close();
        }

        //業務パックフォルダ情報の設定変更
        private void SetMasterFolder() {
            Master master = env.Master;
            master.Folder = tbxWizard02Master.Text;
            master.TortoiseSVN = cbxWizard03Master.Checked;
            master.URL = tbxWizard03Master.Text;
            env.Master = master;
            env.SaveEnvData();
            string[] directories = Directory.GetDirectories(master.Folder);
            SvnManager svn = new SvnManager(env);
            if (directories.Length > 0)
                svn.TortoiseSvn("Master", "update", master.Folder, "");
             else 
                svn.TortoiseSvn("Master", "checkout", master.Folder, master.URL);
        }

        //プロジェクトの削除
        private bool DeleteProject() {
            bool flag = true;
            DataRow[] rows = data_set.Tables["ProjectData"].Select("項番=" + lblProjectNo.Text);
            if (rows.Length == 0) {
                MessageBox.Show("削除データが見つかりませんでした", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                flag = false;
            } else {
                data_set.Tables["ProjectData"].Rows.Remove(rows[0]);
                data_set.WriteXml(env.Documents.Path, XmlWriteMode.WriteSchema);
                if (cboxDeleteFolder.Checked) {
                    string del_dir = env.Project.Folder + env.Project.SelectedFolder;
                    if (Directory.Exists(del_dir)) {
                        Directory.Delete(del_dir, true);
                    } else {
                        MessageBox.Show("フォルダが見つかりませんでした", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        flag = false;
                    }
                }
            }
            return flag;
        }

        //事業所情報の追加
        private DataTable CreateNewCompanyTable() {
            DataTable dt = new DataTable();
            dt.TableName = "Company";
            dt.Columns.Add("会社名", Type.GetType("System.String"));
            dt.Columns.Add("所在地", Type.GetType("System.String"));
            dt.Columns.Add("代表者役職", Type.GetType("System.String"));
            dt.Columns.Add("代表社名", Type.GetType("System.String"));
            dt.Columns.Add("電話番号", Type.GetType("System.String"));
            dt.Columns.Add("ＦＡＸ番号", Type.GetType("System.String"));
            DataRow dr = dt.NewRow();
            dr[0] = tbxWizard0201.Text;
            dr[1] = tbxWizard0202.Text;
            return dt;
        }

        //プロジェクト情報の初期化
        private void InitProjectData() {
            data_set = new DataSet();
            data_set.ReadXml(env.Documents.Path, XmlReadMode.ReadSchema);
            DataTable dt = data_set.Tables["ProjectData"];
            DataRow[] rows = dt.Select("フォルダ名='" + env.Project.SelectedFolder + "'");
            DataRow p_detail = rows.Length > 0 ? rows[0] : dt.NewRow();
            project_no = p_detail[0].ToString();
            tbxProjectName.Text = p_detail[1].ToString();
            foreach (DataRow dr in data_set.Tables["担当者"].Rows) {
                cmbPerson.Items.Add(dr[0].ToString());
            }
            cmbPerson.Text = p_detail[3].ToString();
            tbxStartDate.Text = p_detail[4].ToString();
            tbxEndDate.Text = p_detail[5].ToString();
            tbxRenewalDate.Text = p_detail[6].ToString();
            foreach (DataRow dr in data_set.Tables["状態"].Rows) {
                cmbStatus.Items.Add(dr[0].ToString());
            }
            cmbStatus.Text = p_detail[7].ToString();
            TextBox[] tbxs = new TextBox[] { tbxStartDate, tbxEndDate, tbxRenewalDate };
            foreach (TextBox tbx in tbxs) {
                switch (tbx.Name) {
                    case "tbxStartDate":
                        dtpStartDate.Value =ChangeToDateTime(tbx.Text);
                        break;
                    case "tbxEndDate":
                        dtpEndDate.Value = ChangeToDateTime(tbx.Text);
                        break;
                    case "tbxRenewalDate":
                        dtpRenewalDate.Value = ChangeToDateTime(tbx.Text);
                        break;
                }
            }
        }

        //文字列をDateTime型に変更する
        private DateTime ChangeToDateTime(string value) {
            if (value == "") return DateTime.Now;
            if (value.IndexOf('/') < 0) return DateTime.Now;
            string[] dates = value.Split('/');
            int yy, mm, dd;
            if (dates.Length != 3) return DateTime.Now;
            if (!int.TryParse(dates[0], out yy)) yy = DateTime.Now.Year;
            if (!int.TryParse(dates[1], out mm)) mm = DateTime.Now.Month;
            if (dates[2].Length > 2) dates[2] = dates[2].Substring(0, 2);
            if (!int.TryParse(dates[2], out dd)) dd = DateTime.Now.Day;
            return new DateTime(yy, mm, dd);
        }

        //プロジェクト情報を取り込む
        private void GetProjectFolder() {
            log.Write("GetProjectFolderBEGIN");

            string strProjectFolder;
            string strProjectName;
            string strCopyFromProjectName;
            if (lviewProjectList.SelectedItems.Count > 0) {
                strProjectFolder = env.Project.Folder + lviewProjectList.SelectedItems[0].SubItems[2].Text;
                strProjectName = lviewProjectList.SelectedItems[0].SubItems[1].Text;
            } else {
                strProjectFolder = tbxProjectFolder.Text;
                strProjectName = "";
            }

            if (strProjectFolder.IndexOf(".dpr") > 0) {
                tbxProjectFolder.Text = strProjectFolder;
                strCopyFromProjectName = "未設定";
            } else {
                log.Write("Step2");
                if (!File.Exists(strProjectFolder + @"\task.xml") && !File.Exists(strProjectFolder + @"\task.xls")) {
                    MessageBox.Show("取り込み元のプロジェクトデータが存在しません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Close();
                    return;
                }
                tbxProjectFolder.Text = strProjectFolder;
                strCopyFromProjectName = strProjectName;
                Project pj = env.Project;
                pj.SelectedFolder =Path.GetFileName(strProjectFolder);
                env.Project = pj;
                env.SaveEnvData();
            }
            pnlGetProject.Visible = false;
            InitProjectData();
            this.Text = "プロジェクト情報取り込み";
            label26.Text = "プロジェクト情報取り込み";
            label28.Text = "プロジェクトの情報を外部フォルダ／ファイルから取り込み、下記のとおり設定します。";
            btnChange2.Text = "取り込み";

            if (tbxProjectFolder.Text.IndexOf(".dpr") > 0) {
                DataSet ds_project = new DataSet();
                if (tbxProjectFolder.Text.IndexOf(".dprx") > 0) {
                    ds_project.ReadXml(tbxProjectFolder.Text, XmlReadMode.ReadSchema);
                } else {
                    XMLConverter cvtr = new XMLConverter(XmlType.Dpr, tbxProjectFolder.Text);
                    cvtr.Convert(out ds_project);
                }
                DataRow dr;
                try {
                    dr = ds_project.Tables["ProjectData"].Rows[0];
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Close();
                    return;
                }
                if (ds_project.Tables["ProjectData"].Columns.Count < 8) {
                    MessageBox.Show("ファイルが正しくありません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Close();
                    return;
                }
                tbxProjectName.Text = dr[1].ToString();
                label28.Text += "\n元フォルダ:" + dr[2].ToString();
                tbxProjectFolder.Text += ";" + dr[2].ToString();
                cmbPerson.Text = dr[3].ToString();
                tbxStartDate.Text = dr[4].ToString();
                tbxEndDate.Text = dr[5].ToString();
                tbxRenewalDate.Text = dr[6].ToString();
                cmbStatus.Text = dr[7].ToString();
                ds_project.Dispose();
            } else {
                label28.Text += "\n元ファイル:" + tbxProjectFolder.Text;
                tbxProjectName.Text = "（コピー）" + strCopyFromProjectName;
            }
            pnlProjectData.Visible = true;

            log.Write("GetProjectFolderEND");
            log.Close();
        }

        //プロジェクト取り込みの際に表示される、プロジェクトリストの作成
        private void CreateListViewItem() {
            lviewProjectList.Items.Clear();
            ListViewItem lvi;
            foreach (DataRow dr in data_set.Tables["ProjectData"].Rows) {
                lvi = new ListViewItem();
                lvi.Text = dr[0].ToString();
                lvi.SubItems.Add(dr[1].ToString());
                lvi.SubItems.Add(dr[2].ToString());
                lviewProjectList.Items.Add(lvi);
            }
        }
        #endregion


        //イベント処理
        #region Events

        //新規プロジェクトリストウィザードを表示するためのボタン処理
        private void btnNewProjectList_Click(object sender, EventArgs e) {
            Init(OpenWindowType.CREATE_PROJECT_LIST);
            
        }

        //業務パックフォルダ設定ウィザードを表示する為のボタン
        private void btnSetGFolder_Click(object sender, EventArgs e) {
            Init(OpenWindowType.GYOUMU_PACK);
        }

        //環境設定画面でプロジェクトファイルを取得する為のボタン処理
        private void btnSelectFile_Click(object sender, EventArgs e) {
            log.Write("getProjectFolderBEGIN " + DateTime.Now);
            log.Write("Step1 " + DateTime.Now);
            log.Close();

            DialogResult result;
            ofd0101.Filter = "docmakerPMプロジェクト (index.xml;index.xls)|*.xml;*.xls";
            ofd0101.FilterIndex = 1;
            ofd0101.FileName = "index.xml";
            ofd0101.Title = "プロジェクト一覧ファイルを開く";
            result = ofd0101.ShowDialog();
            if (result == DialogResult.OK) {
                tbx0101.Text = ofd0101.FileName;
                if (folder.Length == 0) {
                    DirectoryInfo dinf = Directory.GetParent(ofd0101.FileName);
                    folder = dinf.FullName;
                }
            }
        }

        //環境設定画面でエディタを取得する為のボタン処理
        private void btnSelectEditor_Click(object sender, EventArgs e) {
            log.Write("getTextEditorBEGIN " + DateTime.Now);
            log.Write("Step1 " + DateTime.Now);
            log.Close();

            DialogResult result;
            ofd0101.Filter = "テキストエディタ(*.exe)|*.exe";
            ofd0101.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            ofd0101.Title = "テキストエディタを選択する";
            result = ofd0101.ShowDialog();
            if (result == DialogResult.OK) {
                txtEditor.Text = ofd0101.FileName;
                if (txtEditor.Text != "") cbxEditor.Checked = false;
            }
        }

        //環境設定画面で、プロジェクトフォルダを取得する為のボタン処理
        private void btnSelectFolder_Click(object sender, EventArgs e) {
            DialogResult result;
            fbd0201.SelectedPath = folder;
            result = fbd0201.ShowDialog();
            if (result == DialogResult.OK) {
                tbx0201.Text = fbd0201.SelectedPath + "\\";
            }
        }

        //環境設定画面で、業務パックフォルダを取得する為のボタン処理
        private void btnSelectGFolder_Click(object sender, EventArgs e) {
            DialogResult result;
            fbd0201.SelectedPath = folder;
            result = fbd0201.ShowDialog();
            if (result == DialogResult.OK) {
                tbx0301.Text = fbd0201.SelectedPath + "\\";
            }
        }

        //実行ボタンの処理
        private void btnSubmit_Click(object sender, EventArgs e) {
            switch (c_window) {
                //環境設定
                case OpenWindowType.ENV_SETTINGS:
                    if (SaveEnvData()) this.Close();
                    break;
                //タスクの追加
                case OpenWindowType.ADD_TASK:
                    if (SaveTask(-1)) this.Close();
                    break;
                //タスクの変更
                case OpenWindowType.CHANGE_TASK:
                    if (SaveTask(current_task_index)) this.Close();
                    break;
                //プロジェクトの変更
                case OpenWindowType.PROJECT_DATA:
                    if (SaveProject()) this.Close();
                    break;
                //プロジェクトの取り込み
                case OpenWindowType.GET_PROJECT:
                    if (pnlGetProject.Visible) {
                        if (tbxProjectFolder.Text == "") {
                            MessageBox.Show("取り込み元が選択されていません", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        } else
                            GetProjectFolder();
                    } else if (pnlProjectData.Visible) {
                        if (SaveProject()) this.Close();
                    }
                    break;
                //プロジェクトの削除
                case OpenWindowType.DELETE_PROJECT:
                    if (DeleteProject()) this.Close();
                    break;
                //事業所マスタ変更
                case OpenWindowType.OFFICE_MASTER:
                    if (MessageBox.Show("このデータを保存しますか？","確認",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                        SaveOfficeMaster();
                    this.Close();
                    break;
                //担当者マスタ変更
                case OpenWindowType.PERSON_MASTER:
                    if (MessageBox.Show("担当者情報を保存しますか？","確認",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                        SavePersonMaster();
                    this.Close();
                    break;
                //処理内容マスタ変更
                case OpenWindowType.CATEGORY_MASTER:
                    if (MessageBox.Show("処理内容情報を保存しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        SaveCategoryMaster();
                    this.Close();
                    break;
                //状態マスタ変更
                case OpenWindowType.STATUS_MASTER:
                    if (MessageBox.Show("状態情報を保存しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        SaveStatusMaster();
                    this.Close();
                    break;
            }
        }

        //タスク情報画面で、外部ファイルを取得する為のボタン処理
        private void btnAddFile_Click(object sender, EventArgs e) {
            DialogResult result;
            ofd0101.Filter = "外部ファイル (*.*)|*.*";
            ofd0101.FilterIndex = 1;
            ofd0101.InitialDirectory = folder + env.Project.SelectedFolder;
            ofd0101.FileName = "";
            ofd0101.Title = "外部ファイルを選択する";
            result = ofd0101.ShowDialog();
            if (result == DialogResult.OK) {
                lbl01.Text = ofd0101.FileName;
            }

        }

        //タスク情報画面で、外部ファイルを削除するためのボタン処理
        private void btnDeleteFile_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("タスクの関連ファイルを削除します", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes) lbl01.Text = "削除";
        }

        //ウイザード画面での実行ステップ処理「次へ」「戻る」ボタン処理
        private void btnWizardStep_Click(object sender, EventArgs e) {
            Button clicked = (Button)sender;
            //次へボタン
            if (clicked.Name == "btnNext") {
                string mess = "";
                if (wizard_step == 1 && tbxWizard0201.Text == "" && (c_window == OpenWindowType.CREATE_PROJECT || c_window == OpenWindowType.CREATE_PROJECT_LIST)) {
                    mess = c_window == OpenWindowType.CREATE_PROJECT ? "申請社名" : "担当者名";
                    mess += "を入力してください";
                } else if (wizard_step == 1 && !Directory.Exists(tbxWizard02Master.Text) && c_window == OpenWindowType.GYOUMU_PACK) {
                    mess = "業務パックフォルダを選択してください";
                } else if (wizard_step == 2 && cmbWizard0301.SelectedIndex < 0 && c_window==OpenWindowType.CREATE_PROJECT) {
                    mess = "申請案件を選択してください";
                } else if (wizard_step == 2 && cmbWizard0302.SelectedIndex < 0 && c_window==OpenWindowType.CREATE_PROJECT_LIST) {
                    mess = "担当者を選択してください";
                } else if (wizard_step == 3) {
                    data_set = new DataSet();
                    data_set.ReadXml(master_file, XmlReadMode.ReadSchema);
                    ComboBox cmb = new ComboBox();
                    bool flag = false;
                    foreach (DataRow dr in data_set.Tables["オプション一覧"].Rows) {
                        if (tbcWizard10.TabPages[dr[2].ToString()] == null) continue;
                        cmb = (ComboBox)tbcWizard10.TabPages[dr[2].ToString()].Controls[dr[2].ToString()];
                        if (cmb.SelectedIndex == -1) {
                            flag = true;
                            MessageBox.Show(dr[2].ToString() + "を選択してください", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    if (flag) return;
                }
                if (mess != "") { MessageBox.Show(mess); return; }

            }

            if (clicked.Name == "btnPrev") {//戻るボタンクリック
                wizard_step--;
                if (wizard_step == 3 && c_window == OpenWindowType.CREATE_PROJECT_LIST) wizard_step = 1;
                else if (wizard_step == 3 && c_window == OpenWindowType.GYOUMU_PACK) wizard_step = 2;
            } else if (clicked.Name == "btnNext") {//次へボタン
                wizard_step++;
                if (wizard_step == 2 && c_window == OpenWindowType.CREATE_PROJECT_LIST) wizard_step = 4;
                else if (wizard_step == 3 && c_window == OpenWindowType.GYOUMU_PACK) wizard_step = 4;

            } else return;

            pnlWizard02.Visible = false;
            pnlWizard03.Visible = false;
            pnlWizard10.Visible = false;
            pnlWizard20.Visible = false;
            pnlWizard02Master.Visible = false;
            pnlWizard03Master.Visible = false;
            switch (wizard_step) {
                case 0:
                    if (c_window == OpenWindowType.CREATE_PROJECT) {    //プロジェクト作成
                        lblWizardTitle.Text = "申請プロジェクトについて";
                        lblWizard01.Text = "docmaker.netではひとつの申請案件のことを\n「申請プロジェクト」と呼びます。";
                        lblWizard01.Text += "\n\nこのウィザードの質問に順に答えることにより、\n申請プロジェクトを自動的に作成し追加する\nことができます。";
                    } else if (c_window == OpenWindowType.CREATE_PROJECT_LIST) {    //プロジェクトリスト作成
                        lblWizardTitle.Text = "プロジェクト一覧について";
                        lblWizard01.Text = "docmaker.netでは自分が管理する\n申請案件を一覧にしたファイルを\n「プロジェクト一覧」と呼びます。";
                        lblWizard01.Text += "\n\nこのウィザードでは、新規に空のプロジェクト\n一覧を作成します。";
                    } else if (c_window == OpenWindowType.GYOUMU_PACK) {    //業務パックフォルダ設定
                        this.Text = "業務パックフォルダ設定ウイザード";
                        lblWizardTitle.Text = "業務パックについて";
                        lblWizard01.Text = "docmaker.net Project Managerでは申請に\n必要な様式、手順をしるしたファイルをまとめた";
                        lblWizardTitle.Text += "ものを「業務パック」と呼びます。";
                        lblWizard01.Text += "\n\nこのウイザードでは、お使いのパソコンに業務\nパックを保存するフォルダを設定します。";
                        lblWizard01.Text += "\n\n併せて、インターネット経由で最新の業務\nパックに更新する設定も行うことができます。";
                    } else
                        return;
                    btnPrev.Enabled = false;
                    pictureBox1.Image = Properties.Resources.wizard01;
                    break;
                case 1:
                    if (c_window == OpenWindowType.CREATE_PROJECT) {    //プロジェクト作成
                        lblWizardTitle.Text = "申請者の概要の入力";
                        lblWizard02.Text = "新規に追加したい申請プロジェクトの\nための申請者の概要を下記に入力して\nください。";
                        label20.Text = "申請者";
                        label21.Visible = true;
                        tbxWizard0202.Visible = true;
                    } else if (c_window==OpenWindowType.CREATE_PROJECT_LIST) {  //プロジェクトリスト作成
                        lblWizardTitle.Text = "担当者の概要の入力";
                        lblWizard02.Text = "新規に作成したいプロジェクト一覧に\n登録する担当者の概要を下記に\n入力してください。";
                        lblWizard02.Text += "\n\nこの情報は「担当者マスタを編集」\nメニューにより、後から変更できます。";
                        label20.Text = "担当者";
                        label21.Visible = false;
                        tbxWizard0202.Visible = false;

                    } else if (c_window == OpenWindowType.GYOUMU_PACK) {    //業務パックフォルダ設定
                        lblWizardTitle.Text = "業務パックフォルダの設定";
                    }
                    btnPrev.Enabled = true;
                    pictureBox1.Image = Properties.Resources.wizard02;
                    if (c_window == OpenWindowType.CREATE_PROJECT || c_window == OpenWindowType.CREATE_PROJECT_LIST)
                        pnlWizard02.Visible = true;
                    else if (c_window == OpenWindowType.GYOUMU_PACK)
                        pnlWizard02Master.Visible = true;
                    break;
                case 2:
                    if (c_window == OpenWindowType.CREATE_PROJECT) {    //プロジェクト作成
                        lblWizardTitle.Text = "申請案件の選択";
                        pnlWizard03.Visible = true;
                    } else if (c_window == OpenWindowType.GYOUMU_PACK) {    //業務パックフォルダ設定
                        lblWizardTitle.Text = "インターネット経由で更新する場合の設定";
                        pnlWizard03Master.Visible = true;
                    }
                    btnPrev.Enabled = true;
                    pictureBox1.Image = Properties.Resources.wizard03;
                    break;
                case 3:
                    lblWizardTitle.Text = "プロジェクト内容の設定 " + tbxWizard0201.Text;
                    btnPrev.Enabled = true;
                    pictureBox1.Image = Properties.Resources.wizard10;
                    pnlWizard10.Visible = true;

                    data_set = new DataSet();
                    data_set.ReadXml(master_file, XmlReadMode.ReadSchema);
                    DataTable dt = data_set.Tables["申請一覧表"];
                    DataTable dt_option;
                    string strApplicationName = dt.Rows[cmbWizard0301.SelectedIndex][1].ToString();
                    string strCaption11;
                    string strCaption12;
                    int intOptionCount;
                    if (!int.TryParse(dt.Rows[cmbWizard0301.SelectedIndex][2].ToString(), out intOptionCount)) intOptionCount = 0;
                    int intOptionNumber;
                    int j;
                    Label lblWizard10_11;
                    Label lblWizard10_12;
                    ComboBox cmbWizard10_1;
                    dt = data_set.Tables[strApplicationName];
                    dt_option = data_set.Tables["オプション一覧"];
                    tbcWizard10.TabPages.Clear();
                    for (var i = 1; i <= intOptionCount; i++) {
                        if (!int.TryParse(dt.Columns[7 + i].ColumnName, out intOptionNumber)) intOptionNumber = 0;
                        strCaption11 = dt_option.Rows[intOptionNumber - 1][1].ToString();
                        strCaption12 = dt_option.Rows[intOptionNumber - 1][2].ToString();

                        tbcWizard10.TabPages.Add(strCaption12, strCaption12);
                        tbcWizard10.TabPages[strCaption12].Select();
                        tbcWizard10.TabPages[strCaption12].Tag = intOptionNumber;
                        lblWizard10_11 = new Label();
                        tbcWizard10.TabPages[strCaption12].Controls.Add(lblWizard10_11);
                        lblWizard10_11.Location = new Point(6, 12);
                        lblWizard10_11.AutoSize = true;
                        lblWizard10_11.Font = new Font("MS UI Gothic", 11);
                        lblWizard10_11.Text = strCaption11;

                        lblWizard10_12 = new Label();
                        tbcWizard10.TabPages[strCaption12].Controls.Add(lblWizard10_12);
                        lblWizard10_12.Location = new Point(6, 100);
                        lblWizard10_12.AutoSize = true;
                        lblWizard10_12.Font = new Font("MS UI Gothic", 11);
                        lblWizard10_12.Text = strCaption12;

                        cmbWizard10_1 = new ComboBox();
                        tbcWizard10.TabPages[strCaption12].Controls.Add(cmbWizard10_1);
                        cmbWizard10_1.Location = new Point(12, 123);
                        cmbWizard10_1.Size = new Size(280, 18);
                        cmbWizard10_1.Font = new Font("MS UI Gothic", 11);
                        cmbWizard10_1.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbWizard10_1.Name = strCaption12;

                        j = 0;
                        while (dt_option.Rows[intOptionNumber - 1][3 + j].ToString() != "") {
                            cmbWizard10_1.Items.Add(dt_option.Rows[intOptionNumber - 1][3 + j].ToString());
                            j++;
                        }

                    }
                    break;
                case 4:
                    string[] lines;
                    if (c_window == OpenWindowType.CREATE_PROJECT) {        //プロジェクト作成
                        lblWizardTitle.Text = "プロジェクト内容の確定";
                        lblWizard20.Text = lblWizard20.Text.Replace("プロジェクト一覧の", "プロジェクトの");                        
                        data_set = new DataSet();
                        data_set.ReadXml(master_file, XmlReadMode.ReadSchema);
                        lines = new string[data_set.Tables["オプション一覧"].Rows.Count + 3];
                        lines[0] = "申請者名 ＝ " + tbxWizard0201.Text;
                        lines[1] = "申請案件名 ＝ " + cmbWizard0301.Text;
                        lines[2] = "担当者名 ＝ " + cmbWizard0302.Text;
                        data_set = new DataSet();
                        data_set.ReadXml(master_file, XmlReadMode.ReadSchema);
                        ComboBox cmb;
                        foreach (DataRow dr in data_set.Tables["オプション一覧"].Rows) {
                            if (tbcWizard10.TabPages[dr[2].ToString()] == null) continue;
                            cmb = (ComboBox)tbcWizard10.TabPages[dr[2].ToString()].Controls[dr[2].ToString()];
                            lines[(int)dr[0] + 2] = dr[2].ToString() + " ＝ " + cmb.Text;
                        }
                    } else if (c_window==OpenWindowType.CREATE_PROJECT_LIST) {　//プロジェクトリスト作成
                        lblWizardTitle.Text = "プロジェクト一覧の内容の確定";
                        lblWizard20.Text = lblWizard20.Text.Replace("プロジェクトの", "プロジェクト一覧の");
                        lines = new string[1];
                        lines[0] = "担当者名＝" + tbxWizard0201.Text;

                    } else if (c_window == OpenWindowType.GYOUMU_PACK) {        //業務パックフォルダ設定
                        lblWizardTitle.Text = "設定内容の確定";
                        lines = new string[3];
                        lines[0] = "業務パックフォルダ＝" + tbxWizard02Master.Text;
                        lines[1] = "TortoiseSVNを";
                        lines[1]+=cbxWizard03Master.Checked? "使用する" : "使用しない";
                        lines[2] = cbxWizard03Master.Checked? "ULR="+tbxWizard03Master.Text:"";
                    }else
                        lines=new string[]{""};
                    btnPrev.Enabled = true;
                    pictureBox1.Image = Properties.Resources.wizard20;
                    pnlWizard20.Visible = true;

                    tbxProjectDetail.Lines = lines;
                    break;
                case 5:
                    if (c_window == OpenWindowType.CREATE_PROJECT)              //プロジェクト作成
                        AddProject();
                    else if (c_window == OpenWindowType.CREATE_PROJECT_LIST) {    //プロジェクトリスト作成
                        CreateIndex();
                        Init(OpenWindowType.ENV_SETTINGS);
                    } else if (c_window == OpenWindowType.GYOUMU_PACK) {            //業務パックフォルダ設定
                        SetMasterFolder();
                        Init(OpenWindowType.ENV_SETTINGS);
                    }
                    break;

            }
        }

        //日付設定ボタン(DateTimePicker）の値が変化した際の処理
        private void DateTimePicker_ValueChanged(object sender, EventArgs e) {
            DateTimePicker dtp = (DateTimePicker)sender;
            string date = dtp.Value.ToString("yyyy/MM/dd");
            switch (dtp.Name) {
                case "dtpStartDate":
                    tbxStartDate.Text = date;
                    break;
                case "dtpEndDate":
                    tbxEndDate.Text = date;
                    break;
                case "dtpRenewalDate":
                    tbxRenewalDate.Text = date;
                    break;
                case "dtpTaskStartDate":
                    tbxTaskStartDate.Text = date;
                    break;
                case "dtpTaskEndDate":
                    tbxTaskEndDate.Text = date;
                    break;
            }
        }

        //プロジェクトリストビューの選択行が変化した際の処理
        private void lviewProjectList_SelectedIndexChanged(object sender, EventArgs e) {
            ListView lview = (ListView)sender;
            if (lview.SelectedIndices.Count == 0) return;
            Project project = env.Project;
            project.SelectedFolder = lview.Items[lview.SelectedIndices[0]].SubItems[2].Text;
            tbxProjectFolder.Text = project.Folder + project.SelectedFolder;
            env.Project = project;
        }

        //プロジェクトリストビューの行をダブルクリックした際の処理
        private void lviewProjectList_MouseDoubleClick(object sender, MouseEventArgs e) {
            lviewProjectList_SelectedIndexChanged(sender, new EventArgs());
            GetProjectFolder();
        }

        //プロジェクト取り込みで「外部ファイル」ボタンをクリックした処理
        private void btnOutFolder_Click(object sender, EventArgs e) {
            fbd0201.Description = "プロジェクトとして取り込みたいフォルダを選択してください";
            fbd0201.ShowNewFolderButton = false;
            DialogResult result = fbd0201.ShowDialog();
            if (result == DialogResult.OK) {
                tbxProjectFolder.Text = fbd0201.SelectedPath;
            }
        }

        //プロジェクト取り込みで「プロジェクト情報」ボタンをクリックした処理
        private void btnProjectInfo_Click(object sender, EventArgs e) {
            if (env.Project.TortoiseSVN || env.Project.DropBox) {
                ofd0101.Filter = "docmakerPM プロジェクト情報(*.dpr;*.dprx)|*.dpr;*.dprx";
                ofd0101.Title = "プロジェクト情報ファイルを開く";
                ofd0101.Multiselect = false;
                ofd0101.InitialDirectory = Directory.GetCurrentDirectory();
                DialogResult result = ofd0101.ShowDialog();
                if (result == DialogResult.OK) {
                    tbxProjectFolder.Text = ofd0101.FileName;
                }
            } else {
                MessageBox.Show("この機能は、TortoiseSVNあるいはDropBoxでプロジェクト共有している場合にのみ有効です。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //業務パックフォルダ設定画面で、「選択」ボタンをクリックした処理
        private void btnSelectGPack_Click(object sender, EventArgs e) {
            fbd0201 = new FolderBrowserDialog();
            fbd0201.Description = "業務パックフォルダを選択してください";
            fbd0201.SelectedPath = tbxWizard02Master.Text;
            if (fbd0201.ShowDialog() == DialogResult.OK) {
                tbxWizard02Master.Text = fbd0201.SelectedPath;
            }
        }

        //担当者マスタ、処理内容マスタ、状態マスタで「追加」、「削除」ボタンを押した処理
        private void btnMasterListView_Click(object sender, EventArgs e) {
            Button button = (Button)sender;
            if (button.Name == "btnAddMaster"){
                string item="";
                switch(c_window){
                    case OpenWindowType.PERSON_MASTER:
                        item="○○　○○";
                        break;
                    case OpenWindowType.CATEGORY_MASTER:
                        item="書類作成";
                        break;
                    case OpenWindowType.STATUS_MASTER:
                        item="作業中";
                        break;
                }
                lviewMaster.Items.Add(new ListViewItem(item));
            } else if (button.Name == "btnDelMaster") {
                if (lviewMaster.SelectedItems.Count > 0) lviewMaster.SelectedItems[0].Remove();
            }
        }

        //キャンセルボタンをクリックした処理
        private void btnCancel_Click(object sender, EventArgs e) {
            if (c_window == OpenWindowType.CREATE_PROJECT_LIST || c_window == OpenWindowType.GYOUMU_PACK)
                Init(OpenWindowType.ENV_SETTINGS);
            else
                this.Close();
        }
        #endregion

    }
}
