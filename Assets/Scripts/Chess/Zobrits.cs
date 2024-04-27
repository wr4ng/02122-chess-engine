using System;
using System.Collections.Generic;

namespace Chess
{
    public static class ZobristHash
    {
        // random number generator
        public static Random rand = new Random();
        // color, piece, square
        // 0 = white, 1 = black
        // 0 = pawn, 1 = knight, 2 = bishop, 3 = rook, 4 = queen, 5 = king
        // file and rank
        public static ulong[,,,] piece;
        // enpassant for each file (0-7)
        public static ulong[] enpassant;
        // castle rights
        // 0 = white kingside, 1 = white queenside, 2 = black kingside, 3 = black queenside
        public static ulong[] castle;
        // side to move
        // if black to move
        public static ulong side;


        /// <summary>
        /// To start up the random numbers, it only needs to be called once at the start of the program
        /// </summary>
        public static void __init__()
        {
            piece = new ulong[2, 6, 8, 8];
            enpassant = new ulong[8];
            castle = new ulong[4];
            side = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        for (int l = 0; l < 8; l++)
                        {
                            piece[i, j, k, l] = RandomInt64();
                        }
                    }
                }
            }
            for (int i = 0; i < 8; i++)
            {
                enpassant[i] = RandomInt64();
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


        public static ulong Start(Board board)
        {
            ulong hash = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.squares[i, j] != 0)
                    {
                        hash ^= piece[Piece.ColorTo1Dig(board.squares[i, j]), Piece.Type(board.squares[i, j]) - 1, i, j];
                    }
                }
            }
            if (board.enPassantSquare != (-1, -1))
            {
                hash ^= enpassant[board.enPassantSquare.Item1];   
            }
            if (board.castlingRights.HasFlag(CastlingRights.WhiteKingside))
            {
                hash ^= castle[0];
            } else if (board.castlingRights.HasFlag(CastlingRights.WhiteQueenside))
            {
                hash ^= castle[1];
            } else if (board.castlingRights.HasFlag(CastlingRights.BlackKingside))
            {
                hash ^= castle[2];
            } else if (board.castlingRights.HasFlag(CastlingRights.BlackQueenside))
            {
                hash ^= castle[3];
            }
            if (board.colorToMove == Piece.Black)
            {
                hash ^= side;
            }



            return hash;
        }


        
    }
}