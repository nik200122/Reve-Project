using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAIController : MonoBehaviour
{
    private NavMeshAgent agent;
    private IHittable attacker;
    private EnemyManager enemyManager;
    private EnemyCharacterStatus enemyCharacterStatus;
    private Animator animator;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private DamageSystemManager damageSystemManager;
    [SerializeField] private LayerMask hittableLayer;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float patrolSpeed = 2f;

    //Patrolling
    [SerializeField] private List<Vector3> walkPointList;
    [SerializeField] private List<float> timePerPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;
    private int walkPointIndex;
    private float waitTime;

    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private float viewAngle = 60f; // mezzo angolo (es. 60° = 120° totali)

    [SerializeField] private bool isPlayerInSightRange, isPlayerInAttackRange;
    [SerializeField] private float attackInterval;
    [SerializeField] private Transform attackPos;
    [SerializeField] private LayerMask playerLayer;

    private float attackTimer; 
    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
        attacker = GetComponent<IHittable>();
        enemyCharacterStatus = GetComponent<EnemyCharacterStatus>();
    }

    private void Update(){
        if(GameStateManager.Instance.CurrentState == GameState.FreeRoam){
            isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, hittableLayer);
            isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, hittableLayer);

            if(!isPlayerInSightRange && !isPlayerInAttackRange)
                Patrolling();
            if(isPlayerInSightRange && !isPlayerInAttackRange)
                ChasePlayer();
            if(isPlayerInSightRange && isPlayerInAttackRange)
                HandleAttack();
        }else 
            agent.ResetPath();
    }

    private void HandleAttack(){
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval){
            AttackPlayer();
            attackTimer = 0f; // resetta il timer
        }
    }

    private void Patrolling(){
        if(walkPointIndex < walkPointList.Count){
            agent.SetDestination(walkPointList[walkPointIndex]);
        }else 
            walkPointIndex = 0;
        Vector3 distanceToWalkPoint = transform.position - walkPointList[walkPointIndex];
        //walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f)
            walkPointIndex++;
    }

    private void ChasePlayer(){
        agent.speed = chaseSpeed;
        agent.SetDestination(playerTransform.position);
    }
    
    private void AttackPlayer(){
        transform.LookAt(playerTransform);

        enemyCharacterStatus.SetIsAttacking(true);
    }

    public void PerformAttack(){
        Collider[] hitPlayer = Physics.OverlapSphere(attackPos.position, attackRange, playerLayer);
        foreach (Collider playerCollider in hitPlayer){
            Debug.Log("NOME: "+playerCollider.name);
            //Rigidbody enemyRb = enemyCollider.GetComponent<Rigidbody>();
            IHittable defender= playerCollider.GetComponent<IHittable>();
            damageSystemManager.ApplyEffectiveDamage(attacker, defender);
        }
    }

    public void ResetAttack(){
        enemyCharacterStatus.SetIsAttackInProgress(false);
        enemyCharacterStatus.SetIsAttacking(false);
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(forwardOffset, sightRange);
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
