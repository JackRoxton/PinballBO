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
    [SerializeField] protected Color color;

    protected Animator animator;
    private AudioSource source;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        if (color == Color.black)
            color = meshRenderers[0].material.color;
        else
            ChangeNeonsColor(color);
    }

    #region Bumping
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
    #endregion

    #region Challenge
    protected virtual void AddPoints()
    {
        challenge.ChangeScore(point, this.gameObject);
        // ChangeNeonIntensity();
    }

    public void SetChallenge(FlipperChallenge challenge)
    {
        this.challenge = challenge;
    }
    #endregion

    #region Neons
    protected virtual void ChangeNeonsColor(Color color)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            foreach (Material mat in renderer.materials)
                mat.color = color;
        }
    }
    #endregion

    // Feedbacks
    protected virtual IEnumerator PostProcessBump()
    {
        yield return PostProcessingManager.Instance.BloomIntensity(20, 1);
        yield return PostProcessingManager.Instance.BloomIntensity(5, 1);
    }

}
