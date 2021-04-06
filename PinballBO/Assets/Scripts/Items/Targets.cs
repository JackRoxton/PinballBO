using UnityEngine;

public class Targets : MonoBehaviour
{
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
            SetLights(!lightState);
            bill.GetComponent<Rigidbody>().velocity = -collision.GetContact(0).normal * 5;
        }

    }
}
