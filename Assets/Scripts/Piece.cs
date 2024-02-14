public class Piece
{
    private PieceType type;
    private PieceColor color;

    public Piece(PieceType type, PieceColor color)
    {
        this.type = type;
        this.color = color;
    }
    public string toString()
    {
        return this.color.ToString() + " " + this.type.ToString();
    }
    public char toFENchar()
    {
        char FENchar = (this.type == PieceType.Knight) ? 'N' : this.type.ToString().ToUpper()[0]; //exception for knight because K is already taken by king
        return (this.color == PieceColor.White) ? FENchar : char.ToLower(FENchar); //Uppercase for white, lowercase for black
    }
}

public enum PieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}

public enum PieceColor
{
    Black,
    White
}