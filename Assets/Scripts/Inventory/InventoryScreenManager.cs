using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryScreenManager : MonoBehaviour
{
    [SerializeField] private UIInventoryScreen inventoryScreenUI;
    [SerializeField] private InputHandler input;
    
    //wrapper che contiene la lista di tutte le trigger-actions
    private AudioTriggerActionsWrapper wrapperTriggerActions;
    private PlayerManager playerManager;
    private Inventory inventory;
    List<Item> filteredItemList;

    private int selectedItem = 0;
    private int previousSelection = 0;

    //var per capire che dati carivare tra equip e consumabili
    private bool isEquipableMode;
    
    void Start(){
        playerManager = FindAnyObjectByType<PlayerManager>();
        inventoryScreenUI.SetNotifyText(" ");
        foreach (var triggerActionConfig in wrapperTriggerActions.TriggerActions)
        {
            IAudioAction action = ActionFactory.CreateAudioAction(triggerActionConfig.Action.type, triggerActionConfig.Action.Parameters);
            if (action != null)
            {
                AudioTriggerActionManager.Instance.RegisterAction(this.gameObject, triggerActionConfig.Trigger, action);
            }
        }
    }

    public void Update(){
        if (GameStateManager.Instance.CurrentState == GameState.MenuOpened){
            CheckScrollDownActionPerformed();
            CheckScrollUpActionPerformed();
            CheckScrollRightActionPerformed();
            CheckScrollLeftActionPerformed();
            CheckSelectionActionPerformed();
            CheckEmptyInventory();
        }
    }

    private void CheckEmptyInventory(){
        if (filteredItemList.Count == 0){
            inventoryScreenUI.SetNullData();
            if (!isEquipableMode)
                inventoryScreenUI.SetNotifyText("you don't have any consumable items");
            else
                inventoryScreenUI.SetNotifyText("you don't have any equippable items");
        }
    }

    private void CheckScrollLeftActionPerformed(){
        if(input.scrollLeftAction){
            isEquipableMode = !isEquipableMode;
            input.scrollLeftAction = false;
            SetItemData();
            inventoryScreenUI.SetNotifyText(" ");
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
            inventoryScreenUI.SetNotifyText(" ");
            selectedItem = 0;
            previousSelection = 0;
            UpdateItemSelection();
        }
    }

    private void CheckScrollUpActionPerformed(){
        if(input.scrollUpAction && filteredItemList.Count > 0){
            --selectedItem;
            inventoryScreenUI.SetNotifyText(" ");
            UpdateItemSelection();
            input.scrollUpAction = false;
            AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuNavigation);
        }
    }

    private void CheckScrollDownActionPerformed(){
        if(input.scrollDownAction && filteredItemList.Count > 0){
            ++selectedItem;
            UpdateItemSelection();
            inventoryScreenUI.SetNotifyText(" ");
            input.scrollDownAction = false;
            AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuNavigation);
        }
    }

    private void CheckSelectionActionPerformed(){
        bool isUsed;
        if(input.selectionPerformed && filteredItemList.Count > 0){

            if (filteredItemList[selectedItem] is EquipableItem item){
                if (item.isEquipped){
                    //UseItem ijn questo caso disequipaggia oggetto
                    isUsed = filteredItemList[selectedItem].UseItem(playerManager);
                    inventoryScreenUI.SetHighlight(selectedItem, isUsed);
                }else{
                    //UseItem ijn questo caso disequipaggia oggetto
                    isUsed = filteredItemList[selectedItem].UseItem(playerManager);
                    if (isUsed){
                        AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuSelection);
                        inventoryScreenUI.SetHighlight(selectedItem, isUsed);
                    }else{
                        inventoryScreenUI.SetNotifyText("You have reached the maximum number of equipped items for this category: " + item.equipableItemCategory);
                        AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnInvalidSelection);
                    }
                }
            }else{
                filteredItemList[selectedItem].UseItem(playerManager);
                AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuSelection);
                if (filteredItemList[selectedItem].count == 0)
                    inventory.itemList.Remove(filteredItemList[selectedItem]);
                SetItemData();
                UpdateItemSelection();
            }
            input.selectionPerformed = false;
        }
    }

    public void OpenInventoryScreen(PlayerManager playerManager){
        inventory = playerManager.GetInventory();
        inventoryScreenUI.SetActive(true);
        
        SetItemData();
        UpdateItemSelection();
    }

    public void CloseInventoryScreen(){
        inventoryScreenUI.SetActive(false);
    }

    public void SetItemData(){
        filteredItemList = inventory.itemList
        .Where(item => isEquipableMode ? item is EquipableItem : item is ConsumableItem)
        .ToList();

        inventoryScreenUI.SetData(filteredItemList);
    }

    private void UpdateItemSelection(){
        if(filteredItemList.Count > 0){
            //ci assicuriamo che non avvenga un outOfindex
            selectedItem = Mathf.Clamp(selectedItem, 0, filteredItemList.Count - 1);
            previousSelection = Mathf.Clamp(previousSelection, 0, filteredItemList.Count - 1);

            inventoryScreenUI.Deselect(previousSelection);
            inventoryScreenUI.Select(selectedItem);

            previousSelection = selectedItem;
        }
    }

    public void SetTriggerActions(AudioTriggerActionsWrapper wrapperTriggerActions){
        this.wrapperTriggerActions = wrapperTriggerActions;
    }
}
