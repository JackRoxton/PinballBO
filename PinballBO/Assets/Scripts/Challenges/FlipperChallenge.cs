using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperChallenge : MonoBehaviour
{
    private int score = 0;
    private int goal;
    private float multiplier = 1;

    private void Awake()
    {
        BumpObject[] bumpers = GetComponentsInChildren<BumpObject>();
        foreach (BumpObject bump in bumpers)
            bump.SetChallenge(this);
        Targets[] targets = GetComponentsInChildren<Targets>();
        foreach (Targets target in targets)
            target.SetChallenge(this);

        Begin();
    }

    public void Begin()
    {
        score = 0;
        multiplier = 1;
        goal = 1000;
    }

    public void ChangeScore(int amount)
    {
        score += (int)(amount * multiplier);
        Debug.Log(score + " / " + goal);
        if (score >= goal)
            Victory();
    }

    public void ChangeMultiplier(float amount)
    {
        multiplier += amount;
    }

    private void Victory()
    {
        // UIManager.Victory();
        Debug.Log("Victory");
    }
}
