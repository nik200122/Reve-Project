using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
public class Combo
{
    [XmlAttribute("id")]
    public string Id { get; set; }
    
    [XmlArray("Sequence")]
    [XmlArrayItem("AttackStep")]
    public List<AttackStep> Sequence { get; set; }
}
