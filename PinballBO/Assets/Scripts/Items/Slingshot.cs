﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : BumpObject
{
    private void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.gameObject.GetComponent<Bill>();
        if (bill != null)
        {
            Vector3 direction = -collision.GetContact(0).normal;
            Bump(bill, direction, collision.GetContact(0).thisCollider.name);
        }
    }

}
