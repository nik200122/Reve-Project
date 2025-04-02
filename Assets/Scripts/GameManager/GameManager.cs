using System;
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
        playerCombat.SetPlayerLoadout(gameLoader.LoadedPlayerLoadout);
        playerCombat.SetComboRules(gameLoader.LoadedComboRules);
        playerCombat.SetAbilityList(gameLoader.LoadedAbilityList);
        playerCombat.SetAttackDataList(gameLoader.LoadedAttackDataList);

        // Inizializza l'AbilityScreenManager con la lista globale delle abilità
        abilityScreenManager.Initialize(gameLoader.LoadedAbilityList);

        //PER DEBUG
        //playerManager.UseItem("HpPotion01");
        
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
                playerInput.SwitchCurrentActionMap("UI");

                abilityScreenManager.Show(gameLoader.LoadedPlayerLoadout);
            }
        }
        else if(GameStateManager.Instance.CurrentState == GameState.AbilitiesScreen){
            if(input.back){
                GameStateManager.Instance.ChangeState(GameState.FreeRoam);
                playerInput.SwitchCurrentActionMap("Gameplay");
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
                inventoryScreenManager.OpenInventoryScreen(playerManager.GetInventory());
            }
        }
        else if(GameStateManager.Instance.CurrentState == GameState.MenuOpened){
            if(input.back){
                Debug.Log("MENU Closed");
                GameStateManager.Instance.ChangeState(GameState.FreeRoam);
            }
        }
        
        
    }
}
