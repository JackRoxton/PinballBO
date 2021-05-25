using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public ParticleSystem deathParticle;

    private void FixedUpdate()
    {
        transform.Rotate(0, 1f, 0, Space.Self);
        if (gameObject.transform.rotation.y >= 180f)
        {
            transform.Rotate(0, 0, 0, Space.Self);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.Instance.Play("Coin");
            GameManager.Instance.AddCoin();
            UIManager.Instance.AddCoin();

            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
