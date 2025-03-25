using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
[XmlRoot("NPCs")]
public class NPCDataList
{
    [XmlElement("NPC")]
    public List<NPCData> npcs;
}