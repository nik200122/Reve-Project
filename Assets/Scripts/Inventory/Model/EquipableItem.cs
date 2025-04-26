using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipableItem : Item
{   
    public EquipableItemCategory equipableItemCategory;
    public bool isEquipped;

    public EquipableItem(){}
    public EquipableItem(string tag, string name, string description, int count, List<StatModifier> modifiers, string spritePath, EquipableItemCategory equipableItemCategory, bool isEquipped){
        this.tag = tag;
        this.name = name;
        this.description = description;
        this.count= count;
        this.modifiers = modifiers;
        this.spritePath = spritePath;
        this.equipableItemCategory = equipableItemCategory;
        this.isEquipped = isEquipped;
    }
    
    public override bool UseItem(PlayerManager playerManager) {
        if(isEquipped){
            playerManager.Unequip(this);
            isEquipped = false;
            return false;
        }

        // Trova la regola associata alla categoria dell'oggetto
        var rule = playerManager.GetEquipmentRuleList().FirstOrDefault(r => r.equipableItemCategory == this.equipableItemCategory);
        // Conta quanti oggetti di questa categoria sono già equipaggiati
        // Conta quanti oggetti di questa categoria sono già nell'inventario
        int currentEquipCount = playerManager.GetInventory().itemList.Count(e => e is EquipableItem equipableItem && equipableItem.equipableItemCategory == this.equipableItemCategory && equipableItem.isEquipped);

        // Verifica se il numero massimo di oggetti per quella categoria è stato raggiunto
        if (currentEquipCount >= rule.maxEquipableNumber){
            Debug.Log($"Limite massimo di {rule.maxEquipableNumber} oggetti per categoria {this.equipableItemCategory} raggiunto.");
            return false;  // Limite raggiunto, non si può equipaggiare
        }

        //altrimenti lo equipaggi
        playerManager.Equip(this);
        isEquipped = true;
        return true;
    }
    public override Item Clone()
    {
        return new ConsumableItem
        {
            tag = this.tag,
            name = this.name,
            description = this.description,
            count = this.count,
            modifiers = new List<StatModifier>(this.modifiers),
            spritePath = this.spritePath,
        };
    }
}
