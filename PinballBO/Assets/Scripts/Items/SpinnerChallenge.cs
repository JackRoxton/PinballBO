using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerChallenge : MonoBehaviour
{
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
            Loupiote.GetComponent<MeshRenderer>().material = Light;
            LoupioteDoor.GetComponent<MeshRenderer>().material = Light;
        }
    }

    public bool SendState()
    {
        return stateSpun;
    }

}
