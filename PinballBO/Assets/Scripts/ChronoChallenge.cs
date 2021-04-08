using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoChallenge : MonoBehaviour
{
    public GameObject endChallenge;
    public GameObject timerVisual;
    public ChronoChallengeEnd EndOfTheChallenge;
    private GameObject Bill;
    public Timer timer;
    private bool isFinished = true;

    public float challengeTime = 60f; //change into the inspector depending on the challenge
    public Vector3 respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isFinished)
        {
            Debug.Log("Whats up collider");
            //Son de debut du challenge

            //Fermeture Porte debut et ouverture Porte fin

            //Set Respawn ? Ou alors rouvrir les portes

            timerVisual.SetActive(true);
            timer.SetTime(challengeTime, this);
            isFinished = false;
            EndOfTheChallenge.isFinishedTrigger(isFinished);
        }
    }

    // Update is called once per frame
    public void OpenDoors(bool win)
    {
        isFinished = true;
        EndOfTheChallenge.isFinishedTrigger(isFinished);
        timerVisual.SetActive(false);
        Debug.Log("tu devrais pas être la ptdr");
        if (win)
        {
            timer.SetTime(10000f, null);
        }
        else
        {
            Bill = GameObject.FindGameObjectWithTag("Player");
            Bill.transform.position = respawnPoint;
        }
        //ouvrir la porte debut et faire respawn
    }
}
