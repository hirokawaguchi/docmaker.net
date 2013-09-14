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
using System.Text;
using System.IO;

namespace net.docmaker.Classes {
    //ログファイル管理用クラス
    internal class LogManager {
        private string f_name; //ログファイル名
        private FileStream stream;
        private StreamWriter sw;

        internal LogManager(bool isCreate) {
            f_name = "dmpm.log";
            if (isCreate) {
                if (File.Exists(f_name)) File.Delete(f_name);
                stream = new FileStream(f_name, FileMode.Create);
                sw = new StreamWriter(stream);
            } else
                Open();
        }

        internal void Open() {
            sw = File.AppendText(f_name);
        }

        internal void Write(string val) {
            if (sw.BaseStream==null) Open();
            sw.WriteLine(val);
        }

        internal void Close() {
            bool flag =(stream!=null && stream.CanWrite) ? true : false;
            sw.Close();
            if (flag) stream.Close();
        }
    }
}
