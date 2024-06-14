using System;

namespace Chess
{
    public class UCI
    {

        public static string MoveToUCI(Move move)
        {
            string uci = "";

            uci += PGN.CoordinateToAlgebraic(move.from.file, move.from.rank);
            uci += PGN.CoordinateToAlgebraic(move.to.file, move.to.rank);
            if(move.promotionType != Piece.None)
            {
                switch (move.promotionType)
                {
                    case Piece.Knight:
                        uci += "n";
                        break;
                    case Piece.Bishop:
                        uci += "b";
                        break;
                    case Piece.Rook:
                        uci += "r";
                        break;
                    case Piece.Queen:
                        uci += "q";
                        break;
                }
            }

            return uci;
        }

        public static Move UCItoMove(string uci, Board board)
        {
            (int file, int rank) from = PGN.AlgebraicToCoordinate(uci.Substring(0, 2));
            (int file, int rank) to = PGN.AlgebraicToCoordinate(uci.Substring(2, 2));
            int promotionType = Piece.None;
            if(uci.Length == 5)
            {
                switch (uci[4])
                {
                    case 'n':
                        promotionType = Piece.Knight;
                        break;
                    case 'b':
                        promotionType = Piece.Bishop;
                        break;
                    case 'r':
                        promotionType = Piece.Rook;
                        break;
                    case 'q':
                        promotionType = Piece.Queen;
                        break;
                }
            }

            return new Move(from, to, promotionType: promotionType);
        }



        

    }
}