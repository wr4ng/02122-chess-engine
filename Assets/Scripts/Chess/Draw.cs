using System;
using System.Collections.Generic;
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

        public Draw()
        {
            positionCount = new Dictionary<string, int>();
            isDraw = false;
        }

        /// <summary>
        /// Updates the position count and ckecks threefold repetition
        /// </summary>
        /// <param name="position">The position to be updated</param>
        public void updatePositionCount(string position)
        {
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
        /// Checks all the conditions for a draw
        /// </summary>
        /// <returns>If the game is a draw</returns>
        public bool getIsDraw()
        {
            return isDraw;
        }
    }
}