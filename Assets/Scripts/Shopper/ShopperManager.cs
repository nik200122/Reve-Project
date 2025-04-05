using UnityEngine;

public class ShopperManager : MonoBehaviour, IInteractable
{   
    private Inventory inventory;
    private ShopScreenManager shopScreenManager;

    private void Start(){
        shopScreenManager = FindAnyObjectByType<ShopScreenManager>();
    }

    public void SetInventory(Inventory loadedInventory){
        inventory = loadedInventory;
        //Debug.Log("CONTEGGIO: "+inventory.itemList.Count);
    }

    public string GetInteractText(){
        return "";
    }

    public Transform GetTransform(){
        return transform;
    }

    public void Interact(Transform interactorTransform){
        Debug.Log("CALLED");
        shopScreenManager.OpenShopScreen();
    }

    public Inventory GetInventory(){
        return inventory;
    }

    public void TerminateInteract(){
      Debug.Log("CHIAMATO");
      shopScreenManager.CloseShopScreen();
    }
}
