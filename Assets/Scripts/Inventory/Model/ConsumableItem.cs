using UnityEngine;

public class ConsumableItem : Item
{
    public override bool UseItem(PlayerManager manager){
        return true;
    }
}
