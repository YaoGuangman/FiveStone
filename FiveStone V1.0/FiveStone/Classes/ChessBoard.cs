using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;//
using System.Resources;//

namespace FiveStone
{
    /// <summary>
    /// 棋盘类，该类模拟的是棋盘下棋过程
    /// </summary>
    public class ChessBoard
    {
        //arrChessBoard为棋盘情况数组，arrChessBoard[i,j]=2表示此处无子，arrChessBoard[i,j]=0表示此处为黑子，arrChessBoard[i,j]=1表示此处为白子
        private int[,] arrChessBoard = new int[15, 15];
        //创建绘图板
        private Graphics chessGraphics;
        //棋子对象
        private Stone stone;
        //电脑对象
        private Computer computer;
        //判断当前正要落子的棋子是黑(true)是白(false)，默认是黑色
        private bool stoneFlag = true;
        //判断先手玩家对象是：电脑(true)，人(false)，注意：五子棋标准规定先手下黑棋）
        private bool objectFirstFlag;
        //上一个落子点的坐标信息
        Point point = new Point();
        //记录落子点坐标的堆栈
        private Stack lastDownHistory=new System.Collections.Stack ();
        
        /// <summary>
        /// 构造函数，创建新的游戏
        /// </summary>
        /// <param name="graphics"></param>
        public ChessBoard(Graphics graphics)
        {
            //获取绘图板
            chessGraphics = graphics;
            //开始一局新的比赛
            Start(false);
        }

        /// <summary>
        /// 读取或设置当前棋盘信息的属性
        /// </summary>
        public int[,] ArrChessBoard
        {
            get
            {
                return arrChessBoard;
            }
            set
            {
                arrChessBoard = value;
            }
        }

        /// <summary>
        /// 根据资源文件（或图片文件）绘制一幅新的棋盘
        /// </summary>
        public void ChessBoardDraw()
        {
            //从资源文件中读取黑子，白子图片
            ResourceManager resource = new ResourceManager(typeof(Images));
            Image imgChessboard = (Image)resource.GetObject("ChessBoard");//获取黑子资源

            /*直接读取图片文件方式读取棋盘图像
            Image imgChessboard = System.Drawing.Image.FromFile("Images\\ChessBoard.png");  //棋盘图片 大小600*600像素
             */
            
            //绘制棋盘
            chessGraphics.DrawImage(imgChessboard, 0, 0, imgChessboard.Width, imgChessboard.Height);
        }

        /// <summary>
        /// 开始一局新的比赛
        /// </summary>
        /// <param name="firstFlag">设置先手玩家对象是：电脑(true)，人(false)，注意：五子棋标准规定先手下黑棋</param>
        public void Start(bool firstFlag)
        {
            //清除上一场比赛的位置信息
            point.X = -1; point.Y = -1;
            //清除记录落子点坐标的堆栈为空
            lastDownHistory.Clear();

            //保存先手玩家记录
            objectFirstFlag = firstFlag;
            //构造棋子对象
            stone = new Stone(chessGraphics);
            //构造电脑对象
            computer = new Computer(objectFirstFlag);
            //置当前要走的棋为黑棋（注意：五子棋标准规定先手下黑棋）
            stoneFlag = true;

            //把当前棋盘中所有位置信息清为空（未下子）
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    arrChessBoard[i, j] = 2;
                }
            }
            //绘制新的棋盘
            ChessBoardDraw();

