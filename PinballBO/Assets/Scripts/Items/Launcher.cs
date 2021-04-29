using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private int force;
    Animator animator;
    bool isLaunching = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator Launch(Bill bill)
    {
        isLaunching = true;
        animator.SetTrigger("Launch");
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(.2f);
        bill.GetComponent<Rigidbody>().velocity = transform.right * force;
        yield return new WaitForSeconds(.5f);
        isLaunching = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!isLaunching)
        {
            Bill bill = other.GetComponent<Bill>();
            if (bill != null)
            {
                StartCoroutine(Launch(bill));
            }
        }
    }
}
