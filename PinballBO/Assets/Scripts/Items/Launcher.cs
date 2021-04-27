using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private int force;
    Animator animator;
    bool isLaunching = false;
    public BoxCollider collider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator Launch()
    {
        isLaunching = true;
        animator.SetTrigger("Launch");
        yield return new WaitForSeconds(1);
        isLaunching = false;
        collider.enabled = true;
        yield return new WaitForSeconds(.5f);
        collider.enabled = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.collider.GetComponent<Bill>();
        if (bill != null)
        {
            if (!isLaunching)
                StartCoroutine(Launch());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLaunching)
            if (other.GetComponent<Bill>() != null)
            {
                other.GetComponent<Rigidbody>().velocity = transform.right * force;
            }
    }
}
