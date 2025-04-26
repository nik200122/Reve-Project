using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{   
    public ConsumableItem(){}

    public ConsumableItem(string tag, string name, string description, int count, List<StatModifier> modifiers, string spritePath){
        this.tag = tag;
        this.name = name;
        this.description = description;
        this.count= count;
        this.modifiers = modifiers;
        this.spritePath = spritePath;
    }
    //per ora nessun check se quella statistica è al max. Esempio Hp sono già al massimo? Hai usato la pozione? L'hai sprecata...
    public override bool UseItem(PlayerManager playerManager){
        Debug.Log("USATO!");
        foreach(var modifier in this.modifiers)
            playerManager.ApplyModifier(modifier);
        this.count--;
        
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
