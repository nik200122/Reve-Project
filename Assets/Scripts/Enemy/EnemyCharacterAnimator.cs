using System;
using UnityEngine;

public class EnemyCharacterAnimator : MonoBehaviour
{
    private const string animIDSpeed = "Speed";
    private const string animIDGrounded = "Grounded";
    private const string animIDFreeFall = "FreeFall";
    private const string animIDMotionSpeed = "MotionSpeed";
    private const string animIDRoll = "Roll"; 

    [SerializeField] private GameObject hitVfx;

    private Animator animator;
    private Enemy enemy;

    [SerializeField] EnemyCharacterStatus status;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update(){
        CheckIsAttacking();
        if(status.GetCanMove()){
            UpdateStatus(status.IsGrounded(), status.GetAnimationBlend(), status.GetInputMagnitude(), 
                status.IsFalling(), status.IsRolling());
        }else{
            UpdateStatus(true, 0, 0, false, false);
        }
    }

    private void CheckIsAttacking(){
        if(status.IsAttacking()){
            if(!status.IsAttackInProgress()){
                PlayAttackAnimation();
            }
        }
    }

    private AttackData attackData;
    private int attackIndex = 0;
    private void PlayAttackAnimation(){
        status.SetIsAttackInProgress(true);
        if (attackIndex >= enemy.attackDataList.Count)
            attackIndex = 0;    
        
        attackData = enemy.GetAttackData(attackIndex);
        animator.runtimeAnimatorController = attackData.AnimatorOverrideController;
        attackIndex++;

        //poichè perform attack è un animation event, fare animator.Play equivale a dire "attacca"
        animator.Play("Attack", 0, 0);
    }

    void UpdateStatus(bool isGrounded, float animationBlend, float inputMagnitude, bool isFalling, bool isRolling){
            animator.SetBool(animIDGrounded, isGrounded);
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, 1f);
            animator.SetBool(animIDFreeFall, isFalling);
            animator.SetBool(animIDRoll, isRolling);
    }

    public void SetRunTimeAnimatorController(AnimatorOverrideController animatorOverrideController){
        animator.runtimeAnimatorController = animatorOverrideController;
    }

    public void SpawnHitVfx(Vector3 Pos_){
        Instantiate(hitVfx, Pos_, Quaternion.identity);
    }
}
