using System;
using System.Collections.Generic;

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
            if(!isBlocked){
                moves.Add(new Move(start, (start.Item1, start.Item2 + direction),false));
                bool atStartPosition = ((color == Color.White && start.Item2 == 1) || (color == Color.Black && start.Item2 == 6));
                isBlocked = pieces[start.Item1, start.Item2 + direction * 2] != null;
                if(atStartPosition && !isBlocked){
                    moves.Add(new Move(start, (start.Item1, start.Item2 + direction*2),false));
                }
            }
            //Attack moves
            if(start.Item1 != 0){
                Piece attackedPiece = pieces[start.Item1 - 1, start.Item2 + direction];
                if((start.Item1 - 1, start.Item2 + direction) == board.GetEnPassantCoords() || (attackedPiece != null && attackedPiece.GetColor() != color)){
                    moves.Add(new Move(start, (start.Item1 - 1, start.Item2 + direction),true));
                }
            }
            if(start.Item1 != 7){
                Piece attackedPiece = pieces[start.Item1 + 1, start.Item2 + direction];
                if((start.Item1 + 1, start.Item2 + direction) == board.GetEnPassantCoords() || (attackedPiece != null && attackedPiece.GetColor() != color)){
                    moves.Add(new Move(start, (start.Item1 + 1, start.Item2 + direction),true));
                }
            }
            return moves;
        }
    }
}