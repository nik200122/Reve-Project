using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryScreenManager : MonoBehaviour
{
    [SerializeField] private UIInventoryScreen inventoryScreenUI;
    [SerializeField] private InputHandler input;

    private PlayerManager playerManager;
    private Inventory inventory;
    List<Item> filteredItemList;

    private int selectedItem = 0;
    private int previousSelection = 0;

    //var per capire che dati carivare tra equip e consumabili
    private bool isEquipableMode;
    private bool isInventoryUpdated = false;
    
    void Start(){
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    public void Update(){
        if(GameStateManager.Instance.CurrentState == GameState.MenuOpened){
            CheckScrollDownActionPerformed();
            CheckScrollUpActionPerformed();
            CheckScrollRightActionPerformed();
            CheckScrollLeftActionPerformed();
            CheckSelectionActionPerformed();
        }
    }

    private void CheckScrollLeftActionPerformed(){
        if(input.scrollLeftAction){
            isEquipableMode = !isEquipableMode;
            input.scrollLeftAction = false;
            SetItemData();
            selectedItem = 0;
            previousSelection = 0;
            UpdateItemSelection();
        }
    }

    private void CheckScrollRightActionPerformed(){
        if(input.scrollRightAction){
            isEquipableMode = !isEquipableMode;
            input.scrollRightAction = false;
            SetItemData();
            selectedItem = 0;
            previousSelection = 0;
            UpdateItemSelection();
        }
    }

    private void CheckScrollUpActionPerformed(){
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

    private void OnUpdateInventory(){
        isInventoryUpdated = false;
    }

    public void SetItemData(){
        if (isEquipableMode){
            filteredItemList = inventory.itemList
                .Where(item => item is EquipableItem)
                .ToList();
        }else{
            filteredItemList = inventory.itemList
                .Where(item => item is ConsumableItem)
                .ToList();
        }

        inventoryScreenUI.SetData(filteredItemList);
        //inventoryScreenUI.SetData(inventory.itemList);
    }

     private void UpdateItemSelection(){
        //ci assicuriamo che non avvenga un outOfindex
        selectedItem = Mathf.Clamp(selectedItem, 0, filteredItemList.Count - 1);
        previousSelection = Mathf.Clamp(previousSelection, 0, filteredItemList.Count - 1);

        inventoryScreenUI.Deselect(previousSelection);
        inventoryScreenUI.Select(selectedItem);

        previousSelection = selectedItem;
    }

    public void CloseInventoryScreen(){
        //non ci disiscriviamo altrimenti non chiama mai OnUpdateInventory
        //inventory.OnUpdateInventory -= OnUpdateInventory;

        inventoryScreenUI.SetActive(false);
    }
}
