using System;
using System.Collections.Generic;

namespace Chess
{
    public class Zobrist
    {
        private ulong hash;

        // the final hash where enpassant is added, so the next hash is easier to calculate
        private ulong hashFinal;

        List<ulong> zobristList = new List<ulong>();

        // Zobrist hash values precomputed
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

        public Zobrist(Board board)
        {
            //the hash starts at 0
            hash = 0;
            //takes all the pieces on the board and xor's them with the hash
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.squares[i, j] != 0)
                    {
                        hash ^= piece[Piece.ColorTo1Dig(board.squares[i, j]), Piece.Type0To5(board.squares[i, j]), i, j];
                    }
                }
            }
            //if there is an enpassant square, xor it with the hash
            // and enpassant square is only defined by the file, as we have the turns color the enpassant is 
            if (board.enPassantSquare != (-1, -1))
            {
                hash ^= enpassant[board.enPassantSquare.Item1];
            }

            // if there are castling rights, xor them with the hash
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
        }

        /// <summary>
        /// To start up the random numbers, it only needs to be called once at the start of the program
        /// </summary>
        public static void ZobristValues()
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

        public ulong Hash
        {
            get
            {
                return hash;
            }
        }

        public ulong MakeMove(Board board, Move move){
            ulong newhash = hash;
            int pieceMove = board.squares[move.from.file, move.from.rank];

            // remove the piece from the from square
            newhash ^= piece[Piece.ColorTo1Dig(pieceMove), Piece.Type0To5(pieceMove), move.from.file, move.from.rank];

            // add the piece to the to square
            if (move.promotionType != Piece.None)
            {
                newhash ^= piece[Piece.ColorTo1Dig(move.promotionType), Piece.Type0To5(move.promotionType), move.to.file, move.to.rank];
            }
            else
            {
                newhash ^= piece[Piece.ColorTo1Dig(pieceMove), Piece.Type0To5(pieceMove), move.to.file, move.to.rank];
            }

            // if there is a capture, remove the piece from the captured square
            if (move.capturedPiece != Piece.None && !move.isEnPassantCapture)
            {
                newhash ^= piece[Piece.ColorTo1Dig(move.capturedPiece), Piece.Type0To5(move.capturedPiece), move.to.file, move.to.rank];
            }
            
            if (move.isEnPassantCapture)
            {
                int forward = board.colorToMove == Piece.White ? 1 : -1;
				int enPassantPiece = board.squares[move.to.file, move.to.rank - forward];
                // remove the piece from the enpassant square
                newhash ^= piece[Piece.ColorTo1Dig(enPassantPiece), Piece.Type0To5(enPassantPiece), move.to.file, move.to.rank - forward];
            }

            



            hash = newhash;
            // enpassant square
            if (move.isEnPassantCapture)
            {
                newhash ^= enpassant[move.to.file];
            }
            
            hashFinal = newhash;
            zobristList.Add(hashFinal);
            return newhash;
        }


    }
}