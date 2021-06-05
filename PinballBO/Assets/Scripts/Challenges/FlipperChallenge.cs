using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperChallenge : MonoBehaviour
{
    public static FlipperChallenge Instance;
    [SerializeField] private ParticleSystem yeah;
    public GameObject door;
    public Cinemachine.CinemachineVirtualCamera doorCamera;
    public GameObject[] lightPack;
    public Color[] colors;
    private Coroutine tunnel;

    public int score { get; private set; }
    public int bestScore { get; private set; }
    public int goal { get; private set; }
    public int scoreToReach;

    private float targetCount = 1;
    private bool playing = false;
    public bool cleared { get; private set; }

    private void Start()
    {
        Instance = this;
        cleared = false;
        yeah.gameObject.SetActive(false);
    }

    public void Begin()
    {
        BumpObject[] bumpers = GetComponentsInChildren<BumpObject>();
        foreach (BumpObject bump in bumpers)
            bump.SetChallenge(this);

        Targets[] targets = GetComponentsInChildren<Targets>();
        foreach (Targets target in targets)
            target.SetChallenge(this);

        Rail[] rails = GetComponentsInChildren<Rail>();
        foreach (Rail rail in rails)
            rail.SetChallenge(this);

        playing = true;

        score = 0;
        targetCount = 1;
        goal = scoreToReach;

        GameManager.Instance.SetCurrentChallenge(GameManager.Challenge.Flipper);
    }

    public void StopChallenge()
    {
        playing = false;
        UIManager.Instance.FlipperChallengeWin();
        GameManager.Instance.SetCurrentChallenge(GameManager.Challenge.Free);
    }

    public void ChangeScore(int amount, GameObject item)
    {
        if (!playing ||
            GameManager.Instance.GameState == GameManager.gameState.Win) return;

        // Increase Score
        int amountScore = (int)(amount * Multiplier());
        score += amountScore;
        if (amount > 0)
            UIManager.Instance.DisplayScore(amountScore, item);  // Feedback : Score qui bouge jusqu'à l'interface

        // Rajoute du temps au timer
        if (item.layer != 11)
            UIManager.Instance.timer.AddTime(.7f);
        else
            UIManager.Instance.timer.AddTime(Time.deltaTime);

        // Feedback Sonore



        if (score >= goal)
            Victory();
    }

    public void TargetTouch()
    {
        targetCount += .1f;
    }

    private float Multiplier()
    {
        return targetCount * UIManager.Instance.timer.multiplier;
    }

    private void Victory()
    {
        if (score > bestScore) bestScore = score;
        UIManager.Instance.FlipperChallengeWin();
        yeah.gameObject.SetActive(true);

        StartCoroutine(OpenDoor());

        // items do not earn points anymore
        BumpObject[] bumpers = GetComponentsInChildren<BumpObject>();
        foreach (BumpObject bump in bumpers)
        {
            bump.InvokeRepeating("Rainbow", 0, Time.deltaTime);
            bump.SetChallenge(null);
        }

        Targets[] targets = GetComponentsInChildren<Targets>();
        foreach (Targets target in targets)
            target.SetChallenge(null);

        Rail[] rails = GetComponentsInChildren<Rail>();
        foreach (Rail rail in rails)
            rail.SetChallenge(null);

        playing = false;
        StopCoroutine(tunnel);

        cleared = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bill>() != null)
            if (!playing)
                Begin();

        if (tunnel != null)
            StopCoroutine(tunnel);
        tunnel = StartCoroutine(Tunnel());
    }

    IEnumerator OpenDoor()
    {
        Vector3 targetPos = door.transform.position + Vector3.up * 2.5f;
        CameraManager.Instance.SetCameraActive(doorCamera.gameObject);
        yield return new WaitForSeconds(.5f);
        for (float d = 0; d < 2.5f; d += Time.deltaTime)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, targetPos, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2);
        CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
    }


    #region Tunnel
    IEnumerator Tunnel()
    {
        yield return Allumage();
        yield return Vortex();
        int offset = 0;
        while (true)
        {
            for (int i = 0; i < lightPack.Length; i++)
            {
                Color color = colors[(i + offset) % colors.Length];
                ArcLight(lightPack[i].GetComponentsInChildren<Light>(), color);
            }
            yield return new WaitForSeconds(.05f);
            offset++;
        }
    }


    IEnumerator Allumage()
    {
        foreach (GameObject go in lightPack)
            foreach (Light light in go.GetComponentsInChildren<Light>())
                light.color = Color.black;

        for (int i = 0; i < lightPack[i].GetComponentsInChildren<Light>().Length; i++)
        {
            foreach (Light light in lightPack[i].GetComponentsInChildren<Light>())
            {
                light.color = Color.cyan;
                yield return new WaitForSeconds(.006f);
            }
        }
    }

    IEnumerator Vortex()
    {
        int length = lightPack[0].GetComponentsInChildren<Light>().Length - 1;
        foreach (GameObject go in lightPack)
            ArcLight(go.GetComponentsInChildren<Light>(), Color.black);

        for (int i = 0; i < length; i += 2)
        {
            EdgeLight(i, Color.white);
            if (i > 0)
                EdgeLight(i - 1, Color.black);
            yield return new WaitForSeconds(.07f);
        }

        EdgeLight(length, Color.black);

        for (int i = length; i > 0; i -= 2)
        {
            EdgeLight(i, Color.white);
            yield return new WaitForSeconds(.07f);
        }
    }

    private void ArcLight(Light[] arc, Color color)
    {
        foreach (Light light in arc)
        {
            light.color = color;
        }
    }

    private void EdgeLight(int index, Color color)
    {
        foreach (GameObject go in lightPack)
        {
            go.GetComponentsInChildren<Light>()[index].color = color;
        }
    }
    #endregion
}
