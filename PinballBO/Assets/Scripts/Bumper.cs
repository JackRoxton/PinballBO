using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public int force;
    public int factor;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.collider.GetComponent<Bill>();
        if (bill != null)
        {
            Vector3 direction = collision.GetContact(0).normal;
            AudioManager.Instance.PlayClip(source, source.clip);
            bill.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log(bill.GetComponent<Rigidbody>().velocity.magnitude);
            bill.GetComponent<Rigidbody>().AddForce(-direction * (force + bill.GetComponent<Rigidbody>().velocity.magnitude * factor));
        }
    }
}
