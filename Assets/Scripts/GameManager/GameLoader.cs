using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameLoader : MonoBehaviour
{   
    private const string playerStatsFile = "Assets/Resources/XML/playerStats.xml";
    private const string abilitiesRulesFile = "Assets/Resources/XML/AbilitiesTypeRules.xml";
    private const string playerInventoryDataFile = "Assets/Resources/XML/playerInventoryData.xml";
    private const string playerOffensiveDamageTypeListFile = "Assets/Resources/XML/playerOffensiveDamageTypeList.xml";
    private const string playerVulnerabilitiesFile = "Assets/Resources/XML/playerVulnerabilities.xml";
    private const string shopper01InventoryDataFile = "Assets/Resources/XML/shopper01InventoryData.xml";
    private const string enemy01AttackDataFile = "Assets/Resources/XML/enemy01AttacksData.xml";
    private const string enemy01StatDataFile = "Assets/Resources/XML/enemy01Stats.xml";
    private const string enemy01OffensiveDamageTypeListFile = "Assets/Resources/XML/enemy01OffensiveDamageTypeList.xml";
    private const string enemy01VulnerabilitiesFile = "Assets/Resources/XML/enemy01Vulnerabilities.xml";

    private const string itemDataFile = "Assets/Resources/XML/itemsData.xml";
    private const string equipmentRulesFile = "Assets/Resources/XML/EquipmentRules.xml";
    private const string worldDataFile = "Assets/Resources/XML/worldInfoReducted.xml";
    private const string attackDataFile = "Assets/Resources/XML/attacksData.xml";
    private const string LoadoutPlayerFile = "Assets/Resources/XML/playerLoadout.xml";
    private const string comboRulesFile = "Assets/Resources/XML/comboRules.xml";
    private const string abilitiesFile = "Assets/Resources/XML/Abilities.xml";
    private const string damageFormulaFile = "Assets/Resources/XML/DamageFormula.xml";
    private const string damageApplicationRuleFile = "Assets/Resources/XML/damageApplicationRule.xml";
    
    public WorldData LoadedWorldData { get; private set; }
    public AttackDataList LoadedAttackDataList { get; private set; }
    public PlayerLoadout LoadedPlayerLoadout { get; private set; }
    public AbilityList LoadedAbilityList { get; private set; }
    public ComboRules LoadedComboRules { get; private set; }
    public AbilitiesRules LoadedAbilitiesRules { get; private set; }
    public Player LoadedPlayerStats { get; private set; }
    public List<Item> LoadedItems { get; private set; }
    public List<EquipmentRule> LoadedEquipmentRuleList { get; private set; }
    public Inventory LoadedPlayerInventory { get; private set; }
    public List<DamageType> LoadedPlayerOffensiveDamageTypeList { get; private set; }
    public HashSet<DamageTypeTag> LoadedPlayerVulnerabilities { get; private set; }
    public Inventory LoadedShopper01Inventory { get; private set; }
    public List<AttackData> LoadedEnemy01AttackData { get; private set; }
    public List<Stat> LoadedEnemy01Stats { get; private set; }    
    public List<DamageType> LoadedEnemy01OffensiveDamageTypeList { get; private set; }
    public HashSet<DamageTypeTag> LoadedEnemy01Vulnerabilities { get; private set; }
    public DamageFormula LoadedDamageFormula { get; private set; }
    public DamageApplicationRule LoadedDamageApplicationRule { get; private set; }

    public void LoadData(){

        LoadPlayerData();

        LoadWorldData();

        LoadAbilityRules();

        LoadAbilities();

        LoadPlayerLoadout();

        LoadAttackData();

        LoadShoppersData();

        LoadEnemyData();
        
        LoadAssets();

        LoadDamageFormula();

        LoadDamageApplicationRule();
    }

    private void LoadDamageFormula(){
        LoadedDamageFormula = XMLHelper.LoadFromXml<DamageFormula>(damageFormulaFile);
    }

    private void LoadDamageApplicationRule(){
        LoadedDamageApplicationRule = XMLHelper.LoadFromXml<DamageApplicationRule>(damageApplicationRuleFile);
    }

    private void LoadAbilityRules(){
        LoadedAbilitiesRules = XMLHelper.LoadFromXml<AbilitiesRules>(abilitiesRulesFile);
    }

    private void LoadPlayerData(){
        LoadedPlayerStats = XMLHelper.LoadFromXml<Player>(playerStatsFile);
       // Debug.Log("VALORE "+LoadedPlayerStats.GetStat("Hp").GetStatTag());
        LoadedPlayerInventory = XMLHelper.LoadFromXml<Inventory>(playerInventoryDataFile);
        //Debug.Log(""+LoadedPlayerInventory.GetItem("HpPotion01").name);
        LoadedEquipmentRuleList = XMLHelper.LoadFromXml<List<EquipmentRule>>(equipmentRulesFile);
        LoadedPlayerOffensiveDamageTypeList = XMLHelper.LoadFromXml<List<DamageType>>(playerOffensiveDamageTypeListFile);
        LoadedPlayerVulnerabilities = XMLHelper.LoadFromXml<HashSet<DamageTypeTag>>(playerVulnerabilitiesFile);
    }
    private void LoadWorldData(){
        LoadedWorldData = XMLHelper.LoadFromXml<WorldData>(worldDataFile);
    }

    private void LoadPlayerLoadout(){
        LoadedPlayerLoadout = XMLHelper.LoadFromXml<PlayerLoadout>(LoadoutPlayerFile);
        /*Debug.Log("--- Player Loadout ---");
        foreach (var ability in LoadedPlayerLoadout.Abilities)
        {
            Debug.Log($"Ability ID: {ability.Id}, Active: {ability.IsActive}");
        }*/
        
    }

    private void LoadAbilities(){
        LoadedAbilityList = XMLHelper.LoadFromXml<AbilityList>(abilitiesFile);
        foreach (var ability in LoadedAbilityList.Abilities)
        {
            ability.LoadSprite();
            
        }
       /* Debug.Log("--- Abilities ---");
        foreach (var ability in LoadedAbilityList.Abilities)
        {
            Debug.Log($"Ability ID: {ability.id}, Active: {ability.isActive}");
        }*/
        
    }

    private void LoadAttackData(){
        LoadedAttackDataList = XMLHelper.LoadFromXml<AttackDataList>(attackDataFile);
        
        foreach (var attack in LoadedAttackDataList.Attacks)
        {
            attack.LoadAnimatorOverrideController();
            
        }
        LoadedComboRules = XMLHelper.LoadFromXml<ComboRules>(comboRulesFile);

         /* Controllo Attacchi
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
        */
        
    }

    private void LoadAssets(){
        //LoadedItems = XMLHelper.LoadFromXml<List<Item>>(itemDataFile);
        //Debug.Log("NOME ITEM: "+LoadedItems[0].name);
    }

    private void LoadShoppersData(){
        LoadedShopper01Inventory = XMLHelper.LoadFromXml<Inventory>(shopper01InventoryDataFile);
    }
    private void LoadEnemyData(){
        LoadedEnemy01AttackData = XMLHelper.LoadFromXml<List<AttackData>>(enemy01AttackDataFile);
        foreach (var attack in LoadedEnemy01AttackData){
            attack.LoadAnimatorOverrideController();
        }

        LoadedEnemy01Stats = XMLHelper.LoadFromXml<List<Stat>>(enemy01StatDataFile);
        LoadedEnemy01OffensiveDamageTypeList = XMLHelper.LoadFromXml<List<DamageType>>(enemy01OffensiveDamageTypeListFile);
        LoadedEnemy01Vulnerabilities = XMLHelper.LoadFromXml<HashSet<DamageTypeTag>>(enemy01VulnerabilitiesFile);


    }
}