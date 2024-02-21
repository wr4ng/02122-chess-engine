using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField, Tooltip("Input field to pass on FEN or PGN notation")] private TMP_Text inputFieldCo;

    

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Takes the imput from the text field and sets the position 
    public void playWithInput()
    {
        string input = inputFieldCo.text;
        Debug.Log(input);

        // TODO : Add the logic to set the position from the input and check if it is valid FEN or PGN notation
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
