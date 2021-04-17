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
            Vector3 direction = -collision.GetContact(0).normal;
            direction = Vector3.ProjectOnPlane(direction, transform.up);
            LaunchPlayer(bill, direction);
            
        }
    }

    // Classic Bumper
    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.GetComponent<Bill>();
        if (bill != null)
        {
            Vector3 direction = (bill.transform.position - transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z).normalized;
            direction = Vector3.ProjectOnPlane(direction, transform.up);
            LaunchPlayer(bill, direction);

        }
    }

    void LaunchPlayer(Bill bill, Vector3 direction)
    {
        Rigidbody billBody = bill.GetComponent<Rigidbody>();
        billBody.velocity = Vector3.zero;
        AudioManager.Instance.PlayClip(source, source.clip);
        animator.SetTrigger("Bump");
        billBody.AddForce(direction * (force + billBody.velocity.magnitude * factor));
    }
}
