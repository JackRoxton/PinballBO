using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperChallenge : MonoBehaviour
{
    public static FlipperChallenge Instance;
    public int score { get; private set; }
    public int bestScore { get; private set; }
    public int goal { get; private set; }
    private float targetCount = 1;
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
        targetCount = 1;
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

        // Increase Score
        int amountScore = (int)(amount * Multiplier());
        score += amountScore;
        UIManager.Instance.DisplayScore(amountScore, item);  // Feedback : Score qui bouge jusqu'à l'interface

        // Rajoute du temps au timer
        if (item.layer != 11)
            UIManager.Instance.timer.AddTime(.7f);
        else
            UIManager.Instance.timer.AddTime(Time.deltaTime);

        // Feedback Sonore



        if (score >= goal)
            Victory();
    }

    public void TargetTouch()
    {
        targetCount += .1f;
    }

    private float Multiplier()
    {
        return targetCount * UIManager.Instance.timer.multiplier;
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
    Timer++             --> ScoretargetCount +1
    Timer--             --> ScoretargetCount -1
     */
}
