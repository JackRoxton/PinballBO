using UnityEngine;

public class Targets : MonoBehaviour
{
    private FlipperChallenge challenge;
    public int force;
    [SerializeField] Material lightOn;
    Material initial;
    [SerializeField] MeshRenderer neon;
    [SerializeField] private bool lightState = false;
    

    private void Awake()
    {
        initial = neon.materials[1];    
    }

    public void SetLights(bool state)
    {
        lightState = state;
        var matArray = neon.materials;
        matArray[1] = state ? lightOn : initial;

        neon.materials = matArray;
    }

    public bool SendLights()
    {
        return lightState;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bill bill = collision.collider.GetComponent<Bill>();
        if (bill != null)
        {
            if (!lightState)
                SetLights(true);

            Rigidbody billBody = bill.GetComponent<Rigidbody>();
            billBody.velocity = Vector3.zero;
            billBody.AddForce(-collision.GetContact(0).normal * force);

            if (challenge != null)
                IncreaseMultiplicater();
        }

    }

    void IncreaseMultiplicater()
    {
        challenge.ChangeMultiplier(.1f);
    }

    public void SetChallenge(FlipperChallenge challenge)
    {
        this.challenge = challenge;
    }
}
