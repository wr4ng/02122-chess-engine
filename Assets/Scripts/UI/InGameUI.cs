using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Chess;

public class InGameUI : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField]
	private TMP_Text botNameText;

	[SerializeField]
	private Image selectedPromotionImage;

	//TODO Make scriptable object or similar holding the current theme
	[Header("Sprites")]
	[SerializeField] private Sprite knighSprite;
	[SerializeField] private Sprite bishopSprite;
	[SerializeField] private Sprite rookSprite;
	[SerializeField] private Sprite queenSprite;

	private void Update()
	{
		if (GameManager.againstBot)
		{
			botNameText.text = $"{GameManager.Instance.bot.Name()}{(GameManager.Instance.botIsCalculating ? " is thinking..." : "")}";
		}
		//TODO A little stupid to do this every frame...
		switch (GameManager.Instance.selectedPromotionType)
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
				break;
		}
	}

	public void QuitToMenu()
	{
		SceneManager.LoadScene((int)SceneIndex.Menu);
	}
}
