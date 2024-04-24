using Chess;

namespace Tests;

class TestUtil
{
	// Filter list of moves to those matching given type, based on board
	public static List<Move> FilterForPieceType(List<Move> moves, int type, Board board)
	{
		return moves.Where(move => Piece.Type(board.squares[move.from.file, move.from.rank]) == type).ToList();
	}
}