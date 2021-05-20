using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fronton : MonoBehaviour
{
    public List<GameObject> lights = new List<GameObject>();

    public Material neon;
    public Material none;

    private int indicator = 0;
    private bool wait = false;

    private void Start()
    {
        for(int i = 0; i < lights.Count; i++)
        {
            lights[i].GetComponent<MeshRenderer>().material = none;
        }
    }

    private void Update()
    {
        if (indicator >= 5)
            indicator = 0;

        if(!wait)
        StartCoroutine(Blinking());
    }

    private IEnumerator Blinking()
    {
        if(indicator != 0)
        {
            lights[indicator - 1].GetComponent<MeshRenderer>().material = none;
            lights[indicator + 4].GetComponent<MeshRenderer>().material = none;
        }
        else
        {
            lights[4].GetComponent<MeshRenderer>().material = none;
            lights[9].GetComponent<MeshRenderer>().material = none;
        }

        lights[indicator].GetComponent<MeshRenderer>().material = neon;
        lights[indicator+5].GetComponent<MeshRenderer>().material = neon;

        indicator++;
        wait = true;
        yield return new WaitForSeconds(1);
        wait = false;
    }
}
