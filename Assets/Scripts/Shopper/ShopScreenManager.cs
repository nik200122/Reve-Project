using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopScreenManager : MonoBehaviour
{
    [SerializeField] private UIShopScreen shopScreenUI;
    [SerializeField] private InputHandler input;

    private PlayerManager playerManager;
    GameStateManager gameStateManager;
    private Inventory inventory;
    List<Item> filteredItemList;

    //wrapper che contiene la lista di tutte le trigger-actions
    private AudioTriggerActionsWrapper wrapperTriggerActions;

    private int selectedItem = 0;
    private int previousSelection = 0;

    //var per capire che dati carivare tra equip e consumabili
    private bool isEquipableMode;
    private bool isInventoryUpdated = false;
    
    void Start(){
        playerManager = FindAnyObjectByType<PlayerManager>();
        gameStateManager = FindAnyObjectByType<GameStateManager>();

        foreach(var triggerActionConfig in wrapperTriggerActions.TriggerActions){
            IAudioAction action = ActionFactory.CreateAudioAction(triggerActionConfig.Action.type, triggerActionConfig.Action.Parameters);
            if(action != null){
                AudioTriggerActionManager.Instance.RegisterAction(this.gameObject, triggerActionConfig.Trigger, action);
            }
        }
    }

    public void Update(){
        if(GameStateManager.Instance.CurrentState == GameState.ShopScreen){
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
            AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuNavigation);
        }
    }

    private void CheckScrollDownActionPerformed(){
        if(input.scrollDownAction){
            ++selectedItem;
            UpdateItemSelection();
            input.scrollDownAction = false;
            AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuNavigation);
        }
    }

    private void CheckSelectionActionPerformed(){
        if(input.selectionPerformed){
            input.selectionPerformed = false;
            TryBuySelectedItem();
        }
    }

    private void TryBuySelectedItem(){
        Item itemToBuy = filteredItemList[selectedItem];
        if (!playerManager.CanAffordItem(itemToBuy.price)){
            AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnInvalidSelection);
            return;
        }
        AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuSelection);
        playerManager.GetInventory().AddItem(itemToBuy);
        Debug.Log("Comprato");
    }

    public void OpenShopScreen(Inventory shopperInventory){
        gameStateManager.ChangeState(GameState.ShopScreen);
        inventory = shopperInventory;
        //inventory.OnUpdateInventory += OnUpdateInventory;
        shopScreenUI.SetActive(true);
        
        if(!isInventoryUpdated){
            SetItemData();
            isInventoryUpdated = true;
        }
        UpdateItemSelection();
    }

    public void CloseShopScreen(){
        shopScreenUI.SetActive(false);
    }

    private void OnUpdateInventory(){
        isInventoryUpdated = false;
    }

    public void SetItemData(){
        filteredItemList = inventory.itemList
        .Where(item => isEquipableMode ? item is EquipableItem : item is ConsumableItem)
        .ToList();

        shopScreenUI.SetData(filteredItemList);
    }

    private void UpdateItemSelection(){
        if(filteredItemList.Count > 0){
            //ci assicuriamo che non avvenga un outOfindex
            selectedItem = Mathf.Clamp(selectedItem, 0, filteredItemList.Count - 1);
            previousSelection = Mathf.Clamp(previousSelection, 0, filteredItemList.Count - 1);

            shopScreenUI.Deselect(previousSelection);
            shopScreenUI.Select(selectedItem);

            previousSelection = selectedItem;
        }
    }

    public void SetTriggerActions(AudioTriggerActionsWrapper wrapperTriggerActions){
        this.wrapperTriggerActions = wrapperTriggerActions;
    }
}
