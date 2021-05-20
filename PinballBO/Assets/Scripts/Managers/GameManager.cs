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
    [System.NonSerialized] public float musicVolume = 1;
    [System.NonSerialized] public float effectVolume = 1;

    private Challenge currentChallenge;
    public enum Challenge
    {
        Flipper,
        Timer,
        Parkour,
        Free,
    }

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
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
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    break;

                case gameState.InGame:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 1;
                    break;

                case gameState.Pause:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    Time.timeScale = 0;
                    break;

                case gameState.Win:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;

                    if (currentChallenge == Challenge.Timer)
                    {
                        UIManager.Instance.Win();

                    }
                    else if (currentChallenge == Challenge.Flipper)
                    {
                        UIManager.Instance.FlipperChallengeWin();

                    }
                    //else if (currentChallenge == Challenge.Parkour)
                    //{
                        //UIManager.Instance.TimerParkourWin();
                        
                    //}


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


    #region Challenge
    public Challenge GetCurrentChallenge()
    {
        return currentChallenge;
    }
    public void SetCurrentChallenge(Challenge challenge)
    {
        if (currentState != gameState.InGame)
            currentState = gameState.InGame;

        currentChallenge = challenge;
        switch (currentChallenge)
        {
            case Challenge.Flipper:
                UIManager.Instance.InitializeFlipperChallengeUI(FlipperChallenge.Instance.goal);

                break;
            case Challenge.Timer:
                
                break;
            case Challenge.Parkour:
                
                break;
        }
    }
    #endregion


    public void AddCoin()
    {
        coins++;

        // Feedback Sonore
    }

    public void StoreMusicVolume(float volume)
    {
        musicVolume = volume;
    }
    public void StoreEffectVolume(float volume)
    {
        effectVolume = volume;
    }
}
