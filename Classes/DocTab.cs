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
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace net.docmaker.Classes {
    // タブ管理用拡張クラス
    internal class DocTab : TabPage {
        private TabInfo ti;

        internal DocTab(TabInfo tab_info, string text = ""): base(text) {
            this.ti = tab_info;
        }

        internal TabInfo TabInfo { get { return ti; } set { ti = value; } }
    }

    //タブ拡張情報保持用構造体
    internal struct TabInfo {
        private bool is_project;
        private string id;
        private string folder;

        internal TabInfo(bool is_project, string id, string folder) {
            this.is_project = is_project;
            this.id = id;
            this.folder = folder;
        }

        internal bool IsProject { get { return is_project; } set { is_project = value; } }
        internal string Id { get { return id; } set { id = value; } }
        internal string SelectedFolder { get { return folder; } set { folder = value; } }
    }
}
