using System;

namespace Chess
{
    public class Check
    {
        public static bool IsInCheck((int, int) kingPosition, Board board)
        {
            if (CheckForBishops(kingPosition, board)) return true;
            if (CheckForRooks(kingPosition, board)) return true;
            if (CheckForHorses(kingPosition, board)) return true;
            if (CheckForPawns(kingPosition, board)) return true;
            if (CheckForKings(kingPosition, board)) return true;
            return false;
        }

        public static bool CheckForBishops((int, int) kingPosition, Board board)
        {
            (int, int)[] directions = new (int, int)[] { (-1, -1), (-1, 1), (1, -1), (1, 1) };
            return checkByDirection(kingPosition, board, directions, PieceType.Bishop);
        }

        public static bool CheckForRooks((int, int) kingPosition, Board board)
        {
            (int, int)[] directions = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
            return checkByDirection(kingPosition, board, directions, PieceType.Rook);
        }

        public static bool CheckForHorses((int, int) kingPosition, Board board)
        {
            (int, int)[] placesForHorses = new (int, int)[] { (1, 2), (2, 1), (2, -1), (1, -2), (-1, -2), (-2, -1), (-2, 1), (-1, 2) };
            return CheckByOffset(kingPosition, board, placesForHorses, PieceType.Knight);
        }

        public static bool CheckForPawns((int, int) kingPosition, Board board)
        {
            Color color = board.GetCurrentPlayer();
            (int, int)[] placesForPawns = (color == Color.White) ? new (int, int)[] { (1, -1), (-1, -1) } : new (int, int)[] { (1, 1), (-1, 1) };
            return CheckByOffset(kingPosition, board, placesForPawns, PieceType.Pawn);
        }

        public static bool CheckForKings((int, int) kingPosition, Board board)
        {
            (int, int)[] placesForKings = new (int, int)[] { (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1) };
            return CheckByOffset(kingPosition, board, placesForKings, PieceType.King);
        }

        private static bool checkByDirection((int, int) kingPosition, Board board, (int, int)[] directions, PieceType pieceType)
        {
            Color color = board.GetCurrentPlayer();
            foreach ((int, int) direction in directions)
            {
                (int, int) position = Util.AddTuples(kingPosition, direction);
                while (Util.InBoard(position))
                {
                    Piece potentialThreat = board.GetPiece(position);
                    if (potentialThreat == null)
                    {

                    }
                    else if (potentialThreat.GetColor() != color)
                    {
                        break;
                    }
                    else if (potentialThreat.GetPieceType() == pieceType || potentialThreat.GetPieceType() == PieceType.Queen)
                    {
                        return true;
                    }
                    position = Util.AddTuples(position, direction);
                }
            }
            return false;
        }
        private static bool CheckByOffset((int, int) kingPosition, Board board, (int, int)[] offsets, PieceType pieceType)
        {
            Color color = board.GetCurrentPlayer();
            (int, int) position;
            foreach ((int, int) offset in offsets)
            {
                position = Util.AddTuples(kingPosition, offset);
                if (!Util.InBoard(position)) continue;
                Piece potentialThreat = board.GetPiece(position);
                if (potentialThreat == null) continue;
                if (potentialThreat.GetColor() == color && potentialThreat.GetPieceType() == pieceType) return true;
            }
            return false;
        }
    }
}