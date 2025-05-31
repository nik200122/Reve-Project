using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private UIHUD hud;
    [SerializeField] private PlayerManager playerManager;
    
    private Player player;

    private void Start()
    {
        player = playerManager.GetPlayerModel();
        SetData();
    }

    private void Update()
    {
        hud.UpdateData(player);
    }

    public void SetData()
    {
        hud.SetData(player);
    }
}
