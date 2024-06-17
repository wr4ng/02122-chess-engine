using System.Collections.Generic;

namespace Chess
{
	public class Element
	{
		public Element next = null;
		public Move move;
		public Element(Move move, Element next)
		{
			this.move = move;
			this.next = next;
		}
	}
	public class MoveList
	{
		public List<Move> checks = new List<Move>();
		public List<Move> captures = new List<Move>(64);
		public List<Move> rest = new List<Move>(64);
		public List<Move> moves;

		public void InsertCheck(Move move)
		{
			checks.Add(move);
		}
		public void InsertCapture(Move move)
		{
			captures.Add(move);
		}
		public void Insert(Move move)
		{
			rest.Add(move);
		}
		public void ConnectList()
		{
			moves = new List<Move>(checks.Count + captures.Count + rest.Count);
			moves.AddRange(checks);
			moves.AddRange(captures);
			moves.AddRange(rest);
		}
		public List<Move> GetList()
		{
			return moves;
		}
	}
}