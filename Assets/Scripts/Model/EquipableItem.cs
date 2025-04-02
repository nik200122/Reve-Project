using System.Collections.Generic;
using UnityEngine;

public class EquipableItem : Item
{   
    public string equipableItemCategory;

    public EquipableItem(){}
    public EquipableItem(string tag, string name, string description, List<PlayerModifier> modifiers, string spritePath, string equipableItemCategory){
        this.tag = tag;
        this.name = name;
        this.description = description;
        this.modifiers = modifiers;
        this.spritePath = spritePath;
        this.equipableItemCategory = equipableItemCategory;
    }
    
    public override void UseItem(){
    }
}
