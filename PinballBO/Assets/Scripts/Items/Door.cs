using System.Collections;
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
    private GameObject bill;

    //cocher use targets **ou** use spinners pour des portes classiques, uniquement bossDoor pour la porte du boss.
    //si bossDoor est coché, il faut aussi remplir la liste de spinners.

    bool open = false;

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

        if (testStates() && !open)
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
        bill = GameObject.Find("Bill");
        Rigidbody rb = bill.GetComponent<Rigidbody>();

        if (cinematic)
        {
            CameraManager.Instance.SetCameraActive(camera.gameObject);
            rb.isKinematic = true;
        }
        yield return new WaitForSeconds(2);
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, pos, 2f);
        open = true;
        yield return new WaitForSeconds(2);
        if (cinematic)
        {
            CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
            rb.isKinematic = false;
        }
    }
    IEnumerator OpenBoss()
    {
        bill = GameObject.Find("Bill");
        Rigidbody rb = bill.GetComponent<Rigidbody>();

        if (cinematic)
        {
            CameraManager.Instance.SetCameraActive(camera.gameObject);
            rb.isKinematic = true;
        }
        yield return new WaitForSeconds(2);
        //turn on the lights of a "lock"
        if (challengesDone == challengesCount)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, pos, 0.05f);
            open = true;
        }
        yield return new WaitForSeconds(2);

        if (cinematic)
        {
            CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
            rb.isKinematic = false;
        }
    }
}
