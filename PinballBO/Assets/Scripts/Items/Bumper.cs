using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : BumpObject
{

    // Classic Bumper
    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.GetComponent<Bill>();
        if (bill != null)
        {
            BumpAway(bill, "Bump");
        }
    }
}
