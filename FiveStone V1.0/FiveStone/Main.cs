using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;//
using System.IO;//
using System.Collections;//


namespace FiveStone
{
    public partial class Main : Form
    {
        /// <summary>
        /// 定义棋盘类
        /// </summary>
        private ChessBoard chessBoard;

        /// <summary>
        /// 定义棋子类
        /// </summary>
        private Stone stone;

        /// <summary>
        /// 定义音乐播放
        /// </summary>
        System.Media.SoundPlayer soundPlayer;

        /// <summary>
        /// 当前玩家使用棋子的颜色（用于保存棋谱时记录玩家的颜色）
        /// </summary>
        string playerColor = "玩家黑色";

        public Main()
        {
            InitializeComponent();

            //构造棋盘类，棋子类
            System.Drawing.Graphics g = this.splitContainer1.Panel2.CreateGraphics();
            chessBoard = new ChessBoard(g);
            stone= new Stone(g);//仅用于读取棋谱时绘制棋子
        }

        /// <summary>
        /// 初始化载入棋盘，播放音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            //打开引导页面
            Login login = new Login();
            login.ShowDialog();

            //初始载入默认玩家先
            chessBoard.Start(false);
        }

        /// <summary>
        /// 点击棋盘下棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitContainer1_MouseDown(object sender, MouseEventArgs e)
        {
            //人下棋子
            chessBoard.PersonDownStone(e.X, e.Y);

            //播放资源文件中音频
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            soundPlayer = new System.Media.SoundPlayer(FiveStone.Properties.Resources.DownStone);
            soundPlayer.Play();
        }

        /// <summary>
        /// 重新开始一局，玩家执黑棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //初始载入默认玩家先
            chessBoard.Start(false);

            //设置当前玩家使用棋子的颜色（用于保存棋谱时记录玩家的颜色）
            playerColor = "玩家黑色";

            //显示玩家是黑棋的状态信息
            //从资源文件中读取黑子，白子图片
            ResourceManager resource = new ResourceManager(typeof(Images));
            Image image1 = (Image)resource.GetObject("BlackStone");//获取黑子资源
            Image image2 = (Image)resource.GetObject("WhiteStone");//获取白子资源
            pictureBox1.BackgroundImage = image1;
            pictureBox2.BackgroundImage = image2;
        }

        /// <summary>
        /// 玩家执白棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //初始载入默认玩家先
            chessBoard.Start(true);

            //设置当前玩家使用棋子的颜色（用于保存棋谱时记录玩家的颜色）
            playerColor = "玩家白色";

            //显示玩家是白棋的状态信息
            //从资源文件中读取黑子，白子图片
            ResourceManager resource = new ResourceManager(typeof(Images));
            Image image1 = (Image)resource.GetObject("BlackStone");//获取黑子资源
            Image image2 = (Image)resource.GetObject("WhiteStone");//获取白子资源
            pictureBox1.BackgroundImage = image2;
            pictureBox2.BackgroundImage = image1;
        }

        /// <summary>
        /// 玩家悔棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            chessBoard.PersonUndo();
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            //电脑帮助玩家下一步棋
            chessBoard.ComputerHelpDownStone();

            //播放资源文件中音频
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            soundPlayer = new System.Media.SoundPlayer(FiveStone.Properties.Resources.DownStone);
            soundPlayer.Play();
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            //打开文件前先重新开始一局，以清理棋盘
            chessBoard.Start(false);

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "棋谱文件 Chess Manual(*.cml)|*.cml|All File(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = File.OpenText(openFileDialog1.FileName))
                {
                    playerColor = sr.ReadLine();
                    if (playerColor == "玩家黑色")
                    {
                        //初始载入默认玩家先
                        chessBoard.Start(false);
                    }
                    else
                    {
                        //初始载入默认玩家先
                        chessBoard.Start(true);
                    }

                    String input;
                    while ((input = sr.ReadLine()) != null)
                    {
                        string[] temp = input.Split(',');
                        if (int.Parse(temp[2]) == 0)
                        {
                            stone.DrawStone(int.Parse(temp[0]), int.Parse(temp[1]), true);
                            chessBoard.ArrChessBoard[int.Parse(temp[0]), int.Parse(temp[1])] = int.Parse(temp[2]);
                        }
                        else if (int.Parse(temp[2]) == 1)
                        {
                            stone.DrawStone(int.Parse(temp[0]), int.Parse(temp[1]), false);
                            chessBoard.ArrChessBoard[int.Parse(temp[0]), int.Parse(temp[1])] = int.Parse(temp[2]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "棋谱文件 Chess Manual(*.cml)|*.cml|All File(*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //删除原有文件
                if (File.Exists(saveFileDialog1.FileName))
                {
                    File.Delete(saveFileDialog1.FileName);
                }

                using (StreamWriter sw = File.CreateText(saveFileDialog1.FileName))
                {
                    sw.WriteLine(playerColor);
                    for (int i = 0; i < 15; i++)
                    {
                        for (int j = 0; j < 15; j++)
                        {
                            sw.WriteLine(i + "," + j + "," + chessBoard.ArrChessBoard[i, j]);
                        }
                    }
                }
            }

            //文件保存后重新开始一局，以清理棋盘
            chessBoard.Start(false);
        }

        /// <summary>
        /// 关闭时提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("您是否退出五子棋游戏？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.OK)
            {
                e.Cancel = false;//关闭程序
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 关于我们
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
    }
}
