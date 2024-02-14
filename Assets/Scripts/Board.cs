public class Board
{
    private Piece[,] board = new Piece[8,8];
    private playerColor currentPlayer;

    public Board()
    {
        currentPlayer = playerColor.White;

        for (int rank = 0; rank < 8; rank++) //TODO check that file and rank is correct, idk
        {
            for (int file = 0; file < 8; file++)
            {
                board[rank,file] = null;
            }
        }
    }

    public void importFromFEN(string fen){

    }
    public string exportToFEN()
    {
        string fen = "";
        //add the board state "https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation"
        //add the current player
        //add the castling rights
        //add the en passant square
        //add the halfmove clock
        //add the fullmove number
        return fen;
    }

    public Piece getPiece(int rank, int file)
    {
        return board[rank,file];
    }

}

public enum playerColor //Technically exists in the piece class
{
    White,
    Black
}