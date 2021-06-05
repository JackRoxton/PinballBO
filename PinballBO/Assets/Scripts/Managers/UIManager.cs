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

    public MainMenuManager mainMenu;
    [Space]
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
    public Text winBestScoreText;
    public Text winCurrentScoreText;
    public Text winScoreText;
    public GameObject NewRecord;

    [Header("Misc")]
    public Timer timer;
    public Text coinsCount;
    private float bestTime;

    [Header("Flipper Challenge")]
    public GameObject FlipperChallengeCanvas;
    public Text FlipperChallengeScore;
    public GameObject scorePrefab;
    public AnimationCurve scoreScaleAnimationCurve;
    private int initialScoreScale;


    private void Awake()
    {
        instance = this;

        initialScoreScale = FlipperChallengeScore.fontSize;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("BestTimeLevel" + GameManager.Instance.currentLevel))
        {
            bestTime = PlayerPrefs.GetFloat("BestTimeLevel" + GameManager.Instance.currentLevel); //faire en sorte qu'on puisse recup le score
        }
        else bestTime = 60;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameManager.gameState.InGame && Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Pause");
            GameManager.Instance.GameState = GameManager.gameState.Pause;
            pauseBestTimeText.text = "Best Time = " + System.Math.Round(bestTime, 2).ToString();
            pauseCurrentTimeText.text = "Current Time = " + System.Math.Round(Time.timeSinceLevelLoad, 2).ToString();


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
                mainMenu.gameObject.SetActive(true);
                mainMenu.OnClickEnter("BackToMenu");
                break;

            case "Restart":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                GameManager.Instance.GameState = GameManager.gameState.InGame;
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
        SetBestTime();

        winBestScoreText.text = "Best Time = " + System.Math.Round(bestTime, 2).ToString();
        winCurrentScoreText.text = "Your Time = " + System.Math.Round(Time.timeSinceLevelLoad, 2).ToString();
        winScoreText.text = coins + "/40";


        if (bestTime > Time.timeSinceLevelLoad)
        {
            NewRecord.SetActive(true);
        }

    }

    private void SetBestTime()
    {
        if (bestTime > Time.timeSinceLevelLoad)
        {
            if (GameManager.Instance.currentLevel == 1)
            {
                PlayerPrefs.SetFloat("BestTimeLevel" + GameManager.Instance.currentLevel, Time.timeSinceLevelLoad);
                bestTime = Time.timeSinceLevelLoad;
            }
        }
    }


    public void AddCoin()
    {
        coins = GameManager.Instance.coins;
        if (coins < 10)
        {
            coinsCount.text = "     " + coins.ToString() + ("/40");
        }
        else if (coins >= 10 && coins < 100)
        {
            coinsCount.text = "   " + coins.ToString() + ("/40");
        }
        else
        {
            coinsCount.text = ("100/100");
        }
    }

    public void Lose()
    {
        defeatScreen.SetActive(true);
        defeatBestTimeText.text = "Best Time = " + System.Math.Round(bestTime, 2).ToString();
    }



    #region FlipperChalllenge
    public void FlipperChallengeWin()
    {
        FlipperChallengeCanvas.SetActive(false);
        timer.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void InitializeFlipperChallengeUI(float goal)
    {
        FlipperChallengeCanvas.SetActive(true);
        FlipperChallengeScore.GetComponent<Text>().text = "0"; // Affiche le score
        FlipperChallengeScore.transform.parent.GetComponent<Text>().text = "      / " + goal.ToString(); // Affiche le score
        timer.transform.GetChild(0).gameObject.SetActive(true);
        timer.SetScore(6, FlipperChallenge.Instance);
    }

    public void DisplayScore(int amount, GameObject item)
    {
        GameObject go = Instantiate(scorePrefab, FlipperChallengeCanvas.transform);
        go.GetComponent<Text>().text = amount.ToString();
        StartCoroutine(AddScoreUI(go.GetComponent<RectTransform>(), item, amount));
    }

    IEnumerator AddScoreUI(RectTransform rect, GameObject target, int amount)
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
        for (float t = 0; t < .25f; t += Time.deltaTime)
        {
            Vector3 newPos = Vector3.Lerp(rect.position, FlipperChallengeScore.rectTransform.position, t * 100 * Time.deltaTime);
            rect.position = newPos;
            yield return new WaitForEndOfFrame();
        }
        FlipperChallengeScore.text = (FlipperChallenge.Instance.score + amount).ToString();
        Destroy(rect.gameObject);

        for (float t = 0; t < 1.1f; t += Time.deltaTime)
        {
            FlipperChallengeScore.fontSize = (int)(scoreScaleAnimationCurve.Evaluate(t) * initialScoreScale);
            yield return new WaitForEndOfFrame();
        }



    }
    #endregion
}