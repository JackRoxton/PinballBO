using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContol : MonoBehaviour
{
    [SerializeField]
    Transform player;

    Vector3 CameraGap = new Vector3(0, 1, -6);

    /*float mouseSensitivity = 75f;
    Vector3 angle;*/

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        Move();
        //Advanced();
    }

    void Move()
    {
        this.transform.position = player.position + CameraGap;
    }

    /*void Advanced() //un WIP qui marche pas encore d'une caméra qui tourne autour de la bille si le besoin est
    {
        angle.x += -Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        angle.y += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        Quaternion lookRotation = Quaternion.Euler(angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.2f);
    }*/
}
