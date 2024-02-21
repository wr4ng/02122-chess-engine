using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Chess
{
    public class Move
    {
        public (int, int) start, end;
        bool isCapture;
        bool isCastle;
        public Move((int, int) start, (int, int) end, bool isCapture)
        {
            this.start = start;
            this.end = end;
            this.isCapture = isCapture;
        }
    }
    public class MoveGenerator
    {
        public static List<Move> GeneratePawnMove((int, int) start, Board board)
        {
            List<Move> moves = new List<Move>();
            Piece[,] pieces = board.GetBoard();
            Color color = pieces[start.Item1, start.Item2].GetColor();
            int direction = (color == Color.White) ? 1 : -1;
            //Foward Moves
            bool isBlocked = pieces[start.Item1, start.Item2 + direction] != null;
            if (!isBlocked)
            {
                moves.Add(new Move(start, (start.Item1, start.Item2 + direction), false));
                bool atStartPosition = ((color == Color.White && start.Item2 == 1) || (color == Color.Black && start.Item2 == 6));
                isBlocked = pieces[start.Item1, start.Item2 + direction * 2] != null;
                if (atStartPosition && !isBlocked)
                {
                    moves.Add(new Move(start, (start.Item1, start.Item2 + direction * 2), false));
                }
            }
            //Attack moves
            if (start.Item1 != 0)
            {
                Piece attackedPiece = pieces[start.Item1 - 1, start.Item2 + direction];
                if ((start.Item1 - 1, start.Item2 + direction) == board.GetEnPassantCoords() || (attackedPiece != null && attackedPiece.GetColor() != color))
                {
                    moves.Add(new Move(start, (start.Item1 - 1, start.Item2 + direction), true));
                }
            }
            if (start.Item1 != 7)
            {
                Piece attackedPiece = pieces[start.Item1 + 1, start.Item2 + direction];
                if ((start.Item1 + 1, start.Item2 + direction) == board.GetEnPassantCoords() || (attackedPiece != null && attackedPiece.GetColor() != color))
                {
                    moves.Add(new Move(start, (start.Item1 + 1, start.Item2 + direction), true));
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
                        moves.Add(new Move(start, position, false));
                    }
                    else if (attackedPiece.GetColor() != color)
                    {
                        moves.Add(new Move(start, position, true));
                        break;
                    }
                    else
                    {
                        break; //because it would be a piece of same colour
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
                    moves.Add(new Move(start, position, false));
                }
                else if (attackedPiece.GetColor() != color)
                {
                    moves.Add(new Move(start, position, true));
                }
            }
            return moves;
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