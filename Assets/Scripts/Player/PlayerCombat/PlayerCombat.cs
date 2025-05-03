using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using DG.Tweening;


public class PlayerCombat : MonoBehaviour
{
    [Header("Combo Data")]
    [Tooltip("Combo corrente definita nei dati (XML)")]
    [SerializeField] private Combo currentCombo;

    [Tooltip("Lista di tutti gli attacchi caricati (dati da AttackDefinitions.xml)")]
    private AttackDataList attackDataList;
    private PlayerLoadout playerLoadout;
    private AbilityList abilityList;

    [Header("Combat Settings")]
    [Tooltip("Tempo massimo (in secondi) tra gli attacchi per continuare la combo")]
    [SerializeField] private float comboResetTime = 1.0f; 

    [Header("References")]
    private Animator animator;
    [SerializeField] private InputHandler input;

    // Dizionario per accedere rapidamente agli attacchi tramite il loro ID
    private Dictionary<string, AttackData> attackDictionary;
    private List<StatModifier> currentModifiers;
    private CharacterStatus characterStatus;

    // Variabili per gestire la combo
    private int currentStepIndex = 0;
    private float lastAttackTime = 0f;
    private bool attackInProgress = false;
    private bool queuedAttack = false;

    // Flag per abilitare la coda degli input, attivato a >75% dell'animazione
    private bool allowQueue = false;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();

    }

    void Start()
    {
        input.OnAttackEvent += OnAttackInput;
    }

    void OnDestroy()
    {
        input.OnAttackEvent -= OnAttackInput;
    }

    void Update()
    {
        // Se non siamo in attacco e abbiamo superato il tempo massimo, resetta la combo.
        if (!attackInProgress && (Time.time - lastAttackTime > comboResetTime))
        {
            ResetCombo();
        }
    }

    void OnAttackInput()
    {
        if(characterStatus.GetCanAttack()){
            // Se siamo in attacco ma l'input è stato ricevuto dopo il 75% (allowQueue==true), metti in coda l'attacco.
            if (attackInProgress)
            {
                if (allowQueue)
                {
                    queuedAttack = true;
                    // Puoi anche aggiornare lastAttackTime qui per "rinfrescare" il timer,
                    // in modo che il reset non avvenga se l'input arriva giusto in tempo.
                    lastAttackTime = Time.time;
                }
                // Se l'input arriva prima del 75% dell'animazione, lo ignori.
            }
            else
            {
                characterStatus.StartAttackMovement();
                StartAttack();
            }
        }
        
    }
    private AttackData GetAttackForCurrentStep(AttackStep step)
    {
        // Cerca tra le abilità equipaggiate quella attiva con abilityType uguale allo StepType dello step
        foreach (var abilityRef in playerLoadout.Abilities)
        {
            Ability ability = abilityList.Abilities.FirstOrDefault(a => a.id == abilityRef.Id);
            if (ability != null && abilityRef.IsActive && 
                string.Equals(ability.abilityType.ToString(), step.stepType.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // Se l'abilità ha attacchi equipaggiabili, usiamo il primo (o implementiamo una logica di scelta)
                if (ability.equippableAttacks.Count > 0)
                {
                    if (attackDictionary.TryGetValue(ability.equippableAttacks[0].Id, out AttackData abilityAttack))
                    {
                        // Combina i modificatori dello step e quelli dell'abilità
                        currentModifiers = step.modifiers.Concat(ability.modifiers).ToList();
                        return abilityAttack;
                    }
                }
            }
        }
        
        // Se non ci sono abilità attive che influenzano questo step, usa l'attacco di default
        if (attackDictionary.TryGetValue(step.defaultAttack.Id, out AttackData defaultAttack))
        {
            currentModifiers = new List<StatModifier>(step.modifiers);
            return defaultAttack;
        }
        
        return null;
    }



    void StartAttack(){
        if (Time.time - lastAttackTime > comboResetTime){
            currentStepIndex = 0;
        }

        if (currentStepIndex < currentCombo.AttackSteps.Count){
            AttackStep step = currentCombo.AttackSteps[currentStepIndex];

            // Usa il nuovo metodo per ottenere l'attacco corretto
            AttackData attackData = GetAttackForCurrentStep(step);
            if (attackData != null)
            {
                /*foreach (var modifier in currentModifiers){
                    Debug.Log(modifier.ToString());
                }*/
                if (attackData.AnimatorOverrideController != null)
                {
                    animator.runtimeAnimatorController = attackData.AnimatorOverrideController;
                }
                else
                {
                    Debug.LogWarning("Override controller non caricato per l'attacco " + attackData.Id);
                }

                
                animator.Play("Attack", 0, 0);
                
                lastAttackTime = Time.time;
                currentStepIndex++;
                attackInProgress = true;
                queuedAttack = false;
                allowQueue = false;
            }
    }
}


    // Questo metodo viene chiamato da AttackAnimationBehaviour.OnStateExit quando l'animazione termina.
    public void OnAttackAnimationComplete()
    {
        attackInProgress = false;
        characterStatus.EndAttackMovement();

        // Se c'era un input in coda, avvia immediatamente il prossimo attacco.
        if (queuedAttack){
            characterStatus.StartAttackMovement();
            StartAttack();
        }
        // Altrimenti, il timer in Update gestirà il reset della combo se non arriva altro input.
    }

    // Metodo chiamato dall'AttackAnimationBehaviour per abilitare la coda (dopo il 75% dell'animazione).
    public void AllowAttackQueueing()
    {
        allowQueue = true;
    }

    void ResetCombo()
    {
        currentStepIndex = 0;
    }

    // Imposta i dati delle combo (caricati da XML)
    public void SetComboRules(ComboRules loadedComboRules)
    {
        currentCombo = loadedComboRules.Combo;
    }

    // Imposta la lista degli attacchi e costruisce il dizionario.
    public void SetAttackDataList(AttackDataList attackDataList)
    {
        this.attackDataList = attackDataList;
        attackDictionary = new Dictionary<string, AttackData>();
        foreach (var attack in attackDataList.Attacks)
        {
            attackDictionary[attack.Id] = attack;
        }
    }

    public void SetAbilityList(AbilityList loadedAbilityList)
    {
        this.abilityList = loadedAbilityList;
    }
    public void SetPlayerLoadout(PlayerLoadout loadedPlayerLoadout)
    {
        playerLoadout = loadedPlayerLoadout;
    }

    public List<StatModifier> GetCurrentModifiers(){
        return currentModifiers;
    }




//qui parte la gestione dell'attacco
    
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask hittableLayer;
    [SerializeField] private float reachTime = 0.3f;
    [SerializeField] private float autoTargetRange = 5f;
    [SerializeField] private DamageSystemManager damageSystemManager;

    [SerializeField] IHittable attacker;


    //animation Event
    public void PerformAttack(){
        Collider[] hitEnemies = Physics.OverlapSphere(attackPos.position, attackRange, hittableLayer);
        TryAutoTarget(hitEnemies);
        foreach (Collider enemyCollider in hitEnemies){
            EnemyCharacterStatus status = enemyCollider.GetComponent<EnemyCharacterStatus>();
            status.SetIsHit(true);

            IHittable defender= enemyCollider.GetComponent<IHittable>();
            damageSystemManager.ApplyEffectiveDamage(attacker, defender);
        }
    }

    public void FaceThis(Vector3 target){
        //transform.LookAt(target);
        Vector3 target_ = new Vector3(target.x, target.y, target.z);
        Quaternion lookAtRotation = Quaternion.LookRotation(target_ - transform.position);
        lookAtRotation.x = 0;
        lookAtRotation.z = 0;
        transform.DOLocalRotateQuaternion(lookAtRotation, 0.2f);
    }

    //animation event
    public void ResetAttack(){}

    public void TryAutoTarget(Collider[] hittableColliderList){
        Collider[] enemies = Physics.OverlapSphere(transform.position, autoTargetRange, hittableLayer);

        if (enemies.Length == 0){
            Debug.Log("❌ Nessun nemico nel raggio.");
            return;
        }

        Transform closest = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider col in enemies){
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < minDistance){
                minDistance = dist;
                closest = col.transform;
            }
        }

        if (closest != null){
            Debug.Log("✅ Nemico trovato: " + closest.name);
            MoveTowardsTarget(closest.position, minDistance, "Attack");
        }
}

    public void MoveTowardsTarget(Vector3 target_, float deltaDistance, string animationName_){
        FaceThis(target_);
        // Calcola la distanza effettiva tra il player e il nemico
        float distanceToTarget = Vector3.Distance(transform.position, target_);

        // Definisci una distanza minima per fermare il movimento
        float stopDistance = 1.5f;  // Modifica questa distanza come preferisci
    
        if (distanceToTarget <= stopDistance){
            // Se siamo abbastanza vicini, non muoviamo più
            Debug.Log("Sufficientemente vicino al nemico. Arrestando movimento.");
            return;
        }

        // Scala la velocità del movimento in base alla distanza
        // Ad esempio, se il nemico è molto lontano, ci avviciniamo più lentamente
        float moveSpeed = Mathf.Lerp(0.1f, 1f, 1 - Mathf.Clamp01(distanceToTarget / autoTargetRange));

        // Usa la distanza tra il player e il nemico come deltaDistance per il movimento.
        // Limita deltaDistance al massimo, in modo che non si sposti troppo lontano.
        float moveDistance = Mathf.Min(deltaDistance, distanceToTarget) * moveSpeed;

        Vector3 newPos = Vector3.MoveTowards(transform.position, target_, moveDistance);
        //transform.position = newPos;
        transform.DOMove(newPos, reachTime);
    }
}
