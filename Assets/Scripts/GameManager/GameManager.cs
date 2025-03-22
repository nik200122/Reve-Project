using UnityEngine;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private InputHandler input;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private DeepSeek deepSeek;
    private GameLoader gameLoader;
    private bool isMenuActionPerformed;

    private void Awake()
    {
        gameLoader = gameObject.GetComponent<GameLoader>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {    
        gameLoader.LoadData();
        deepSeek.SetNPCDataList(gameLoader.LoadedNPCDataList);
        deepSeek.SetWorldInfo(gameLoader.LoadedWorldData);
        playerManager.SetPlayerModel(gameLoader.LoadedPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsMenuActionPerformed();
    }

    private void CheckIsMenuActionPerformed()
    {   
        if(input.menuAction && !isMenuActionPerformed){
            Debug.Log("MENU OPENED");
            isMenuActionPerformed = true;
        }
        if(!input.menuAction){
            isMenuActionPerformed = false;
        }
    }
}
