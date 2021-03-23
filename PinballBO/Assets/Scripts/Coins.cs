using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.AddCoin();
            UIManager.Instance.AddCoin();
            //Mettre un son pour les pieces
            Destroy(this.gameObject);
        }
    }
}
