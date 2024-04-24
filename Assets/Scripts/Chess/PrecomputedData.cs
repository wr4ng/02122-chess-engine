using System.Collections.Generic;

namespace Chess
{
	public static class PrecomputedData
	{
		// Pre-calculated values
		// file, rank, dir (SW, S, SE, E, NE, N, NW, W)
		public static int[][][] squaresToEdge;

		// [file, rank][moveNo]
		public static (int file, int rank)[,][] knightMoves;

		//TODO Precompute knight, pawn and king attack bitboards for more efficient check checking

		public static void Compute()
		{
			ComputeSquaresToEdge();
			ComputeKnightMoves();
			//TODO Precompute pawn and king moves?
		}

		static void ComputeSquaresToEdge()
		{
			squaresToEdge = new int[8][][];

			for (int file = 0; file < 8; file++)
			{
				squaresToEdge[file] = new int[8][];

				for (int rank = 0; rank < 8; rank++)
				{
					squaresToEdge[file][rank] = new int[8];

					int south = rank;
					int west = file;
					int north = 7 - south;
					int east = 7 - west;

					// SW, S, SE, E, NE, N, NW, W
					squaresToEdge[file][rank][0] = System.Math.Min(south, west);
					squaresToEdge[file][rank][1] = south;
					squaresToEdge[file][rank][2] = System.Math.Min(south, east);
					squaresToEdge[file][rank][3] = east;
					squaresToEdge[file][rank][4] = System.Math.Min(north, east);
					squaresToEdge[file][rank][5] = north;
					squaresToEdge[file][rank][6] = System.Math.Min(north, west);
					squaresToEdge[file][rank][7] = west;
				}
			}
		}

		static void ComputeKnightMoves()
		{
			knightMoves = new (int, int)[8, 8][];

			for (int file = 0; file < 8; file++)
			{
				for (int rank = 0; rank < 8; rank++)
				{
					List<(int, int)> moves = new();
					foreach ((int dx, int dy) in MoveGenerator.knightDirections)
					{
						(int file, int rank) square = (file + dx, rank + dy);
						if (Util.InsideBoard(square.file, square.rank))
						{
							moves.Add(square);
						}
					}
					knightMoves[file, rank] = moves.ToArray();
				}
			}
		}
	}
}