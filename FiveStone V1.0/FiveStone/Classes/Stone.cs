using System;
using System.Collections;
using System.Drawing;
using System.Resources;//

namespace FiveStone
{
	/// <summary>
	/// 棋子类，加载一个指定属性的棋子。
	/// </summary>
	public class Stone
	{
        //创建画图板
		private Graphics chessGraphics;
		//从文件获取
		private	Image imgBlackStone;  //黑子图片
		private	Image imgWhiteStone;  //白子图片
		private	Image imgRedTriangle; //红三角（用于标识最后一子）

        /// <summary>
        /// 构造函数，从资源文件（或图片文件）中读取黑子，白子图片。获取绘图板初始值
        /// </summary>
        /// <param name="graphics"></param>
        public Stone(Graphics graphics)
		{
            //从资源文件中读取黑子，白子图片
            ResourceManager resource = new ResourceManager(typeof(Images));
            imgBlackStone = (Image)resource.GetObject("BlackStone");//获取黑子资源
            imgWhiteStone = (Image)resource.GetObject("WhiteStone");//获取黑子资源
            imgRedTriangle = (Image)resource.GetObject("RedTriangle");//获取黑子资源

			/*直接读取图片文件方式读取黑子，白子图片
            imgBlackStone = System.Drawing.Image.FromFile("Images\\BlackStone.png");  //黑子图片
            imgWhiteStone = System.Drawing.Image.FromFile("Images\\WhiteStone.png");  //白子图片
            imgRedTriangle = System.Drawing.Image.FromFile("Images\\RedTriangle.png");  //红三角图片
             */

            //获取绘图板初始值
            chessGraphics = graphics;
		}

		/// <summary>
        /// 绘制棋子
		/// </summary>
		/// <param name="x">棋子的坐标X</param>
        /// <param name="y">棋子的坐标Y</param>
        /// <param name="isBlackFlag">标识绘制的是否是黑棋（true），还是白棋（false）</param>
        /// <param name="lx">上一次棋子点的位置坐标X</param>
        /// <param name="ly">上一次棋子点的位置坐标Y</param>
		public void DrawStone(int x,int y,bool isBlackFlag,int lx,int ly)
		{
            if (isBlackFlag)
			{
				//覆盖上一子的标识
                chessGraphics.DrawImage(imgWhiteStone, lx * 40, ly * 40, imgWhiteStone.Width, imgWhiteStone.Height);
				//写当前子为黑子
                chessGraphics.DrawImage(imgBlackStone, x * 40, y * 40, imgBlackStone.Width, imgBlackStone.Height);
                chessGraphics.DrawImage(imgRedTriangle, x * 40, y * 40, imgRedTriangle.Width, imgRedTriangle.Height);
			}
			else
			{
				//覆盖上一子的标识
                chessGraphics.DrawImage(imgBlackStone, lx * 40, ly * 40, imgBlackStone.Width, imgBlackStone.Height);
				//写当前子为白子
                chessGraphics.DrawImage(imgWhiteStone, x * 40, y * 40, imgWhiteStone.Width, imgWhiteStone.Height);
                chessGraphics.DrawImage(imgRedTriangle, x * 40, y * 40, imgRedTriangle.Width, imgRedTriangle.Height);
			}
		}

        /// <summary>
        /// 绘制无红三角标识的棋子（重载方法）
        /// </summary>
        /// <param name="x">棋子的坐标X</param>
        /// <param name="y">棋子的坐标Y</param>
        /// <param name="isBlackFlag">标识绘制的是否是黑棋（true），还是白棋（false）</param>
        public void DrawStone(int x, int y, bool isBlackFlag)
        {
            if (isBlackFlag)
            {
                //写当前子为黑子
                chessGraphics.DrawImage(imgBlackStone, x * 40, y * 40, imgBlackStone.Width, imgBlackStone.Height);
            }
            else
            {
                //写当前子为白子
                chessGraphics.DrawImage(imgWhiteStone, x * 40, y * 40, imgWhiteStone.Width, imgWhiteStone.Height);
            }
        }
	}
}
