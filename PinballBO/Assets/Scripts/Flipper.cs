using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField]
    Animator Anim;

    bool IsMoving = false;

    [SerializeField]
    int force;
    float timer = 0;

    void Start()
    {
        timer += (Random.Range(4, 8));
        Anim = Anim.GetComponent<Animator>();
    }

    void Update()
    {
        RandomBehaviour();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject bill = collision.gameObject;
        if (bill.GetComponent<Bill>() != null && IsMoving)
        {
            bill.GetComponent<Rigidbody>().AddForce((bill.transform.position - this.transform.position) * force /** (bill.GetComponent<Rigidbody>().velocity.x) * (bill.GetComponent<Rigidbody>().velocity.z)*/);
        }
    }
    private void OnCollisionStay(Collision collision) //pour éviter les problêmes de collision au contact prolongé
    {
        GameObject bill = collision.gameObject;
        if (bill.GetComponent<Bill>() != null && IsMoving)
        {
            bill.GetComponent<Rigidbody>().AddForce((bill.transform.position - this.transform.position) * force);
        }
    }

    void RandomBehaviour() // pousse de manière random
    {
        if(timer <= 0)
        {
            Debug.Log("it is time!");
            timer += (Random.Range(3,7));
            Anim.Play("FlipperMove");
            IsMoving = true;
        }
        else
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).IsName("NotMoving"))
            {
                IsMoving = false;
            }
            timer -= Time.deltaTime;
        }
    }
}
