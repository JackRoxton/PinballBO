using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperChallenge : MonoBehaviour
{
    private int score = 0;
    private int goal;

    public void LaunchChallenge()
    {
        score = 0;
    }

    public void ChangeScore(int amount)
    {
        score += amount;
        if (score >= goal)
            Victory();
    }

    private void Victory()
    {

    }
}
