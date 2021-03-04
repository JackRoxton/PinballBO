using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContol : MonoBehaviour
{
    private float horitontalTemp, verticalTemp;
    private Vector3 transf;
    private void Start()
    {
        transf = this.GetComponent<Transform>().position;
    }
    void Update()
    {

        horitontalTemp = transf.x + Input.GetAxis("Horizontal");

        verticalTemp = transf.z + Input.GetAxis("Vertical");


    }
}
