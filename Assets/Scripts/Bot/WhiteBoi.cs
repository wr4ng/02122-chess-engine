using System.Collections.Generic;
using Chess;

namespace Bot
{
	public class WhiteBoi : Bot
	{
		int depth;
		//implement mini max / negamax
		//Senere med pruning
		public WhiteBoi(int depth)
		{
			this.depth = depth;
		}

		// Implement Bot interface method
		public Move GetBestMove(Board board)
		{
			return ((board.colorToMove == Piece.White) ? miniMaxWhiteAB(board, depth, float.NegativeInfinity, float.PositiveInfinity) : miniMaxBlackAB(board, depth, float.NegativeInfinity, float.PositiveInfinity)).move;
		}
		public Move GetBestNonPruned(Board board)
		{
			return ((board.colorToMove == Piece.White) ? miniMaxWhite(board, depth) : miniMaxBlack(board, depth)).move;
		}
		public Move GetBestNoPrio(Board board)
		{
			return ((board.colorToMove == Piece.White) ? miniMaxWhiteABNoAttack(board, depth, float.NegativeInfinity, float.PositiveInfinity) : miniMaxBlackABNoAttack(board, depth, float.NegativeInfinity, float.PositiveInfinity)).move;
		}

		public string Name() => $"WhiteBoi({depth} inches)";

		private (Move move, float evaluation) miniMaxWhiteAB(Board board, int depth, float alpha, float beta) //wants highest score
		{
			if (board.drawState != Board.DrawState.None)
			{
				return (new(), 0);
			}
			// If we're at final depth, return the evaluated score
			if (depth == 0)
			{
				return (new(), Evaluation.EvaluatePosition(board));
			}
			List<Move> moves = board.moveGenerator.GenerateMoves();
			// Handle check/stalemate
			if (moves.Count == 0)
			{
				// Check if king is in check
				bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
				if (kingAttacked)
					// No moves + king attacked = checkmate. Use depth to make earlier checksmates worse (and better for opponent)
					return (new(), -Evaluation.ChechmateScore * (depth + 1));
				else
					// No moves + king not attacked = stalemate
					return (new(), 0);
			}
			Move bestMove = new();
			if (moves.Count > 0) bestMove = moves[0];
			float maxEval = float.MinValue;
			foreach (Move move in moves)
			{
				board.MakeMove(move);

				(Move m, float score) = miniMaxBlackAB(board, depth - 1, alpha, beta);
				if (score > alpha)
				{
					alpha = score;
				}
				if (score > maxEval)
				{
					bestMove = move;
					maxEval = score;
				}
				if (score >= beta)
				{
					board.UndoPreviousMove();
					break;
				}
				board.UndoPreviousMove();
			}
			return (bestMove, maxEval);
		}

		private (Move move, float evaluation) miniMaxBlackAB(Board board, int depth, float alpha, float beta) //wants lowest score
		{
			if (board.drawState != Board.DrawState.None)
			{
				return (new(), 0);
			}
			// If we're at final depth, return the evaluated score
			if (depth == 0)
			{
				return (new(), Evaluation.EvaluatePosition(board));
			}
			List<Move> moves = board.moveGenerator.GenerateMoves();
			// Handle check/stalemate
			if (moves.Count == 0)
			{
				// Check if king is in check
				bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
				if (kingAttacked)
					// No moves + king attacked = checkmate. Use depth to make earlier checksmates worse (and better for opponent)
					return (new(), Evaluation.ChechmateScore * (depth + 1));
				else
					// No moves + king not attacked = stalemate
					return (new(), 0);
			}
			Move bestMove = new();
			if (moves.Count > 0) bestMove = moves[0];
			float minEval = float.MaxValue;
			foreach (Move move in moves)
			{
				board.MakeMove(move);
				(Move m, float score) = miniMaxWhiteAB(board, depth - 1, alpha, beta);
				if (score < beta) beta = score;
				if (score < minEval)
				{
					bestMove = move;
					minEval = score;
				}
				if (score <= alpha)
				{
					board.UndoPreviousMove();
					break;
				}
				board.UndoPreviousMove();
			}
			return (bestMove, minEval);
		}
		private (Move move, float evaluation) miniMaxWhite(Board board, int depth) //wants highest score
		{
			if (board.drawState != Board.DrawState.None)
			{
				return (new(), 0);
			}
			// If we're at final depth, return the evaluated score
			if (depth == 0)
			{
				return (new(), Evaluation.EvaluatePosition(board));
			}
			List<Move> moves = board.moveGenerator.GenerateMoves();
			// Handle check/stalemate
			if (moves.Count == 0)
			{
				// Check if king is in check
				bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
				if (kingAttacked)
					// No moves + king attacked = checkmate. Use depth to make earlier checksmates worse (and better for opponent)
					return (new(), -Evaluation.ChechmateScore * (depth + 1));
				else
					// No moves + king not attacked = stalemate
					return (new(), 0);
			}
			Move bestMove = new();
			if (moves.Count > 0) bestMove = moves[0];
			float maxEval = float.MinValue;
			foreach (Move move in moves)
			{
				board.MakeMove(move);

				(Move m, float score) = miniMaxBlack(board, depth - 1);
				if (score > maxEval)
				{
					bestMove = move;
					maxEval = score;
				}
				board.UndoPreviousMove();
			}
			return (bestMove, maxEval);
		}

