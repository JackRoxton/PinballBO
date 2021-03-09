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
    private float timeLeft;
    private float timeFinished;
    private int bestTime;
    private int seconds;

    private void Start()
    {
        timeLeft = timeTotal;
    }

    void Update()
    {
        //Ajouter "si victoire alors", faire en sorte que le best time s'affiche a la win bestTime = (int)timeFinished;

        /*else */
        if (timeLeft < 0)
        {
            //Game Over
        }
        else
        {
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
        }
    }

    void FillCircleValue(float value)
    {
        float fillAmount = (value / 100.0f);
        circleFillImage.fillAmount = fillAmount;
    }

}
