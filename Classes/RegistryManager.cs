/*--------------------------------------------
 * 
 *   docmaker.net Ver.2
 *
 *   Producer: Hiroyuki Kawaguchi (Hiro KAWAGUCHI Labo.) 
 *   Author:Kenji Takanashi (HL Will Co.,Ltd.)
 *   Since:2013/10/1
 *   
 *--------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace net.docmaker.Classes {
    //レジストリ管理　（外部ファイルを開く際に使用）
    internal class RegistryManager {
        RegistryKey reg_key;

        internal RegistryManager() {}

        internal string GetApplicationFileByExtension(string ext) {
            reg_key = Registry.CurrentUser.OpenSubKey(
                                String.Format(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\{0}\OpenWithList", ext));
            if (reg_key == null) return "";
            string mru_list = (string)reg_key.GetValue("MRUList");
            if (mru_list == null) return "";
            string app_file = (string)reg_key.GetValue(mru_list.Substring(0, 1));
            if (app_file.IndexOf("dmpm")>=0 && mru_list.Length>1) app_file=(string)reg_key.GetValue(mru_list.Substring(1,1));
            reg_key.Close();
            return app_file;
        }

        internal string GetApplicationFullPathByExtension(string ext) {
            string app_file = GetApplicationFileByExtension(ext);
            string dir = "";
            if (app_file == "") return "";
            reg_key=Registry.ClassesRoot.OpenSubKey(String.Format(@"Applications\{0}\shell\open\command",app_file));
            if (reg_key == null) {
                reg_key = Registry.ClassesRoot.OpenSubKey(String.Format(@"Applications\{0}\shell\edit\command", app_file));
            }
            if (reg_key==null) return "";
            dir = (string)reg_key.GetValue("");
            reg_key.Close();
            string[] dirs = dir.Split('\"');
            foreach (string d in dirs) {
                if (d.Length > 5) { dir = d; break; }
            }
            return dir;
        }

        internal string GetValue(string keyName,string valueName) {
            object val = Registry.GetValue(keyName, valueName, "");
            if (val == null)
                return "";
            else
                return val.ToString();
        }

    }
}
