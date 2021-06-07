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
    [Range(.001f, 1)] public float breakForce;
    public float chargeForce = 1;
    [Header("Controls")]
    [Range(0, 2)] public float lossSpeedOnSlopes;
    [Range(0, 1)] public float highSpeedControl = .15f;
    [Header("On Rails")]
    [Range(1, 1.25f)] public float onRailAcceleration;
    [Range(1, 20)] public float onRailMaxSpeed;
    [Space]
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip land;

    [SerializeField] private ParticleSystem brakeParticles;
    private Vector3 brakeParticlePos;
    float particleFlag = 0;
    
    private AudioSource source;
    Rigidbody rb;
    CinemachineVirtualCamera camera;

    private float currentRotation = 0;
    private float chargeRotation = 0;
    private float chargeAcceleration = 0;
    float tour;
    Vector3 slopeNormal = Vector3.up;
    bool canBreak; // { get { return ParkourChallenge.cleared; } }
    bool canCharge { get { return ChronoChallenge.cleared; } }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        tour = Mathf.PI * GetComponent<SphereCollider>().radius;

        camera = GetComponentInChildren<CinemachineVirtualCamera>();
    }
    void Start()
    {
        AudioManager.Instance.effectSources.Add(source);

        currentState = FreeMoving;
    }


    void FixedUpdate()
    {
        currentState();
    }


    void FreeMoving()
    {
        // Déplacements
        if (rb.velocity.magnitude < speed)
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

        RotationOverDistance();


        if (Input.GetButton("Break")) // Break
            Break();

        if (Input.GetButton("Charge")) // Charge
        {
            if (canCharge)
            {
                currentState = ChargingState;
                chargeRotation = currentRotation;
                chargeAcceleration = 0;
            }
        }

        Debug.DrawRay(transform.position, slopeNormal * 2, Color.blue);
    }

    void ChargingState()
    {

        chargeAcceleration = chargeAcceleration < 50 ? chargeAcceleration + 1 : 50;
        chargeRotation += chargeAcceleration;
        transform.rotation = Quaternion.Euler(chargeRotation, Camera.main.transform.eulerAngles.y, 0);

        if (Input.GetKey(KeyCode.Space))
            Break();

        if (!Input.GetKey(KeyCode.F))
        {
            Vector3 direction = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * Vector3.forward;
            direction = Vector3.ProjectOnPlane(direction, slopeNormal); // Movements on slopes
            direction = Vector3.ClampMagnitude(direction, 1);
            rb.velocity = direction * chargeAcceleration * chargeForce;

            currentState = FreeMoving;
            return;
        }
    }

    private void RotationOverDistance()
    {
        if (rb.velocity.magnitude > .1f)
        {
            // Angle to look at
            float targetAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;
            // Rotate the character
            float distance = rb.velocity.magnitude * Time.deltaTime;
            currentRotation += (distance * 90) / tour;
            transform.rotation = Quaternion.Euler(currentRotation, targetAngle, 0);
            // Rolling Sound
            source.volume = rb.velocity.magnitude / 10;
            source.pitch = (rb.velocity.magnitude / 10) * Time.deltaTime;
        }
    }

    public void Bumped()
    {
        source.PlayOneShot(hit);
    }

    #region Rail
    void OnRail() // When using a rail
    {
        if (rb.velocity.magnitude > .1f)
        {
            rb.velocity *= onRailAcceleration;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, onRailMaxSpeed);

            RotationOverDistance();
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
    #endregion


    private void Break() // Frein in French
    {
        Vector3 breakVelocity = rb.velocity + (-rb.velocity * breakForce);
        rb.velocity = breakVelocity.magnitude > .4f ? breakVelocity : Vector3.zero;

        if (particleFlag <= 0)
        {
            Vector3 brakeParticlePos = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z);
            Instantiate(brakeParticles, brakeParticlePos, Quaternion.identity);
            particleFlag = 4;
        }
        else
            particleFlag -= rb.velocity.magnitude * .5f;
    }



    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.GetContact(0).normal;
        if (normal.y > .7f)
            slopeNormal = normal;
        else
            source.PlayOneShot(hit);
        if (collision.collider.material.name == "Floor (Instance)")
            if (!source.isPlaying)
            {
                source.PlayOneShot(land);
                source.Play();
            }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.material.name == "Floor (Instance)")
            source.Stop();
    }
    
    public void IncreasePerformance()
    {

    }
}