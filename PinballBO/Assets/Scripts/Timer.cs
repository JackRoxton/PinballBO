using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //Filling
    [Range(0, 100)]
    public float fillValue = 50;
    public Image circleFillImage;
    public Text chronoText;

    //Timer
    public float timeTotal = 30f;
    private float timeLeft;
    public float timeFinished;
    public float bestTime;
    private int seconds;

    //Win and lose
    public GameObject defeatScreen;
    private ChronoChallenge challenge;

    private void Start()
    {
        timeLeft = timeTotal;
        Time.timeScale = 1;
        GameManager.Instance.GameState = GameManager.gameState.InGame;

        if (PlayerPrefs.HasKey("BestTimeLevel" + GameManager.Instance.currentLevel))
        {
            bestTime = PlayerPrefs.GetFloat("BestTimeLevel" + GameManager.Instance.currentLevel); //faire en sorte qu'on puisse recup le score
        }
        else bestTime = 60;
    }

    void Update()
    {
        
            if (timeLeft > timeTotal) //au cas où on reçoive du temps bonus durant un challenge
            {
                timeLeft = timeTotal;
            }
            seconds = (int)timeLeft;
            chronoText.text = seconds.ToString();


            fillValue = timeLeft / timeTotal * 100;

            timeLeft -= Time.deltaTime;
            timeFinished += Time.deltaTime;
            

            FillCircleValue(fillValue);

        if (timeLeft < 0)
        {
            Defeat();
        }
    }

    void FillCircleValue(float value)
    {
        float fillAmount = (value / 100.0f);
        circleFillImage.fillAmount = fillAmount;
    }

    private void Defeat()
    {
        if (challenge == null)
        {
            timeLeft = 10000f;
            timeTotal = 10000f;
        }
        else
        {
            challenge.End(false);
            challenge = null;
            //afficher un text "challenge failed" ou "time's up"
        }
        /* if (GameManager.Instance.GameState == GameManager.gameState.GameOver)  // if Gamestate = GameOver, it means that this has already been called
             return;

         GameManager.Instance.GameState = GameManager.gameState.GameOver; */
    }

    public void SetTime(float time, ChronoChallenge currentChallenge)
    {
        timeLeft = time;
        timeTotal = time;
        challenge = currentChallenge;
    }

    public float SetBestTime()
    {
        if (bestTime > timeFinished)
        {
            if (GameManager.Instance.currentLevel == 1)
            {
                PlayerPrefs.SetFloat("BestTimeLevel" + GameManager.Instance.currentLevel, timeFinished);
            }
        }

        return bestTime;
    }

}
