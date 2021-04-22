using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBill : MonoBehaviour
{
    public float ballSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xspeed = Input.GetAxis("Horizontal");
        float yspeed = Input.GetAxis("Vertical");

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddTorque(new Vector3(xspeed, 0, yspeed) * ballSpeed * Time.deltaTime);
    }
}
