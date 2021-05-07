﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperChallenge : MonoBehaviour
{
    public static FlipperChallenge Instance;
    public int score { get; private set; }
    public int goal { get; private set; }
    private float multiplier = 1;
    private bool playing = false;

    private void Start()
    {
        Instance = this;

        #region Initialization
        BumpObject[] bumpers = GetComponentsInChildren<BumpObject>();
        foreach (BumpObject bump in bumpers)
            bump.SetChallenge(this);

        Targets[] targets = GetComponentsInChildren<Targets>();
        foreach (Targets target in targets)
            target.SetChallenge(this);

        Rail[] rails = GetComponentsInChildren<Rail>();
        foreach (Rail rail in rails)
            rail.SetChallenge(this);
        #endregion

        Begin();
    }

    public void Begin()
    {
        playing = true;

        score = 0;
        multiplier = 1;
        goal = 1000000;

        UIManager.Instance.InitializeFlipperChallengeUI(goal);
    }

    public void StopChallenge()
    {
        playing = false;
    } 


    public void ChangeScore(Rail rail, int amount)
    {
        ChangeScore(amount);
    }

    public void ChangeScore(int amount, BumpObject bumper = null)
    {
        // if GameManager.GameState == Victory return;
        score += (int)(amount * multiplier);
        // Feedback Sonore

        if (bumper != null)
            UIManager.Instance.DisplayScore((int)(amount * multiplier), bumper);

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
