using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private GameObject equippedIndicator;
    private TextMeshProUGUI descriptionText;
    private Image icon;
    private RectTransform rectTransform;

    Item item;

    private void Awake(){
        rectTransform = GetComponent<RectTransform>();
    }

    public void Deselect()
    {
        nameText.color = Color.black;
    }

    public void Select()
    {   
        descriptionText.text = item.description;
        icon.sprite = Resources.Load<Sprite>(item.spritePath);
        if (item.spritePath == null)
{
    Debug.LogError("Sprite not found at path: " + item.spritePath);
}
        nameText.color = Color.blue;
    }

    public void SetHighlight(bool isEquipped){
            equippedIndicator.SetActive(isEquipped);
    }

    public void SetData(Item item, TextMeshProUGUI descriptionText, Image icon){
        this.item = item;
        this.descriptionText = descriptionText;
        this.icon = icon;
        this.countText.text = item.count.ToString();

        nameText.text = item.name;

        // if(item is RecoveryItem recoveryItem)
        //     countText.text = $"X{recoveryItem.GetAmount()}";
    }

    public float GetRectTransformHeight(){
        return rectTransform.rect.height;
    }
}
