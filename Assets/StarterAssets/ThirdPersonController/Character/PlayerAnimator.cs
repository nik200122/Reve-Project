using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string animIDSpeed = "Speed";
    private const string animIDGrounded = "Grounded";
    private const string animIDJump = "Jump";
    private const string animIDFreeFall = "FreeFall";
    private const string animIDMotionSpeed = "MotionSpeed"; 
    private Animator animator;

    [SerializeField] PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(animIDGrounded, player.IsGrounded());
        animator.SetFloat(animIDSpeed, player.GetAnimationBlend());
        animator.SetFloat(animIDMotionSpeed, player.GetInputMagnitude());
        animator.SetBool(animIDJump, player.IsJumping());
        animator.SetBool(animIDFreeFall, player.IsFalling());
    }

}
