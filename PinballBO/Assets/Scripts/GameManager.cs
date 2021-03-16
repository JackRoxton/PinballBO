using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

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

    public UIManager uiManager;

    private void Awake()
    {
        instance = this;
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
                    uiManager.Win();
                    Time.timeScale = 0;
                    break;
                    

                case gameState.GameOver:
                    //Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    uiManager.Lose();
                    Time.timeScale = 0;
                    break;
            }
        }
    }
    #endregion

}
