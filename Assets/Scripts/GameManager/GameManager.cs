using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private InputHandler input;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private AbilityScreenManager abilityScreenManager;
    [SerializeField] private DeepSeek deepSeek;
    [SerializeField] private InventoryScreenManager inventoryScreenManager;
    [SerializeField] private BattleCalculatorManager battleCalculatorManager;
    [SerializeField] private GlobalRulesManager globalRulesManager;

    //per ora un solo nemico
    [SerializeField] private EnemyManager enemyManager;

    //per ora un solo shopper
    [SerializeField] private ShopperManager shopperManager;
    private GameLoader gameLoader;
    PlayerInput playerInput;
   // private bool isMenuActionPerformed;
    //private bool menuOpened = false;

    private void Awake()
    {
        gameLoader = gameObject.GetComponent<GameLoader>();
        playerInput = input.gameObject.GetComponent<PlayerInput>();
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {    
        gameLoader.LoadData();
        deepSeek.SetWorldInfo(gameLoader.LoadedWorldData);
        playerManager.SetPlayerModel(gameLoader.LoadedPlayerStats);
        playerManager.SetInventory(gameLoader.LoadedPlayerInventory);
        playerManager.SetEquipmentRuleList(gameLoader.LoadedEquipmentRuleList);
        playerManager.SetAbilitiesRules(gameLoader.LoadedAbilitiesRules);
        playerManager.SetAbilityList(gameLoader.LoadedAbilityList);
        playerManager.SetPlayerLoadout(gameLoader.LoadedPlayerLoadout);
        playerManager.SetOffensiveDamageTypeList(gameLoader.LoadedPlayerOffensiveDamageTypeList);
        playerManager.InitializeDictionary();
        playerCombat.SetPlayerLoadout(gameLoader.LoadedPlayerLoadout);
        playerCombat.SetComboRules(gameLoader.LoadedComboRules);
        playerCombat.SetAbilityList(gameLoader.LoadedAbilityList);
        playerCombat.SetAttackDataList(gameLoader.LoadedAttackDataList);
        shopperManager.SetInventory(gameLoader.LoadedShopper01Inventory);
        enemyManager.SetAttackDataList(gameLoader.LoadedEnemy01AttackData);
        enemyManager.SetOffensiveDamageTypeList(gameLoader.LoadedEnemy01OffensiveDamageTypeList);
        enemyManager.SetEnemyStats(gameLoader.LoadedEnemy01Stats);
        battleCalculatorManager.SetDamageFormula(gameLoader.LoadedDamageFormula);
        globalRulesManager.SetDamageApplicationRule(gameLoader.LoadedDamageApplicationRule);


        //Debug.Log("VALORE FORMULA: "+gameLoader.LoadedDamageFormula.Evaluate(playerManager, playerManager));

        // var formula = new DamageFormula();
        // formula.tokens.Add(new StatToken(StatSource.Attacker, "Attack")); // metti una stat esistente
        // formula.tokens.Add(new ConstantToken(2f));
        // formula.tokens.Add(new OperatorToken(MathOperator.Multiply));

        // float result = formula.Evaluate(playerManager, playerManager);
        // Debug.Log("🔥 RISULTATO CALCOLATO: " + result);

        
        // Inizializza l'AbilityScreenManager con la lista globale delle abilità
        abilityScreenManager.Initialize(gameLoader.LoadedAbilityList, gameLoader.LoadedPlayerLoadout);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsMenuActionPerformed();
        CheckIsAbilityActionPerformed();
    }

    private void CheckIsAbilityActionPerformed()
    {
       if(GameStateManager.Instance.CurrentState == GameState.FreeRoam){
            if(input.ability){
                GameStateManager.Instance.ChangeState(GameState.AbilitiesScreen);
                // Assumi di avere un riferimento al tuo PlayerInput
                //playerInput.SwitchCurrentActionMap("UI");

                abilityScreenManager.Open();
            }
        }
        else if(GameStateManager.Instance.CurrentState == GameState.AbilitiesScreen){
            if(input.back){
                GameStateManager.Instance.ChangeState(GameState.FreeRoam);
                //playerInput.SwitchCurrentActionMap("Player");
                abilityScreenManager.Hide();
            }
        }
    }

    private void CheckIsMenuActionPerformed()
    {
        if(GameStateManager.Instance.CurrentState == GameState.FreeRoam){
            
            if(input.menuAction){
                Debug.Log("MENU Opened");
                Debug.Log("INVENTORYCOUNT PPLAYER: "+playerManager.GetInventory().itemList.Count);
                GameStateManager.Instance.ChangeState(GameState.MenuOpened);
                inventoryScreenManager.OpenInventoryScreen(playerManager);
            }
        }
        else if(GameStateManager.Instance.CurrentState == GameState.MenuOpened){
            if(input.back){
                Debug.Log("MENU Closed");
                GameStateManager.Instance.ChangeState(GameState.FreeRoam);
                inventoryScreenManager.CloseInventoryScreen();
            }
        }
    }
}
