using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
public class AttackStep
{
    [XmlAttribute("attackId")]
    public string AttackId { get; set; }
}