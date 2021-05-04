using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : BumpObject
{
    public float angle;

    private void Update()
    {
        Debug.DrawRay(transform.position, Quaternion.Euler(0, angle, 0) * transform.forward * 40, Color.green);
    }

    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.GetComponent<Bill>();
        if (bill != null)
        {
            if (transform.localScale.x > 0)
                NormalShoot(bill);
            else
                InverseShoot(bill);
        }
    }

    private void NormalShoot(Bill bill) // if scale = 1
    {
        float angle = Vector3.SignedAngle(transform.forward, bill.transform.position - transform.position, transform.up) * transform.localScale.x;

        //Shoot Right
        if (angle > -38 && angle < 33)
            Bump(bill, Quaternion.Euler(0, 15, 0) * transform.forward, "ShootRight");

        // Shoot Left
        else if (angle > 33 && angle <= 163)
            Bump(bill, Quaternion.Euler(0, 90, 0) * transform.forward, "ShootLeft");

        // Shoot Forward
        else
            Bump(bill, Quaternion.Euler(0, -121, 0) * transform.forward, "ShootForward");
    }

    private void InverseShoot(Bill bill) // if scale = -1
    {
        float angle = -Vector3.SignedAngle(transform.forward, bill.transform.position - transform.position, transform.up) * transform.localScale.x;
        Debug.Log(angle);
        //Shoot Right
        if (angle > -52 && angle <= 46)
            Bump(bill, Quaternion.Euler(0, -15, 0) * transform.forward, "ShootRight");

        // Shoot Left
        else if (angle < -52 && angle >= -167)
            Bump(bill, Quaternion.Euler(0, -90, 0) * transform.forward, "ShootLeft");

        // Shoot Forward
        else
            Bump(bill, Quaternion.Euler(0, 121, 0) * transform.forward, "ShootForward");

    }
}
