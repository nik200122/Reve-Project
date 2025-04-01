using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameLoader : MonoBehaviour
{   
    private const string playerStatsFile = "Assets/Resources/XML/playerStats.xml";
    private const string playerInventoryDataFile = "Assets/Resources/XML/playerInventoryData.xml";
    private const string itemDataFile = "Assets/Resources/XML/itemsData.xml";
    private const string worldDataFile = "Assets/Resources/XML/worldInfoReducted.xml";
    private const string attackDataFile = "Assets/Resources/XML/attacksData.xml";
    private const string LoadoutPlayerFile = "Assets/Resources/XML/playerLoadout.xml";
    private const string comboRulesFile = "Assets/Resources/XML/comboRules.xml";
    private const string abilitiesFile = "Assets/Resources/XML/Abilities.xml";
    
    public WorldData LoadedWorldData { get; private set; }

    public AttackDataList LoadedAttackDataList { get; private set; }
    public PlayerLoadout LoadedPlayerLoadout { get; private set; }
    public AbilityList LoadedAbilityList { get; private set; }
    public ComboRules LoadedComboRules { get; private set; }
    public Player LoadedPlayerStats { get; private set; }
    public List<Item> LoadedItems { get; private set; }
    public Inventory LoadedPlayerInventory { get; private set; }

    public void LoadData(){
        LoadPlayerData();

        LoadWorldData();

        LoadAbilities();

        LoadPlayerLoadout();

        LoadAttackData();
        
        LoadAssets();
    }

    private void LoadPlayerData(){
        LoadedPlayerStats = XMLHelper.LoadFromXml<Player>(playerStatsFile);
       // Debug.Log("VALORE "+LoadedPlayerStats.GetStat("Hp").GetStatTag());
        LoadedPlayerInventory = XMLHelper.LoadFromXml<Inventory>(playerInventoryDataFile);
        //Debug.Log(""+LoadedPlayerInventory.GetItem("HpPotion01").name);
    }
    private void LoadWorldData(){
        LoadedWorldData = XMLHelper.LoadFromXml<WorldData>(worldDataFile);
    }

    private void LoadPlayerLoadout(){
        LoadedPlayerLoadout = XMLHelper.LoadFromXml<PlayerLoadout>(LoadoutPlayerFile);
        Debug.Log("--- Player Loadout ---");
        foreach (var ability in LoadedPlayerLoadout.Abilities)
        {
            Debug.Log($"Ability ID: {ability.Id}, Active: {ability.IsActive}");
        }
        
    }

    private void LoadAbilities(){
        LoadedAbilityList = XMLHelper.LoadFromXml<AbilityList>(abilitiesFile);
        Debug.Log("--- Abilities ---");
        foreach (var ability in LoadedAbilityList.Abilities)
        {
            Debug.Log($"Ability ID: {ability.id}, Active: {ability.isActive}");
        }
        
    }

    private void LoadAttackData(){
        LoadedAttackDataList = XMLHelper.LoadFromXml<AttackDataList>(attackDataFile);
        
        foreach (var attack in LoadedAttackDataList.Attacks)
        {
            attack.LoadAnimatorOverrideController();
            
        }
        LoadedComboRules = XMLHelper.LoadFromXml<ComboRules>(comboRulesFile);

         // Controllo Attacchi
        if (LoadedAttackDataList != null && LoadedAttackDataList.Attacks.Count > 0)
        {
            Debug.Log($"✅ AttackData caricato con successo! Numero di attacchi: {LoadedAttackDataList.Attacks.Count}");
            foreach (var attack in LoadedAttackDataList.Attacks)
            {
                Debug.Log($"🔹 Attacco ID: {attack.Id}, Path Animazione: {attack.OverrideControllerPath}");
                foreach (var modifier in attack.Modifiers)
                {
                    Debug.Log($"   ➜ Modifica Statistica: {modifier.targetStat}, Valore: {modifier.value}, Tipo: {modifier.modifierType}");
                }
            }
        }
        else
        {
            Debug.LogError($"❌ Errore nel caricamento di {attackDataFile} o nessun attacco definito.");
        }

        // Controllo Combo
        if (LoadedComboRules != null && LoadedComboRules.Combo.AttackSteps.Count > 0)
        {
            Debug.Log($"✅ ComboRules caricato con successo! Numero di passi: {LoadedComboRules.Combo.AttackSteps.Count}");
            foreach (var step in LoadedComboRules.Combo.AttackSteps)
            {
                Debug.Log($"🔹 Step #{step.Index} - Attacchi permessi: {step.AllowedAttacks.Count}");
                foreach (var attackRef in step.AllowedAttacks)
                {
                    Debug.Log($"   ➜ Attacco Permesso: {attackRef.Id}");
                }

                foreach (var modifier in step.Modifiers)
                {
                    Debug.Log($"   ➜ Modifica Step: Statistica: {modifier.targetStat}, Valore: {modifier.value}, Tipo: {modifier.modifierType}");
                }
            }
        }
        else
        {
            Debug.LogError($"❌ Errore nel caricamento di {comboRulesFile} o nessun passo di combo definito.");
        }

        
    }

    private void LoadAssets(){
        LoadedItems = XMLHelper.LoadFromXml<List<Item>>(itemDataFile);
        //Debug.Log("NOME ITEM: "+LoadedItems[0].name);
    }
}