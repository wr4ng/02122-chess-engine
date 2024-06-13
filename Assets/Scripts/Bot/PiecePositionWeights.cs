namespace Bot
{
	public static class PositionWeights
	{
		public static int[] pawnWeight = {
			 0,  0,  0,  0,  0,  0,  0,  0,
			50, 50, 50, 50, 50, 50, 50, 50,
			10, 10, 20, 30, 30, 20, 10, 10,
			5,  5,  10, 25, 25, 10,  5,  5,
			0,  0,   0, 20, 20,  0,  0,  0,
			1,  1,   1,  1,  1,  1,  1,  1,
			5, 10,  10,-20,-20, 10, 10,  5,
			0,  0,   0,  0,  0,  0,  0,  0
		};

		// public static int[] pawnWeight = {
		// 	0,  0,  0,  0,  0,  0,  0,  0,
		// 	50, 50, 50, 50, 50, 50, 50, 50,
		// 	10, 10, 20, 30, 30, 20, 10, 10,
		// 	5,  5, 10, 25, 25, 10,  5,  5,
		// 	0,  0,  0, 20, 20,  0,  0,  0,
		// 	5, 1, 1,  1,  1, 1, 1,  5,
		// 	0, 0, 0,-20,-20, 0, 0,  0,
		// 	0,  0,  0,  0,  0,  0,  0,  0
		// };

		public static int[] bishopWeight = {
			-20,-10,-10,-10,-10,-10,-10,-20,
			-10,  0,  0,  0,  0,  0,  0,-10,
			-10,  0,  5, 10, 10,  5,  0,-10,
			-10,  5,  5, 10, 10,  5,  5,-10,
			-10,  0, 10, 10, 10, 10,  0,-10,
			-10, 10, 10, 10, 10, 10, 10,-10,
			-10,  5,  0,  0,  0,  0,  5,-10,
			-20,-10,-10,-10,-10,-10,-10,-20,
		};
		public static int[] knightWeight = {
			-50,-40,-30,-30,-30,-30,-40,-50,
			-40,-20,  0,  0,  0,  0,-20,-40,
			-30,  0, 10, 15, 15, 10,  0,-30,
			-30,  5, 15, 20, 20, 15,  5,-30,
			-30,  0, 15, 20, 20, 15,  0,-30,
			-30,  5, 10, 15, 15, 10,  5,-30,
			-40,-20,  0,  5,  5,  0,-20,-40,
			-50,-40,-30,-30,-30,-30,-40,-50,
		};
		public static int[] rookWeight = {
			0,  0,  0,  0,  0,  0,  0,  0,
			5, 10, 10, 10, 10, 10, 10,  5,
		   -5,  0,  0,  0,  0,  0,  0, -5,
		   -5,  0,  0,  0,  0,  0,  0, -5,
		   -5,  0,  0,  0,  0,  0,  0, -5,
		   -5,  0,  0,  0,  0,  0,  0, -5,
		   -5,  0,  0,  0,  0,  0,  0, -5,
			0,  0,  0,  5,  5,  0,  0,  0
		};
		public static int[] queenWeight = {
		  -20,-10,-10, -5, -5,-10,-10,-20,
		  -10,  0,  0,  0,  0,  0,  0,-10,
		  -10,  0,  5,  5,  5,  5,  0,-10,
		   -5,  0,  5,  5,  5,  5,  0, -5,
			0,  0,  5,  5,  5,  5,  0, -5,
		  -10,  5,  5,  5,  5,  5,  0,-10,
		  -10,  0,  5,  0,  0,  0,  0,-10,
		  -20,-10,-10, -5, -5,-10,-10,-20
		};
		public static int[] kingWeight = {
		  -30,-40,-40,-50,-50,-40,-40,-30,
		  -30,-40,-40,-50,-50,-40,-40,-30,
		  -30,-40,-40,-50,-50,-40,-40,-30,
		  -30,-40,-40,-50,-50,-40,-40,-30,
		  -20,-30,-30,-40,-40,-30,-30,-20,
		  -10,-20,-20,-20,-20,-20,-20,-10,
		   20, 20,  0,  0,  0,  0, 20, 20,
		   20, 30, 10,  0,  0, 10, 30, 20
		};

		public static int[][] pieceWeights = {
			pawnWeight,
			bishopWeight,
			knightWeight,
			rookWeight,
			queenWeight,
			kingWeight
		};
	}
}