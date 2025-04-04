using System.Collections.Generic;
using System.Xml.Serialization;

public class Inventory
{
//L'attributo [XmlArrayItem("EquipableItem", typeof(EquipableItem))] dice al XmlSerializer:
// "Guarda, dentro questa lista ci saranno oggetti di tipo EquipableItem"
// "Quando li trovi nell'XML con il nome <EquipableItem>, deserializzali come EquipableItem"

    [XmlArrayItem("EquipableItem", typeof(EquipableItem))]
    [XmlArrayItem("ConsumableItem", typeof(ConsumableItem))]
    public List<Item> itemList;

    public Inventory(){}

    public Item GetItem(string itemTag){
        return itemList.Find(obj => obj.tag == itemTag);
    }
}
