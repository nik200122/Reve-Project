using System.Xml.Serialization;
using UnityEngine;

public class AttackRef
{
    [XmlAttribute("id")]
    public string Id { get; set; }
}