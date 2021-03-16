using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            UIManager.Instance.AddCoin();
            GameManager.Instance.AddCoin();
            Destroy(this.gameObject);
        }
    }
}
