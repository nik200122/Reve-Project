using System.Xml.Serialization;
using UnityEngine;

public class AbilityRef
{
    [XmlAttribute("id")]
    public string Id { get; set; }
    
    public bool IsActive { get; set; }
}
