using System;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

// Classe principale per NPC
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

    [XmlElement("detectionRadius")]
    public int detectionRadius;

    [XmlElement("detectionAngle")]
    public int detectionAngle;

    [XmlElement("Position")]
    public SerializableVector3 InitialPosition;

    // Lista completa degli oggetti che l'NPC può dare.
    [XmlArray("GiveableItems")]
    [XmlArrayItem("EquipableItem", typeof(EquipableItem))]
    [XmlArrayItem("ConsumableItem", typeof(ConsumableItem))]
    public List<Item> giveableItems;

    // Importante: specificare che TriggerActions è una lista
    [XmlArray("TriggerActions")]
    [XmlArrayItem("TriggerAction")]
    public List<TriggerActionConfig> TriggerActions;

    public string GetPrompt()
    {
        return $"NPC Name: {Name}\nBackstory: {Backstory}\nPersonality: {Personality}";
    }
}

// Classe per la serializzazione di Vector3
[Serializable]
public class SerializableVector3
{
    [XmlAttribute("x")]
    public float x;
    [XmlAttribute("y")]
    public float y;
    [XmlAttribute("z")]
    public float z;

    // Costruttore senza parametri necessario per la serializzazione XML
    public SerializableVector3() { }

    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static implicit operator Vector3(SerializableVector3 sv) => new Vector3(sv.x, sv.y, sv.z);
    public static implicit operator SerializableVector3(Vector3 v) => new SerializableVector3(v.x, v.y, v.z);
}

// Classe per rappresentare i TriggerAction
[Serializable]
public class TriggerActionConfig
{
    [XmlElement("Trigger")]
    public NPCTriggerType Trigger; // Se intendi usare un enum, puoi specificare anche NPCTriggerType

    [XmlElement("Action")]
    public ActionConfig Action;
}

// Classe per rappresentare l'Action
[Serializable]
public class ActionConfig
{
    // L'attributo "type" nell'elemento Action
    [XmlAttribute("type")]
    public string type;

    // Se l'azione include uno o più Parameter
    [XmlElement("Parameter")]
    public List<ParameterConfig> Parameters;
}

// Classe per rappresentare ogni Parameter
[Serializable]
public class ParameterConfig
{
    [XmlAttribute("key")]
    public string key;

    [XmlAttribute("value")]
    public string value;
}


