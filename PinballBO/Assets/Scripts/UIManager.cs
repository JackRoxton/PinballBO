using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{



    //Buttons
    public void OnClickEnter(string button)
    {
        switch (button)
        {
            case "GoMainMenu":
                SceneManager.LoadScene(0);
                break;

            case "Restart":
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;

            case "NextLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;

            default:
                Debug.LogError("\"" + button + "\"" + " not found in switch statement");
                break;
        }
    }
}
