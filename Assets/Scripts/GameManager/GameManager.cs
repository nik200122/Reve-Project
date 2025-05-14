using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private InputHandler input;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private AbilityScreenManager abilityScreenManager;
    [SerializeField] private LLMManager llmManager;
    [SerializeField] private InventoryScreenManager inventoryScreenManager;
    [SerializeField] private ShopScreenManager shopScreenManager;
    [SerializeField] private BattleCalculatorManager battleCalculatorManager;
    [SerializeField] private GlobalRulesManager globalRulesManager;

    //per ora un solo nemico
    [SerializeField] private List<EnemyManager> enemyManagerList;

    //per ora un solo shopper
    [SerializeField] private ShopperManager shopperManager;
    private GameLoader gameLoader;
   // private bool isMenuActionPerformed;
    //private bool menuOpened = false;

    // private void Awake()
    // {
    //     gameLoader = gameObject.GetComponent<GameLoader>();
    //     playerInput = input.gameObject.GetComponent<PlayerInput>();
        
    // }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake(){    
        gameLoader = gameObject.GetComponent<GameLoader>();

        gameLoader.LoadData();
        llmManager.SetWorldInfo(gameLoader.LoadedWorldData);
        playerManager.SetPlayerModel(gameLoader.LoadedPlayerModel);
        playerManager.SetInventory(gameLoader.LoadedPlayerInventory);
        playerManager.SetEquipmentRuleList(gameLoader.LoadedEquipmentRuleList);
        playerManager.SetAbilitiesRules(gameLoader.LoadedAbilitiesRules);
        playerManager.SetAbilityList(gameLoader.LoadedAbilityList);
        playerManager.SetPlayerLoadout(gameLoader.LoadedPlayerLoadout);
        playerManager.InitializeDictionary();
        playerCombat.SetPlayerLoadout(gameLoader.LoadedPlayerLoadout);
        playerCombat.SetComboRules(gameLoader.LoadedComboRules);
        playerCombat.SetAbilityList(gameLoader.LoadedAbilityList);
        playerCombat.SetAttackDataList(gameLoader.LoadedAttackDataList);
        shopperManager.SetInventory(gameLoader.LoadedShopper01Inventory);

        //si può sicuramente trovare un modo più pulito di fare questo
        enemyManagerList[0].SetEnemyModel(gameLoader.LoadedEnemy01Model);
        enemyManagerList[0].GetComponent<EnemyAIController>().SetTriggerActions(gameLoader.LoadedOnHitAudioData);
        enemyManagerList[1].SetEnemyModel(gameLoader.LoadedEnemy02Model);
        enemyManagerList[1].GetComponent<EnemyAIController>().SetTriggerActions(gameLoader.LoadedOnHitAudioData);

        battleCalculatorManager.SetDamageFormula(gameLoader.LoadedDamageFormula);
        globalRulesManager.SetDamageApplicationRule(gameLoader.LoadedDamageApplicationRule);
        globalRulesManager.SetDefeatRule(gameLoader.LoadedDefeatRule);

        // Inizializza l'AbilityScreenManager con la lista globale delle abilità
        abilityScreenManager.Initialize(gameLoader.LoadedAbilityList, gameLoader.LoadedPlayerLoadout);

        inventoryScreenManager.SetTriggerActions(gameLoader.LoadedMenuAudioData);
        abilityScreenManager.SetTriggerActions(gameLoader.LoadedMenuAudioData);
        shopScreenManager.SetTriggerActions(gameLoader.LoadedMenuAudioData);
        playerCombat.SetTriggerActions(gameLoader.LoadedOnHitAudioData);

        SetTriggerActions(gameLoader.LoadedGameMusicData);
    }

    //wrapper che contiene la lista di tutte le trigger-actions
    private AudioTriggerActionsWrapper wrapperTriggerActions;
    private void Start(){
        foreach(var triggerActionConfig in wrapperTriggerActions.TriggerActions){
            IAudioAction action = ActionFactory.CreateAudioAction(triggerActionConfig.Action.type, triggerActionConfig.Action.Parameters);
            if(action != null){
                AudioTriggerActionManager.Instance.RegisterAction(this.gameObject, triggerActionConfig.Trigger, action);
            }
        }

        AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.onGameState);
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

    public void SetTriggerActions(AudioTriggerActionsWrapper wrapperTriggerActions){
        this.wrapperTriggerActions = wrapperTriggerActions;
    }
}
