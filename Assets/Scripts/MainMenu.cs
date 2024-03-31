using Chess;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

enum SceneIndex
{
	Menu = 0,
	Game = 1,
	Perft = 2
}

public class MainMenu : MonoBehaviour
{
	[Header("GameObjects")]
	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject playMenu;
	[SerializeField] private GameObject optionsMenu;
	[SerializeField] private GameObject testingMenu;

	[SerializeField, Tooltip("Input field to pass on FEN or PGN notation")]
	private TMP_InputField fenInputField;

	public void PlayGame()
	{
		if (fenInputField.text != "")
		{
			// Validate FEN
			bool isValidFEN = Board.TryParseFEN(fenInputField.text.Trim(), out _);
			if (isValidFEN)
			{
				GameManager.IMPORT_FEN = fenInputField.text;
				SceneManager.LoadScene((int)SceneIndex.Game);
			}
			else
			{
				fenInputField.text = "INVALID FEN!";
			}
		}
		else
		{
			SceneManager.LoadScene((int)SceneIndex.Game);
		}
	}

	public void SetDefaultFEN()
	{
		fenInputField.text = FEN.STARTING_POSITION_FEN;
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}

	public void ShowPlayMenu()
	{
		mainMenu.SetActive(false);
		playMenu.SetActive(true);
	}

	public void ShowOptions()
	{
		mainMenu.SetActive(false);
		optionsMenu.SetActive(true);
	}

	public void ShowTesting()
	{
		mainMenu.SetActive(false);
		testingMenu.SetActive(true);
	}

	public void Back()
	{
		mainMenu.SetActive(true);
		playMenu.SetActive(false);
		optionsMenu.SetActive(false);
		testingMenu.SetActive(false);
	}

	public void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}
}