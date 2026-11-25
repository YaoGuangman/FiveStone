using System;

namespace FiveStone
{
	/// <summary>
	/// 游戏规则类，用于判断输赢，平局等。
	/// </summary>
	public class Rule
	{
		/// <summary>
        /// 判断某个下棋位置是否有子
		/// </summary>
        /// <param name="x">下棋位置的坐标X </param>
        /// <param name="y">下棋位置的坐标Y </param>
		/// <param name="arrChessBoard">整个棋盘中棋子的信息数组</param>
		/// <returns></returns>
		public static bool IsExist(int x, int y, int[,] arrChessBoard)
		{
			if (arrChessBoard[x,y] < 2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
        /// 返回检查失败，平局，胜利的结果
		/// </summary>
        /// <param name="x">下棋位置的坐标X </param>
        /// <param name="y">下棋位置的坐标Y </param>
        /// <param name="arrChessBoard">整个棋盘中棋子的信息数组</param>
		/// <returns></returns>
		public static int RuleResult(int x, int y, int[,] arrChessBoard)
		{
			//m,n点四个方向的连子数，依次正东正西，正南正北方，西北东南，西南东北
			int[] arrf = new int[4];
			arrf[0] = Xnum(x,y,arrChessBoard);
			arrf[1] = Ynum(x,y,arrChessBoard);
			arrf[2] = YXnum(x,y,arrChessBoard);
			arrf[3] = XYnum(x,y,arrChessBoard);

			//检查是否失败
			if (IsFail(arrf, arrChessBoard[x,y]) > 0)
			{
				return IsFail(arrf, arrChessBoard[x,y]);
			}
			else
			{
				//检查是否胜利
				if (IsWin(arrf))
				{
					return 4;
				}
				else
				{
					//检查是否平局
					if (IsTie(arrChessBoard))
					{
						return 5;
					}
					else
					{
						return 6;
					}
				}
			}
		}

		#region  检查是否失败，是否胜利，是否平局

		/// <summary>
		/// 验证是否失败，及返回失败的原因
		/// </summary>
		/// <param name="arr">四个方向检测连子个数的数组</param>
		/// <param name="stoneFlag">标识当前验证的棋子是: 黑棋（2），白棋（1）</param>
        /// <returns>返回验证后的结果: 没有失败（0），双三禁手（1），双四禁手（2）,长连禁手(3)</returns>
		public static int IsFail(int[] arr, int stoneFlag)
		{
			if (stoneFlag == 1)
			{
				//如果是白棋不验证因为白棋无禁手（注意：五子棋标准规则白棋无禁手）
				return 0;
			}
			else
			{
				//验证双三禁手（该处的双三因该是活的双三吧）
				int num = 0;		//活的3子相连的个数
				for (int i=0;i<4;i++)
				{
					if (arr[i] == 3)
					{
						num++;
					}
				}
				if (num > 1)
				{
					return 1;
				}
				//验证双四禁手（该处的双四因该是活的双四吧）
				num = 0;		//活的4子相连的个数
				for (int i=0;i<4;i++)
				{
					if (arr[i] == 4)
					{
						num++;
					}
				}
				if (num > 1)
				{
					return 2;
				}
				//验证长连禁手
				for (int i=0;i<4;i++)
				{
					if (Math.Abs(arr[i]) > 5)
					{
						return 3;
					}
				}
				return 0;
			}
		}

		/// <summary>
        /// 验证是否胜利
		/// </summary>
        /// <param name="arr">四个方向检测连子个数的数组</param>
		/// <returns>返回是否胜利：胜利（true）,没有胜利（false）</returns>
		private static bool IsWin(int[] arr)
		{
			for (int i=0;i<4;i++)
			{
				if (Math.Abs(arr[i]) == 5)
				{
					return true;
				}
			}
			return false;
		}

		
		/// <summary>
        /// 验证是否平局
		/// </summary>
        /// <param name="arrChessBoard">整个棋盘中棋子的信息数组</param>
        /// <returns>返回是否平局：平局（true）,没有平局（false）</returns>
		private static bool IsTie(int[,] arrChessBoard)
		{
			//当所有位置都有子时为平局
			for (int i=0;i<15;i++)
			{
				for (int j=0;j<15;j++)
				{
					if (arrChessBoard[i,j] == 2)
						return false;
				}
			}
			return true;
		}

		#endregion

		#region 从四个方向检测，棋盘中连子个数

		/// <summary>
		/// 正东正西检测与x，y点棋子相同的棋子个数
		/// </summary>
		/// <param name="x">棋子点坐标X </param>
        /// <param name="y">棋子点坐标Y  </param>
        /// <param name="arrChessBoard">整个棋盘中棋子的信息数组</param>
        /// <returns>返回相同棋子的数量，如果返回负值则表示该方向堵死或单活</returns>
		public static int Xnum(int x, int y, int[,] arrChessBoard)
		{
            //检查是否无子可下（当flag=1时表示单方向不能下，flag=2时表示两个方向都不能下）
			int flag = 0;
			//连子个数（含本子）
			int num = 1;

			//正东方向检查(x+)
			int i = x+1;
			//不超出棋格
			while(i < 15)
			{
				//前方的棋子与m,n点不同时跳出循环
				if (arrChessBoard[i,y] == arrChessBoard[x,y])
				{
					num++;
					i++;
				}
				else
				{
					break;
				}
			}

			//正东方向超出棋格或正东方向有别的子不可在下
            if (i == 15 || arrChessBoard[i, y] != 2)
			{
				flag++;
			}

			//正西方向检查(x-)
            i = x-1;
			while(i >= 0)
			{
				//前方的棋子与m,n点不同时跳出循环
				if (arrChessBoard[i,y] == arrChessBoard[x,y])
				{
					num++;
					i--;
				}
				else
				{
					break;
				}
			}

            //正西方向超出棋格或正西方向有别的子不可在下
            if (i == -1 || arrChessBoard[i, y] != 2)
            {
                flag++;
            }

            //检查是否堵死或单活，如果堵死或单活返回负值
            if (flag == 1 || flag == 2)
            {
                //堵死无子可下或单活，则返回负值
                return -num;
            }
            else
            {
                return num;
            }
		}

		/// <summary>
        /// 正南正北方向检测与x，y点棋子相同的棋子个数
		/// </summary>
        /// <param name="x">棋子点坐标X </param>
        /// <param name="y">棋子点坐标Y  </param>
        /// <param name="arrChessBoard">整个棋盘中棋子的信息数组</param>
        /// <returns>返回相同棋子的数量，如果返回负值则表示改方向堵死或单活</returns>
		public static int Ynum(int x, int y, int[,] arrChessBoard)
		{
            //检查是否无子可下（当flag=1时表示单方向不能下，flag=2时表示两个方向都不能下）
			int flag = 0;
			//连子个数
			int num = 1;
			//正南方向检查(y+)
			int i = y+1;
			while(i<15)
			{
				//前方的棋子与m,n点不同时跳出循环
				if (arrChessBoard[x,i] == arrChessBoard[x,y])
				{
					num++;
					i++;
				}
				else
				{
					break;
				}
			}

            //正南方向超出棋格或正南方向有别的子不可在下
            if (i == 15 || arrChessBoard[x, i] != 2)
            {
                flag++;
            }

			//正北方向检查(y-)
			i = y-1;
			while(i >= 0)
			{
				//前方的棋子与m,n点不同时跳出循环
				if (arrChessBoard[x,i] == arrChessBoard[x,y])
				{
					num++;
					i--;
				}
				else
				{
					break;
				}
			}

            //正北方向超出棋格或正北方向有别的子不可在下
            if (i == -1 || arrChessBoard[x, i] != 2)
            {
                flag++;
            }

            //检查是否堵死或单活，如果堵死或单活返回负值
            if (flag == 1 || flag == 2)
            {
                //堵死无子可下或单活，则返回负值
                return -num;
            }
            else
            {
                return num;
            }
		}

		/// <summary>
        /// 西北东南方向检查与x，y点棋子相同的棋子个数
		/// </summary>
        /// <param name="x">棋子点坐标X </param>
        /// <param name="y">棋子点坐标Y  </param>
        /// <param name="arrChessBoard">整个棋盘中棋子的信息数组</param>
        /// <returns>返回相同棋子的数量，如果返回负值则表示改方向堵死或单活</returns>
		public static int YXnum(int x, int y, int[,] arrChessBoard)
		{
            //检查是否无子可下（当flag=1时表示单方向不能下，flag=2时表示两个方向都不能下）
			int flag = 0;
			//连子个数
			int num = 1;
			
			//东南方向(x+,y+)
			int i = x+1;
			int j = y+1;
			//不超出棋格
			while(i < 15 && j < 15)
			{
				//前方的棋子与m,n点不同时跳出循环
				if (arrChessBoard[i,j] == arrChessBoard[x,y])
				{
					num++;
					i++;
					j++;
				}
				else
				{
					break;
				}
			}

            //东南方向超出棋格或东南方向有别的子不可在下
            if (i == 15 || j == 15 || arrChessBoard[i, j] != 2)
            {
                flag++;
            }

			//西北方向(x-,y-)
			i = x-1;
			j = y-1;
			//不超出棋格
			while(i >= 0 && j >= 0)
			{
				//前方的棋子与m,n点不同时跳出循环
				if (arrChessBoard[i,j] == arrChessBoard[x,y])
				{
					num++;
					i--;
					j--;
				}
				else
				{
					break;
				}
			}

            //西北方向超出棋格或西北方向有别的子不可在下
            if (i == -1 || j == -1 || arrChessBoard[i, j] != 2)
            {
                flag++;
            }

            //检查是否堵死或单活，如果堵死或单活返回负值
            if (flag == 1 || flag == 2)
            {
                //堵死无子可下或单活，则返回负值
                return -num;
            }
            else
            {
                return num;
            }
		}

		/// <summary>
        /// 西南东北方向检查与x，y点棋子相同的棋子个数
		/// </summary>
        /// <param name="x">棋子点坐标X </param>
        /// <param name="y">棋子点坐标Y  </param>
        /// <param name="arrChessBoard">整个棋盘中棋子的信息数组</param>
        /// <returns>返回相同棋子的数量，如果返回负值则表示改方向在无子可下</returns>
		public static int XYnum(int x, int y, int[,] arrChessBoard)
		{
            //检查是否无子可下（当flag=1时表示单方向不能下，flag=2时表示两个方向都不能下）
			int flag = 0;
			//连子个数
			int num = 1;

			//西南方向(x-,y+)
			int i = x-1;
			int j = y+1;
			//不超出棋格
			while(i >= 0 && j < 15)
			{
				//前方的棋子与m,n点不同时跳出循环
				if (arrChessBoard[i,j] == arrChessBoard[x,y])
				{
					num++;
					i--;
					j++;
				}
				else
				{
					break;
				}
			}

            //西南方向超出棋格或西南方向有别的子不可在下
            if (i == -1 || j == 15 || arrChessBoard[i, j] != 2)
            {
                flag++;
            }

			//东北方向(x+,y-)
			i = x+1;
			j = y-1;
			//不超出棋格
			while(i < 15 && j >= 0)
			{
				//前方的棋子与m,n点不同时跳出循环
				if (arrChessBoard[i,j] == arrChessBoard[x,y])
				{
					num++;
					i++;
					j--;
				}
				else
				{
					break;
				}
			}

            //西南方向超出棋格或西南方向有别的子不可在下
            if (i == 15 || j == -1 || arrChessBoard[i, j] != 2)
            {
                flag++;
            }

            //检查是否堵死或单活，如果堵死或单活返回负值
            if (flag == 1 || flag == 2)
            {
                //堵死无子可下或单活，则返回负值
                return -num;
            }
            else
            {
                return num;
            }
		}

		#endregion
	}
}
