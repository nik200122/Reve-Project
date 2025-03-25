using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    private const string animIDMotionSpeed = "MotionSpeed";
    private const string animIDSpeed = "Speed";
    private const string animIDTalk = "Talk";
    private Animator animator;
    AiMovement aiMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        aiMovement = GetComponent<AiMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(animIDSpeed, aiMovement.GetSpeed());
    }

    public void SetTalk(){
        animator.SetTrigger(animIDTalk);
    }
}
