using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : IHittable
{
    private Player player;
    private Inventory inventory;
    private List<EquipmentRule> equipmentRuleList;
    private PlayerLoadout playerLoadout;
    private AbilitiesRules abilitiesRules;
    private AbilityList abilityList;
    private Dictionary<AbilityType, int> activeAbilitiesCount = new Dictionary<AbilityType, int>();
    private List<StatModifier> modifiers = new List<StatModifier>();
    private CharacterStatus characterStatus;


    [SerializeField] private BattleCalculatorManager battleCalculatorManager;
    [SerializeField] private GlobalRulesManager globalRulesManager;

    private void Awake()
    {
        characterStatus = GetComponent<CharacterStatus>();
    }

    private void Update()
    {
        CheckIsDead();
        ApplyActiveModifiers();
    }

    private void ApplyActiveModifiers(){
        foreach (var stat in player.stats){
            float modifiedValue = CalculateModifiedStatValue(stat.GetStatTag(), stat.baseValue);
            //Debug.Log(stat.statTag + ": " + modifiedValue);
            player.SetStat(stat.GetStatTag(), modifiedValue);
        }
    }

    public Player GetPlayerModel(){
        return player;
    }

    public void SetPlayerModel(Player loadedPlayer){
        player = loadedPlayer;
    }

    public Inventory GetInventory(){
        return inventory;
    }

    public void SetInventory(Inventory loadedInventory){
        inventory = loadedInventory;
        AddModifiersFromEquippedItems();
        //Debug.Log("CONTEGGIO: "+inventory.itemList.Count);
    }
    /*public void SetPlayerAttackData(AttackDataList attackDataList){
        this.attackDataList = attackDataList;
        foreach(AttackData attackData in attackDataList.Attacks){
            Debug.Log(""+attackData.ToString());
        }
    }*/

    public List<EquipmentRule> GetEquipmentRuleList(){
        return equipmentRuleList;
    }

    public void ApplyModifier(StatModifier modifier){
        Debug.Log("MODIFIER CHIAMATO");
        player.SetBaseStat(modifier.targetStat, player.GetStat(modifier.targetStat).baseValue + modifier.value);
        Debug.Log("OGGETTO USATO CORRETTAMENTE" + player.GetStat(modifier.targetStat).baseValue);
    }

    private void AddModifiers(List<StatModifier> modifiersToAdd){
        foreach (var modifier in modifiersToAdd){
            // Aggiungi solo se non è già presente
            if (!modifiers.Contains(modifier)){
                modifiers.Add(modifier);
            }
        }
        Debug.Log("Modificatori aggiunti. Totale: " + modifiers.Count);
    }

    private void RemoveModifiers(List<StatModifier> modifiersToRemove){
        foreach (var modifier in modifiersToRemove){
            // Rimuovi solo se il modificatore è attualmente nella lista
            if (modifiers.Contains(modifier)){
                modifiers.Remove(modifier);
            }
        }
        Debug.Log("Modificatori rimossi. Totale: " + modifiers.Count);
    }
    
    

    public bool CheckAbility(AbilityRef abilityRef)
    {
        Ability ability = abilityList.Abilities.FirstOrDefault(a => a.id == abilityRef.Id);
        if (ability == null)
        {
            Debug.LogWarning("Abilità non trovata nella lista globale: " + abilityRef.Id);
            return false;
        }

        AbilityType abilityType = ability.abilityType;

        if (abilityRef.IsActive)
        {
            abilityRef.IsActive = false;
            activeAbilitiesCount[abilityType] = Mathf.Max(0, activeAbilitiesCount[abilityType] - 1);
            RemoveModifiers(ability.modifiers);  // 🔹 Passiamo direttamente i modificatori
            Debug.Log("Abilità disattivata: " + abilityRef.Id);
            return true;
        }
        else
        {
            AbilityRule rule = abilitiesRules.Rules.FirstOrDefault(r => r.abilityType == abilityType);
            if (rule == null)
            {
                Debug.LogWarning("Nessuna regola definita per la categoria: " + abilityType);
                return false;
            }

            if (activeAbilitiesCount[abilityType] < rule.quantity)
            {
                abilityRef.IsActive = true;
                activeAbilitiesCount[abilityType]++;
                AddModifiers(ability.modifiers);  // 🔹 Passiamo direttamente i modificatori
                Debug.Log("Abilità attivata: " + abilityRef.Id);
                return true;
            }
            else
            {
                Debug.Log("Limite massimo di abilità attive per la categoria " + abilityType + " raggiunto.");
                return false;
            }
        }
    }
    

    public void InitializeDictionary()
    {
        activeAbilitiesCount.Clear();  // 🔹 Reset per evitare problemi

        //modifiers.Clear();  // 🔹 Reset della lista modificatori
        // Supponiamo di avere una lista di tutte le regole
        foreach (var rule in abilitiesRules.Rules)
        {
            activeAbilitiesCount[rule.abilityType] = 0;
        }

        // Se ci sono abilità già attive nel loadout, aggiorniamo il conteggio
        foreach (var abilityRef in playerLoadout.Abilities)
        {
            if (abilityRef.IsActive)
            {
                Ability ability = abilityList.Abilities.FirstOrDefault(a => a.id == abilityRef.Id);
                if (ability != null)
                {
                    if (activeAbilitiesCount.ContainsKey(ability.abilityType))
                        activeAbilitiesCount[ability.abilityType]++;
                    else
                        activeAbilitiesCount[ability.abilityType] = 1;
                    // 🔹 Aggiungiamo i modificatori delle abilità attive
                    modifiers.AddRange(ability.modifiers);
                }
            }
        }
    }

    public void SetAbilitiesRules(AbilitiesRules loadedAbilitiesRules){
        this.abilitiesRules = loadedAbilitiesRules;
    }

    internal void SetAbilityList(AbilityList loadedAbilityList){
        abilityList = loadedAbilityList;
    }

    public void SetPlayerLoadout(PlayerLoadout playerLoadout){
        this.playerLoadout = playerLoadout;
    }

    public void SetEquipmentRuleList(List<EquipmentRule> equipmentRuleList){
        this.equipmentRuleList = equipmentRuleList;
    }

    public void Equip(EquipableItem equipableItem){
        //equippedItemList.Add(equipableItem);
        AddModifiers(equipableItem.modifiers);
    }

    public void Unequip(EquipableItem equipableItem){
        //equippedItemList.Remove(equipableItem);
        RemoveModifiers(equipableItem.modifiers);
    }

    private void AddModifiersFromEquippedItems(){
        foreach (var item in inventory.itemList){
            if (item is EquipableItem equipableItem && equipableItem.isEquipped){
                AddModifiers(item.modifiers);
            }
        }
        // Debug per confermare il numero di modificatori aggiornati
        Debug.Log("Modificatori aggiornati. Totale: " + modifiers.Count);
    }

    public bool CanAffordItem(int itemPrice){
        if (player.GetStat(globalRulesManager.GetCurrencyRule().currencyTargetStatTag).baseValue < itemPrice){
            Debug.Log("non abbastanza soldi");
            return false;
        }

        player.GetStat(globalRulesManager.GetCurrencyRule().currencyTargetStatTag).baseValue -= itemPrice;
        return true;
    }

    public override List<DamageType> GetOffensiveDamageTypeList(){
        return player.offensiveDamageType;
    }

    public override List<DamageTypeTag> GetVulnerabilities(){
        return player.vulnerabilities;
    }

    public override Stat GetStat(string statTag){
        return player.GetStat(statTag);
    }

    public bool CheckIsDead(){
        string defeatTargetStat = globalRulesManager.GetDefeatRule().defeatTargetStatTag;
        float defeatValue = globalRulesManager.GetDefeatRule().defeatValue;
        if (GetStat(defeatTargetStat).currentValue <= defeatValue){
            Debug.Log(" energia player: " + GetStat(defeatTargetStat).currentValue);
            characterStatus.SetIsDead(true);
            return true;
        }
        return false;
    }
    
    private float CalculateModifiedStatValue(string statTag, float baseValue){
        float additive = 0f;
        float multiplicative = 1f;

        foreach (var modifier in modifiers){
            if (modifier.targetStat == statTag){
                switch (modifier.modifierType){
                    case ModifierType.Additive:
                        additive += modifier.value;
                        break;
                    case ModifierType.Multiplicative:
                        multiplicative *= modifier.value;
                        break;
                }
            }
        }

        float finalValue = (baseValue + additive) * multiplicative;
        return finalValue;
    }

    public void ChangeCombatModifiers(List<StatModifier> previousModifiers, List<StatModifier> currentModifiers)
    {
        Debug.Log($"[PlayerManager] Changing combat modifiers - Previous: {previousModifiers?.Count ?? 0}, Current: {currentModifiers?.Count ?? 0}");
    
        if (previousModifiers != null)
        {
            foreach (var mod in previousModifiers)
            {
                Debug.Log($"[PlayerManager] Removing modifier: {mod.targetStat} {mod.modifierType} {mod.value}");
            }
        }
        
        if (currentModifiers != null)
        {
            foreach (var mod in currentModifiers)
            {
                Debug.Log($"[PlayerManager] Adding modifier: {mod.targetStat} {mod.modifierType} {mod.value}");
            }
        }
        
        RemoveModifiers(previousModifiers);
        AddModifiers(currentModifiers);
    }
}
