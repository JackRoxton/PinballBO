using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    [SerializeField,Range(-10,20)]
    private float dragStrength;
    private float deathTimer = 5;

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

    private void Update()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
            Destroy(this.gameObject);
    }
}
