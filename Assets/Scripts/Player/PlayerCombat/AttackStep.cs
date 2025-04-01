using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
public class AttackStep
{
    [XmlAttribute("index")]
    public int Index { get; set; }

    [XmlArray("AllowedAttacks")]
    [XmlArrayItem("AttackRef")]
    public List<AttackRef> AllowedAttacks { get; set; } = new List<AttackRef>();

    [XmlArray("modifiers")]
    [XmlArrayItem("PlayerModifier")]
    public List<PlayerModifier> Modifiers { get; set; }
}