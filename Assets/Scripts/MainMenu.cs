using System;
using Bot;
using Chess;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

enum SceneIndex
{
	Menu = 0,
	Game = 1,
	Perft = 2,
	BotTest = 3,
}

public class MainMenu : MonoBehaviour
{
	[Header("GameObjects")]
	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject playMenu;
	[SerializeField] private GameObject optionsMenu;
	[SerializeField] private GameObject testingMenu;

	[SerializeField] private TMP_Text whiteBoiText;

	[SerializeField, Tooltip("Input field to pass on FEN")]
	private TMP_InputField fenInputField;
	[SerializeField, Tooltip("Input field to pass on PGN")]
	private TMP_InputField pgnInputField;

	private int botDepth = 5;

	private void Update()
	{
		if (playMenu.activeInHierarchy)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				botDepth--;
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				botDepth++;
			}
			botDepth = Math.Clamp(botDepth, 1, WhiteBoi.MAX_DEPTH);
			whiteBoiText.text = $"WhiteBoi({botDepth})";
		}
	}

	public void PlayGame()
	{
		if (fenInputField.text != "")
		{
			// Validate FEN
			try
			{
				Board.FromFEN(fenInputField.text.Trim());
				// Set FEN for GameManager and load game scene
				GameManager.IMPORT_FEN = fenInputField.text;
				SceneManager.LoadScene((int)SceneIndex.Game);
			}
			catch (Exception e)
			{
				fenInputField.text = $"Invalid fen: {e.Message}";
			}
		} if (pgnInputField.text != "")
		{
			// Validate PGN
			try
			{
				Board.FromPGN(pgnInputField.text);
				// Set PGN for GameManager and load game scene
				GameManager.IMPORT_PGN = pgnInputField.text;
				SceneManager.LoadScene((int)SceneIndex.Game);
			}
			catch (Exception e)
			{
				pgnInputField.text = $"Invalid pgn: {e.Message}";
			}
		}
		else
		{
			SceneManager.LoadScene((int)SceneIndex.Game);
		}
	}

	public void PlayAgainstBot(int botType)
	{
		GameManager.againstBot = true;
		GameManager.botType = (BotType)botType;
		GameManager.botDepth = botDepth;

		PlayGame();
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