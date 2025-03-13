using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private const string animIDSpeed = "Speed";
    private const string animIDGrounded = "Grounded";
    private const string animIDJump = "Jump";
    private const string animIDFreeFall = "FreeFall";
    private const string animIDMotionSpeed = "MotionSpeed"; 
    private Animator animator;

    [SerializeField] CharacterStatus status;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(animIDGrounded, status.IsGrounded());
        animator.SetFloat(animIDSpeed, status.GetAnimationBlend());
        animator.SetFloat(animIDMotionSpeed, status.GetInputMagnitude());
        animator.SetBool(animIDJump, status.IsJumping());
        animator.SetBool(animIDFreeFall, status.IsFalling());
    }

}
