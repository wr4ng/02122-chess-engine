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
	private TMP_InputField fenInputField;

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
	public void PlayWithInput()
	{
		GameManager.IMPORT_FEN = fenInputField.text.Trim();
		//TODO Validate FEN before loading main scene, and show error if it is invalid
		SceneManager.LoadScene((int)SceneIndex.Main);
	}
}