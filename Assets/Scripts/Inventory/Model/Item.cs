using System.Collections.Generic;
using GLTFast.Schema;
using UnityEngine;

public abstract class Item
{   
    public string tag;
    //public string type;
    public string name;
    public string description;
    public int count;
    public int price;
    public List<StatModifier> modifiers;
    public string spritePath;

    public Item(){}

    // public Item(string tag, string name, string description, List<PlayerModifier> modifiers, string spritePath){
    //     this.tag = tag;
    //     //this.type = type;
    //     this.name = name;
    //     this.description=description;
    //     this.modifiers = modifiers;
    //     this.spritePath = spritePath;
    // }
    
    public abstract bool UseItem(PlayerManager playerManager);
    // Metodo per clonare l'oggetto. Implementalo in base alle tue esigenze.
    public abstract Item Clone();
    
}
