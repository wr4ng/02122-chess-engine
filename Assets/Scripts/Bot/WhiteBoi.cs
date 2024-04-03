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
			return ((board.GetCurrentPlayer() == Color.White) ? miniMaxWhite(board, depth) : miniMaxBlack(board, depth)).move;
		}

		public string Name() => $"WhiteBoi({depth})";

        //TODO find en bedre måde at gemme bestmove på
        private (Move move, float evaluation) miniMaxWhite(Board board, int depth) //wants highest score
        {
            if (depth == 0) return (null, Evaluation.EvaluatePosition(board));
            Move bestMove = null;
            float maxEval = float.MinValue;
            foreach (Move move in board.GenerateLegalMoves(true))
            {
                board.PlayMove(move);
                (Move m, float score) = miniMaxBlack(board, depth - 1);
                if (score > maxEval)
                {
                    bestMove = move;
                    maxEval = score;
                }
                board.UndoPreviousMove();
            }
            if (bestMove == null) bestMove = board.GetLegalMoves()[0];
            return (bestMove, maxEval);
        }

        private (Move move, float evaluation) miniMaxBlack(Board board, int depth) //wants lowest score
        {
            if (depth == 0) return (null, Evaluation.EvaluatePosition(board));
            Move bestMove = null;
            float minEval = float.MaxValue;
            foreach (Move move in board.GetLegalMoves())
            {
                board.PlayMove(move);
                (Move m, float score) = miniMaxWhite(board, depth - 1);
                if (score < minEval)
                {
                    bestMove = move;
                    minEval = score;
                }
                board.UndoPreviousMove();
            }
            if (bestMove == null) bestMove = board.GetLegalMoves()[0];
            return (bestMove, minEval);
        }
    }
}