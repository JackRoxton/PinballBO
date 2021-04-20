using UnityEngine;

public class BumpObject : MonoBehaviour
{
    [SerializeField] protected int force;
    [SerializeField] protected int factor;
    protected Animator animator;
    protected AudioSource source;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    protected void BumpAway(Bill bill, string animatorString = "")
    {
        Vector3 direction = bill.transform.position - transform.position;
        direction.y = 0;
        direction = Vector3.ProjectOnPlane(direction, transform.up);

        if (animatorString != "")
            animator.SetTrigger(animatorString);

        Rigidbody billBody = bill.GetComponent<Rigidbody>();
        billBody.velocity = Vector3.zero;
        float force = this.force + billBody.velocity.magnitude * factor;
        billBody.AddForce(direction * force);
    }

    protected void Bump(Bill bill, Vector3 direction, string animatorString = "")
    {
        if (animatorString != "")
            animator.SetTrigger(animatorString);

        Rigidbody billBody = bill.GetComponent<Rigidbody>();
        //billBody.velocity = Vector3.zero;
        float force = this.force + billBody.velocity.magnitude * factor;
        billBody.AddForce(direction * force);
    }

    // Change Neons Color
}
