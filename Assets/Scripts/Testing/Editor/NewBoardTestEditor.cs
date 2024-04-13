using UnityEditor;
using UnityEngine;

namespace Chess.Testing
{
	[CustomEditor(typeof(NewBoardTest))]
	public class NewBoardTestEditor : Editor
	{
		NewBoardTest newBoardTest;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			GUILayout.Space(10);
			if (GUILayout.Button("Run Test"))
			{
				newBoardTest.TestNewBoard();
			}
		}

		void OnEnable()
		{
			newBoardTest = (NewBoardTest)target;
		}
	}
}