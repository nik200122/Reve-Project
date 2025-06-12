using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using DG.Tweening;

public class PlayerCombat : MonoBehaviour
{
    


    [Header("Combat Settings")]
    [Tooltip("Tempo massimo (in secondi) tra gli attacchi per continuare la combo")]
    [SerializeField] private float comboResetTime = 1.0f;

    [Header("References")]
    private Animator animator;
    [SerializeField] private InputHandler input;
    [SerializeField] private AttackSelectionManager attackSelectionManager; // Riferimento essenziale

    // Rimosso: attackDictionary
    private List<StatModifier> currentModifiers = new List<StatModifier>();
    List<StatModifier> previousModifiers = new List<StatModifier>();
    private CharacterStatus characterStatus;
    PlayerManager playerManager;

    private int currentStepIndex = 0;
    private float lastAttackTime = 0f;
    private bool attackInProgress = false;
    private bool queuedAttack = false;
    private bool allowQueue = false;
    private AudioTriggerActionsWrapper wrapperTriggerActions;

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();
        playerManager = FindAnyObjectByType<PlayerManager>();
        if (attackSelectionManager == null)
        {
            Debug.LogError("AttackSelectionManager non è stato assegnato a PlayerCombat nell'Inspector!");
            this.enabled = false;
        }
        if (attacker == null) attacker = GetComponent<IHittable>();
    }

    void Start()
    {   
        input.OnAttackEvent += OnAttackInput;
        if (wrapperTriggerActions != null && wrapperTriggerActions.TriggerActions != null)
        {
            foreach (var triggerActionConfig in wrapperTriggerActions.TriggerActions)
            {
                if (triggerActionConfig != null && triggerActionConfig.Action != null)
                {
                    IAudioAction action = ActionFactory.CreateAudioAction(triggerActionConfig.Action.type, triggerActionConfig.Action.Parameters);
                    if (action != null)
                    {
                        AudioTriggerActionManager.Instance.RegisterAction(this.gameObject, triggerActionConfig.Trigger, action);
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        input.OnAttackEvent -= OnAttackInput;
    }

    void Update()
    {
        if (!attackInProgress && !queuedAttack && (Time.time - lastAttackTime > comboResetTime))
        {
            if (currentStepIndex != 0)
            {
                ResetCombo();
            }
        }
    }

    void OnAttackInput()
    {
        if (characterStatus.GetCanAttack())
        {
            if (attackInProgress)
            {
                if (allowQueue)
                {
                    queuedAttack = true;
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                characterStatus.StartAttackMovement();
                float timeSinceLastEffectiveAttack = Time.time - lastAttackTime;
                StartAttack(timeSinceLastEffectiveAttack);
            }
        }
    }

    void StartAttack(float timeSinceLastEffectiveAttack)
    {
        Debug.Log($"[PlayerCombat] === ATTACK DEBUG ===");
        Debug.Log($"[PlayerCombat] currentStepIndex BEFORE: {currentStepIndex}");
        Debug.Log($"[PlayerCombat] timeSinceLastEffectiveAttack: {timeSinceLastEffectiveAttack}");
        Debug.Log($"[PlayerCombat] comboResetTime: {comboResetTime}");
        Debug.Log($"[PlayerCombat] Will reset combo? {timeSinceLastEffectiveAttack > comboResetTime}");

        // I dati (PlayerLoadout, AbilityList, AttackDictionary) sono ora interni ad AttackSelectionManager
        AttackSelectionManager.AttackExecutionDetails attackDetails = attackSelectionManager.PrepareAttack(
            currentStepIndex,
            timeSinceLastEffectiveAttack,
            comboResetTime
        
        );
        

        if (attackDetails != null && attackDetails.IsValidAttack && attackDetails.AttackToExecute != null)
        {
            AttackData attackDataToExecute = attackDetails.AttackToExecute;
             // ✅ DEBUG: Forza reset per test
            previousModifiers = new List<StatModifier>(currentModifiers);
            playerManager.ChangeCombatModifiers(previousModifiers, new List<StatModifier>());
            // LOG PRIMA DEL CAMBIO
            Debug.Log($"[PlayerCombat] Previous modifiers count: {previousModifiers.Count}");
            Debug.Log($"[PlayerCombat] New modifiers count: {attackDetails.AppliedModifiers?.Count ?? 0}");
            Debug.Log($"[PlayerCombat] Attack executed: {attackDataToExecute.Id}");

            if (attackDetails.AppliedModifiers != null)
            {
                foreach (var mod in attackDetails.AppliedModifiers)
                {
                    Debug.Log($"[PlayerCombat] New modifier: {mod.targetStat} {mod.modifierType} {mod.value}");
                }
            }
            currentModifiers.Clear();

            currentModifiers.AddRange(attackDetails.AppliedModifiers ?? new List<StatModifier>());
            playerManager.ChangeCombatModifiers(new List<StatModifier>(), currentModifiers);

            if (attackDataToExecute.AnimatorOverrideController != null)
            {
                animator.runtimeAnimatorController = attackDataToExecute.AnimatorOverrideController;
            }
            else
            {
                Debug.LogWarning("Override controller non caricato per l'attacco " + attackDataToExecute.Id);
            }

            animator.Play("Attack", 0, 0);
            lastAttackTime = Time.time;
            currentStepIndex = attackDetails.NextStepIndex;
            attackInProgress = true;
            queuedAttack = false;
            allowQueue = false;
        }
        else
        {
            currentStepIndex = attackDetails.NextStepIndex;
            characterStatus.EndAttackMovement();
        }
    }

    public void OnAttackAnimationComplete()
    {
        attackInProgress = false;
        characterStatus.EndAttackMovement();

        if (queuedAttack)
        {
            queuedAttack = false;
            characterStatus.StartAttackMovement();
            StartAttack(Time.time - lastAttackTime);
        }
    }


    public void AllowAttackQueueing()
    {
        allowQueue = true;
    }

    void ResetCombo()
    {
        currentStepIndex = 0;
        previousModifiers = new List<StatModifier>(currentModifiers);
        playerManager.ChangeCombatModifiers(previousModifiers, new List<StatModifier>());
        currentModifiers.Clear();
    }
    
    

    

    public List<StatModifier> GetCurrentModifiers()
    {
        return currentModifiers;
    }

    // --- Logica di Esecuzione dell'Attacco (Collisioni, Danno, Movimento) ---
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask hittableLayer;
    [SerializeField] private float reachTime = 0.3f;
    [SerializeField] private float autoTargetRange = 5f;
    [SerializeField] private DamageSystemManager damageSystemManager;
    [SerializeField] IHittable attacker;

    public void PerformAttack()
    {
        if (damageSystemManager == null) { Debug.LogError("DamageSystemManager non assegnato!"); return; }
        if (attacker == null) { Debug.LogError("Attacker (IHittable) non trovato!"); return; }
        if (attackPos == null) { Debug.LogError("AttackPos non assegnato!"); return; }

        Collider[] hitEnemiesInitial = Physics.OverlapSphere(attackPos.position, attackRange, hittableLayer);
        TryAutoTarget(hitEnemiesInitial);
        Collider[] hitEnemiesAfterTargeting = Physics.OverlapSphere(attackPos.position, attackRange, hittableLayer);

        foreach (Collider enemyCollider in hitEnemiesAfterTargeting)
        {
            if (AudioTriggerActionManager.Instance != null)
            {
                AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnHit);
            }
            IHittable defender = enemyCollider.GetComponent<IHittable>();
            if (defender != null)
            {
                damageSystemManager.ApplyEffectiveDamage(attacker, defender);
            }
        }
    }

    //animationEvent
    public void ResetAttack() { }

    public void FaceThis(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        if (direction.sqrMagnitude < 0.001f) return;
        Quaternion lookAtRotation = Quaternion.LookRotation(direction);
        lookAtRotation.x = 0;
        lookAtRotation.z = 0;
        transform.DOLocalRotateQuaternion(lookAtRotation, 0.2f).SetEase(Ease.OutQuad);
    }

    public void TryAutoTarget(Collider[] nearbyHittables)
    {
        Collider[] enemiesInAutoTargetRange = Physics.OverlapSphere(transform.position, autoTargetRange, hittableLayer);
        if (enemiesInAutoTargetRange.Length == 0) return;

        Transform closestEnemy = null;
        float minSqrDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider col in enemiesInAutoTargetRange)
        {
            float sqrDist = (col.transform.position - currentPosition).sqrMagnitude;
            if (sqrDist < minSqrDistance)
            {
                minSqrDistance = sqrDist;
                closestEnemy = col.transform;
            }
        }
        if (closestEnemy != null)
        {
            MoveTowardsTarget(closestEnemy.position);
        }
    }

    public void MoveTowardsTarget(Vector3 targetPosition)
    {
        FaceThis(targetPosition);
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float stopDistance = 1.5f;
        if (distanceToTarget <= stopDistance) return;

        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        Vector3 finalMovementPosition = targetPosition - directionToTarget * stopDistance;
        transform.DOMove(finalMovementPosition, reachTime).SetEase(Ease.OutQuad);
    }

    public void SetTriggerActions(AudioTriggerActionsWrapper loadedWrapperTriggerActions)
    {
        this.wrapperTriggerActions = loadedWrapperTriggerActions;
    }
}