		private (Move move, float evaluation) miniMaxBlack(Board board, int depth) //wants lowest score
		{
			if (board.drawState != Board.DrawState.None)
			{
				return (new(), 0);
			}
			// If we're at final depth, return the evaluated score
			if (depth == 0)
			{
				return (new(), Evaluation.EvaluatePosition(board));
			}
			List<Move> moves = board.moveGenerator.GenerateMoves();
			// Handle check/stalemate
			if (moves.Count == 0)
			{
				// Check if king is in check
				bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
				if (kingAttacked)
					// No moves + king attacked = checkmate. Use depth to make earlier checksmates worse (and better for opponent)
					return (new(), Evaluation.ChechmateScore * (depth + 1));
				else
					// No moves + king not attacked = stalemate
					return (new(), 0);
			}
			Move bestMove = new();
			if (moves.Count > 0) bestMove = moves[0];
			float minEval = float.MaxValue;
			foreach (Move move in moves)
			{
				board.MakeMove(move);
				(Move m, float score) = miniMaxWhite(board, depth - 1);
				if (score < minEval)
				{
					bestMove = move;
					minEval = score;
				}
				board.UndoPreviousMove();
			}
			return (bestMove, minEval);
		}

	private (Move move, float evaluation) miniMaxWhiteABNoAttack(Board board, int depth, float alpha, float beta) //wants highest score
		{
			if (board.drawState != Board.DrawState.None)
			{
				return (new(), 0);
			}
			// If we're at final depth, return the evaluated score
			if (depth == 0)
			{
				return (new(), Evaluation.EvaluatePosition(board));
			}
			List<Move> moves = board.moveGenerator.GenerateMovesNoPrio();
			// Handle check/stalemate
			if (moves.Count == 0)
			{
				// Check if king is in check
				bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
				if (kingAttacked)
					// No moves + king attacked = checkmate. Use depth to make earlier checksmates worse (and better for opponent)
					return (new(), -Evaluation.ChechmateScore * (depth + 1));
				else
					// No moves + king not attacked = stalemate
					return (new(), 0);
			}
			Move bestMove = new();
			if (moves.Count > 0) bestMove = moves[0];
			float maxEval = float.MinValue;
			foreach (Move move in moves)
			{
				board.MakeMove(move);

				(Move m, float score) = miniMaxBlackABNoAttack(board, depth - 1, alpha, beta);
				if (score > alpha)
				{
					alpha = score;
				}
				if (score > maxEval)
				{
					bestMove = move;
					maxEval = score;
				}
				if (score >= beta)
				{
					board.UndoPreviousMove();
					break;
				}
				board.UndoPreviousMove();
			}
			return (bestMove, maxEval);
		}

		private (Move move, float evaluation) miniMaxBlackABNoAttack(Board board, int depth, float alpha, float beta) //wants lowest score
		{
			if (board.drawState != Board.DrawState.None)
			{
				return (new(), 0);
			}
			// If we're at final depth, return the evaluated score
			if (depth == 0)
			{
				return (new(), Evaluation.EvaluatePosition(board));
			}
			List<Move> moves = board.moveGenerator.GenerateMovesNoPrio();
			// Handle check/stalemate
			if (moves.Count == 0)
			{
				// Check if king is in check
				bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
				if (kingAttacked)
					// No moves + king attacked = checkmate. Use depth to make earlier checksmates worse (and better for opponent)
					return (new(), Evaluation.ChechmateScore * (depth + 1));
				else
					// No moves + king not attacked = stalemate
					return (new(), 0);
			}
			Move bestMove = new();
			if (moves.Count > 0) bestMove = moves[0];
			float minEval = float.MaxValue;
			foreach (Move move in moves)
			{
				board.MakeMove(move);
				(Move m, float score) = miniMaxWhiteABNoAttack(board, depth - 1, alpha, beta);
				if (score < beta) beta = score;
				if (score < minEval)
				{
					bestMove = move;
					minEval = score;
				}
				if (score <= alpha)
				{
					board.UndoPreviousMove();
					break;
				}
				board.UndoPreviousMove();
			}
			return (bestMove, minEval);
		}
	}
}