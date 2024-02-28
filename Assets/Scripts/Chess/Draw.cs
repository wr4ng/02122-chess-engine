using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;

namespace Chess
{
    /// <summary>
    /// Class to check for a any draw
    /// </summary>
    public class Draw
    {
        private Dictionary<string, int> positionCount;
        private bool isDraw;
        private int countOfMoves;


        // The count of the pieces on the board 1 for bishop and knight and 2 for the other pieces       
        private int count;
        private bool blackInsufficientMaterial;
        private bool whiteInsufficientMaterial;


        public Draw()
        {
            positionCount = new Dictionary<string, int>();
            isDraw = false;
            count = 0;
            blackInsufficientMaterial = false;
            whiteInsufficientMaterial = false;
        }

        // TODO: Call from end of turn
        /// <summary>
        /// Updates the position count and ckecks threefold repetition
        /// </summary>
        /// <param name="position">The FEN position of the game</param>
        public void updatePositionCount(string position)
        {
            // split the string to remove the last 2 parts of the FEN
            int spaceCount = 0;
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i] == ' ')
                {
                    spaceCount++;
                    if (spaceCount >= 4)
                    {
                        position = position[..i];
                        break;
                    }
                }
            }


            if (positionCount.ContainsKey(position))
            {
                if (positionCount[position] >= 2)
                {
                    isDraw = true;
                }
                else
                {
                    positionCount[position]++;
                }
            }
            else
            {
                positionCount.Add(position, 1);
            }


        }

        /// <summary>
        /// Checks for the 50 move rule
        /// Just at the end of the turn, after the move has been made 
        /// </summary>
        /// <param name="pieceMoved">The piece that was moved</param>
        /// <param name="pieceCaptured">The piece that was captured</param>
        public void fiftyMoveRule(Piece pieceMoved, Piece pieceCaptured)
        {
            // Check for 50 move rule
            if (pieceMoved != null && pieceMoved.GetPieceType() != PieceType.Pawn && pieceCaptured == null)
            {
                countOfMoves++;
            }
            else
            {
                countOfMoves = 0;
            }
        }

        /// <summary>
        /// Material count for dead position
        /// add 1 to the count if pieace is a bishop or knight, add 2 if it is a rook, queen or pawn
        /// </summary>
        public void countForDeadPossition(int count)
        {
            this.count += count;
        }

        /// <summary>
        /// Checks for dead position
        /// If there are only 2 pieces left on the board, it checks if they are bishops or knights
        /// 
        /// </summary>
        /// <param name="board">The board</param>


        public bool isSameSquaredBishop(Board board)
        {
            int type1 = 2;
            int type2 = 2;
            for (int i = 0; i < board.BOARD_SIZE)
            {
                for (int j = 0; j < board.BOARD_SIZE)
                {
                    if (board.GetBoard()[i, j] != null && board.GetBoard()[i, j].GetPieceType() == PieceType.Bishop)
                    {
                        if ((i + j) % 2 == 0 && type1 == 2)
                        {
                            type1 = 0;
                        }
                        else if ((i + j) % 2 != 0 && type1 == 2)
                        {
                            type1 = 1;
                        }
                        else if ((i + j) % 2 == 0 && type1 == 0)
                        {
                            return true;
                        }
                        else if ((i + j) % 2 != 0 && type1 == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //Check stalemate
        /// <summary>
        /// Checks for a stalemate
        /// </summary>
        /// <param name="moves">List of possible moves</param>
        /// <param name="isInCheck">Call from Check class</param>
        public void checkStalemate(List<Move> moves, bool isInCheck)
        {
            if (moves.Count == 0 && !isInCheck)
            {
                isDraw = true;
            }
        }


        /// <summary>
        /// Checks all the conditions for a draw
        /// </summary>
        /// <returns>If the game is a draw</returns>
        public bool getIsDraw()
        {
            // 50 moves, 1 move is 2 turns
            else if (countOfMoves >= 100)
            {
                isDraw = true;
            }

            return isDraw;
        }
    }
}
