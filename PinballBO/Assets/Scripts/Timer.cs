using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject defeatScreen;
    private float timeLeft;
    private float timeFinished;
    private int bestTime;
    private int seconds;

    private void Start()
    {
        timeLeft = timeTotal;
        Time.timeScale = 1;
        GameManager.Instance.GameState = GameManager.gameState.InGame;
    }

    void Update()
    {
        //Ajouter "si victoire alors", faire en sorte que le best time s'affiche a la win bestTime = (int)timeFinished;

        /*else */
            if (timeLeft > timeTotal)
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
        if (GameManager.Instance.GameState == GameManager.gameState.GameOver)  // if Gamestate = GameOver, it means that this has already been called
            return;

        defeatScreen.SetActive(true);
        GameManager.Instance.GameState = GameManager.gameState.GameOver;
    }

}
