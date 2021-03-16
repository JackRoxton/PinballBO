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

    int currentLevel;

    private void Start()
    {
        timeLeft = timeTotal;
        Time.timeScale = 1;
        GameManager.Instance.GameState = GameManager.gameState.InGame;
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel == 1)
        {
            if (PlayerPrefs.HasKey("Level1BestTime"))
            {
                bestTime = PlayerPrefs.GetFloat("Level1BestTime"); //faire en sorte qu'on puisse recup le score
            }
            else bestTime = 60;

        }
    }

    void Update()
    {
        //Ajouter "si victoire alors", faire en sorte que le best time s'affiche a la win bestTime = (int)timeFinished;
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

        GameManager.Instance.GameState = GameManager.gameState.GameOver;
    }

    public float SetBestTime()
    {
        if (bestTime > timeFinished)
        {
            if (currentLevel == 1)
            {
                PlayerPrefs.SetFloat("Level1BestTime", timeFinished);
            }
        }

        return bestTime;
    }

}
