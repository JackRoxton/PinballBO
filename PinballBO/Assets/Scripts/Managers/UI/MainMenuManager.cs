using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject menuUI;

    //If Multiple Levels are in the game
    //[SerializeField] private GameObject levelManagerUI;

    public void OnClickEnter(string button)
    {
        switch (button)
        {
            case "StartGame":
                //levelManagerUI.SetActive(true);
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                GameManager.Instance.coins = 0;
                Bill.Instance.Reset();
                UIManager.Instance.coinsCount.gameObject.SetActive(true);
                CameraManager.Instance.SetCameraActive(CameraManager.Instance.mainCam.gameObject);
                gameObject.SetActive(false);
                UIManager.Instance.AddCoin();
                break;

            case "Options":
                menuUI.SetActive(false);
                optionUI.SetActive(true);
                break;

            case "Quit":
                Application.Quit();
                break;

            case "BackToMenu":
                optionUI.SetActive(false);
                menuUI.SetActive(true);
                break;

            default:
                Debug.LogError(button + " not found in switch statement");
                break;
        }
    }
}