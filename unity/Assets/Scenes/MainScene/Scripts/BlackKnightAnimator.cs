using UnityEngine;

public class BlackKnightAnimator : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        anim.SetBool("isAttack", true);
    }

    public void Standby()
    {
        anim.SetBool("isAttack", false);
    }
}
