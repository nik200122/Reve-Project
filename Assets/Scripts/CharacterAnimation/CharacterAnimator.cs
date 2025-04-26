using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private const string animIDSpeed = "Speed";
    private const string animIDGrounded = "Grounded";
    private const string animIDJump = "Jump";
    private const string animIDFreeFall = "FreeFall";
    private const string animIDMotionSpeed = "MotionSpeed";
    private const string animIDRoll = "Roll"; 
    private const string animIDDeath = "IsDead";
    private Animator animator;

    [SerializeField] CharacterStatus status;

    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        CheckIsDead();   
        if(!isDead && status.GetCanMove()){
            UpdateStatus(status.IsGrounded(), status.GetAnimationBlend(), status.GetInputMagnitude(), 
                status.IsJumping(), status.IsFalling(), status.IsRolling());
        }
        else{
            UpdateStatus(true, 0, 0, false, false, false);
        }
    }

    void UpdateStatus(bool isGrounded, float animationBlend, float inputMagnitude, bool isJumping, 
        bool isFalling, bool isRolling){
            animator.SetBool(animIDGrounded, isGrounded);
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);
            animator.SetBool(animIDJump, isJumping);
            animator.SetBool(animIDFreeFall, isFalling);
            animator.SetBool(animIDRoll, isRolling);
    }

    private void CheckIsDead(){
        if(status.IsDead() && !isDead){
            Debug.Log("SCHIATTATO");
            isDead = true;
            animator.SetTrigger(animIDDeath);
        }
    }


}
