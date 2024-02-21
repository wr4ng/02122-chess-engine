using System;
using System.Collections.Generic;

namespace Chess
{
    public class Move
    {
        public (int, int) start, end;
        bool isCapture;
        bool isCastle;
        public bool isEnPassant;
        bool isPromotion;
        private Piece removedPiece;
        public Move((int, int) start, (int, int) end, bool isCapture)
        {
            this.start = start;
            this.end = end;
            this.isCapture = isCapture;
        }

        public void SetEnpassant(bool isEnPassant)
        {
            this.isEnPassant = isEnPassant;
        }
        public void SetRemovedPiece(Piece piece)
        {
            removedPiece = piece;
        }
        public Piece GetRemovedPiece()
        {
            return removedPiece;
        }
    }
    public class MoveGenerator
    {
        public static List<Move> GeneratePawnMove((int, int) start, Board board)
        {
            List<Move> moves = new List<Move>();
            Color color = board.GetPiece(start).GetColor();
            (int, int) direction = (color == Color.White) ? (0, 1) : (0, -1);
            //Foward Moves
            (int, int) position = AddTuples(start, direction);
            bool isBlocked = board.GetPiece(position) != null;
            if (!isBlocked)
            {
                Move move = new Move(start, position, true);
                AddMove(move, board, moves);
                bool atStartPosition = (color == Color.White && start.Item2 == 1) || (color == Color.Black && start.Item2 == 6);
                position = AddTuples(position, direction);
                isBlocked = board.GetPiece(position) != null;
                if (atStartPosition && !isBlocked)
                {
                    Move move2 = new Move(start, position, true);
                    AddMove(move2, board, moves);
                }
            }
            //Attack moves
            (int, int)[] attackPositions = { AddTuples(direction, (1, 0)), AddTuples(direction, (-1, 0)) };
            foreach ((int, int) attackPosition in attackPositions)
            {
                position = AddTuples(start, attackPosition);
                if (InBoard(position))
                {
                    Piece attackedPiece = board.GetPiece(position);
                    if (attackedPiece != null && attackedPiece.GetColor() != color)
                    {
                        Move move = new Move(start, position, true);
                        AddMove(move, board, moves);
                    }
                    if (position == board.GetEnPassantCoords())
                    {
                        Move move = new Move(start, position, true);
                        move.SetEnpassant(true);
                        AddMove(move, board, moves); //TODO den her er en lidt underlig en fordi vi rykker jo ikke hen på den position hvor dimsen dør
                        //Har bare tilføjet en bool, gad ikke have den i constructor
                    }
                }
            }
            return moves;
        }

        public static List<Move> GenerateBishopMove((int, int) start, Board board)
        {
            return MoveByDirection(start, board, new (int, int)[] { (-1, -1), (-1, 1), (1, -1), (1, 1) }); //TODO vi kunne lave directions til enums maybe?
        }
        public static List<Move> GenerateRookMove((int, int) start, Board board)
        {
            return MoveByDirection(start, board, new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) });
        }
        public static List<Move> GenerateQueenMove((int, int) start, Board board)
        {
            return MoveByDirection(start, board, new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1) });
        }

        public static List<Move> GenerateKnightMove((int, int) start, Board board)
        {
            return MoveByOffset(start, board, new (int, int)[] { (1, 2), (2, 1), (2, -1), (1, -2), (-1, -2), (-2, -1), (-2, 1), (-1, 2) });
        }

        public static List<Move> GenerateKingMove((int, int) start, Board board)
        {
            return MoveByOffset(start, board, new (int, int)[] { (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1) });
        }
        public static List<Move> MoveByDirection((int, int) start, Board board, (int, int)[] directions)
        {
            List<Move> moves = new List<Move>();
            Color color = board.GetPiece(start).GetColor();
            Piece attackedPiece;
            foreach ((int, int) direction in directions)
            {
                (int, int) position = AddTuples(start, direction);
                while (InBoard(position))
                {
                    attackedPiece = board.GetPiece(position);
                    if (attackedPiece == null)
                    {
                        Move move = new Move(start, position, false);
                        AddMove(move, board, moves);
                    }
                    else if (attackedPiece.GetColor() != color)
                    {
                        Move move = new Move(start, position, true);
                        AddMove(move, board, moves);
                        break;
                    }
                    else
                    {
                        break; //because it would be a piece of same color
                    }
                    position = AddTuples(position, direction);
                }
            }
            return moves;
        }
        public static List<Move> MoveByOffset((int, int) start, Board board, (int, int)[] offsets)
        {
            List<Move> moves = new List<Move>();
            Color color = board.GetPiece(start).GetColor();
            Piece attackedPiece;
            foreach ((int, int) offset in offsets)
            {
                if (!InBoard(AddTuples(start, offset))) continue;
                (int, int) position = AddTuples(start, offset);
                attackedPiece = board.GetPiece(position);
                if (attackedPiece == null)
                {
                    Move move = new Move(start, position, false);
                    AddMove(move, board, moves);
                }
                else if (attackedPiece.GetColor() != color)
                {
                    Move move = new Move(start, position, true);
                    AddMove(move, board, moves);
                }
            }
            return moves;
        }
        private static void AddMove(Move move, Board board, List<Move> moves)
        {
            Color kingColor = board.GetCurrentPlayer();
            board.MakeMove(move);
            if (!IsInCheck(board.GetKingPosition(kingColor), board))
            {
                moves.Add(move);
            }
            board.UnmakeMove(move);
        }

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
                (int, int) position = AddTuples(kingPosition, direction);
                while (InBoard(position))
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
                    position = AddTuples(position, direction);
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
                position = AddTuples(kingPosition, offset);
                if (!InBoard(position)) continue;
                Piece potentialThreat = board.GetPiece(position);
                if (potentialThreat == null) continue;
                if (potentialThreat.GetColor() == color && potentialThreat.GetPieceType() == pieceType) return true;
            }
            return false;
        }

        private static bool InBoard((int, int) position)
        {
            return position.Item1 >= 0 && position.Item1 < 8 && position.Item2 >= 0 && position.Item2 < 8;
        }

        public static (int, int) AddTuples((int, int) a, (int, int) b)
        {
            return (a.Item1 + b.Item1, a.Item2 + b.Item2);
        }
    }
}