using System.Collections;
using UnityEngine;

public class BumpObject : MonoBehaviour
{
    protected FlipperChallenge challenge;

    [SerializeField] protected int force;
    [SerializeField] protected int factor;
    [SerializeField] protected int point;

    [Header("Materials")]
    [SerializeField] protected MeshRenderer[] Neons;

    protected Animator animator;
    private AudioSource source;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
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


    // Feedbacks
    protected virtual IEnumerator PostProcessBump()
    {
        yield return PostProcessingManager.Instance.BloomIntensity(20, 1);
        yield return PostProcessingManager.Instance.BloomIntensity(5, 1);
    }

    #region Color
    void SetColor(Color color)
    {
        foreach (MeshRenderer renderer in Neons)
        {
            var newMats = renderer.materials;
            foreach (Material mat in newMats)
            {                
                mat.SetColor("_EmissionColor", color);
            }
            renderer.materials = newMats;
        }
    }

    void RandomColor()
    {

        foreach (MeshRenderer renderer in Neons)
        {
            var newMats = renderer.materials;
            foreach (Material mat in newMats)
            {
                Color newColor = new Color(
                    Random.Range(0.5f, 1),
                    Random.Range(0.5f, 1),
                    Random.Range(0.5f, 1));
                mat.SetColor("_EmissionColor", newColor);
            }
            renderer.materials = newMats;
        }
    }

    void SmoothRandomColor()
    {
        foreach (MeshRenderer renderer in Neons)
        {
            var newMats = renderer.materials;
            foreach (Material mat in newMats)
            {
                Color newColor = new Color();
                newColor.r = .5f + Mathf.Cos(Time.timeSinceLevelLoad * 1) * .5f;
                newColor.g = .5f + Mathf.Cos(Time.timeSinceLevelLoad * 2) * .5f;
                newColor.b = .5f + Mathf.Cos(Time.timeSinceLevelLoad * 3) * .5f;

                mat.SetColor("_EmissionColor", newColor);
            }
            renderer.materials = newMats;
        }
    }


    void Rainbow()
    {
        foreach (MeshRenderer renderer in Neons)
        {
            var newMats = renderer.materials;
            foreach (Material mat in newMats)
            {
                Color newColor = Color.HSVToRGB(.5f + Mathf.Cos(Time.timeSinceLevelLoad * .6f) * .5f, 1, 1);

                mat.SetColor("_EmissionColor", newColor);
            }
            renderer.materials = newMats;
        }
    }
    #endregion
}
