using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameLoader : MonoBehaviour
{   
    private const string playerStatsFile = "Assets/Resources/XML/playerStats.xml";
    private const string playerInventoryDataFile = "Assets/Resources/XML/playerInventoryData.xml";
    private const string itemDataFile = "Assets/Resources/XML/itemsData.xml";
    private const string worldDataFile = "Assets/Resources/XML/worldInfo.xml";
    private const string npcDataFile = "Assets/Resources/XML/npcData.xml";
    
    public WorldData LoadedWorldData { get; private set; }
    public NPCDataList LoadedNPCDataList { get; private set; }
    public Player LoadedPlayerStats { get; private set; }
    public List<Item> LoadedItems { get; private set; }
    public Inventory LoadedPlayerInventory { get; private set; }

    public void LoadData(){
        LoadPlayerData();

        LoadedWorldData = XMLHelper.LoadFromXml<WorldData>(worldDataFile);
        
        // Caricamento dei dati NPC (più NPC in un file)
        LoadedNPCDataList = XMLHelper.LoadFromXml<NPCDataList>(npcDataFile);
        LoadAssets();
    }

    private void LoadPlayerData(){
        LoadedPlayerStats = XMLHelper.LoadFromXml<Player>(playerStatsFile);
        Debug.Log("VALORE "+LoadedPlayerStats.GetStat("Hp").GetStatTag());
        LoadedPlayerInventory = XMLHelper.LoadFromXml<Inventory>(playerInventoryDataFile);
        Debug.Log(""+LoadedPlayerInventory.GetItem("HpPotion01").name);
    }

    private void LoadAssets(){
        LoadedItems = XMLHelper.LoadFromXml<List<Item>>(itemDataFile);
        //Debug.Log("NOME ITEM: "+LoadedItems[0].name);
    }
}