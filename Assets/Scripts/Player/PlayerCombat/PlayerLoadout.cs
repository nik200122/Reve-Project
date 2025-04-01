using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("PlayerLoadout")]
public class PlayerLoadout
{
    [XmlArray("Abilities")]
    [XmlArrayItem("AbilityRef")]
    public List<AbilityRef> Abilities { get; set; } = new List<AbilityRef>();
}