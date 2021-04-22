﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : BumpObject
{
    public float y;
    private void Update()
    {
        Debug.DrawRay(transform.position, Quaternion.Euler(0, y, 0) * transform.forward * 40, Color.green);
    }

    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.GetComponent<Bill>();
        if (bill != null)
        {
            float angle = Vector3.SignedAngle(transform.forward, bill.transform.position - transform.position, transform.up);

            //Shoot Right
            if (angle > -38 && angle < 33)
                Bump(bill, Quaternion.Euler(0, 13, 0) * transform.forward, "ShootRight");

            // Shoot Left
            else if (angle > 33 && angle <= 163)
                Bump(bill, Quaternion.Euler(0, 90, 0) * transform.forward, "ShootLeft");

            // Shoot Forward
            else
                Bump(bill, Quaternion.Euler(0, -121, 0) * transform.forward, "ShootForward");
        }
    }
}