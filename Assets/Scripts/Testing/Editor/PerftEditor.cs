using UnityEditor;
using UnityEngine;

namespace Chess.Testing
{
	[CustomEditor(typeof(Perft))]
	public class PerftEditor : Editor
	{
		Perft perft;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			GUILayout.Space(10);
			GUILayout.Label("Run Tests");
			if (GUILayout.Button("Run Single"))
			{
				perft.SingleTest();
			}
			if (GUILayout.Button("Run Divide"))
			{
				perft.DivideTest();
			}
		}

		void OnEnable()
		{
			perft = (Perft)target;
		}
	}
}