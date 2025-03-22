using System.IO;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    
    public WorldData LoadedWorldData { get; private set; }
    public NPCDataList LoadedNPCDataList { get; private set; }
    public Player LoadedPlayer { get; private set; }
    private const string playerStatsFile = "Assets/Resources/XML/playerStats.xml";
    private const string worldDataFile = "Assets/Resources/XML/worldInfo.xml";
    private const string npcDataFile = "Assets/Resources/XML/npcData.xml";

    public void LoadData(){
        LoadedPlayer = XMLHelper.LoadFromXml<Player>(playerStatsFile);
        Debug.Log("VALORE "+LoadedPlayer.GetStat("SAZIETA").GetStatTag());

        LoadedWorldData = XMLHelper.LoadFromXml<WorldData>(worldDataFile);
        
        // Caricamento dei dati NPC (più NPC in un file)
        LoadedNPCDataList = XMLHelper.LoadFromXml<NPCDataList>(npcDataFile);
        
    }
}