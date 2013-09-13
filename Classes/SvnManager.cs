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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace net.docmaker.Classes {
    //Subversion管理
    internal class SvnManager {
        private string props;
        private string propBase;
        private string dotSvn;
        private bool isProps;
        private bool isPropBase;
        private bool isDotSvn;
        private EnvData env;

        internal SvnManager(EnvData env){
            props = Path.GetDirectoryName(env.Documents.Path)+@"\.svn\props\"+Path.GetFileName(env.Documents.Path)+".svn-work";
            propBase = Path.GetDirectoryName(env.Documents.Path) + @"\.svn\prop-base\" + Path.GetFileName(env.Documents.Path) + ".svn-base";
            dotSvn = Path.GetDirectoryName(env.Documents.Path)+@"\.svn";

            isProps =File.Exists(props);
            isPropBase = File.Exists(propBase);
            isDotSvn = File.Exists(dotSvn);

            this.env = env;
        }

        internal void TortoiseSvn(string strTarget, string strCommand, string strSvnPath, string strSvnUrl) {
            string strTSVN;
            string strCOM;
            string strPATH;
            string strURL;
            string strLOG;
            RegistryManager reg = new RegistryManager();

            if ((strTarget=="Project" && env.Project.TortoiseSVN) || (strTarget=="Master" && env.Master.TortoiseSVN)){
                strTSVN = reg.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\TortoiseSVN","ProcPath");
                if (strTSVN==null || strTSVN=="") return;
                strCOM = " /command:" + strCommand + " /notempfile ";
                strPATH=" /path:\"" + strSvnPath+"\"";
                strURL = " /url:\"" + strSvnUrl + "\"";
                strLOG = " /logmsg:\"" + strCommand + "-" + strSvnPath + "-" + strSvnUrl + "\"";
                Process.Start(strTSVN, strCOM + strPATH + strURL + strLOG);
            }
        }
    }
}
