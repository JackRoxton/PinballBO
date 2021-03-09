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

    private void Start()
    {
        timeLeft = timeTotal;
    }

    void Update()
    {
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
            int Seconds = (int)timeLeft;
            chronoText.text = Seconds.ToString();


            fillValue = timeLeft / timeTotal * 100;
            Debug.Log(fillValue);

            timeLeft -= Time.deltaTime;
            FillCircleValue(fillValue);
        }
    }

    void FillCircleValue(float value)
    {
        float fillAmount = (value / 100.0f);
        circleFillImage.fillAmount = fillAmount;
    }

}
