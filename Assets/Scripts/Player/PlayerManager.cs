using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    private Player player;
    private Inventory inventory;
    private List<EquipmentRule> equipmentRuleList;
    private PlayerLoadout playerLoadout;
    private AbilitiesRules abilitiesRules;
    private AbilityList abilityList;
    private Dictionary<AbilityType, int> activeAbilitiesCount = new Dictionary<AbilityType, int>();
    private List<PlayerModifier> modifiers = new List<PlayerModifier>();
 
    private void Update()
    {
        ApplyActiveModifiers();
    }

    private void ApplyActiveModifiers(){
        foreach (var modifier in modifiers){
            //Debug.Log("Modificatori attivi: " + modifiers.Count);
            player.SetStat(modifier.targetStat, player.GetStat(modifier.targetStat).currentValue + modifier.value);
        }
    }

    public Player GetPlayerModel(){
        return player;
    }

    public void SetPlayerModel(Player loadedPlayer){
       player=loadedPlayer;
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

    public void ApplyModifier(PlayerModifier modifier){
        Debug.Log("MODIFIER CHIAMATO");
        player.SetStat(modifier.targetStat,player.GetStat(modifier.targetStat).currentValue + modifier.value);
        Debug.Log("OGGETTO USATO CORRETTAMENTE" +player.GetStat(modifier.targetStat).currentValue);
    }

    private void AddModifiers(List<PlayerModifier> modifiersToAdd)
    {
        foreach (var modifier in modifiersToAdd){
            // Aggiungi solo se non è già presente
            if (!modifiers.Contains(modifier)){
                modifiers.Add(modifier);
            }
        }
        Debug.Log("Modificatori aggiunti. Totale: " + modifiers.Count);
    }


    private void RemoveModifiers(List<PlayerModifier> modifiersToRemove)
    {
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


    public void InitializeDictionary(){
        activeAbilitiesCount.Clear();  // 🔹 Reset per evitare problemi


        //NECESSARIO IL CLEAR?


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

    public void SetAbilitiesRules(AbilitiesRules loadedAbilitiesRules)
    {
        this.abilitiesRules = loadedAbilitiesRules;
    }

    internal void SetAbilityList(AbilityList loadedAbilityList)
    {
        abilityList= loadedAbilityList;
    }
    public void SetPlayerLoadout(PlayerLoadout playerLoadout)
    {
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

    private void AddModifiersFromEquippedItems() {
        foreach (var item in inventory.itemList){
            if (item is EquipableItem equipableItem && equipableItem.isEquipped){
                AddModifiers(item.modifiers);
            }
        }
        // Debug per confermare il numero di modificatori aggiornati
        Debug.Log("Modificatori aggiornati. Totale: " + modifiers.Count);
    }

    public bool CanAffordItem(int itemPrice){
        if(player.GetStat("Money").currentValue < itemPrice)
            return false;
        
        return true;
    }
}
