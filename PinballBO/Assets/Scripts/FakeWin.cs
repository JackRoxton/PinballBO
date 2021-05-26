using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class FakeWin : MonoBehaviour
{
    public UIManager uimanager;
    private void Update()
    {
        /*//Win
        if (Input.GetKey(KeyCode.W))
        {
            GameManager.Instance.GameState = GameManager.gameState.Win;
            uimanager.Win();
        }

        //Reset Best Time
        if (Input.GetKey(KeyCode.R)) 
        {
            PlayerPrefs.SetFloat("BestTimeLevel" + GameManager.Instance.currentLevel, 60);
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.GameState = GameManager.gameState.Win;
        uimanager.Win();
    }
}
