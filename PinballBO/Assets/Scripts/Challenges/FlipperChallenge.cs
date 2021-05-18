﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperChallenge : MonoBehaviour
{
    public static FlipperChallenge Instance;
    public int score { get; private set; }
    public int bestScore { get; private set; }
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
        goal = 100000;

        GameManager.Instance.SetCurrentChallenge(GameManager.Challenge.Flipper);
    }

    public void StopChallenge()
    {
        playing = false;
        GameManager.Instance.SetCurrentChallenge(GameManager.Challenge.Free);
    } 

    public void ChangeScore(int amount, GameObject item)
    {
        if (!playing || 
            GameManager.Instance.GameState == GameManager.gameState.Win) return;

        score += (int)(amount * multiplier);
        if (item.layer != 11)
            UIManager.Instance.timer.AddTime(2.5f);
        else
        {
            UIManager.Instance.timer.AddTime(Time.deltaTime);
        }

        // Feedback Sonore


        UIManager.Instance.DisplayScore((int)(amount * multiplier), item);

        if (score >= goal)
            Victory();
    }

    public void ChangeMultiplier(float amount)
    {
        multiplier += amount;
    }

    private void Victory()
    {
        GameManager.Instance.GameState = GameManager.gameState.Win;
        if (score > bestScore) bestScore = score;
        UIManager.Instance.FlipperChallengeWin();

        BumpObject[] bumpers = GetComponentsInChildren<BumpObject>();
        foreach (BumpObject bump in bumpers)
            bump.SetChallenge(null);

        Targets[] targets = GetComponentsInChildren<Targets>();
        foreach (Targets target in targets)
            target.SetChallenge(null);

        Rail[] rails = GetComponentsInChildren<Rail>();
        foreach (Rail rail in rails)
            rail.SetChallenge(null);

    }




    /*  Régles Timer
    Bump Avant Timer    --> Timer++
    Timer++             --> ScoreMultiplier +1
    Timer--             --> ScoreMultiplier -1
     */
}
