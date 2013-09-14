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
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;

namespace net.docmaker.Classes {
    //環境設定ファイル管理用
    [XmlType("dmpm")]
    public struct EnvData {
        private Documents doc;      //ドキュメント情報管理
        private Project project;    //プロジェクト情報管理
        private Master master;      //マスタ情報管理
        private Point point;        //ウインドウ位置
        private Size size;          //ウィンドウサイズ
        [XmlIgnore]
        internal const string ENV_FILE_NAME="dmpm.xml";

        public EnvData(Documents documents, Project project, Master master,int top,int left,int width,int height) {
            this.doc = documents;
            this.project = project;
            this.master = master;
            this.point=new Point(left,top);
            this.size = new Size(width, height);
        }

        [XmlElement("Documents")]
        public Documents Documents { get { return doc; } set { doc = value; } }
        [XmlElement("Project")]
        public Project Project { get { return project; } set { project = value; } }
        [XmlElement("Master")]
        public Master Master { get { return master; } set { master = value; } }
        public Point WindowLocation { get { return point; } set { point = value;} }
        public Size WindowSize { get { return size; } set { size = value; } }
        
        public void SaveEnvData() {
            XmlSerializer seri = new XmlSerializer(typeof(EnvData));
            FileStream fs = new FileStream(ENV_FILE_NAME, FileMode.Create);
            seri.Serialize(fs, this);
            fs.Close();
        }
    }

    //ドキュメント情報
    [Serializable]
    public struct Documents{
        private string path;
        private string editor;

        public Documents(string path,string editor) {
            this.path = path;
            this.editor = editor;
        }
        [XmlElement("Path")]
        public String Path { get { return path; } set { path = value; } }

        [XmlElement("Editor")]
        public String Editor { get { return editor; } set { editor = value; } }
    }

    //プロジェクト情報
    [Serializable]
    public struct Project {
        private string folder;
        private bool tortoise;
        private string url;
        private bool dropbox;
        private string s_folder;

        public Project(String folder, bool tortoise, string url, bool dropbox,string selected_folder="") {
            this.folder = folder;
            this.tortoise = tortoise;
            this.url = url;
            this.dropbox = dropbox;
            this.s_folder = selected_folder;
        }
        [XmlElement("Folder")]
        public String Folder { get { return folder; } set { folder = value; } }
        [XmlElement("TortoiseSVN")]
        public bool TortoiseSVN { get { return tortoise; } set { tortoise = value; } }
        [XmlElement("URL")]
        public String URL { get { return url; } set { url = value; } }
        [XmlElement("DropBox")]
        public bool DropBox { get { return dropbox; } set { dropbox = value; } }
        [XmlIgnore]
        public string SelectedFolder { get { return s_folder; } set { s_folder = value; } }
    }

    //マスタ情報
    [Serializable]
    public struct Master {
        private String folder;
        private bool tortoise;
        private string url;
        private string selected_folder;

        public Master(String folder, bool tortoise, string url,string selected_folder="") {
            this.folder = folder;
            this.tortoise = tortoise;
            this.url = url;
            this.selected_folder = selected_folder;
        }
        [XmlElement("Folder")]
        public string Folder { get { return folder; } set { folder = value; } }
        [XmlElement("TortoiseSVN")]
        public bool TortoiseSVN { get { return tortoise; } set { tortoise = value; } }
        [XmlElement("URL")]
        public string URL { get { return url; } set { url = value; } }

        public string SelectedFolder { get { return selected_folder; } set { selected_folder = value; } }
    }
}
