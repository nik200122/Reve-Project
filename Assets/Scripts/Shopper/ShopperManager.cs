using System;
using UnityEngine;

public class ShopperManager : MonoBehaviour
{   
    private ShopScreenManager shopScreenManager;
    public static ShopperManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // shopPanel.SetActive(false); // Nascondi all'inizio
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        shopScreenManager = FindAnyObjectByType<ShopScreenManager>();
    }
    
    public void ProcessShopperInteraction(Inventory inventory){
        Debug.Log("CALLED");
        shopScreenManager.OpenShopScreen(inventory);
    }


    internal void CloseShopScreen()
    {
       shopScreenManager.CloseShopScreen();
    }
}
