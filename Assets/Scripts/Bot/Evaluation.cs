using System;
using Chess;

namespace Bot
{
    public class Evaluation
    {

        public static float EvaluatePosition(Board board)
        {
            float score = 0;
            if (board.gameOver)
            {
                if (board.isDraw)
                {
                    return 0;
                }
                return (board.GetCurrentPlayer() == Color.White) ? int.MinValue : int.MaxValue;
            }
            for (int file = 0; file < Board.BOARD_SIZE; file++)
            {
                for (int rank = 0; rank < Board.BOARD_SIZE; rank++)
                {
                    Piece piece = board.GetPiece(file, rank);
                    if (piece != null)
                    {
                        score += PieceWieght(piece);
                    }
                }
            }
            return score;
        }
        public static float PieceWieght(Piece piece)
        {
            float weight;
            switch (piece.GetPieceType())
            {
                case PieceType.Pawn:
                    weight = (piece.GetColor() == Color.White) ? 1 : -1;
                    break;
                case PieceType.Rook:
                    weight = (piece.GetColor() == Color.White) ? 5 : -5;
                    break;
                case PieceType.Bishop:
                    weight = (piece.GetColor() == Color.White) ? 3 : -3;
                    break;
                case PieceType.Knight:
                    weight = (piece.GetColor() == Color.White) ? 3 : -3;
                    break;
                case PieceType.Queen:
                    weight = (piece.GetColor() == Color.White) ? 9 : -9;
                    break;
                case PieceType.King:
                    weight = (piece.GetColor() == Color.White) ? 100 : -100;
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