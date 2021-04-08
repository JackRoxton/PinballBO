using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour
{
    [SerializeField]
    Transform RespawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.gameObject.GetComponent<Bill>();
        if(bill != null)
        {
            bill.transform.position = RespawnPoint.transform.position;
            bill.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
