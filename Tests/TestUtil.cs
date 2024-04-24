using Chess;

namespace Tests;

class TestUtil
{
	// Filter list of moves to those matching given type, based on board
	public static List<NewMove> FilterForPieceType(List<NewMove> moves, int type, NewBoard board)
	{
		return moves.Where(move => NewPiece.Type(board.squares[move.from.file, move.from.rank]) == type).ToList();
	}
}