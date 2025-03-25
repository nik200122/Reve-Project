using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class NPCData
{
    [XmlAttribute("id")]
    public string id; // Identificativo univoco

    [XmlElement("Name")]
    public string Name;

    [XmlElement("Backstory")]
    public string Backstory;

    [XmlElement("Personality")]
    public string Personality;

    [XmlElement("InteractText")]
    public string InteractText;

    public string GetPrompt()
    {
        return $"NPC Name: {Name}\nBackstory: {Backstory}\nPersonality: {Personality}";
    }
}
