using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Rail : MonoBehaviour
{
    [SerializeField] private int point;
    public float ratePoints;
    private float distanceOnRail = 0;

    private FlipperChallenge challenge;
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
        bill.EnterRail(inRail);
        distanceOnRail = 0;

        // PostProcessing - effet acceleration

    }

    public void SetChallenge(FlipperChallenge challenge)
    {
        this.challenge = challenge;
    }


    public void BillOnRail(float speed)
    {
        if (challenge == null) return;
        distanceOnRail += speed;
        if (distanceOnRail >= ratePoints)
        {
            distanceOnRail -= ratePoints;
            challenge.ChangeScore(null, point);
        }
    }

}
