using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Door : MonoBehaviour   // Quentin Lamy
{
    public FakeWin win;
    public Cinemachine.CinemachineVirtualCamera endCam;

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
    
    private int challengesCount = 3, challengesDone = 0;
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
        pos = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - 20, this.transform.localPosition.z);
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
            return true;
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
            return true;
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
        {
            CameraManager.Instance.SetCameraActive(camera.gameObject);
        }
        yield return new WaitForSeconds(2);
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, pos, 2f);
        open = true;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
        yield return new WaitForSeconds(2);
        if (cinematic)
        {
            CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
        }
    }
    IEnumerator OpenBoss()
    {
        if (cinematic)
        {
            CameraManager.Instance.SetCameraActive(camera.gameObject);
        }

        if (challengesDone == challengesCount)
        {
            win.ParticlesWin();
            //GameManager.Instance.GameState = GameManager.gameState.Win;
        }
        yield return new WaitForSeconds(2);
        //turn on the lights of a "lock"
        if (challengesDone == challengesCount)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, pos, 0.1f);
            open = true;
        }
        yield return new WaitForSeconds(2);

        if (cinematic)
        {
            CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bill>() != null)
            CameraManager.Instance.SetCameraActive(endCam.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Bill>() != null)
            CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
    }
}
