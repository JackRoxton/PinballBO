using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    float maxSpeed = 10f;
    [SerializeField, Range(0, 100)]
    float maxAcceleration = 10f;

    Rigidbody rb;
    Vector3 velocity , desiredVelocity;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        Vector3 adjustment;
        adjustment.x =
            playerInput.x * velocity.magnitude - Vector3.Dot(velocity, new Vector3(1, 0, 0));
        adjustment.z =
            playerInput.y * velocity.magnitude - Vector3.Dot(velocity, new Vector3(0, 0, 1));

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        rb.velocity = velocity;


    }

    void OldGetInput()
    {
        
        rb.AddForce(/*Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) **/ new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }

}
