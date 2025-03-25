using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class NPCDataManager : MonoBehaviour
{
    public static NPCDataManager Instance { get; private set; }

    // Percorso al file XML (può essere modificato in base alle tue esigenze)
    public string npcDataFile = "Assets/Resources/XML/npcData.xml";

    private Dictionary<string, NPCData> npcDataDictionary = new Dictionary<string, NPCData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadNPCData();
    }

    private void LoadNPCData()
    {
        NPCDataList dataList = XMLHelper.LoadFromXml<NPCDataList>(npcDataFile);
        if (dataList != null && dataList.npcs != null)
        {
            foreach (NPCData npc in dataList.npcs)
            {
                if (!npcDataDictionary.ContainsKey(npc.id))
                    npcDataDictionary.Add(npc.id, npc);
            }
        }
        else
        {
            Debug.LogError("Errore nel caricamento dei dati NPC.");
        }
    }

    public NPCData GetNPCData(string npcId)
    {
        if (npcDataDictionary.TryGetValue(npcId, out NPCData data))
            return data;
        return null;
    }
}