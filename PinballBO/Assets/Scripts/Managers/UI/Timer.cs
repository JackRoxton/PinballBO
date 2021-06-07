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
    private int seconds;
    public int multiplier { get; private set; }

    //Win and lose
    public GameObject defeatScreen;
    private ChronoChallenge chronoChallenge;
    private FlipperChallenge flipperChallenge;

    private void Start()
    {
        timeLeft = timeTotal;
    }

    private void Update()
    {
        if (currentState == null)
        {
            currentState = GlobalTime;
        }
        currentState();
    }


    void GlobalTime()
    {
            if (timeLeft > timeTotal) //au cas où on reçoive du temps bonus durant un challenge
            {
                timeLeft = timeTotal;
            }
            seconds = (int)timeLeft;
            chronoText.text = seconds.ToString();


            fillValue = timeLeft / timeTotal * 100;

            timeLeft -= Time.deltaTime;

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
        currentState = GlobalTime;

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
