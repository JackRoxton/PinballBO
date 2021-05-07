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
    public Text FlipperChallengeScore;
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
    public void InitializeFlipperChallengeUI(float goal)
    {
        FlipperChallengeCanvas.SetActive(true);
        FlipperChallengeScore.text = "0 / " + goal.ToString();
    }

    public void DisplayScore(int amount, BumpObject bumper)
    {
        GameObject go = Instantiate(scorePrefab, FlipperChallengeCanvas.transform);
        StartCoroutine(AddScoreUI(go.GetComponent<RectTransform>(), bumper));
    }

    IEnumerator AddScoreUI(RectTransform rect, BumpObject target)
    {
        float offset = 0;

        Vector3 pos = Camera.main.WorldToScreenPoint(target.transform.position) + Vector3.up * offset;
        for (float t = 0; t < .7f; t += Time.deltaTime)
        {
            Vector3 newPos = pos + Vector3.up * offset;
            rect.position = newPos;
            offset += 100 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        for (float t = 0; t < .3f; t += Time.deltaTime)
        {
            Vector3 newPos = Vector3.Lerp(rect.position, FlipperChallengeScore.rectTransform.position, t * 100 * Time.deltaTime);
            rect.position = newPos;
            yield return new WaitForEndOfFrame();
        }
        FlipperChallengeScore.text = FlipperChallenge.Instance.score.ToString() + " / " + FlipperChallenge.Instance.goal.ToString();
        Destroy(rect.gameObject);

    }


    //class ScoreUI : MonoBehaviour
    //{
    //    GameObject go;
    //    int score;
    //    BumpObject target;
    //    float offset;
    //    private void Awake()
    //    {
    //        offset = 0;
    //        Destroy(go, .7f);
    //    }

    //    private void Update()
    //    {
    //        Vector3 pos = Camera.main.WorldToScreenPoint(target.transform.position) + Vector3.up * offset;
    //        offset += 10 * Time.deltaTime;
    //    }
    //}
}