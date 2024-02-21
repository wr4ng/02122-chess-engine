using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


enum SceneIndex
{
	MainMenu = 0,
	Main = 1
}

public class MainMenu : MonoBehaviour
{
	[SerializeField, Tooltip("Input field to pass on FEN or PGN notation")]
	private TMP_Text inputFieldCo;

	public void PlayGame()
	{
		SceneManager.LoadScene((int)SceneIndex.Main);
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}

	// Takes the imput from the text field and sets the position
	public void playWithInput()
	{
		string input = inputFieldCo.text;
		Debug.Log(input);

		// TODO : Add the logic to set the position from the input and check if it is valid FEN or PGN notation
		SceneManager.LoadScene((int)SceneIndex.Main);
	}
}