using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public int force;

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collision");
        Bill bill = collision.collider.GetComponent<Bill>();
        if (bill != null)
        {
            Debug.Log("Collision with Bill");
            Vector3 direction = bill.transform.position - transform.position;
            bill.GetComponent<Rigidbody>().AddForce(direction * force);
        }
    }
}
