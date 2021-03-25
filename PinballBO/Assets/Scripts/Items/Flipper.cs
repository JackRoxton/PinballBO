using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    int force;
    [SerializeField] float rate;
    [SerializeField] float factor;

    bool IsMoving = false;
    float currentVelocity;
    Vector3 normal;

    private void Awake()
    {
        normal = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shoot());
            IsMoving = true;
        }
    }


    IEnumerator Shoot()
    {
        Quaternion target = Quaternion.Euler(0, factor, 0) * transform.rotation;
        Quaternion initial = transform.rotation;

        while (Quaternion.Angle(transform.rotation, target) > 3)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target, rate);
            yield return new WaitForEndOfFrame();
        }

        IsMoving = false;
        while (Quaternion.Angle(transform.rotation, initial) > 3)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, initial, rate);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.collider.GetComponent<Bill>();
        normal = collision.GetContact(0).normal;
        Debug.Log(-normal);
    }

    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.GetComponent<Bill>();
        if (bill != null && IsMoving)
        {
            bill.GetComponent<Rigidbody>().velocity.Normalize();
            bill.GetComponent<Rigidbody>().velocity = Quaternion.Euler(normal) * bill.GetComponent<Rigidbody>().velocity;
        }
    }

}
