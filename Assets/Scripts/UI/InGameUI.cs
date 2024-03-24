using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
	public void QuitToMenu()
	{
		SceneManager.LoadScene((int)SceneIndex.Menu);
	}
}
