using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Text botNameText;

	private void Update()
	{
		if (GameManager.againstBot)
		{
			botNameText.text = $"{GameManager.Instance.bot.Name()}{(GameManager.Instance.botIsCalculating ? " is thinking..." : "")}";
		}
	}

	public void QuitToMenu()
	{
		SceneManager.LoadScene((int)SceneIndex.Menu);
	}
}
