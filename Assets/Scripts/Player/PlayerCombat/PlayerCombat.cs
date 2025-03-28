using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combo Data")]
    [Tooltip("Combo corrente definita nei dati (XML)")]
    [SerializeField] private Combo currentCombo;

    [Tooltip("Lista di tutti gli attacchi caricati (dati da AttackDefinitions.xml)")]
    private AttackDataList attackDataList; 

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

    void StartAttack()
    {
        // Se troppo tempo è passato dall'ultimo attacco, resetta la combo.
        if (Time.time - lastAttackTime > comboResetTime)
        {
            currentStepIndex = 0;
        }

        // Se abbiamo ancora passi nella combo...
        if (currentStepIndex < currentCombo.Sequence.Count)
        {
            AttackStep step = currentCombo.Sequence[currentStepIndex];

            // Recupera l'attacco corrispondente tramite il dizionario.
            if (attackDictionary.TryGetValue(step.AttackId, out AttackData attackData))
            {
                // Imposta l'AnimatorOverrideController per l'attacco corrente.
                if (attackData.AnimatorOverrideController != null)
                {
                    animator.runtimeAnimatorController = attackData.AnimatorOverrideController;
                }
                else
                {
                    Debug.LogWarning("Override controller non caricato per l'attacco " + attackData.Id);
                }

                // Avvia l'animazione di attacco (lo stato "Attack" deve essere definito nell'Animator).
                animator.Play("Attack", 0, 0);

                lastAttackTime = Time.time;
                currentStepIndex++;
                attackInProgress = true;
                queuedAttack = false;
                allowQueue = false; // Resetta il flag finché non sarà abilitato dall'animazione.
            }
            else
            {
                Debug.LogError("Non esiste un attacco con ID: " + step.AttackId);
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
        currentCombo = loadedComboRules.Combos[0];
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
}
