using System;

namespace FiveStone
{
    /// <summary>
    /// 电脑类，计算电脑下棋的位置。
    /// </summary>
    public class Computer
    {
        //电脑当前所使用的是否是黑棋（true），还是白棋（false）
        private bool isBlackFlag;
        //电脑计算的下子的位置坐标的X和Y
        private int x, y;

        /// <summary>
        /// 构造函数，指明当前电脑使用的是否是黑棋（true），还是白棋（false）
        /// </summary>
        /// <param name="stoneFlag">构造的棋子颜色类型：黑棋（true），白棋（false）</param>
        public Computer(bool stoneFlag)
        {
            isBlackFlag = stoneFlag;
        }

        /// <summary>
        /// 返回电脑计算的落子点的坐标X
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// 返回电脑计算的落子点的坐标Y
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// 电脑计算最好的下棋位置
        /// </summary>
        /// <param name="arrChessBoard">整个棋盘中棋子的信息数组</param>
        public void ComputerCalculate(int[,] arrChessBoard)
        {
            //权值数组
            long[,] level = new long[15, 15];
            //基本思路：先计算每个点的权值，在权值最高的位置下棋
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (arrChessBoard[i, j] < 2)
                    {
                        level[i, j] = -1;		//当该位置已有棋子时标注为-1
                    }
                    else
                    {
                        level[i, j] = CheckLevel(i, j, arrChessBoard);
                    }
                }
            }

