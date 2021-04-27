using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnstile : BumpObject
{
    [SerializeField] private float speed = 1;

    private void Start()
    {
        animator.speed = this.speed;

        float force = this.force * speed;
        this.force = (int)force;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.collider.GetComponent<Bill>();
        if (bill != null)
        {
            Vector3 direction = -collision.GetContact(0).normal;
            bill.GetComponent<Rigidbody>().velocity = direction * speed * force;
        }
    }
}
