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