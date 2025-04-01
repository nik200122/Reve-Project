using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


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
        if(GameStateManager.Instance.CurrentState == GameState.FreeRoam){
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
                StartAttack();
            }
        }
        
    }
    // Metodo che restituisce l'AttackData da usare per il currentStep
    private AttackData GetAttackForCurrentStep(AttackStep step){
        
            // Controlla se il player ha un'abilità attiva che fornisce un attacco
        foreach (var abilityRef in playerLoadout.Abilities)  // AbilityRefs contiene solo gli ID delle abilità equipaggiate
        {
            // Cerca l'abilità completa corrispondente all'ID
            Ability ability = abilityList.Abilities.FirstOrDefault(a => a.id == abilityRef.Id);  // Usa 'id' invece di 'Id' per la corrispondenza

            // Se l'abilità è trovata, è attiva e ha attacchi equipaggiabili
            if (ability != null && ability.isActive && ability.equippableAttacks.Count > 0)
            {
                // Controlla se l'attacco dell'abilità è permesso per questo step della combo
                foreach (var attackRef in ability.equippableAttacks)
                {
                    // Verifica se l'attackRef è ammesso in questo step
                    if (step.AllowedAttacks.Exists(a => a.Id == attackRef.Id))
                    {
                        // Trova e restituisci l'attacco corrispondente
                        if (attackDictionary.TryGetValue(attackRef.Id, out AttackData attackData))
                        {
                            return attackData;  // Restituisce il giusto attacco
                        }
                    }
                }
            }
        }

        if(attackDictionary.TryGetValue(step.AllowedAttacks[0].Id, out AttackData attackDataDefault)){
            return attackDataDefault;
        }
        else return null;
    }


    void StartAttack(){
    if (Time.time - lastAttackTime > comboResetTime)
    {
        currentStepIndex = 0;
    }

    if (currentStepIndex < currentCombo.AttackSteps.Count)
    {
        AttackStep step = currentCombo.AttackSteps[currentStepIndex];

        // Usa il nuovo metodo per ottenere l'attacco corretto
        AttackData attackData = GetAttackForCurrentStep(step);
        if (attackData != null)
        {
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

        // Se c'era un input in coda, avvia immediatamente il prossimo attacco.
        if (queuedAttack)
        {
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
    internal void SetPlayerLoadout(PlayerLoadout loadedPlayerLoadout)
    {
        playerLoadout = loadedPlayerLoadout;
    }
}
