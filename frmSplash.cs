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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace net.docmaker {
    //スプラッシュウィンドウの表示
    internal partial class frmSplash : Form {
        private Timer timer1;
        private Timer timer2;
        private Timer timer3;
        public frmSplash() {
            InitializeComponent();

            this.Opacity = 0.0d;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            lblVersion.Text = "Ver"
                + assembly.GetName().Version.Major + "."
                + assembly.GetName().Version.Minor + "."
                + assembly.GetName().Version.Build;

            timer1 = new Timer();
            timer1.Interval = 1;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }

        void timer1_Tick(object sender, EventArgs e) {
            if (this.Opacity < 1) {
                this.Opacity += 0.02;
            } else {
                timer1.Stop();
                timer2 = new Timer();
                timer2.Interval = 1500;
                timer2.Tick += new EventHandler(timer2_Tick);
                timer2.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e) {
            timer2.Stop();
            timer3 = new Timer();
            timer3.Interval = 1;
            timer3.Tick += new EventHandler(timer3_Tick);
            timer3.Start();
            
        }

        private void timer3_Tick(object sender, EventArgs e) {
            if (this.Opacity > 0) {
                this.Opacity -= 0.02;
            } else {
                timer3.Stop();
                this.Close();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
