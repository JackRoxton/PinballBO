using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoChallengeEnd : MonoBehaviour
{
    public ChronoChallenge StartOfTheChallenge;
    private bool isFinished;

    Vector3 posUp;
    Vector3 posDown;

    private void Start()
    {
        posUp = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
        posDown = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - 4.8f, this.transform.localPosition.z);
    }

    public void Starting(bool currentstate)
    {
        isFinished = currentstate;
        StartCoroutine(Open());
    }

    public void Closed()
    {
        StartCoroutine(Close());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isFinished)
        {
            isFinished = true;
            StartOfTheChallenge.End(true);
            StartCoroutine(Close());
        }
    }

    IEnumerator Close()
    {
        while (this.transform.localPosition.y < posUp.y)
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
