using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("UIManager instance not found");

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    int coins = 0;
    [Header("Pause UI")]
    public GameObject pauseScreen;
    public Text pauseCurrentTimeText;
    public Text pauseBestTimeText;

    [Header("Defeat UI")]
    public GameObject defeatScreen;
    public Text defeatBestTimeText;

    [Header("Win UI")]
    public GameObject winScreen;
    public Text winBestTimeText;
    public Text winCurrentTimeText;
    public Text winScoreText;
    public GameObject NewRecord;

    [Header("Misc")]
    public Timer timer;
    public Text coinsCount;

    [Header("Flipper Challenge")]
    public GameObject FlipperChallengeCanvas;
    public GameObject scorePrefab;




    private void Update()
    {
        if (GameManager.Instance.GameState == GameManager.gameState.InGame && Input.GetKey(KeyCode.Escape))
        {
            GameManager.Instance.GameState = GameManager.gameState.Pause;
            pauseBestTimeText.text = "Best Time = " + System.Math.Round(timer.bestTime, 2).ToString();
            pauseCurrentTimeText.text = "Current Time = " + System.Math.Round(timer.timeFinished, 2).ToString();
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
                SceneManager.LoadScene(0);
                break;

            case "Restart":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;

            case "NextLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;

            case "Resume":
                pauseScreen.SetActive(false);
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                break;

            default:
                Debug.LogError("\"" + button + "\"" + " not found in switch statement");
                break;
        }
    }

    public void Win()
    {
        winScreen.SetActive(true);
        timer.SetBestTime();

        winBestTimeText.text = "Best Time = " + System.Math.Round(timer.bestTime, 2).ToString();
        winCurrentTimeText.text = "Your Time = " + System.Math.Round(timer.timeFinished, 2).ToString();
        winScoreText.text = coins + "/100";


        if (timer.bestTime < timer.timeFinished)
        {
            NewRecord.SetActive(false);
        }

    }

    public void Lose()
    {
        defeatScreen.SetActive(true);
        defeatBestTimeText.text = "Best Time = " + System.Math.Round(timer.bestTime, 2).ToString();
    }



    public void AddCoin()
    {
        coins = GameManager.Instance.coins;
        if (coins < 10)
        {
            coinsCount.text = "     " + coins.ToString() + ("/100");
        }
        else if (coins >= 10 && coins < 100)
        {
            coinsCount.text = "   " + coins.ToString() + ("/100");
        }
        else
        {
            coinsCount.text = ("100/100");
        }
    }


    // Score FlipperChallenge
    public void DisplayScore(int amount, BumpObject bumper)
    {
        GameObject go = Instantiate(scorePrefab, FlipperChallengeCanvas.transform);
        //Vector3 pos = Camera.main.WorldToScreenPoint(bumper.transform.position);
        Destroy(go, .7f);
    }

}