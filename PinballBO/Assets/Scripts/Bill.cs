using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    float maxSpeed = 40f;
    [SerializeField, Range(0, 100)]
    float maxAcceleration = 8f;

    float ballRadius = 0.5f;
    float minGroundDotProduct;
    float maxGroundAngle = 25f;

    private bool charged = false;
    public bool Charged { get => charged; }

    public bool frozen = false; // pour les rails ?

    [SerializeField]
    Transform ball = default;

    Rigidbody rb;
    Vector3 velocity, desiredVelocity, contactNormal, lastContactNormal;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        OnValidate();
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

        UpdateBall();

        
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F) && charged == false)
        {
            StartCoroutine(ChargeAttack());
        }

        if (charged == true)
        {
            maxSpeedChange /= 2f;
            rb.velocity /= 2f;
            desiredVelocity /= 4f;
        }

            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        if(!frozen)
        {
            rb.velocity = velocity;
        }
        

        ClearState();

    }

    void UpdateBall()
    {
        Vector3 movement = rb.velocity * Time.deltaTime;
        float distance = movement.magnitude;
        float angle = distance * (180 / Mathf.PI) / ballRadius;
        Vector3 rotationAxis = Vector3.Cross(lastContactNormal, movement).normalized;
        ball.localRotation = Quaternion.Euler(rotationAxis * angle) * ball.localRotation;
    }

    void ClearState()
    {
        lastContactNormal = contactNormal;
        contactNormal = Vector3.zero;
    }

    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minGroundDotProduct)
            {
                contactNormal += normal;
            }
        }
    }

    private void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    public void Freeze()
    {
        if(!frozen)
        {
            frozen = true;
        }
        else
        {
            frozen = false;
        }
    }

    /*void OldGetInput()
    {
        
        rb.AddForce(Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }*/

    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    IEnumerator ChargeAttack()
    {
        Debug.Log("Charging my attack");
        rb.velocity = Vector3.zero;
        charged = true;
        yield return new WaitForSeconds(1);
        Debug.Log("Let's go !");
        rb.velocity += Vector3.forward * 25;
        yield return new WaitForSeconds(2);
        Debug.Log("Charge ready !");
        charged = false;
    }
}
