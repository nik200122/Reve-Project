using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryScreen : MonoBehaviour
{
    [SerializeField] private GameObject inventoryScreenUI;
    [SerializeField] private GameObject itemList;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private RectTransform itemListRect;
    [SerializeField] private ScrollRect itemScrollView;
    [SerializeField] private TextMeshProUGUI notifyText;

    //avrà il riferimento al prefab itemUI per poi gestirlo dinamicamente
    [SerializeField] private UIInventoryItem inventoryItemUI;
    PlayerManager playerManager;

    private List<UIInventoryItem> inventorySlots;

    const int itemsInViewPort = 7;

    public void SetActive(bool val)
    {
        inventoryScreenUI.SetActive(val);
    }

    private void Awake()
    {
        //itemListRect = GetComponent<RectTransform>();
        inventorySlots = new List<UIInventoryItem>();
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    public void SetData(List<Item> items)
    {
        int i = 0;
        inventorySlots.Clear();
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        //per ogni elemento nell'inventario crea un gameobject sotto itemList uguale al prefab inventoryItemUI
        foreach (Item item in items)
        {
            i++;
            Debug.Log("COUNTER: " + i);
            var slotUIobj = Instantiate(inventoryItemUI, itemList.transform);
            slotUIobj.SetData(item, itemDescription, itemIcon);
            if (item is EquipableItem equipableItem && equipableItem.isEquipped)
                slotUIobj.SetHighlight(true);

            // Aggiungi il nuovo slot alla lista
            inventorySlots.Add(slotUIobj);
        }
    }

    public void Select(int selectedItem)
    {
        inventorySlots[selectedItem].Select();
        HandleScrolling(selectedItem);
    }

    public void SetHighlight(int selectedItem, bool isEquipped)
    {
        inventorySlots[selectedItem].SetHighlight(isEquipped);
    }

    private void HandleScrolling(int selectedItem)
    {
        //funzione utile a rendere lo scroll più smooth
        float scrollPos = Mathf.Clamp(selectedItem - itemsInViewPort / 2, 0, selectedItem) * inventorySlots[selectedItem].GetRectTransformHeight();
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);
    }

    public void SetNullData()
    {
        itemDescription.text = " ";
        itemIcon.sprite = null;
    }

    public void Deselect(int deselectedItem)
    {
        inventorySlots[deselectedItem].Deselect();
    }

    public List<UIInventoryItem> GetInventorySlots()
    {
        return itemList.GetComponentsInChildren<UIInventoryItem>().ToList();
    }

    public void SetNotifyText(string text){
        notifyText.text = text;

    }
}
