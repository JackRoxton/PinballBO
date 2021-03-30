using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{
    private bool lightState = true;

    public void SetLights(bool state)
    {
        this.lightState = state;
    }

    public bool SendLights()
    {
        return this.lightState;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.gameObject.GetComponent<Bill>();

        if(bill != null )
        {
            if(!this.lightState)
            {
                this.lightState = true;
            }
            
        }

    }
}
