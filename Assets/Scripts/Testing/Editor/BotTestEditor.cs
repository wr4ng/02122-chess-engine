using UnityEditor;
using UnityEngine;

namespace Chess.Testing
{
	[CustomEditor(typeof(BotTest))]
	public class BotTestEditor : Editor
	{
		BotTest botTest;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			GUILayout.Space(10);
			GUILayout.Label("Run Tests");
			if (GUILayout.Button("Test WhiteBoi"))
			{
				botTest.TestWhiteBoi();
			}
		}

		void OnEnable()
		{
			botTest = (BotTest)target;
		}
	}
}