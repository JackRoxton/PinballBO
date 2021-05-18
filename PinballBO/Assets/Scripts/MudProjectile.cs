using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudProjectile : MonoBehaviour
{
    public GameObject mud;

    private bool destroying = false;

    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.gameObject.GetComponent<Bill>();
        if (bill != null)
            return;

        Mud puddle = other.gameObject.GetComponent<Mud>();
        if (puddle != null)
        {
            Destroy(this.gameObject);
            destroying = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (destroying)
            return;

        Bill bill = collision.gameObject.GetComponent<Bill>();
        if (bill == null)
            Instantiate(mud, this.transform.position - (Vector3.up * 0.3f), Quaternion.identity);

        Destroy(this.gameObject);
    }
}
