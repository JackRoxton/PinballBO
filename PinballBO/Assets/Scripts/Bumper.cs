using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public int force;

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.collider.GetComponent<Bill>();
        if (bill != null)
        {
            Vector3 direction = collision.GetContact(0).normal;
            Debug.Log(direction);
            bill.GetComponent<Rigidbody>().AddForce(-direction * force);
        }
    }
}
