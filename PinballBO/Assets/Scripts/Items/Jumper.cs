using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField,Range(0,100)]
    private float jumperForce;

    Animator anim;

    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.gameObject.GetComponent<Bill>();
        if (bill != null)
        {
            Jump(bill);
        }
    }

    void Jump(Bill bill)
    {
        bill.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumperForce, ForceMode.Impulse);
        anim.Play("Jump");
        anim.Play("Static");
    }
    
}
