using System.Collections.Generic;
using GLTFast.Schema;
using UnityEngine;

public class Item
{   
    public string tag;
    public string type;
    public string name;
    public string description;
    public List<PlayerModifier> modifiers;
    public string spritePath;

    public Item(){}

    public Item(string tag, string type, string name, string description, List<PlayerModifier> modifiers, string spritePath){
        this.tag = tag;
        this.type = type;
        this.name = name;
        this.description=description;
        this.modifiers = modifiers;
        this.spritePath = spritePath;
    }
}
