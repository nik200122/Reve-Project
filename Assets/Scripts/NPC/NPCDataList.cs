using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
[XmlRoot("npcs")]
public class NPCDataList
{
    [XmlElement("npc")]
    public List<NPCData> npcs;

    public NPCData GetNPCByName(string name)
    {
        if(npcs == null) return null;

        foreach (NPCData npc in npcs)
        {
            // Confronto case insensitive
            if (string.Equals(npc.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return npc;
            }
        }
        return null;
    }
}