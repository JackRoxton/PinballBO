using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Rail : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bill>() != null)
            CameraManager.Instance.SetCameraActive(camera.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Bill>() != null)
            CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
    }
}
