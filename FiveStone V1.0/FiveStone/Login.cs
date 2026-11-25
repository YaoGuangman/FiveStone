using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FiveStone
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 读秒进入程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value +=10;
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 阻止关闭操作，如果此时点关闭，则退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                Application.Exit();
            }
        }
    }
}
