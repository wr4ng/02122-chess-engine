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

	[SerializeField, Tooltip("Input field to pass on FEN or PGN notation")]
	private TMP_InputField fenInputField;

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
			// TODO Define some max depth
			botDepth = Math.Clamp(botDepth, 1, 10);
			whiteBoiText.text = $"WhiteBoi({botDepth})";
		}
	}

	public void PlayGame()
	{
		if (fenInputField.text != "")
		{
			// Validate FEN
			//TODO This is not so clean...
			try
			{
				Board.FromFEN(fenInputField.text.Trim());
				// Set FEN for GameManager and load game scene
				GameManager.IMPORT_FEN = fenInputField.text;
				SceneManager.LoadScene((int)SceneIndex.Game);
			}
			catch (System.Exception e)
			{
				fenInputField.text = $"Invalid fen: {e.Message}";
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