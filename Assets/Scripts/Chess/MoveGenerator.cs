using System.Collections.Generic;

namespace Chess
{
    public class MoveGenerator
    {
        public static List<Move> GeneratePawnMove((int, int) start, Board board)
        {
            List<Move> moves = new List<Move>();
            Color color = board.GetPiece(start).GetColor();
            (int, int) direction = (color == Color.White) ? (0, 1) : (0, -1);
            //Foward Moves
            (int, int) position = Util.AddTuples(start, direction);
            bool isBlocked = board.GetPiece(position) != null;
            if (!isBlocked)
            {
                Move move = new Move(start, position, true);
                AddMove(move, board, moves);
                bool atStartPosition = (color == Color.White && start.Item2 == 1) || (color == Color.Black && start.Item2 == 6);
                position = Util.AddTuples(position, direction);
                isBlocked = board.GetPiece(position) != null;
                if (atStartPosition && !isBlocked)
                {
                    Move move2 = new Move(start, position, true);
                    AddMove(move2, board, moves);
                }
            }
            //Attack moves
            (int, int)[] attackPositions = { Util.AddTuples(direction, (1, 0)), Util.AddTuples(direction, (-1, 0)) };
            foreach ((int, int) attackPosition in attackPositions)
            {
                position = Util.AddTuples(start, attackPosition);
                if (Util.InBoard(position))
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
                (int, int) position = Util.AddTuples(start, direction);
                while (Util.InBoard(position))
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
                    position = Util.AddTuples(position, direction);
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
                if (!Util.InBoard(Util.AddTuples(start, offset))) continue;
                (int, int) position = Util.AddTuples(start, offset);
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
            if (!Check.IsInCheck(board.GetKingPosition(kingColor), board))
            {
                moves.Add(move);
            }
            board.UnmakeMove(move);
        }
    }
}