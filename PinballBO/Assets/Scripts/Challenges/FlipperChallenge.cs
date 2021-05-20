using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperChallenge : MonoBehaviour
{
    public static FlipperChallenge Instance;
    [SerializeField] private ParticleSystem yeah;
    public int score { get; private set; }
    public int goal { get; private set; }
    public int scoreToReach;
    private float targetCount = 1;
    private bool playing = false;

    private void Start()
    {
        Instance = this;

        yeah.gameObject.SetActive(false);

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
<<<<<<< HEAD
        targetCount = 1;
        goal = scoreToReach;
=======
        multiplier = 1;
        goal = 1000;
>>>>>>> parent of 71c1930 (Multiplicateur Timer pour FlipperChallenge)

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

<<<<<<< HEAD
        // Increase Score
        int amountScore = (int)(amount * Multiplier());
        score += amountScore;
        if (amount > 0)
            UIManager.Instance.DisplayScore(amountScore, item);  // Feedback : Score qui bouge jusqu'à l'interface

        // Rajoute du temps au timer
        if (item.layer != 11)
            UIManager.Instance.timer.AddTime(.7f);
        else
            UIManager.Instance.timer.AddTime(Time.deltaTime);

        // Feedback Sonore


=======
        score += (int)(amount * multiplier);
        // Feedback Sonore

    
        UIManager.Instance.DisplayScore((int)(amount * multiplier), item);
>>>>>>> parent of 71c1930 (Multiplicateur Timer pour FlipperChallenge)

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
<<<<<<< HEAD
        if (score > bestScore) bestScore = score;
        UIManager.Instance.FlipperChallengeWin();
        yeah.gameObject.SetActive(true);
        


        // items do not earn points anymore
        BumpObject[] bumpers = GetComponentsInChildren<BumpObject>();
        foreach (BumpObject bump in bumpers)
            bump.SetChallenge(null);

        Targets[] targets = GetComponentsInChildren<Targets>();
        foreach (Targets target in targets)
            target.SetChallenge(null);

        Rail[] rails = GetComponentsInChildren<Rail>();
        foreach (Rail rail in rails)
            rail.SetChallenge(null);

        playing = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bill>() != null)
            if(!playing)
                Begin();
    }
=======
        GameManager.Instance.GameState = GameManager.gameState.Win;
        UIManager.Instance.FlipperChallengeWin();
    }
>>>>>>> parent of 71c1930 (Multiplicateur Timer pour FlipperChallenge)
}
