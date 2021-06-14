using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [HideInInspector] public GameObject currentCam;
    public CinemachineFreeLook mainCam;

    private void Start()
    {
        Instance = this;
        currentCam = mainCam.gameObject;
    }

    public void SetCameraActive(GameObject camera)
    {
        CameraType(currentCam).Priority = 0;
        CameraType(camera).Priority = 1000;
        currentCam = camera;
    }

    private ICinemachineCamera CameraType(GameObject camera)
    {
        CinemachineVirtualCamera CM = camera.GetComponent<CinemachineVirtualCamera>();
        if (CM != null)
        {
            return CM;
        }
        else
        {
            return camera.GetComponent<CinemachineFreeLook>();
        }
    }
}
