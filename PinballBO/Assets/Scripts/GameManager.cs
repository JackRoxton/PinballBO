using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("GameManager instance not found");

            return instance;
        }
    }

    public int currentLevel;
    public int coins = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public enum gameState
    {
        MainMenu,
        InGame,
        GameOver,
        Pause,
        Win,
    }

    #region GameState

    private gameState currentState;
    public gameState GameState

    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
            switch (currentState)
            {
                case gameState.MainMenu:
                    //Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    SceneManager.LoadScene(0);
                    break;

                case gameState.InGame:
                        //Cursor.lockState = CursorLockMode.Locked;
                        //Cursor.visible = false;
                        Time.timeScale = 1;
                        break;

                case gameState.Pause:
                    //Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    Time.timeScale = 0;
                    break; 

                case gameState.Win:
                    //Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    UIManager.Instance.Win();

                    if (!PlayerPrefs.HasKey("BestCoinLevel" + currentLevel)) //Checks if there was a previously saved record and if not, sets it to 0
                    {
                        PlayerPrefs.SetInt("BestCoinLevel" + currentLevel, 0);
                    }

                    if (PlayerPrefs.GetInt("BestCoinLevel" + currentLevel) < coins)
                    {
                        PlayerPrefs.SetInt("BestCoinLevel" + currentLevel, coins); //saves the best coin record
                    }

                    Time.timeScale = 0;
                    break;
                    

                case gameState.GameOver:
                    //Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    UIManager.Instance.Lose();
                    Time.timeScale = 0;
                    break;
            }
        }
    }
    #endregion

    public void AddCoin()
    {
        coins++;
    }

}
