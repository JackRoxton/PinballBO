using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public static int count;
    public ParticleSystem deathParticle;
    public AudioClip clip;

    private void Awake()
    {
        count = FindObjectsOfType<Coins>().Length;
    }

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
            AudioManager.Instance.PlayClip(AudioManager.Instance.effectSource, clip);
            GameManager.Instance.AddCoin();
            UIManager.Instance.AddCoin();

            Instantiate(deathParticle, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}