using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Chess.Testing
{
	public class ListTest : MonoBehaviour
	{
		public void TestList()
		{      
            int testAmount = 10000;
            int[] testType = new int[testAmount];
            for(int i = 0; i < testAmount; i++){
                testType[i] = Random.Range(0,2);
            }
            //Our specialized List
            MoveList moveList = new MoveList();
			// Start the stopwatch
			Stopwatch stopwatch = Stopwatch.StartNew();
            for(int i = 0; i < testAmount; i++){
                Move move1 = Move.SimpleMove((1,1),(1,1),PieceType.Pawn);
                if(testType[i] == 0) moveList.InsertCheck(move1);
                else if(testType[i] == 1) moveList.InsertCapture(move1);
                else if(testType[i] == 2) moveList.Insert(move1);
            }
            moveList.ConnectList();
			// Stop the stopwatch and print the elapsed time
			stopwatch.Stop();
			UnityEngine.Debug.Log($"Our: Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

            //Somewhat specialized List
            EasierMoveList mList = new EasierMoveList();
            stopwatch.Restart();
            stopwatch.Start();
            for(int i = 0; i < testAmount; i++){
                Move move2 = Move.SimpleMove((1,1),(1,1),PieceType.Pawn);
                if(testType[i] == 0) mList.InsertCheck(move2);
                else if(testType[i] == 1) mList.InsertCapture(move2);
                else if(testType[i] == 2) mList.Insert(move2);
            }
            mList.ConnectList();
            stopwatch.Stop();
			UnityEngine.Debug.Log($"Mix: Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

            //General List
            // stopwatch.Restart();
            // stopwatch.Start();
            // List<Move> moves = new List<Move>();
            // for(int i = 0; i < testAmount; i++){
            //     Move move1 = null;
            //     if(testType[i] == 0) move1 = Move.CastleMove((1,1),(1,1),(1,1),(1,1));
            //     else if(testType[i] == 1) move1 = Move.CaptureMove((1,1),(1,1),PieceType.Rook, null);
            //     else if(testType[i] == 2) move1 = Move.SimpleMove((1,1),(1,1),PieceType.Pawn);
            //     moves.Add(move1);
            // }
            // moves.Sort((move1, move2) =>
            // {
            //     if (move1.IsCapture() && !move2.IsCapture())
            //         return -1;
            //     else if (!move1.IsCapture() && move2.IsCapture())
            //         return 1;
            //     else
            //         return 0;
            // });
            // moves.Sort((move1, move2) =>
            // {
            //     if (move1.IsCastle() && !move2.IsCastle())
            //         return -1;
            //     else if (!move1.IsCastle() && move2.IsCastle())
            //         return 1;
            //     else
            //         return 0;
            // });
            // stopwatch.Stop();
			// UnityEngine.Debug.Log($"List: Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

            //Reading from our list
            stopwatch.Restart();
            stopwatch.Start();
            Move move = moveList.Get();
            while(move != null){
                move = Move.SimpleMove((2,2),(2,2),PieceType.Pawn);;
                move = moveList.Get();
            }
            stopwatch.Stop();
			UnityEngine.Debug.Log($"Our for loop: Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

            //Reading from mixed list
            stopwatch.Restart();
            stopwatch.Start();
            for(int i = 0; i < mList.moves.Count; i++){
                move = mList.moves[i];
                move = Move.SimpleMove((2,2),(2,2),PieceType.Pawn);
            }
            stopwatch.Stop();
			UnityEngine.Debug.Log($"Mix for loop: Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

            // stopwatch.Restart();
            // stopwatch.Start();
            // for(int i = 0; i < moves.Count; i++){
            //     move = moves[i];
            //     move = null;
            // }
            // stopwatch.Stop();
			// UnityEngine.Debug.Log($"List for loop: Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
		}
	}
}