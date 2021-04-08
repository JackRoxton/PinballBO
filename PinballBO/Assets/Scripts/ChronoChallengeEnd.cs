using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoChallengeEnd : MonoBehaviour
{
    public ChronoChallenge StartOfTheChallenge;
    private bool isFinished;

    public void isFinishedTrigger(bool currentstate)
    {
        isFinished = currentstate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isFinished)
        {
            isFinished = true;
            StartOfTheChallenge.OpenDoors(true);
        }
    }
}
