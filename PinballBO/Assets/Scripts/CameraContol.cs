using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContol : MonoBehaviour
{
    [SerializeField]
    Transform player;

    Vector3 CameraGap = new Vector3(0, 1, -6);

    void Update()
    {
        Move();
    }

    void Move()
    {
        this.transform.position = player.position + CameraGap;
    }
}
