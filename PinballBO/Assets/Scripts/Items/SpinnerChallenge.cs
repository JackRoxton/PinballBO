﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerChallenge : MonoBehaviour
{
    public FlipperChallenge flipper;
    public GameObject LoupioteDoor;
    public GameObject Loupiote;
    public Material Light;

    [SerializeField]
    private bool stateSpun = false;

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.gameObject.GetComponent<Bill>();
        if (bill != null)
        {
            stateSpun = true;
            StartCoroutine(waitForLight());
            if (flipper != null)
                flipper.StopChallenge();
        }
    }

    public bool SendState()
    {
        return stateSpun;
    }

    IEnumerator waitForLight()
    {
        yield return new WaitForSeconds(2);
        Loupiote.GetComponent<MeshRenderer>().material = Light;
        LoupioteDoor.GetComponent<MeshRenderer>().material = Light;
    }

}
