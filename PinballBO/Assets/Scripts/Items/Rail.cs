using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Rail : MonoBehaviour
{
    CinemachineVirtualCamera camera;

    private void Awake()
    {
        camera = GetComponentInChildren<CinemachineVirtualCamera>();

        Bill bill = FindObjectOfType<Bill>();
        camera.LookAt = bill.transform;
        camera.Follow = bill.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bill boule = other.GetComponent<Bill>();
        if (boule != null)
        {
            CameraManager.Instance.SetCameraActive(camera.gameObject);
            boule.Freaze(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Bill boule = other.GetComponent<Bill>();
        if (boule != null)
        {
            CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
            boule.Freaze(false);
        }
    }
}
