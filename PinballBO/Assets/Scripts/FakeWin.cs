using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Cinemachine;

public class FakeWin : MonoBehaviour    // Quentin Lejuez
{
    public List<GameObject> particles = new List<GameObject>();
    public CinemachineVirtualCamera cam;

    public UIManager uimanager;
    /*private void Update()
    {
        //Win
        if (Input.GetKey(KeyCode.W))
        {
            GameManager.Instance.GameState = GameManager.gameState.Win;
            uimanager.Win();
        }

        //Reset Best Time
        if (Input.GetKey(KeyCode.R)) 
        {
            PlayerPrefs.SetFloat("BestTimeLevel" + GameManager.Instance.currentLevel, 60);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        Bill bill = other.gameObject.GetComponent<Bill>();
        if (bill == null)
            return;

        CameraManager.Instance.SetCameraActive(cam.gameObject);
        GameManager.Instance.GameState = GameManager.gameState.Win;
        uimanager.Win();
        bill.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        foreach(GameObject n in particles)
        {
            n.SetActive(true);
            n.GetComponent<ParticleSystem>().Play();
        }
    }

    public void ParticlesWin()
    {
        foreach (GameObject n in particles)
        {
            n.SetActive(true);
            n.GetComponent<ParticleSystem>().Play();
        }
    }
}
