using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Chess;

public class InGameUI : MonoBehaviour
{
	// Singleton instance
	public static InGameUI Instance { get; private set; }

	[Header("UI References")]
	[SerializeField]
	private TMP_Text botNameText;

	[SerializeField]
	private Image selectedPromotionImage;

	[SerializeField]
	private GameObject endGameObject;
	[SerializeField]
	private TMP_Text endGameText;

	[Header("Sprites")]
	[SerializeField] private Sprite knighSprite;
	[SerializeField] private Sprite bishopSprite;
	[SerializeField] private Sprite rookSprite;
	[SerializeField] private Sprite queenSprite;

	[Header("Text Updates")]
	[SerializeField] private TMP_Text evalText;


	private void Awake()
	{
		// Singleton setup
		if (Instance != null)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	private void Update()
	{
		if (GameManager.againstBot)
		{
			botNameText.text = $"{GameManager.Instance.bot.Name()}{(GameManager.Instance.botIsCalculating ? " is thinking..." : "")}";
		}
	}

	public void SetPromotionSprite(int promotionType)
	{
		switch (promotionType)
		{
			case Piece.Knight:
				selectedPromotionImage.sprite = knighSprite;
				break;
			case Piece.Bishop:
				selectedPromotionImage.sprite = bishopSprite;
				break;
			case Piece.Rook:
				selectedPromotionImage.sprite = rookSprite;
				break;
			case Piece.Queen:
				selectedPromotionImage.sprite = queenSprite;
				break;
			default:
				Debug.LogError($"Trying to set invalid promotion type: {promotionType}");
				break;

		}
	}

	public void QuitToMenu()
	{
		GameManager.IMPORT_FEN = ""; // Reset game FEN
		SceneManager.LoadScene((int)SceneIndex.Menu);
	}

	public void EndGame(string endMessage)
	{
		endGameText.text = endMessage;
		endGameObject.SetActive(true);
	}

	public void UpdateEvalText(string eval)
	{
		evalText.SetText($"Eval: {eval}");
	}
}