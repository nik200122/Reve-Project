using System;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
public class GameLoader : MonoBehaviour
{   
    private const string playerDataFile = "Assets/Resources/XML/playerData.xml";
    private const string abilitiesRulesFile = "Assets/Resources/XML/AbilitiesTypeRules.xml";
    private const string playerInventoryDataFile = "Assets/Resources/XML/playerInventoryData.xml";
    private const string shopper01InventoryDataFile = "Assets/Resources/XML/shopper01InventoryData.xml";

    private const string enemy01DataFile = "Assets/Resources/XML/enemy01Data.xml";
    private const string enemy02DataFile = "Assets/Resources/XML/enemy02Data.xml";
    private const string enemy03DataFile = "Assets/Resources/XML/enemy03Data.xml";
    private const string enemy04DataFile = "Assets/Resources/XML/enemy04Data.xml";
    private const string enemy05DataFile = "Assets/Resources/XML/enemy05Data.xml";
    private const string enemy06DataFile = "Assets/Resources/XML/enemy06Data.xml";
    private const string enemy07DataFile = "Assets/Resources/XML/enemy07Data.xml";
    private const string itemDataFile = "Assets/Resources/XML/itemsData.xml";
    private const string equipmentRulesFile = "Assets/Resources/XML/EquipmentRules.xml";
    private const string worldDataFile = "Assets/Resources/XML/worldInfoReducted.xml";
    private const string attackDataFile = "Assets/Resources/XML/attacksData.xml";
    private const string LoadoutPlayerFile = "Assets/Resources/XML/playerLoadout.xml";
    private const string comboRulesFile = "Assets/Resources/XML/comboRules.xml";
    private const string abilitiesFile = "Assets/Resources/XML/Abilities.xml";
    private const string damageFormulaFile = "Assets/Resources/XML/DamageFormula.xml";
    private const string damageApplicationRuleFile = "Assets/Resources/XML/damageApplicationRule.xml";
    private const string defeatRuleFile = "Assets/Resources/XML/defeatRule.xml";

    private const string menuAudioDataFile =  "Assets/Resources/XML/MenuAudioData.xml";
    private const string gameMusicDataFile =  "Assets/Resources/XML/gameMusicData.xml";
    private const string onHitAudioDataFile =  "Assets/Resources/XML/onHitAudioData.xml";
    
    
    
    public WorldData LoadedWorldData { get; private set; }
    public AttackDataList LoadedAttackDataList { get; private set; }
    public PlayerLoadout LoadedPlayerLoadout { get; private set; }
    public AbilityList LoadedAbilityList { get; private set; }
    public ComboRules LoadedComboRules { get; private set; }
    public AbilitiesRules LoadedAbilitiesRules { get; private set; }
    public Player LoadedPlayerModel { get; private set; }
    //public List<Item> LoadedItems { get; private set; }
    public List<EquipmentRule> LoadedEquipmentRuleList { get; private set; }
    public Inventory LoadedPlayerInventory { get; private set; }
    public Inventory LoadedShopper01Inventory { get; private set; }
    public Enemy LoadedEnemy01Model { get; private set; }
    public Enemy LoadedEnemy02Model { get; private set; }
    public Enemy LoadedEnemy03Model { get; private set; }
    public Enemy LoadedEnemy04Model { get; private set; }
    public Enemy LoadedEnemy05Model { get; private set; }
    public Enemy LoadedEnemy06Model { get; private set; }
    public Enemy LoadedEnemy07Model { get; private set; }

    public DamageFormula LoadedDamageFormula { get; private set; }
    public DamageApplicationRule LoadedDamageApplicationRule { get; private set; }
    public DefeatRule LoadedDefeatRule { get; private set; }

    // [XmlArray("TriggerActions")]
    // [XmlArrayItem("TriggerAction")]
    // public List<AudioTriggerActionsConfig> LoadedMenuAudioData { get; private set; }

    public AudioTriggerActionsWrapper LoadedMenuAudioData { get; private set; }
    public AudioTriggerActionsWrapper LoadedGameMusicData { get; private set; }
    public AudioTriggerActionsWrapper LoadedOnHitAudioData { get; private set; }

    public void LoadData(){

        LoadPlayerData();

        LoadWorldData();

        LoadAbilityRules();

        LoadAbilities();

        LoadPlayerLoadout();

        LoadAttackData();

        LoadEnemyData();

        LoadShoppersData();
        
        //LoadAssets();

        LoadAudioData();

        LoadDamageFormula();

        LoadDamageApplicationRule();

        LoadDefeatRule();
    }

    private void LoadAudioData(){
        //LoadedMenuAudioData =  XMLHelper.LoadFromXml<List<AudioTriggerActionsConfig>>(menuAudioDataFile);
        LoadedMenuAudioData = XMLHelper.LoadFromXml<AudioTriggerActionsWrapper>(menuAudioDataFile);
        LoadedGameMusicData = XMLHelper.LoadFromXml<AudioTriggerActionsWrapper>(gameMusicDataFile);
        LoadedOnHitAudioData = XMLHelper.LoadFromXml<AudioTriggerActionsWrapper>(onHitAudioDataFile);
    }

    private void LoadDefeatRule(){
        LoadedDefeatRule = XMLHelper.LoadFromXml<DefeatRule>(defeatRuleFile);
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
        LoadedPlayerModel = XMLHelper.LoadFromXml<Player>(playerDataFile);
        LoadedPlayerInventory = XMLHelper.LoadFromXml<Inventory>(playerInventoryDataFile);
        LoadedEquipmentRuleList = XMLHelper.LoadFromXml<List<EquipmentRule>>(equipmentRulesFile);
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
    
    //non il miglior modo, trovare una soluzione piú scalabile
    private void LoadEnemyData()
    {
        LoadedEnemy01Model = XMLHelper.LoadFromXml<Enemy>(enemy01DataFile);
        LoadedEnemy01Model.SetAnimatorOverrideControllers();

        LoadedEnemy02Model = XMLHelper.LoadFromXml<Enemy>(enemy02DataFile);
        LoadedEnemy02Model.SetAnimatorOverrideControllers();

        LoadedEnemy03Model = XMLHelper.LoadFromXml<Enemy>(enemy03DataFile);
        LoadedEnemy03Model.SetAnimatorOverrideControllers();

        LoadedEnemy04Model = XMLHelper.LoadFromXml<Enemy>(enemy04DataFile);
        LoadedEnemy04Model.SetAnimatorOverrideControllers();

        LoadedEnemy05Model = XMLHelper.LoadFromXml<Enemy>(enemy05DataFile);
        LoadedEnemy05Model.SetAnimatorOverrideControllers();

        LoadedEnemy06Model = XMLHelper.LoadFromXml<Enemy>(enemy06DataFile);
        LoadedEnemy06Model.SetAnimatorOverrideControllers();

        LoadedEnemy07Model = XMLHelper.LoadFromXml<Enemy>(enemy07DataFile);
        LoadedEnemy07Model.SetAnimatorOverrideControllers();
    }
}