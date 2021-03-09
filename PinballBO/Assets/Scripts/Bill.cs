using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill : MonoBehaviour
{
    /*déplacements avec inertie
    -> formule de baisse de vélocité selon l'input du joueur ?*/


        //angulardrag et angularvelocity sur le rigidbody pour contrôler la rotation de la bille
    Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        
        rb.AddForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
         

        /*if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(new Vector3(1, 0, 0));
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(new Vector3(-1, 0, 0));
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            rb.AddForce(new Vector3(0, 0, 1));
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rb.AddForce(new Vector3(0, 0, -1));
        }
        */
        
    }

}
