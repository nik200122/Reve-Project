using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    private Player player;
    private Inventory inventory;
    private AttackDataList attackDataList;
    //[SerializeField] private InputHandler input;

    public Player GetPlayerModel(){
        return player;
    }

    public void SetPlayerModel(Player loadedPlayer){
       player=loadedPlayer;
    }

    public Inventory GetInventory(){
        return inventory;
    }

    public void SetInventory(Inventory loadedInventory){
        inventory = loadedInventory;
        //Debug.Log(""+inventory.GetItem("HpPotion01").name);
    }
    public void SetPlayerAttackData(AttackDataList attackDataList){
        this.attackDataList = attackDataList;
        foreach(AttackData attackData in attackDataList.Attacks){
            Debug.Log(""+attackData.ToString());
        }
        Debug.Log(""+inventory.GetItem("HpPotion01").name);
    }

    public void UseItem(string itemTag){
        Debug.Log("USE ITEM "+inventory.GetItem(itemTag).modifiers[0].ToString());
        foreach (PlayerModifier modifier in inventory.GetItem(itemTag).modifiers)
            ApplyModifier(modifier);
    }

    public void ApplyModifier(PlayerModifier modifier){
        Debug.Log("MODIFIER CHIAMATO");
        player.SetStat(modifier.targetStat,player.GetStat(modifier.targetStat).currentValue + modifier.value);
        Debug.Log("OGGETTO USATO CORRETTAMENTE" +player.GetStat(modifier.targetStat).currentValue);
    }
}
