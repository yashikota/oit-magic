using UnityEngine;

public class BlackKnightAnimator : MonoBehaviour
{
    private static Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public static void Attack()
    {
        anim.SetBool("isAttack", true);
    }

    public static void Standby()
    {
        anim.SetBool("isAttack", false);
    }
}
