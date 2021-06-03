using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostAnim : MonoBehaviour
{
    public float scroll = 0.5f;
    public List<GameObject> arrows = new List<GameObject>();
    int i = 0;
    bool wait = false;

    public Material neon;
    public Material none;

    private void Update()
    {
        if(!wait)
            StartCoroutine(Blinking());
    }

    private IEnumerator Blinking()
    {
        foreach (GameObject o in arrows)
        {
            if (o == arrows[i])
            {
                o.GetComponent<MeshRenderer>().material = neon;
            }
            else
            {
                o.GetComponent<MeshRenderer>().material = none;
            }
        }


        i++;
        if (i >= arrows.Count)
            i = 0;

        wait = true;
        yield return new WaitForSeconds(scroll);
        wait = false;
    }
}


