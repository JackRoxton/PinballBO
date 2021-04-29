﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Door : MonoBehaviour
{
    [Header("Mode")]
    [SerializeField]
    private bool useTargets;
    [SerializeField]
    private bool useSpinners;
    [SerializeField]
    private bool bossDoor;

    [Header("Cinematics")]
    [SerializeField]
    private bool cinematic;
    [SerializeField] CinemachineVirtualCamera camera;

    [Header("Targets")]
    [SerializeField]
    private List<Targets> targets = new List<Targets>();

    [SerializeField]
    int offTargetsCount = 0;

    [Header("Spinners + Boss")]
    [SerializeField]
    private List<SpinnerChallenge> spinners = new List<SpinnerChallenge>();
    
    private int challengesCount = 2, challengesDone = 0;


    //cocher use targets **ou** use spinners pour des portes classiques, uniquement bossDoor pour la porte du boss.
    //si bossDoor est coché, il faut aussi remplir la liste de spinners.


    bool Flag = false;

    Vector3 pos;
    void Start()
    {
        if ((useTargets && useSpinners) || (bossDoor && (useTargets || useSpinners)))
        {
            throw new System.Exception("doorUseError");
        }
        pos = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - 5, this.transform.localPosition.z);
        for (int i = 0; i < offTargetsCount; i++)
        {
            targets[Random.Range(0, targets.Count)].SetLights(false);
        }
    }

    void Update()
    {
        if (testStates())
        {
            if (!bossDoor)
            StartCoroutine(Open());
            if (bossDoor)
            StartCoroutine(OpenBoss());
            
        }
    }

    bool testStates()
    {

        if (useTargets)
        {
            foreach (Targets n in targets)
            {
                if (!n.SendLights())
                {
                    return false;
                }
            }

            if (!Flag)
            {
                return true;
            }
        }
        else if (useSpinners)
        {
            foreach (SpinnerChallenge n in spinners)
            {
                if (!n.SendState())
                {
                    return false;
                }
            }

            if (!Flag)
            {
                return true;
            }
        }
        else if(bossDoor)
        {
            if (challengesDone == challengesCount)
                return true;
            foreach (SpinnerChallenge n in spinners)
            {
                if (n.SendState())
                {
                    challengesDone++;
                    spinners.Remove(n);
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator Open()
    {
            if (cinematic)
                CameraManager.Instance.SetCameraActive(camera.gameObject);
            yield return new WaitForSeconds(2);
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, pos, 2f);
            yield return new WaitForSeconds(2);
            Flag = true;
            if (cinematic)
                CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
    }
    IEnumerator OpenBoss()
    {
            if (cinematic)
                CameraManager.Instance.SetCameraActive(camera.gameObject);
            yield return new WaitForSeconds(2);
            //turn on the lights of a "lock"
            if (challengesDone == challengesCount)
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, pos, 0.05f);
            yield return new WaitForSeconds(2);
            if (cinematic)
                CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
    }
}