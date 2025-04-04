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

    private PlayerManager playerManager;
    private Inventory inventory;

    private int selectedItem = 0;
    private int previousSelection = 0;
    private bool isInventoryUpdated = false;

    void Start(){
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    public void Update(){
        if(GameStateManager.Instance.CurrentState == GameState.MenuOpened){
            CheckScrollDownActionPerformed();
            CheckScrollUpActionPerformed();
            CheckSelectionActionPerformed();
        }
    }

    private void CheckScrollUpActionPerformed(){
        //Debug.Log(input.scrollUpAction);
        if(input.scrollUpAction){
            --selectedItem;
            UpdateItemSelection();
            input.scrollUpAction = false;
        }
    }

    private void CheckScrollDownActionPerformed(){
        if(input.scrollDownAction){
            ++selectedItem;
            UpdateItemSelection();
            input.scrollDownAction = false;
        }
    }

    private void CheckSelectionActionPerformed(){
        bool isUsed;
        if(input.selectionPerformed){
            isUsed = inventory.itemList[selectedItem].UseItem(playerManager);
            // if(inventory.itemList[selectedItem].UseItem(playerManager)){
            //     Debug.Log("equipaggiato");
            // };
            if(inventory.itemList[selectedItem] is EquipableItem){
                inventoryScreenUI.SetHighlight(selectedItem, isUsed);
            }
            input.selectionPerformed = false;
        }
    }

    public void OpenInventoryScreen(PlayerManager playerManager){
        inventory = playerManager.GetInventory();

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
