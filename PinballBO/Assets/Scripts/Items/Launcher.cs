using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private int force;
    Animator animator;
    bool isLaunching = false;
    [SerializeField] private CinemachineVirtualCamera cameraBeforeShoot;
    [SerializeField] private CinemachineVirtualCamera cameraAfterShoot;
    IEnumerator coroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cameraBeforeShoot.LookAt = GameObject.Find("Bill").transform;
    }

    IEnumerator Launch(Bill bill)
    {
        isLaunching = true;
        CameraManager.Instance.SetCameraActive(cameraBeforeShoot.gameObject);
        yield return new WaitForSeconds(2);
        animator.SetTrigger("Launch");
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(.2f);
        if (cameraAfterShoot != null)
            CameraManager.Instance.SetCameraActive(cameraAfterShoot.gameObject);
        else
            CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
        bill.GetComponent<Rigidbody>().velocity = transform.right * force;
        yield return new WaitForSeconds(.5f);
        isLaunching = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!isLaunching)
        {
            Bill bill = other.GetComponent<Bill>();
            if (bill != null)
            {
                Debug.Log("StartCoroutine");
                coroutine = Launch(bill);
                StartCoroutine(coroutine);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        {
            Bill bill = other.GetComponent<Bill>();
            if (bill != null)
            {
                Debug.Log("StopCoroutine");
                StopCoroutine(coroutine);
                coroutine = null;
                isLaunching = false;
                CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
            }
        }        
    }

    
}
