using System;
using System.Collections.Generic;
using UnityEditor;

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

        public static List<Move> GenerateKnightMove((int, int) start, Board board)
        {
            List<Move> moves = new List<Move>();
            Piece[,] pieces = board.GetBoard();
            Color color = pieces[start.Item1, start.Item2].GetColor();
            int[,] offsets = new int[,]{{-2,-1},{-2,1},{-1,-2},{-1,2},{1,-2},{1,2},{2,-1},{2,1}};
            for(int i = 0; i < 8; i++){
                int x = start.Item1 + offsets[i,0];
                int y = start.Item2 + offsets[i,1];
                if(x >= 0 && x < 8 && y >= 0 && y < 8){
                    Piece attackedPiece = pieces[x,y];
                    if(attackedPiece == null || attackedPiece.GetColor() != color){
                        moves.Add(new Move(start, (x,y),attackedPiece != null));
                    }
                }
            }
            return moves;
        }
    }
}