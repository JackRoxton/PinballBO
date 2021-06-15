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
    public Button reflectionButton;
    private bool billReflection = false;

    [Header("Defeat UI")]
    public GameObject defeatScreen;
    public Text defeatBestTimeText;

    [Header("Win UI")]
    public GameObject winScreen;
    public Text winBestScoreText;
    public Text winCurrentScoreText;
    public Text winScoreText;
    public GameObject NewRecord;
    public GameObject credit;

    [Header("Misc")]
    public Timer timer;
    public GameObject timerVisual;
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

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (GameManager.Instance.GameState == GameManager.gameState.InGame)
            {
                GameManager.Instance.GameState = GameManager.gameState.Pause;
                pauseBestTimeText.text = "Best Time = " + System.Math.Round(bestTime, 2).ToString();
                pauseCurrentTimeText.text = "Current Time = " + System.Math.Round(Time.timeSinceLevelLoad, 2).ToString();

                if (GameManager.Instance.GetCurrentChallenge() == GameManager.Challenge.Flipper)
                    FlipperChallengeCanvas.SetActive(false);

                coinsCount.gameObject.SetActive(false);
                pauseScreen.SetActive(true);
            }
            else if (GameManager.Instance.GameState == GameManager.gameState.Pause)
            {
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                OnClickEnter("Resume");
            }


        }

    }

    //Buttons
    public void OnClickEnter(string button)
    {
        switch (button)
        {
            case "GoMainMenu":
                pauseScreen.SetActive(false);
                winScreen.SetActive(false);
                GameManager.Instance.GameState = GameManager.gameState.MainMenu;
                mainMenu.gameObject.SetActive(true);
                mainMenu.OnClickEnter("BackToMenu");
                Bill.Instance.Reset();
                GameManager.Instance.ResetCoins();
                break;
            case "Restart":
                Bill.Instance.Reset();
                winScreen.SetActive(false);
                GameManager.Instance.coins = 0;
                FlipperChallenge.Instance.clearedOnce = false;
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                OnClickEnter("Resume");
                AddCoin();
                GameManager.Instance.ResetCoins();
                break;

            case "Credit":
                winScreen.SetActive(false);
                mainMenu.gameObject.SetActive(false);
                StartCoroutine(Credit());
                break;

            case "Resume":
                pauseScreen.SetActive(false);
                coinsCount.gameObject.SetActive(true);
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                if (GameManager.Instance.GetCurrentChallenge() == GameManager.Challenge.Flipper)
                    FlipperChallengeCanvas.SetActive(true);
                AddCoin();
                break;

            default:
                Debug.LogError("\"" + button + "\"" + " not found in switch statement");
                break;
        }
    }

    public void BillReflection()
    {
        billReflection = !billReflection;
        Bill.Instance.Reflection(billReflection);
        reflectionButton.image.color = billReflection ? Color.white : Color.grey;
        reflectionButton.GetComponentInChildren<Text>().enabled = billReflection;
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

    IEnumerator Credit()
    {
        Vector3 initial = credit.GetComponent<RectTransform>().localPosition;
        credit.SetActive(true);
        Vector3 target = credit.GetComponent<RectTransform>().localPosition;
        target.y += credit.GetComponent<RectTransform>().rect.height;
        yield return new WaitForSecondsRealtime(2);

        for (float d = 0; d < target.y; d += 2)
        {
            credit.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(credit.GetComponent<RectTransform>().localPosition, target, 2);
            yield return new WaitForEndOfFrame();
        }
        credit.SetActive(false);
        credit.GetComponent<RectTransform>().localPosition = initial;

        OnClickEnter("GoMainMenu");
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
            coinsCount.text = "     " + coins.ToString() + "/40";
        }
        else if (coins >= 10 && coins < 100)
        {
            coinsCount.text = "   " + coins.ToString() + "/40";
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
        timerVisual.gameObject.SetActive(false);
    }
    public void InitializeFlipperChallengeUI(float goal)
    {
        FlipperChallengeCanvas.SetActive(true);
        FlipperChallengeScore.GetComponent<Text>().text = "0"; // Affiche le score
        FlipperChallengeScore.transform.parent.GetComponent<Text>().text = "      / " + goal.ToString(); // Affiche le score
        timerVisual.SetActive(true);
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