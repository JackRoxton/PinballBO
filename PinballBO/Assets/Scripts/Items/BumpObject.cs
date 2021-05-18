using System.Collections;
using UnityEngine;

public class BumpObject : MonoBehaviour
{
    protected FlipperChallenge challenge;

    [SerializeField] protected int force;
    [SerializeField] protected int factor;
    [SerializeField] protected int point;

    [Header("Materials")]
    [SerializeField] protected MeshRenderer[] meshRenderers;

    protected Animator animator;
    private AudioSource source;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    protected void BumpAway(Bill bill, string animatorString = "")  // Bump Bill in the direction from the bumper to Bill's position
    {
        Vector3 direction = bill.transform.position - transform.position;
        direction.y = 0;
        direction = Vector3.ProjectOnPlane(direction, transform.up);

        Bump(bill, direction, animatorString);
    }

    protected void Bump(Bill bill, Vector3 direction, string animatorString = "")
    {
        if (animatorString != "")
            animator.SetTrigger(animatorString);

        Rigidbody billBody = bill.GetComponent<Rigidbody>();
        billBody.velocity = Vector3.zero;
        float force = this.force + billBody.velocity.magnitude * factor;
        billBody.AddForce(direction * force);

        source.PlayOneShot(AudioManager.Instance.GetAudioCLip("Bump"));

        StartCoroutine(PostProcessBump());

        if (challenge != null)
            AddPoints();
    }

    protected virtual void AddPoints()
    {
        challenge.ChangeScore(point, this.gameObject);
        // ChangeNeonIntensity();
    }

    protected virtual void ChangeNeonsColor()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            var newMats = renderer.materials;
            foreach (Material mat in newMats)
                mat.color = Color.HSVToRGB(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f));
            renderer.materials = newMats;
        }
    }


    public void SetChallenge(FlipperChallenge challenge)
    {
        this.challenge = challenge;
    }


    protected virtual IEnumerator PostProcessBump()
    {
        yield return PostProcessingManager.Instance.BloomIntensity(20, 1);
        yield return PostProcessingManager.Instance.BloomIntensity(5, 1);
    }

}
