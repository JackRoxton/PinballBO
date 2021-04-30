using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerChallenge : MonoBehaviour
{
    //l'apparence de ce spinner pourra être changé

    [SerializeField]
    private bool stateSpun = false;

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.gameObject.GetComponent<Bill>();
        if (bill != null)
        {
            stateSpun = true;
        }
    }

    public bool SendState()
    {
        return stateSpun;
    }

}
