using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseScreen;

    //pause
    public void Update()
    {
        if(GameManager.Instance.GameState == GameManager.gameState.InGame && Input.GetKey(KeyCode.Escape))
        {
            GameManager.Instance.GameState = GameManager.gameState.Pause;
            pauseScreen.SetActive(true);
        }
    }

    //Buttons
    public void OnClickEnter(string button)
    {
        switch (button)
        {
            case "GoMainMenu":
                GameManager.Instance.GameState = GameManager.gameState.MainMenu;
                break;

            case "Restart":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;

            case "NextLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;

            case "Continue":
                pauseScreen.SetActive(false);
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                break;

            default:
                Debug.LogError("\"" + button + "\"" + " not found in switch statement");
                break;
        }
    }
}
