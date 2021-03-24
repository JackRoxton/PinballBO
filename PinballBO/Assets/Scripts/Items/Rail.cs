using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Rail : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] Bill bill;
    Rigidbody rb;
    private void Awake()
    {
        camera.LookAt = bill.transform;
        camera.Follow = bill.transform;

        rb = bill.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Bill boule = other.GetComponent<Bill>();
        if (boule != null)
        {
            CameraManager.Instance.SetCameraActive(camera.gameObject);
            bill.frozen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Bill boule = other.GetComponent<Bill>();
        if (boule != null)
        {
            CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
            bill.frozen = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Bill boule = other.GetComponent<Bill>();
        if (boule != null)
        {
            if (rb.velocity.magnitude < 15)
                rb.velocity = rb.velocity * 1.04f;
        }
    }
}
