using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField,Range(0,100)]
    private float boosterForce;

    private void OnTriggerStay(Collider other)
    {
        Bill bill = other.gameObject.GetComponent<Bill>();
        if(bill != null)
        {
            Boost(bill);
        }
    }

    void Boost(Bill bill)
    {
        bill.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.localRotation * Vector3.forward * boosterForce,ForceMode.Acceleration);
    }
}
