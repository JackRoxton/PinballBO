using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    private Action currentState;

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
    public int multiplier { get; private set; }

    //Win and lose
    public GameObject defeatScreen;
    private ChronoChallenge chronoChallenge;
    private FlipperChallenge flipperChallenge;

    private void Start()
    {
        timeLeft = timeTotal;
        Time.timeScale = 1;
        GameManager.Instance.GameState = GameManager.gameState.InGame;

        if (PlayerPrefs.HasKey("BestTimeLevel" + GameManager.Instance.currentLevel) && GameManager.Instance.GetCurrentChallenge() == GameManager.Challenge.Timer)
        {
            bestTime = PlayerPrefs.GetFloat("BestTimeLevel" + GameManager.Instance.currentLevel); //faire en sorte qu'on puisse recup le score
        }
        else bestTime = 60;
    }

    private void Update()
    {
        currentState();
    }


    void TimeChallenge()
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

    void FlipperChallenge()
    {
        if (timeLeft > timeTotal) //au cas où on reçoive du temps bonus durant un challenge
        {
            multiplier++;
            timeLeft -= timeTotal;
            chronoText.text = "x " + multiplier.ToString();
        }

        fillValue = timeLeft / timeTotal * 100;

        timeLeft -= Time.deltaTime;
        
        if (timeLeft < 0)
        {
            if (multiplier == 1)
            {
                timeLeft = 0;
                return;
            }
            timeLeft += timeTotal;
            multiplier--;
            chronoText.text = "x " + multiplier.ToString();
        }

        FillCircleValue(fillValue);


    }


    void FillCircleValue(float value)
    {
        float fillAmount = (value / 100.0f);
        circleFillImage.fillAmount = fillAmount;
    }


    public void SetTime(float time, ChronoChallenge currentChallenge)
    {
        currentState = TimeChallenge;

        timeLeft = time;
        timeTotal = time;
        chronoChallenge = currentChallenge;
    }

    public void SetScore(float time, FlipperChallenge currentChallenge)
    {
        currentState = FlipperChallenge;
        multiplier = 1;

        timeLeft = time;
        timeTotal = time;
        flipperChallenge = currentChallenge;
        chronoText.text = "x 1";
    }
    
    public void AddTime(float amount)
    {
        timeLeft += amount;
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



    private void Defeat()
    {
        if (chronoChallenge == null)
        {
            timeLeft = 10000f;
            timeTotal = 10000f;
        }
        else
        {
            chronoChallenge.End(false);
            chronoChallenge = null;
            //afficher un text "challenge failed" ou "time's up"
        }
        /* if (GameManager.Instance.GameState == GameManager.gameState.GameOver)  // if Gamestate = GameOver, it means that this has already been called
             return;

         GameManager.Instance.GameState = GameManager.gameState.GameOver; */
    }

}
