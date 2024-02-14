public class piece
{
    public piece(PieceType type, PieceColor color)
    {
        this.type = type;
        this.color = color;
    }

    public toString()
    {
        return " ";
    }
}

enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}
enum PieceColor
{
    White,
    Black
}