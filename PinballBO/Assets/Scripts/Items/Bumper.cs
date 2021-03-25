using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public int force;
    public int factor;

    AudioSource source;
    Animator animator;

    private bool isTrigger = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        foreach (MeshCollider meshCollider in GetComponentsInChildren<MeshCollider>())
        {
            if (meshCollider.isTrigger)
                isTrigger = true;
            return;
        }
    }

    // SlingShots   ( triangle bumper )
    private void OnCollisionEnter(Collision collision)
    {
        if (isTrigger) return;

        Bill bill = collision.collider.GetComponent<Bill>();
        if (bill != null)
        {
            Vector3 direction = collision.GetContact(0).normal;
            AudioManager.Instance.PlayClip(source, source.clip);
            bill.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bill.GetComponent<Rigidbody>().AddForce(-direction * (force + bill.GetComponent<Rigidbody>().velocity.magnitude * factor));
        }
    }

    // Classic Bumper
    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.GetComponent<Bill>();
        if (bill != null)
        {
            Rigidbody billBody = bill.GetComponent<Rigidbody>();
            Vector3 direction = (bill.transform.position - transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z);

            Debug.Log("Bump");
            AudioManager.Instance.PlayClip(source, source.clip);
            animator.SetTrigger("Bump");
            billBody.velocity = Vector3.zero;
            billBody.AddForce(direction * (force + billBody.velocity.magnitude * factor));
        }
    }
}
