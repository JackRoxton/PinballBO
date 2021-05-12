using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bill : MonoBehaviour
{
    private Action currentState;

    [Header("Speed")]
    [Range(1, 100)] public float acceleration;
    [Range(1, 100)] public float speed;
    [Range(0, .5f)] public float breakForce;
    [Header("Controls")]
    [Range(0, 2)] public float lossSpeedOnSlopes;
    [Range(0, 1)] public float highSpeedControl = .15f;
    [Header("On Rails")]
    [Range(1, 1.25f)] public float onRailAcceleration;
    [Range(1, 20)] public float onRailMaxSpeed;

    [SerializeField] private ParticleSystem brakeParticles;
    private Vector3 brakeParticlePos;
    int particleFlag = 0;

    Rigidbody rb;
    CinemachineVirtualCamera camera;

    private float currentRotation = 0;
    float tour;
    Vector3 slopeNormal = Vector3.up;

    private bool chargeState = false;
    [Space]
    [Range(1,400)]public float chargeStrength;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tour = Mathf.PI * GetComponent<SphereCollider>().radius;

        camera = GetComponentInChildren<CinemachineVirtualCamera>();
    }
    void Start()
    {
        currentState = FreeMoving;
    }

    
    void FixedUpdate()
    {
        currentState();

        if (rb.velocity.magnitude > .01f) // Rotation
        {
            // Angle to look at
            float targetAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;
            // Rotate the character
            float distance = rb.velocity.magnitude * Time.deltaTime;
            currentRotation += (distance * 90) / tour;
            transform.rotation = Quaternion.Euler(currentRotation, targetAngle, 0);
        }
    }

    void FreeMoving()
    {

        if (rb.velocity.magnitude < speed) // Déplacements
        {
            Vector3 direction = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // Direction Inputs

            float slopeAngle = Vector3.Angle(direction, slopeNormal) * Mathf.Deg2Rad; // If slope too steep, avoid move
            direction = Vector3.ProjectOnPlane(direction, slopeNormal); // Movements on slopes
            direction = Vector3.ClampMagnitude(direction, 1);

            float sinAngle = Mathf.Abs(Mathf.Cos(slopeAngle));
            float acceleration = this.acceleration * (1 - (sinAngle * lossSpeedOnSlopes)); // Reduce acceleration on slopes

            rb.velocity += direction * acceleration * Time.deltaTime;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);

            Debug.DrawRay(transform.position, direction.normalized * 2, Color.red);
        }
        else // Dévier trajectoire à vitesse élevée sans accélerer davantage
        {
            float trajectoire = Input.GetAxis("Horizontal");
            rb.velocity = Quaternion.Euler(0, trajectoire * highSpeedControl, 0) * rb.velocity;
        }


        if (Input.GetButton("Break")) // Break
            Break();

        if(Input.GetKeyDown(KeyCode.F) && chargeState == false) //Charge (Input.GetButton("Charge"))
            StartCoroutine(Charge());

        Debug.DrawRay(transform.position, slopeNormal * 2, Color.blue);
    }

    void OnRail() // When using a rail
    {
        if (rb.velocity.magnitude > .1f)
        {
            rb.velocity *= onRailAcceleration;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, onRailMaxSpeed);
        }
        else
        {
            FreeMoving();
        }
    }


    public void EnterRail(bool onRail)
    {
        if (onRail)
        {
            currentState = OnRail;
        }
        else
            currentState = FreeMoving;
    }

    private void Break() // Frein in French
    {
        rb.velocity /= (breakForce + 1);

        if (particleFlag == 0)
        {
            Vector3 brakeParticlePos = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z);
            Instantiate(brakeParticles, brakeParticlePos, Quaternion.identity);
            particleFlag = 4;
        }
        else
            particleFlag--;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.GetContact(0).normal;
        if (normal.y > .7f)
            slopeNormal = normal;
    }

    private void OnTriggerEnter(Collider other)
    {
        Rail rail = other.GetComponent<Rail>();
        if (rail != null)
        {
            rail.BillInRail(this, true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rail rail = other.GetComponent<Rail>();
        if (rail != null)
            rail.BillOnRail(rb.velocity.magnitude);
    } 

    private void OnTriggerExit(Collider other)
    {
        Rail rail = other.GetComponent<Rail>();
        if (rail != null)
        {
            rail.BillInRail(this, false);
        }
    }

    

    IEnumerator Charge() //Bill charges, losing his current velocity and than releases his inner strength to go all out on speed. Can't be used motionless.
    {
        if (this.rb.velocity != Vector3.zero)
        {
            this.rb.velocity = Vector3.zero;
            chargeState = true;
            for (int i = 0; i < 120; i++)
            {
                currentRotation += 1000 * Time.deltaTime;
                yield return new WaitForEndOfFrame(); //Bill is charging the attack, spinning.
            }
            Vector3 direction = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * Vector3.forward;
            this.rb.velocity += direction * acceleration * Time.deltaTime * chargeStrength; //Bill releases his attack. (ability to damage the boss is to be implemented)
            yield return new WaitForSeconds(2); //Bill is on a slight cooldown from exhaustion.
            chargeState = false;
        }
    }
}


#region Old Script
//[SerializeField, Range(0, 100)]
//float maxacceleration = 40f;
//[SerializeField, Range(0, 100)]
//float maxAcceleration = 8f;

//float ballRadius = 0.5f;
//float minGroundDotProduct;
//float maxGroundAngle = 25f;

//private bool charged = false;
//public bool Charged { get => charged; }

//public bool frozen = false; // pour les rails ?

//[SerializeField]
//Transform ball = default;

//Rigidbody rb;
//Vector3 velocity, desiredVelocity, contactNormal, lastContactNormal;

//void Awake()
//{
//    rb = this.GetComponent<Rigidbody>();
//    OnValidate();
//}

//void Update()
//{
//    Vector2 playerInput;
//    playerInput.x = Input.GetAxis("Horizontal");
//    playerInput.y = Input.GetAxis("Vertical");
//    playerInput = Vector2.ClampMagnitude(playerInput, 1f);

//    Vector3 adjustment;
//    adjustment.x =
//        playerInput.x * velocity.magnitude - Vector3.Dot(velocity, new Vector3(1, 0, 0));
//    adjustment.z =
//        playerInput.y * velocity.magnitude - Vector3.Dot(velocity, new Vector3(0, 0, 1));

//    desiredVelocity = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(playerInput.x, 0f, playerInput.y) * maxacceleration;

//    UpdateBall();


//}

//private void FixedUpdate()
//{
//    if (frozen) return; // Pas de contrôle lors du cheminement d'un rail

//    velocity = rb.velocity;
//    float maxaccelerationChange = maxAcceleration * Time.deltaTime;
//    if (Input.GetKeyDown(KeyCode.F) && charged == false)
//    {
//        StartCoroutine(ChargeAttack());
//    }

//    if (charged == true)
//    {
//        maxaccelerationChange /= 2f;
//        rb.velocity /= 2f;
//        desiredVelocity /= 4f;
//    }

//    velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxaccelerationChange);
//    velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxaccelerationChange);

//    if (!frozen)
//    {
//        rb.velocity = velocity;
//    }


//    ClearState();

//}

//void UpdateBall()
//{
//    Vector3 movement = rb.velocity * Time.deltaTime;
//    float distance = movement.magnitude;
//    float angle = distance * (180 / Mathf.PI) / ballRadius;
//    Vector3 rotationAxis = Vector3.Cross(lastContactNormal, movement).normalized;
//    ball.localRotation = Quaternion.Euler(rotationAxis * angle) * ball.localRotation;
//}

//void ClearState()
//{
//    lastContactNormal = contactNormal;
//    contactNormal = Vector3.zero;
//}

//void EvaluateCollision(Collision collision)
//{
//    for (int i = 0; i < collision.contactCount; i++)
//    {
//        Vector3 normal = collision.GetContact(i).normal;
//        if (normal.y >= minGroundDotProduct)
//        {
//            contactNormal += normal;
//        }
//    }
//}

//private void OnValidate()
//{
//    minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
//}

//public void Freeze()
//{
//    if (!frozen)
//    {
//        frozen = true;
//    }
//    else
//    {
//        frozen = false;
//    }
//}

///*void OldGetInput()
//{

//    rb.AddForce(Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
//}*/

//void OnCollisionEnter(Collision collision)
//{
//    EvaluateCollision(collision);
//}

//void OnCollisionStay(Collision collision)
//{
//    EvaluateCollision(collision);
//}

//IEnumerator ChargeAttack()
//{
//    Debug.Log("Charging my attack");
//    rb.velocity = Vector3.zero;
//    charged = true;
//    yield return new WaitForSeconds(1);
//    Debug.Log("Let's go !");
//    rb.velocity += Vector3.forward * 25;
//    yield return new WaitForSeconds(2);
//    Debug.Log("Charge ready !");
//    charged = false;
//}
#endregion
