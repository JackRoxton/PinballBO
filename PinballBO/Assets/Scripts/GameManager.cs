using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void RestartGame()
    {
        
    }

    public void QuitGame()
    {
        Debug.Log("Has quit game");
        Application.Quit();
    }

    public void GameOver()
    {

    }
}
