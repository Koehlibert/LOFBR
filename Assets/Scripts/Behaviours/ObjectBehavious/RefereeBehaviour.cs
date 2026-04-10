using UnityEngine;

public class RefereeBehaviour : MonoBehaviour
{
    public void Init()
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("Offside");
        Destroy(this, 2f);
    }
}
