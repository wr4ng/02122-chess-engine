using System;

namespace Chess
{
	public static class Zobrist
	{
		// Random number generator
		public static Random rand = new();

		// [Color, Piece, Square]
		// 0 = White, 1 = Black
		// 0 = Pawn, 1 = Knight, 2 = Bishop, 3 = Rook, 4 = Queen, 5 = King
		// then file and rank
		public static ulong[,,,] pieces = new ulong[2, 6, 8, 8];

		//TODO Separate number for no en Passant?
		// en Passantfor each file (0-7)
		public static ulong[] enpassant = new ulong[8];

		// Castlingrights
		// 0 = white kingside, 1 = white queenside, 2 = black kingside, 3 = black queenside
		public static ulong[] castle = new ulong[4];

		// side to move
		// if black to move
		public static ulong side;

		public static ulong GenerateHash(Board board)
		{
			// Hash starts at 0
			ulong hash = 0;

			// Loop through board and XOR pieces with hash
			for (int file = 0; file < 8; file++)
			{
				for (int rank = 0; rank < 8; rank++)
				{
					int piece = board.squares[file, rank];
					if (piece != Piece.None)
					{
						hash ^= pieces[Piece.ColorIndex(piece), Piece.TypeIndex(piece), file, rank];
					}
				}
			}
			// If there is an enpassant square, XOR it with the hash
			// and enpassant square is only defined by the file, as we have the turns color the enpassant is
			if (board.enPassantSquare != (-1, -1))
			{
				hash ^= enpassant[board.enPassantSquare.file];
			}

			// If there are castling rights, XOR them with the hash
			// they are all added when they have the right to castle and removed when they don't
			if (board.castlingRights.HasFlag(CastlingRights.WhiteKingside))
			{
				hash ^= castle[0];
			}
			if (board.castlingRights.HasFlag(CastlingRights.WhiteQueenside))
			{
				hash ^= castle[1];
			}
			if (board.castlingRights.HasFlag(CastlingRights.BlackKingside))
			{
				hash ^= castle[2];
			}
			if (board.castlingRights.HasFlag(CastlingRights.BlackQueenside))
			{
				hash ^= castle[3];
			}

			// is added just after white has moved and removed just after black has moved
			// as it is the side to move
			if (board.colorToMove == Piece.Black)
			{
				hash ^= side;
			}

			return hash;
		}

		/// <summary>
		/// Initialize random numbers. Only needs to be called once at the start of the program
		/// </summary>
		public static void Initialize()
		{
			for (int side = 0; side < 2; side++)
			{
				for (int type = 0; type < 6; type++)
				{
					for (int file = 0; file < 8; file++)
					{
						for (int rank = 0; rank < 8; rank++)
						{
							pieces[side, type, file, rank] = RandomInt64();
						}
					}
				}
			}
			for (int file = 0; file < 8; file++)
			{
				enpassant[file] = RandomInt64();
			}
			for (int i = 0; i < 4; i++)
			{
				castle[i] = RandomInt64();
			}
			side = RandomInt64();
		}

		public static ulong RandomInt64()
		{
			byte[] buffer = new byte[8];
			rand.NextBytes(buffer);
			return BitConverter.ToUInt64(buffer, 0);
		}
	}
}