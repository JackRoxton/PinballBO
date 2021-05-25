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
    [SerializeField] protected Color[] Colors;

    protected Animator animator;
    private AudioSource source;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        for (int i = 0; i < Neons.Length; i++)
        {
            SetColor(Neons[i].material, Colors[i]);
        }
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

        //Feedbacks
        StartCoroutine(PostProcessBump());
        StartCoroutine(BumpBlinking());
        
        source.PlayOneShot(AudioManager.Instance.GetAudioCLip("Bump"));

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

    protected virtual IEnumerator BumpBlinking()
    {
        Color[] initials = new Color[Neons.Length];
        for (int i = 0; i < Neons.Length; i++)
        {
            initials[i] = Neons[i].material.GetColor("_EmissionColor");
        }


        RandomColor();
        yield return new WaitForSeconds(.3f);
        for (int i = 0; i < initials.Length; i++)
            SetColor(Neons[i].material, initials[i]);
    }

    #region Color
    void SetColor(Material mat, Color color)
    {
        mat.SetColor("_EmissionColor", color);
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
