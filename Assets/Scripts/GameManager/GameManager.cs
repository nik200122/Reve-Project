using UnityEngine;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private InputHandler input;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private DeepSeek deepSeek;
    private GameLoader gameLoader;
   // private bool isMenuActionPerformed;
    //private bool menuOpened = false;

    private void Awake()
    {
        gameLoader = gameObject.GetComponent<GameLoader>();
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {    
        gameLoader.LoadData();
        deepSeek.SetWorldInfo(gameLoader.LoadedWorldData);
        playerManager.SetPlayerModel(gameLoader.LoadedPlayerStats);
        playerManager.SetInventory(gameLoader.LoadedPlayerInventory);
        playerManager.SetPlayerAttackData(gameLoader.LoadedPlayerAttackDataList);
        playerCombat.SetComboRules(gameLoader.LoadedComboRules);
        playerCombat.SetAttackDataList(gameLoader.LoadedPlayerAttackDataList);

        //PER DEBUG
        playerManager.UseItem("HpPotion01");
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsMenuActionPerformed();
    }

    private void CheckIsMenuActionPerformed()
    {
        if(GameStateManager.Instance.CurrentState == GameState.FreeRoam){
            if(input.menuAction){
                Debug.Log("MENU OPENED");
                GameStateManager.Instance.ChangeState(GameState.MenuOpened);
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
