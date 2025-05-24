using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class AttackSelectionManager : MonoBehaviour
{
    // --- DATI INTERNI DEL MANAGER ---
    private AttackDataList attackDataList;
    private Dictionary<string, AttackData> attackDictionary;
    private PlayerLoadout playerLoadout;
    private AbilityList abilityList;
    [Header("Combo Data")]
    [Tooltip("Combo corrente definita nei dati (XML)")]
    [SerializeField] private Combo currentCombo;

    // --- FINE DATI INTERNI ---

    public class AttackExecutionDetails
    {
        public AttackData AttackToExecute { get; }
        public List<StatModifier> AppliedModifiers { get; }
        public int NextStepIndex { get; }
        public bool IsValidAttack { get; }

        public AttackExecutionDetails(AttackData attack, List<StatModifier> modifiers, int nextStepIndex, bool isValid)
        {
            AttackToExecute = attack;
            AppliedModifiers = modifiers;
            NextStepIndex = nextStepIndex;
            IsValidAttack = isValid;
        }
    }

    // --- METODI DI INIZIALIZZAZIONE CHIAMATI DAL GAMEMANAGER ---
    public void InitializeAttackData(AttackDataList loadedAttackDataList)
    {
        this.attackDataList = loadedAttackDataList;
        this.attackDictionary = new Dictionary<string, AttackData>();
        if (this.attackDataList != null && this.attackDataList.Attacks != null)
        {
            foreach (var attack in this.attackDataList.Attacks)
            {
                if (attack != null && !string.IsNullOrEmpty(attack.Id))
                {
                    this.attackDictionary[attack.Id] = attack;
                }
            }
        }
        else
        {
            Debug.LogWarning("AttackDataList o la sua lista di Attacks è null in AttackSelectionManager.");
        }
    }

    public void InitializePlayerLoadout(PlayerLoadout loadedPlayerLoadout)
    {
        this.playerLoadout = loadedPlayerLoadout;
    }

    public void InitializeAbilityList(AbilityList loadedAbilityList)
    {
        this.abilityList = loadedAbilityList;
    }
    // --- FINE METODI DI INIZIALIZZAZIONE ---

    public AttackExecutionDetails PrepareAttack(
        int currentPotentialStepIndex,
        float timeSinceLastAttackInput,
        float comboTimeout)
    {
        // Validazione dei dati interni necessari
        if (this.attackDictionary == null || this.playerLoadout == null || this.abilityList == null)
        {
            Debug.LogError("AttackSelectionManager non è stato inizializzato correttamente con tutti i dati necessari (attackDictionary, playerLoadout, abilityList).");
            return new AttackExecutionDetails(null, null, currentPotentialStepIndex, false);
        }

        int actualStepIndex = currentPotentialStepIndex;

        if (timeSinceLastAttackInput > comboTimeout)
        {
            actualStepIndex = 0;
        }

        if (currentCombo == null || currentCombo.AttackSteps == null || currentCombo.AttackSteps.Count == 0 || actualStepIndex >= currentCombo.AttackSteps.Count)
        {
            return new AttackExecutionDetails(null, null, actualStepIndex, false);
        }

        AttackStep currentStepConfig = currentCombo.AttackSteps[actualStepIndex];
        List<StatModifier> determinedModifiers = new List<StatModifier>();
        AttackData selectedAttack = null;

        // 1. Cerca tra le abilità equipaggiate
        if (this.playerLoadout.Abilities != null && this.abilityList.Abilities != null)
        {
            foreach (var abilityRef in this.playerLoadout.Abilities)
            {
                Ability ability = this.abilityList.Abilities.FirstOrDefault(a => a.id == abilityRef.Id);
                if (ability != null && abilityRef.IsActive &&
                    string.Equals(ability.abilityType.ToString(), currentStepConfig.stepType.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    if (ability.equippableAttacks != null && ability.equippableAttacks.Count > 0)
                    {
                        if (this.attackDictionary.TryGetValue(ability.equippableAttacks[0].Id, out AttackData abilityAttack))
                        {
                            selectedAttack = abilityAttack;
                            determinedModifiers = (currentStepConfig.modifiers ?? new List<StatModifier>())
                                                  .Concat(ability.modifiers ?? new List<StatModifier>())
                                                  .ToList();
                            break;
                        }
                    }
                }
            }
        }

        // 2. Se nessun attacco da abilità, usa l'attacco di default per lo step
        if (selectedAttack == null && currentStepConfig.defaultAttack != null && !string.IsNullOrEmpty(currentStepConfig.defaultAttack.Id))
        {
            if (this.attackDictionary.TryGetValue(currentStepConfig.defaultAttack.Id, out AttackData defaultAttackData))
            {
                selectedAttack = defaultAttackData;
                determinedModifiers = new List<StatModifier>(currentStepConfig.modifiers ?? new List<StatModifier>());
            }
        }

        if (selectedAttack != null)
        {
            return new AttackExecutionDetails(selectedAttack, determinedModifiers, actualStepIndex + 1, true);
        }

        return new AttackExecutionDetails(null, null, actualStepIndex, false);
    }
    public void SetComboRules(ComboRules loadedComboRules)
    {
        currentCombo = loadedComboRules?.Combo;
    }
}
