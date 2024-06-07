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
            return ((board.colorToMove == Piece.White) ? miniMaxWhiteAB(board, depth, float.NegativeInfinity, float.PositiveInfinity) : miniMaxBlackAB(board, depth, float.NegativeInfinity, float.PositiveInfinity)).move;
        }

        public string Name() => $"WhiteBoi({depth} inches)";

        private (Move move, float evaluation) miniMaxWhiteAB(Board board, int depth, float alpha, float beta, int addedDepth = 0) //wants highest score
        {
            //TODO Handle null
            if (depth == 0) return (new(), Evaluation.EvaluatePosition(board));
            List<Move> moves = board.moveGenerator.GenerateMoves();
            // Handle check/stalemate
            if (moves.Count == 0)
            {
                // Check if king is in check
                bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
                if (kingAttacked)
                {
                    // No moves + king attacked = checkmate
                    //TODO Null move!
                    return (new(), float.MinValue);
                }
                else
                {
                    // No moves + king not attacked = stalemate
                    //TODO Null move!
                    return (new(), 0);
                }
            }
            Move bestMove = new();
            if (moves.Count > 0) bestMove = moves[0];
            float maxEval = float.MinValue;
            foreach (Move move in moves)
            {
                board.MakeMove(move);
                if(moves.Count < 10 && addedDepth < 3){
                    addedDepth += 1;
                    depth++;
                }
                (Move m, float score) = miniMaxBlackAB(board, depth - 1, alpha, beta,addedDepth);
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

        private (Move move, float evaluation) miniMaxBlackAB(Board board, int depth, float alpha, float beta, int addedDepth = 0) //wants lowest score
        {
            //TODO Handle null move
            if (depth == 0) return (new(), Evaluation.EvaluatePosition(board));
            List<Move> moves = board.moveGenerator.GenerateMoves();
            // Handle check/stalemate
            if (moves.Count == 0)
            {
                // Check if king is in check
                bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
                if (kingAttacked)
                {
                    // No moves + king attacked = checkmate
                    //TODO Null move!
                    return (new(), float.MaxValue);
                }
                else
                {
                    // No moves + king not attacked = stalemate
                    //TODO Null move!
                    return (new(), 0);
                }
            }
            Move bestMove = new();
            if (moves.Count > 0) bestMove = moves[0];
            float minEval = float.MaxValue;
            foreach (Move move in moves)
            {
                board.MakeMove(move);
                if(moves.Count < 10 && addedDepth < 3){
                    addedDepth += 1;
                    depth++;
                }
                (Move m, float score) = miniMaxWhiteAB(board, depth - 1, alpha, beta, addedDepth);
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