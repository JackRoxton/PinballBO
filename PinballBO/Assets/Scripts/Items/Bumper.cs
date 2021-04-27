using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : BumpObject
{
    [SerializeField] private ParticleSystem bumpParticle;
    private Vector3 particlePos;

    // Classic Bumper
    private void OnTriggerEnter(Collider other)
    {
        Vector3 particlePos = new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
        Bill bill = other.GetComponent<Bill>();
        if (bill != null)
        {
            BumpAway(bill, "Bump");
            Instantiate(bumpParticle, particlePos, Quaternion.identity);
        }
    }
}
