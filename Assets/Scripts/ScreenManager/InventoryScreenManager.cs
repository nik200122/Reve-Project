using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InventoryScreenManager : MonoBehaviour
{
    [SerializeField] private UIInventoryScreen inventoryScreenUI;
    //[SerializeField] private GameInput gameInput;
    [SerializeField] private InputHandler input;

    private Inventory inventory;
    private List<Item> itemsList;

    private int selectedItem = 0;
    private int previousSelection = 0;
    private bool isInventoryUpdated = false;

    public event Action<Item> OnItemSelected;
    public event Action OnBackAction;

    public void Update(){
        if(GameStateManager.Instance.CurrentState == GameState.MenuOpened){
            CheckScrollDownActionPerformed();
            CheckScrollUpActionPerformed();
        }
        // 🔹 Resetta il valore subito dopo l'uso
        input.scrollUpAction = false;
        input.scrollDownAction = false;
    }

    private void CheckScrollUpActionPerformed(){
        if(input.scrollUpAction){
            --selectedItem;
            UpdateItemSelection();
        }
    }

    private void CheckScrollDownActionPerformed(){
        
        if(input.scrollDownAction){
            ++selectedItem;
            UpdateItemSelection();
        }
    
    }

    public void OpenInventoryScreen(Inventory inventory){
        this.inventory = inventory;

        Debug.Log("INVENTORY COUNT: "+inventory.itemList.Count);

        //inventory.OnUpdateInventory += OnUpdateInventory;

        inventoryScreenUI.SetActive(true);
        
        if(!isInventoryUpdated){
            SetItemData();
            isInventoryUpdated = true;
        }

        UpdateItemSelection();
    }

    private void OnUpdateInventory()
    {
        isInventoryUpdated = false;
    }

    public void SetItemData(){
        //recoveryItemsList = inventory.GetRecoveryItems();
        inventoryScreenUI.SetData(inventory.itemList);
    }

    private void UpdateItemSelection(){
        //ci assicuriamo che non avvenga un outOfindex
        selectedItem = Mathf.Clamp(selectedItem, 0, inventory.itemList.Count - 1);
        previousSelection = Mathf.Clamp(previousSelection, 0, inventory.itemList.Count - 1);

        inventoryScreenUI.Deselect(previousSelection);
        inventoryScreenUI.Select(selectedItem);

        previousSelection = selectedItem;
    }


    public void CloseInventoryScreen(){
        // gameInput.OnScrollItemDownAction -= GameInput_OnScrollItemDownAction;
        // gameInput.OnScrollItemUpAction -= GameInput_OnScrollItemUpAction;
        // gameInput.OnSelectionButtonAction -= GameInput_OnSelectionButtonAction;
        // gameInput.OnBackAction -= GameInput_OnBackAction;
        
        //non ci disiscriviamo altrimenti non chiama mai OnUpdateInventory
        //inventory.OnUpdateInventory -= OnUpdateInventory;

        inventoryScreenUI.SetActive(false);
    }
}
