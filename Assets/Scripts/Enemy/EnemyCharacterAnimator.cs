using System;
using UnityEngine;

public class EnemyCharacterAnimator : MonoBehaviour
{
    private const string animIDSpeed = "Speed";
    private const string animIDGrounded = "Grounded";
    private const string animIDFreeFall = "FreeFall";
    private const string animIDMotionSpeed = "MotionSpeed";
    private const string animIDRoll = "Roll"; 
    private const string animIDDeath = "IsDead";

    [SerializeField] private GameObject hitVfx;

    private bool isDead = false;

    private Animator animator;
    private EnemyManager enemyManager;
    private Enemy enemy;

    [SerializeField] EnemyCharacterStatus status;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        animator = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        enemy = enemyManager.GetEnemyModel();
    }

    // Update is called once per frame
    void Update(){
        CheckIsDead();
        if(!isDead){
            if(status.GetCanMove()){
                CheckIsAttacking();
                UpdateStatus(status.IsGrounded(), status.GetAnimationBlend(), status.GetInputMagnitude(), 
                    status.IsFalling(), status.IsRolling());
            }else{
                UpdateStatus(true, 0, 0, false, false);
            }
        }
    }

    private void CheckIsDead(){
        if(status.IsDead() && !isDead){
            Debug.Log("SCHIATTATO");
            isDead = true;
            animator.SetTrigger(animIDDeath);
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
        //Debug.Log("Enemy vulnerabilities: "+enemy.vulnerabilities[0]);
        Debug.Log(enemy.offensiveDamageType[0].damageTypeTag);
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
