using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Porte : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camera;

    [SerializeField]
    List<Targets> targets = new List<Targets>();

    [SerializeField]
    int offTargetsCount = 0;

    Vector3 pos;
    void Start()
    {
        pos = new Vector3(this.transform.localPosition.x - 2, this.transform.localPosition.y, this.transform.localPosition.z);
        for (int i = 0; i < offTargetsCount; i++)
        {
            targets[Random.Range(0, targets.Count)].SetLights(false);
        }
    }

    void Update()
    {
        if (testLights())
        {
            StartCoroutine(Open());
        }
    }

    bool testLights()
    {
        foreach (Targets n in targets)
        {
            if(!n.SendLights())
            {
                return false;
            }
        }
        return true;

    }

    IEnumerator Open()
    {
        CameraManager.Instance.SetCameraActive(camera.gameObject);
        yield return new WaitForSeconds(2);
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, pos, 0.05f);
        yield return new WaitForSeconds(2);
        CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
    }
}
