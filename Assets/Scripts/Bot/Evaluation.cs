using System;
using Chess;

namespace Bot
{
    public class Evaluation
    {

        public static float EvaluatePosition(NewBoard board)
        {
            float score = 0;
			//TODO Handle game over (once Draw is implemented in NewBoard)
            // if (board.gameOver)
            // {
            //     if (board.isDraw)
            //     {
            //         return 0;
            //     }
            //     return (board.GetCurrentPlayer() == Color.White) ? int.MinValue : int.MaxValue;
            // }
            for (int file = 0; file < Board.BOARD_SIZE; file++)
            {
                for (int rank = 0; rank < Board.BOARD_SIZE; rank++)
                {
                    int piece = board.squares[file, rank];
                    if (piece != NewPiece.None)
                    {
                        score += PieceWieght(piece);
                    }
                }
            }
            return score;
        }
        public static float PieceWieght(int piece)
        {
			//TODO Use switch-expression
            float weight;
            switch (NewPiece.Type(piece))
            {
                case NewPiece.Pawn:
                    weight = NewPiece.IsColor(piece, NewPiece.White) ? 1 : -1;
                    break;
                case NewPiece.Rook:
                    weight = NewPiece.IsColor(piece, NewPiece.White) ? 5 : -5;
                    break;
                case NewPiece.Bishop:
                    weight = NewPiece.IsColor(piece, NewPiece.White) ? 3 : -3;
                    break;
                case NewPiece.Knight:
                    weight = NewPiece.IsColor(piece, NewPiece.White) ? 3 : -3;
                    break;
                case NewPiece.Queen:
                    weight = NewPiece.IsColor(piece, NewPiece.White) ? 9 : -9;
                    break;
                case NewPiece.King:
                    weight = NewPiece.IsColor(piece, NewPiece.White) ? 100 : -100;
                    break;
                default:
                    weight = 0;
                    break;
            }
            return weight;
        }

        public float PiecePosition(Board board)
        {
            throw new NotImplementedException();
        }

        //Other evaluation functions
    }
}