            //找出权值最大的点
            MaxLevel(level);

        }

        /// <summary>
        /// 根据冒泡排序找出权值最大点
        /// </summary>
        /// <param name="level">所有权重数组</param>
        private void MaxLevel(long[,] level)
        {
            long max = 0;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (level[i, j] > max)
                    {
                        x = i;
                        y = j;
                        max = level[i, j];
                    }
                }
            }
        }

        /// <summary>
        /// 检查某个点的权重值
        /// </summary>
        /// <param name="m">某个位置的坐标X </param>
        /// <param name="n">某个位置的坐标Y </param>
        /// <param name="arrChessBoard">整个棋盘中棋子的信息数组</param>
        /// <returns>返回权重值，值越大，说明该点越有利</returns>
        private long CheckLevel(int m, int n, int[,] arrChessBoard)
        {
            //权重值
            long level = 0;

            //根据不同类型设置不同的权重值
            long w5_1 = 5000000000000;//我方落子(成五)必胜点权重 【因不管落子成活五还是死五都必胜，所以不区分活五或死五。因为我方落子成五必胜，所以我方权重大。】
            long w5_2 = 1000000000000;//对方落子(成五)必胜点权重 

            long w4_1 = 50000000000;//我方落子(成活四)准胜点权重 【因为我方落子成活四时必胜，所以我方权重大】
            long w4_2 = 10000000000;//对方落子(成活四)准胜点权重 

            long w4_3 = 500000000;//我方落子(成死四)点权重 【因为我方落子成死四先于对手取胜，所以我方权重大】
            long w4_4 = 100000000;//对方落子(成死四)点权重

            long w3_1 = 1000000;//我方落子(成活三)点权重 
            long w3_2 = 5000000;//对方落子(成活三)点权重 【谁落子都不能取胜，但尽量提前防止对手落子成三，所以对手权重大】

            long w3_3 = 10000;//我方落子(成死三)点权重 
            long w3_4 = 50000;//对方落子(成死三)点权重 【谁落子都不能取胜，但尽量提前防止对手落子成三，所以对手权重大】

            long w2_1 = 500;//我方落子(成活二)点权重 【谁落子都不能取胜，但尽量增加我方连子数量，所以我方权重大】
            long w2_2 = 100;//对方落子(成活二)点权重   

            long w2_3 = 5;//我方落子(成死二)点权重 【谁落子都不能取胜，但尽量增加我方连子数量，所以我方权重大】
            long w2_4 = 1;//对方落子(成死二)点权重  

            long w0 = -10000000000000000;//我方的失败点

            //***************************************************************************************************
            //判断我方是那种类型的棋子（黑子或者白子），假设该位置放【我方的棋子】
            //那么判断该棋子是否在同类型棋子中是取胜点，三个相连点，两个相连点，
            //并根据判断的结果，选择赋予权重值。
            //****************************************

            //判断出我方是黑子还是白子
            if (isBlackFlag)
            {
                //判断出我方是黑子
                arrChessBoard[m, n] = 0;
            }
            else
            {
                //判断出我方是白子
                arrChessBoard[m, n] = 1;
            }

            //根据判断出我方是黑子还是白子，
            //来判断该棋子是否在同类型棋子中是取胜点，三个相连点，两个相连点。
            int[] arrf = new int[4];
            arrf[0] = Rule.Xnum(m, n, arrChessBoard);
            arrf[1] = Rule.Ynum(m, n, arrChessBoard);
            arrf[2] = Rule.YXnum(m, n, arrChessBoard);
            arrf[3] = Rule.XYnum(m, n, arrChessBoard);

            //中心点权值加1
            if (m == 7 && n == 7) { level += 1; }

            //将该位置的权重增加权重值
            for (int i = 0; i < 4; i++)
            {
                //我方若放子(成五),不管是活五还是死五，必取胜 
                if (Math.Abs(arrf[i]) == 5)
                {
                    level += w5_1;
                }
                //我方若放子(成活四),准取胜
                if (arrf[i] == 4)
                {
                    level += w4_1;
                }
                //我方若放子(成死四)
                if (arrf[i] == -4)
                {
                    level += w4_3;
                }
                //我方若放子(成活三)
                if (arrf[i] == 3)
                {
                    level += w3_1;
                }
                //我方若放子(成死三)
                if (arrf[i]== -3)
                {
                    level += w3_3;
                }
                //我方若放子(成活二)
                if (arrf[i] == 2)
                {
                    level += w2_1;
                }
                //我方若放子(成死二)
                if (arrf[i] == -2)
                {
                    level += w2_3;
                }

                //如果我方为黑棋，还要检查失败点,如果是失败点则权重置为最小
                if (isBlackFlag)
                {
                    if (Rule.IsFail(arrf, arrChessBoard[m, n]) > 0)
                    {
                        level += w0;
                    }
                }
            }

            //***************************************************************************************************
            //判断对手是那种类型的棋子（黑子或者白子），假设该位置放【对手的棋子】
            //那么判断该棋子是否在同类型棋子中是取胜点，三个相连点，两个相连点，
            //并根据判断的结果，选择赋予权重值。
            //*****************************************

            //判断出对手是黑子还是白子
            if (isBlackFlag)
            {
                //判断出对手是白子
                arrChessBoard[m, n] = 1;
            }
            else
            {
                //判断出对手是黑子
                arrChessBoard[m, n] = 0;
            }

            //根据判断出对手是黑子还是白子，
            //来判断该棋子是否在同类型棋子中是取胜点，三个相连点，两个相连点。
            arrf[0] = Rule.Xnum(m, n, arrChessBoard);
            arrf[1] = Rule.Ynum(m, n, arrChessBoard);
            arrf[2] = Rule.YXnum(m, n, arrChessBoard);
            arrf[3] = Rule.XYnum(m, n, arrChessBoard);

            //因为是对手的棋子，将该位置的权重增加权重值
            for (int i = 0; i < 4; i++)
            {
                //对手若放子(成五),不管是活五还是死五，必取胜 
                if (Math.Abs(arrf[i]) == 5)
                {
                    level += w5_2;
                }
                //对手若放子(成活四),准取胜
                if (arrf[i] == 4)
                {
                    level += w4_2;
                }
                //对手若放子(成死四)
                if (arrf[i] == -4)
                {
                    level += w4_4;
                }
                //对手若放子(成活三)
                if (arrf[i] == 3)
                {
                    level += w3_2;
                }
                //对手若放子(成死三)
                if (arrf[i] == -3)
                {
                    level += w3_4;
                }
                //对手若放子(成活二)
                if (arrf[i] == 2)
                {
                    level += w2_2;
                }
                //对手若放子(成死二)
                if (arrf[i] == -2)
                {
                    level += w2_4;
                }
            }

            //因为是假设的落子情况，所以探测完后恢复到默认值
            arrChessBoard[m, n] = 2;
            return level;
        }
    }
}
