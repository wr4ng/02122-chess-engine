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
}