            //如果先手玩家为电脑则电脑先下棋
            if (objectFirstFlag)
            {
                ComputerDownStone();
            }
        }

        /// <summary>
        /// 人下棋
        /// </summary>
        /// <param name="x">鼠标点击棋盘的位置坐标X </param>
        /// <param name="y">鼠标点击棋盘的位置坐标Y </param>
        public void PersonDownStone(int x, int y)
        {
            if (x < 600 && y < 600)
            {
                //将鼠标点击的位置转化为棋盘的下棋点
                int m = (int)Math.Floor((double)x / 40);
                int n = (int)Math.Floor((double)y / 40);
                if (!Rule.IsExist(m, n, arrChessBoard))
                {
                    //人落子
                    bool isGameOver= DownStone(m, n);

                    //在两次落子之间制造时间间隔，以看清楚落子
                    System.Threading.Thread.Sleep(300);

                    //如果游戏还没有分出结果，则电脑可以继续下，否则不能再下了
                    if (!isGameOver)
                    {
                        //电脑接着下棋
                        ComputerDownStone();
                    }
                }
            }
        }

        /// <summary>
        /// 电脑下棋
        /// </summary>
        private void ComputerDownStone()
        {
            int X, Y, count = 0;
            do
            {
                //计算电脑的落子点
                computer.ComputerCalculate(arrChessBoard);
                X = computer.X;
                Y = computer.Y;
                count++;
                if (count > 100)
                {
                    MessageBox.Show("程序异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //重新开始一局比赛
                    Start(objectFirstFlag);
                    return;
                }
            }
            while (Rule.IsExist(X, Y, arrChessBoard));
            //电脑落子
            DownStone(X, Y);
        }

        /// <summary>
        /// 落子
        /// </summary>
        /// <param name="x">落子的位置坐标X </param>
        /// <param name="y">落子的位置坐标Y </param>
        /// <returns>返回游戏是否已经分出胜负，已经分出:true，没有分出：false</returns>
        private bool DownStone(int x, int y)
        {
            //游戏是否已经分出胜负，已经分出:true，没有分出：false
            bool gameOver = false;

            //在棋盘中绘制棋子
            stone.DrawStone(x, y, stoneFlag, point.X, point.Y);

            //记录落子位置的棋子信息
            if (stoneFlag)
            {
                arrChessBoard[x, y] = 0;
            }
            else
            {
                arrChessBoard[x, y] = 1;
            }

            //记录本次落子点的位置为历史记录并将位置保存到堆栈
            point.X = x;
            point.Y = y;
            lastDownHistory.Push(point);

            //判断落子后的胜负情况
            if (Rule.RuleResult(x, y, arrChessBoard) < 6)
            {
                switch (Rule.RuleResult(x, y, arrChessBoard))
                {
                    case 1:
                        MessageBox.Show("黑棋双三禁手犯规，白棋胜利！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 2:
                        MessageBox.Show("黑棋双四禁手犯规，白棋胜利！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 3:
                        MessageBox.Show("黑棋长连禁手犯规，白棋胜利！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 4:
                        if (stoneFlag)
                        {
                            MessageBox.Show("黑棋胜利！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("白棋胜利！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case 5:
                        MessageBox.Show("平局！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
                //设置游戏已经分出胜负
                gameOver = true;

                //重新开始一局比赛
                Start(objectFirstFlag);
            }
            else
            {
                //交换当前棋子颜色，使下次对象的落子为其他的棋子颜色
                stoneFlag = !stoneFlag;
            }

            //返回游戏是否已经分出胜负，已经分出:true，没有分出：false
            return gameOver;
        }

        /// <summary>
        /// 人悔棋
        /// </summary>
        public void PersonUndo()
        {
            //如果棋盘上有大于两个的棋子，则可以悔棋
            if (lastDownHistory.Count > 2)
            {
                //绘制新的棋盘
                ChessBoardDraw();

                //根据记录的堆栈信息分别清除上次人下的棋子和电脑下的棋子
                Point point = (Point)lastDownHistory.Pop();
                arrChessBoard[point.X, point.Y] = 2;
                point = (Point)lastDownHistory.Pop();
                arrChessBoard[point.X, point.Y] = 2;

                //重新绘制棋子信息
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        if (arrChessBoard[i, j] == 0)
                        {
                            //绘制黑色棋子
                            stone.DrawStone(i, j, true);
                        }
                        else if (arrChessBoard[i, j] == 1)
                        {
                            //绘制白色棋子
                            stone.DrawStone(i, j, false);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("已经是最后一步了,将重新开始一局新的游戏！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //重新开始一局比赛
                Start(objectFirstFlag);
            }
        }

        /// <summary>
        /// 电脑帮助玩家下棋
        /// </summary>
        public void ComputerHelpDownStone()
        {
            //新建一个以玩家身份出现的电脑帮助者对象
            Computer computerHelper = new Computer(!objectFirstFlag);
            //电脑帮助者计算落子位置
            int X, Y, count = 0;
            do
            {
                //计算电脑的落子点
                computerHelper.ComputerCalculate(arrChessBoard);
                X = computerHelper.X;
                Y = computerHelper.Y;
                count++;
                if (count > 100)
                {
                    MessageBox.Show("程序异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //重新开始一局比赛
                    Start(objectFirstFlag);
                    return;
                }
            }
            while (Rule.IsExist(X, Y, arrChessBoard));
            //电脑帮助者以玩家身份落子
            bool isGameOver=DownStone(X, Y);

            //如果游戏还没有分出结果，则电脑可以继续下，否则不能再下了
            if (!isGameOver)
            {
                //电脑接着下棋
                ComputerDownStone();
            }
        }
    }
}