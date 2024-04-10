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
			// return ((board.GetCurrentPlayer() == Color.White) ? miniMaxWhite(board, depth) : miniMaxBlack(board, depth)).move;
			return ((board.GetCurrentPlayer() == Color.White) ? miniMaxWhiteAB(board, depth, float.NegativeInfinity, float.PositiveInfinity) : miniMaxBlackAB(board, depth,float.NegativeInfinity, float.PositiveInfinity)).move;
		}

		public string Name() => $"WhiteBoi({depth} inches)";

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

        private (Move move, float evaluation) miniMaxWhiteAB(Board board, int depth, float alpha, float beta) //wants highest score
        {
            if (depth == 0) return (null, Evaluation.EvaluatePosition(board));
            List<Move> moves = board.GetLegalMoves();
            if(moves.Count == 0) return (null, float.MinValue);
            Move bestMove = moves[0];
            float maxEval = float.MinValue;
            foreach (Move move in board.GenerateLegalMoves(true))
            {
                board.PlayMove(move);
                (Move m, float score) = miniMaxBlackAB(board, depth - 1, alpha, beta);
                if (score > alpha){
                    alpha = score;
                }
                if (score > maxEval)
                {
                    bestMove = move;
                    maxEval = score;
                }
                if(score >= beta){
                    board.UndoPreviousMove();
                    break;
                }
                board.UndoPreviousMove();
            }
            if (bestMove == null) bestMove = board.GetLegalMoves()[0];
            return (bestMove, maxEval);
        }

        private (Move move, float evaluation) miniMaxBlackAB(Board board, int depth, float alpha, float beta) //wants lowest score
        {
            if (depth == 0) return (null, Evaluation.EvaluatePosition(board));
            List<Move> moves = board.GenerateLegalMoves(true);
            if(moves.Count == 0) return (null, float.MaxValue);
            Move bestMove = moves[0];
            float minEval = float.MaxValue;
            foreach (Move move in moves)
            {
                board.PlayMove(move);
                (Move m, float score) = miniMaxWhiteAB(board, depth - 1, alpha, beta);
                if (score < beta) beta = score;
                if (score < minEval)
                {
                    bestMove = move;
                    minEval = score;
                }
                if (score <= alpha){
                    board.UndoPreviousMove();
                    break;
                }
                board.UndoPreviousMove();
            }
            if (bestMove == null) bestMove = board.GetLegalMoves()[0];
            return (bestMove, minEval);
        }
    }
}