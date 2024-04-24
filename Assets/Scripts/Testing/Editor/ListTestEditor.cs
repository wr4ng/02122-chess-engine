using UnityEditor;
using UnityEngine;

namespace Chess.Testing
{
	[CustomEditor(typeof(ListTest))]
	public class ListTestEditor : Editor
	{
		ListTest listTest;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			GUILayout.Space(10);
			GUILayout.Label("Run Tests");
			if (GUILayout.Button("Test WhiteBoi"))
			{
				listTest.TestList();
			}
		}

		void OnEnable()
		{
			listTest = (ListTest)target;
		}
	}
}