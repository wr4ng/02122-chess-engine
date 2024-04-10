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
                        score += PieceWeight(piece);
                        score += PiecePosition(piece, file+ 8 * rank);
                    }
                }
            }
            return score;
        }

        public static float PieceWeight(Piece piece)
        {
            float weight;
            switch (piece.GetPieceType())
            {
                case PieceType.Pawn:
                    weight = (piece.GetColor() == Color.White) ? 10 : -10;
                    break;
                case PieceType.Rook:
                    weight = (piece.GetColor() == Color.White) ? 50 : -50;
                    break;
                case PieceType.Bishop:
                    weight = (piece.GetColor() == Color.White) ? 30 : -30;
                    break;
                case PieceType.Knight:
                    weight = (piece.GetColor() == Color.White) ? 30 : -30;
                    break;
                case PieceType.Queen:
                    weight = (piece.GetColor() == Color.White) ? 90 : -90;
                    break;
                case PieceType.King:
                    weight = (piece.GetColor() == Color.White) ? 1000 : -1000;
                    break;
                default:
                    weight = 0;
                    break;
            }
            return weight;
        }

        public static float PiecePosition(Piece piece, int index)
        {
            float weight;
            switch (piece.GetPieceType())
            {
                case PieceType.Pawn:
                    weight = (piece.GetColor() == Color.White) ? PositionWeights.pawnWeight[63 - index] : -PositionWeights.pawnWeight[index];
                    break;
                case PieceType.Rook:
                    weight = (piece.GetColor() == Color.White) ? PositionWeights.rookWeight[63 - index] : -PositionWeights.rookWeight[index];
                    break;
                case PieceType.Bishop:
                    weight = (piece.GetColor() == Color.White) ? PositionWeights.bishopWeight[63 - index] : -PositionWeights.bishopWeight[index];
                    break;
                case PieceType.Knight:
                    weight = (piece.GetColor() == Color.White) ? PositionWeights.knightWeight[63 - index] : -PositionWeights.knightWeight[index];
                    break;
                case PieceType.Queen:
                    weight = (piece.GetColor() == Color.White) ? PositionWeights.queenWeight[63 - index] : -PositionWeights.queenWeight[index];
                    break;
                case PieceType.King:
                    weight = (piece.GetColor() == Color.White) ? PositionWeights.kingWeight[63 - index] : -PositionWeights.kingWeight[index];
                    break;
                default:
                    weight = 0;
                    break;
            }
            return weight;
        }

        //Other evaluation functions
    }
}