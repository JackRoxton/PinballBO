using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerChallenge : MonoBehaviour
{
    public FlipperChallenge flipper;
    public GameObject LoupioteDoor;
    public GameObject Loupiote;
    public Material noLight;
    public Material Light;

    [SerializeField]
    private bool stateSpun = false;

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.gameObject.GetComponent<Bill>();
        if (bill != null)
        {
            StartCoroutine(waitForLight(bill));
            stateSpun = true;
            if (flipper != null)
                flipper.StopChallenge();
        }
    }

    public bool SendState()
    {
        return stateSpun;
    }

    IEnumerator waitForLight(Bill bill)
    {
        if(!stateSpun)
            bill.GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitForSeconds(2);
        Loupiote.GetComponent<MeshRenderer>().material = Light;
        LoupioteDoor.GetComponent<MeshRenderer>().material = Light;
        bill.GetComponent<Rigidbody>().isKinematic = false;
    }

    static void Reset()
    {
        foreach(SpinnerChallenge spinner in FindObjectsOfType<SpinnerChallenge>())
        {
            spinner.Loupiote.GetComponent<MeshRenderer>().material = spinner.noLight;
            spinner.LoupioteDoor.GetComponent<MeshRenderer>().material = spinner.noLight;
            spinner.stateSpun = false;
        }
    }

}
