using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoChallenge : MonoBehaviour
{
    public GameObject endChallenge;
    public ChronoChallengeEnd EndOfTheChallenge;
    public GameObject timerVisual;
    private GameObject Bill;
    public Timer timer;
    private bool isFinished = true;

    public float challengeTime = 60f; //change into the inspector depending on the challenge
    public Vector3 respawnPoint;
    Vector3 posUp;
    Vector3 posDown;

    private void Start()
    {
        posUp = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 4.8f, this.transform.localPosition.z);
        posDown = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isFinished) //Commencer le challenge
        {
            Debug.Log("Whats up collider");
            //Son de debut du challenge

            //fermer porte debut -0.04
            StartCoroutine(Close());
            EndOfTheChallenge.starting(isFinished);
            //ouvrir porte fin

            timerVisual.SetActive(true);
            timer.SetTime(challengeTime, this);

            isFinished = false;
            EndOfTheChallenge.starting(isFinished);
        }
    }

    // Update is called once per frame
    public void End(bool win)
    {
        isFinished = true;
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
        StartCoroutine(Open());
    }

    IEnumerator Close()
    {
        while(this.transform.localPosition.y < posUp.y)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, posUp, 0.08f);
            yield return new WaitForSeconds(0.04f);
        }
    }

    IEnumerator Open()
    {
        while (this.transform.localPosition.y > posDown.y)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, posDown, 0.08f);
            yield return new WaitForSeconds(0.04f);
        }
    }

}
