using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : BumpObject
{
    [SerializeField] float speed;
    [SerializeField] float angle;
    [SerializeField] bool random;



    bool IsMoving = false;
    BoxCollider[] triggers = new BoxCollider[2];
    SphereCollider sphereTrigger;

    private void Start()
    {
        if (random)
            StartCoroutine(Timer());

        #region Triggers

        // Triggers which push the ball
        triggers = GetComponentsInChildren<BoxCollider>(); 
        foreach (BoxCollider trigger in triggers) 
            trigger.gameObject.SetActive(false);


        // Trigger which Detect when the ball is near
        sphereTrigger = GetComponentInChildren<SphereCollider>(); 

        if (!random)
            sphereTrigger.enabled = true;
        else
            sphereTrigger.enabled = false;
        
        #endregion
    }

    IEnumerator Timer() // For random behaviour
    {
        yield return new WaitForSeconds(Random.Range(1, 7));
        yield return Shoot();
        StartCoroutine(Timer());
    }

    IEnumerator Shoot()
    {
        IsMoving = true;

        Quaternion target = Quaternion.Euler(0, angle, 0) * transform.rotation;
        Quaternion initial = transform.rotation;

        foreach (BoxCollider trigger in triggers)   // Enable triggers to push the ball
            trigger.gameObject.SetActive(true);

        source.Play();

        while (Quaternion.Angle(transform.rotation, target) > 3) // Rotate the flipper
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        IsMoving = false;

        foreach (BoxCollider trigger in triggers) // Disable triggers to avoid the ball to be pushed 
            trigger.gameObject.SetActive(false);

        while (Quaternion.Angle(transform.rotation, initial) > 3) // Rotate the flipper to his initial rotation
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, initial, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        transform.rotation = initial; 
        
        if (!random)
        {
            yield return new WaitForSeconds(.5f);
            sphereTrigger.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Bill bill = other.GetComponent<Bill>();
        
        if ( bill != null && !random && !IsMoving)
        {
            if (Mathf.Sign(Vector3.SignedAngle(-transform.forward,
                bill.transform.position - transform.position, Vector3.up)) != Mathf.Sign(angle))
                    return; // Faut pas taper dans ce cas là
            sphereTrigger.enabled = false;
            StartCoroutine(Shoot());
        }

        if (bill != null && IsMoving)
        {
            float angleShoot = angle > 0 ? angle + 20 : angle - 20;
            Vector3 direction = Quaternion.Euler(0, transform.eulerAngles.y + angleShoot, 0)     // Direction depends of the revolution of the flipper
                * (-Vector3.forward * force * speed);
            Bump(bill, direction);
        }
    }

}
