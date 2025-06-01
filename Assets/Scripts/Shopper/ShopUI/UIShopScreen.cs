using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopScreen : MonoBehaviour
{
    [SerializeField] private GameObject shopScreenUI;
    [SerializeField] private GameObject itemList;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private RectTransform itemListRect;
    [SerializeField] private ScrollRect itemScrollView;
    [SerializeField] private TextMeshProUGUI notifyText;

    //avrà il riferimento al prefab itemUI per poi gestirlo dinamicamente
    [SerializeField] private UIShopItem shopItemUI;

    private List<UIShopItem> inventorySlots;

    const int itemsInViewPort = 7;

    public void SetActive(bool val)
    {
        shopScreenUI.SetActive(val);
    }

    private void Awake()
    {
        //itemListRect = GetComponent<RectTransform>();
        inventorySlots = new List<UIShopItem>();
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
            var slotUIobj = Instantiate(shopItemUI, itemList.transform);
            slotUIobj.SetData(item, itemDescription, itemIcon);

            // Aggiungi il nuovo slot alla lista
            inventorySlots.Add(slotUIobj);
        }
    }

    public void Select(int selectedItem)
    {
        inventorySlots[selectedItem].Select();
        HandleScrolling(selectedItem);
    }

    private void HandleScrolling(int selectedItem)
    {
        //funzione utile a rendere lo scroll più smooth
        float scrollPos = Mathf.Clamp(selectedItem - itemsInViewPort / 2, 0, selectedItem) * inventorySlots[selectedItem].GetRectTransformHeight();
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);
    }

    public void Deselect(int deselectedItem)
    {
        inventorySlots[deselectedItem].Deselect();
    }

    public List<UIInventoryItem> GetInventorySlots()
    {
        return itemList.GetComponentsInChildren<UIInventoryItem>().ToList();
    }

    public void SetNotifyText(string text)
    {
        notifyText.text = text;
    }
}
