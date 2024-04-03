using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Text botNameText;

	private void Start()
	{
		// Show name of bot if playing against bot
		botNameText.text = GameManager.againstBot ? GameManager.Instance.bot.Name() : "";
	}

	public void QuitToMenu()
	{
		SceneManager.LoadScene((int)SceneIndex.Menu);
	}
}
