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

    public void BillInRail(Bill bill, bool inRail)
    {
        GameObject cam = inRail ? camera.gameObject : CameraManager.Instance.mainCam.gameObject;
        CameraManager.Instance.SetCameraActive(cam);
        bill.Freaze(inRail);
    }
}
