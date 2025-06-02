using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject startMenuUI;
    //per ora un solo shopper
    [SerializeField] private ShopperManager shopperManager;
    private GameLoader gameLoader;
    [SerializeField] private AttackSelectionManager attackSelectionManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {   
        // Sblocca e mostra il cursore
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Logga quando la finestra prende/perde il focus
        Application.focusChanged += (bool hasFocus) =>
        {
            Debug.Log("Focus: " + hasFocus);
        };
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
        attackSelectionManager.InitializePlayerLoadout(gameLoader.LoadedPlayerLoadout);
        attackSelectionManager.SetComboRules(gameLoader.LoadedComboRules);
        attackSelectionManager.InitializeAbilityList(gameLoader.LoadedAbilityList);
        attackSelectionManager.InitializeAttackData(gameLoader.LoadedAttackDataList);
        shopperManager.SetInventory(gameLoader.LoadedShopper01Inventory);

        SetEnemyData();

        battleCalculatorManager.SetDamageFormula(gameLoader.LoadedDamageFormula);
        globalRulesManager.SetDamageApplicationRule(gameLoader.LoadedDamageApplicationRule);
        globalRulesManager.SetDefeatRule(gameLoader.LoadedDefeatRule);
        globalRulesManager.SetCurrencyRule(gameLoader.LoadedCurrencyRule);

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
    private void Start()
    {
        startMenuUI = GameObject.Find("StartMenuUI");
        foreach (var triggerActionConfig in wrapperTriggerActions.TriggerActions)
        {
            IAudioAction action = ActionFactory.CreateAudioAction(triggerActionConfig.Action.type, triggerActionConfig.Action.Parameters);
            if (action != null)
            {
                AudioTriggerActionManager.Instance.RegisterAction(this.gameObject, triggerActionConfig.Trigger, action);
            }
        }
        AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.onGameState);
    }

    // Update is called once per frame
    void Update()
    {
        // é gestito cosí per pigrizia
        if (GameStateManager.Instance.CurrentState == GameState.StartMenu)
        {
            if (input.startGame)
            {
                startMenuUI.SetActive(false);
                //input.gameObject.SetActive(true);
                GameStateManager.Instance.ChangeState(GameState.FreeRoam);
            }
        }
        else
        {
            CheckIsMenuActionPerformed();
            CheckIsAbilityActionPerformed();
            CheckGameOver();
        }
    }

    private void CheckIsAbilityActionPerformed()
    {
        if (GameStateManager.Instance.CurrentState == GameState.FreeRoam)
        {
            if (input.ability)
            {
                GameStateManager.Instance.ChangeState(GameState.AbilitiesScreen);
                // Assumi di avere un riferimento al tuo PlayerInput
                //playerInput.SwitchCurrentActionMap("UI");

                abilityScreenManager.Open();
            }
        }
        else if (GameStateManager.Instance.CurrentState == GameState.AbilitiesScreen)
        {
            if (input.back)
            {
                GameStateManager.Instance.ChangeState(GameState.FreeRoam);
                //playerInput.SwitchCurrentActionMap("Player");
                abilityScreenManager.Hide();
            }
        }
    }

    private void CheckIsMenuActionPerformed()
    {
        if (GameStateManager.Instance.CurrentState == GameState.FreeRoam)
        {

            if (input.menuAction)
            {
                Debug.Log("MENU Opened");
                Debug.Log("INVENTORYCOUNT PLAYER: " + playerManager.GetInventory().itemList.Count);
                GameStateManager.Instance.ChangeState(GameState.MenuOpened);
                inventoryScreenManager.OpenInventoryScreen(playerManager);
            }
        }
        else if (GameStateManager.Instance.CurrentState == GameState.MenuOpened)
        {
            if (input.back)
            {
                Debug.Log("MENU Closed");
                GameStateManager.Instance.ChangeState(GameState.FreeRoam);
                inventoryScreenManager.CloseInventoryScreen();
            }
        }
    }

    public void SetTriggerActions(AudioTriggerActionsWrapper wrapperTriggerActions)
    {
        this.wrapperTriggerActions = wrapperTriggerActions;
    }

    private bool gameOver;
    private void CheckGameOver()
    {
        if (playerManager.CheckIsDead() && !gameOver)
        {
            gameOver = true;
            RestartDemo();
        }
    }

    private void RestartDemo()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //startMenuUI = GameObject.Find("StartMenuUI");
    }

    private void SetEnemyData(){
        //si può sicuramente trovare un modo più pulito di fare questo
        enemyManagerList[0].SetEnemyModel(gameLoader.LoadedEnemy01Model);
        enemyManagerList[0].GetComponent<EnemyAIController>().SetTriggerActions(gameLoader.LoadedOnHitAudioData);
        enemyManagerList[1].SetEnemyModel(gameLoader.LoadedEnemy02Model);
        enemyManagerList[1].GetComponent<EnemyAIController>().SetTriggerActions(gameLoader.LoadedOnHitAudioData);
        enemyManagerList[2].SetEnemyModel(gameLoader.LoadedEnemy03Model);
        enemyManagerList[2].GetComponent<EnemyAIController>().SetTriggerActions(gameLoader.LoadedOnHitAudioData);
        enemyManagerList[3].SetEnemyModel(gameLoader.LoadedEnemy04Model);
        enemyManagerList[3].GetComponent<EnemyAIController>().SetTriggerActions(gameLoader.LoadedOnHitAudioData);
        enemyManagerList[4].SetEnemyModel(gameLoader.LoadedEnemy05Model);
        enemyManagerList[4].GetComponent<EnemyAIController>().SetTriggerActions(gameLoader.LoadedOnHitAudioData);
        enemyManagerList[5].SetEnemyModel(gameLoader.LoadedEnemy06Model);
        enemyManagerList[5].GetComponent<EnemyAIController>().SetTriggerActions(gameLoader.LoadedOnHitAudioData);
        enemyManagerList[6].SetEnemyModel(gameLoader.LoadedEnemy07Model);
        enemyManagerList[6].GetComponent<EnemyAIController>().SetTriggerActions(gameLoader.LoadedOnHitAudioData);
    }
}
