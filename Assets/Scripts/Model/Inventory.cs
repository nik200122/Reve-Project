using System.Collections.Generic;

public class Inventory
{
    public List<Item> itemList;

    public Inventory(){}

    public Item GetItem(string itemTag){
        return itemList.Find(obj => obj.tag == itemTag);
    }

    public void AddItem(Item item){
        itemList.Add(item);
    }
}
