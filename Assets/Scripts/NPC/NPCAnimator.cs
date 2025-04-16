using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    private const string animIDSpeed = "Speed";
    private const string animIDTalk = "Talk";
    private const string animIDGreet = "Greet";
    private Animator animator;
    NPCStatus nPCStatus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        nPCStatus = GetComponent<NPCStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(animIDSpeed, nPCStatus.GetSpeed());
    }

    public void TriggerTalk(AnimatorOverrideController animatorOverrideController = null){
        if(animatorOverrideController != null){
            animator.runtimeAnimatorController = animatorOverrideController;
            animator.SetTrigger(animIDTalk);
        }
    }

    internal void TriggerGreet(AnimatorOverrideController animatorOverrideController = null)
    {
        if(animatorOverrideController != null){
            animator.runtimeAnimatorController = animatorOverrideController;
            animator.SetTrigger(animIDGreet);
        }
        
    }
}
