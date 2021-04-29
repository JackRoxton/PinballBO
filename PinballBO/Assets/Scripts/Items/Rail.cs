using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Rail : MonoBehaviour
{
    [SerializeField] private int point;
    [SerializeField] private float ratePoints;

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

        if (challenge == null) return;
        if (inRail)
            StartCoroutine(BillOnRail());
        else
        {
            StopAllCoroutines(); // Faudrait ne pas arrêter toutes les coroutine plus tard
        }

        // PostProcessing - effet acceleration

    }

    public void SetChallenge(FlipperChallenge challenge)
    {
        this.challenge = challenge;
    }


    IEnumerator BillOnRail()
    {
        while (true)
        {
            yield return new WaitForSeconds(ratePoints);
            challenge.ChangeScore(point);
        }
    }
}
