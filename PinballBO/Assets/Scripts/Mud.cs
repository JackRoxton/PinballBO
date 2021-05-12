using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    [SerializeField,Range(0,25)]
    private float dragStrength;

    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.gameObject.GetComponent<Bill>();
        if(bill != null)
        {
            bill.gameObject.GetComponent<Rigidbody>().drag += dragStrength;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Bill bill = other.gameObject.GetComponent<Bill>();
        if (bill != null)
        {
            bill.gameObject.GetComponent<Rigidbody>().drag -= dragStrength;
        }
    }
}
