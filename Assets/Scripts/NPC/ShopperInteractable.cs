using UnityEngine;

public class ShopperInteractable : MonoBehaviour, IInteractable
{
    [Header("Shopper Configuration")]
    [SerializeField] private string shopperId; // ID univoco, deve corrispondere a un ID in ShopperInventories.xml
    [SerializeField] private string defaultInteractText = "Browse Wares";
    [SerializeField] private string defaultShopperName = "Merchant";

    // Riferimento al manager
    [SerializeField ]private ShopperManager shopperManager; 

    private Inventory shopperInventory; // L'inventario specifico di questo shopper
    private string currentInteractText;
    private string currentShopperName;

    private void Awake()
    {
        /*if (shopperManager == null)
        {
            shopperManager = ShopperManager.Instance;
            if (shopperManager == null)
            {
                Debug.LogError($"[ShopperInteractable] ShopScreenManager non trovato per {gameObject.name}. Assicurati che esista nella scena e sia un Singleton, o assegnalo manualmente nell'Inspector.");
            }
        }*/
    }

    private void Start()
    {

        // Recupera i dati dell'inventario (che includono l'oggetto Inventory)
        Inventory inventory = ShopperDataManager.Instance.GetShopperInventory(shopperId);

        if (inventory != null)
        {
            shopperInventory = inventory;
            currentShopperName = defaultShopperName; 
            currentInteractText = defaultInteractText; 

            Debug.Log($"[ShopperInteractable] Dati caricati per shopper: {shopperId}, Nome: {currentShopperName}. Inventario con {shopperInventory.itemList.Count} tipi di item.");
        }
        else
        {
            Debug.LogError($"[ShopperInteractable] Dati dell'inventario non trovati o stock nullo per shopperId: {shopperId} in {gameObject.name}. Controlla xml e l'ID.", this);
            shopperInventory = new Inventory(); // Inventario vuoto per evitare null ref, ma lo shop sarà vuoto
            currentInteractText = "Out of Stock";
            currentShopperName = "Unavailable Merchant";
        }
    }


    public void Interact(Transform interactorTransform)
    {
            if (shopperManager == null)
            {
                Debug.LogError($"[ShopperInteractable] shopScreenManager non è assegnato per {gameObject.name}. Impossibile aprire il negozio.");
                return;
            }

            if (shopperInventory != null)
            {
                Debug.Log($"[ShopperInteractable] Interazione con {currentShopperName}. Apertura negozio con {shopperInventory.itemList.Count} tipi di item.");
                GameStateManager.Instance.ChangeState(GameState.Interaction);
                shopperManager.ProcessShopperInteraction(shopperInventory);

            }
            else
            {
                Debug.LogWarning($"[ShopperInteractable] {currentShopperName} ({shopperId}) non ha un inventario caricato da mostrare.");
            }
        
        
    }

    public string GetInteractText()
    {
        return currentInteractText;
    }

    public string GetName()
    {
        return currentShopperName;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void TerminateInteract()
    {
        Debug.Log($"[ShopperInteractable] Terminazione interazione con {currentShopperName}. Chiusura negozio.");
        shopperManager.CloseShopScreen();
        
    }

    // Metodo per ottenere l'ID, utile per debug o altre logiche
    public string GetShopperId()
    {
        return shopperId;
    }
}