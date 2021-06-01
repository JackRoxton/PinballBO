using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostAnim : MonoBehaviour
{
    public float scroll = 1f;

    private void Update()
    {
        float offset = Time.time * scroll;
        this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset, offset);
    }
}


