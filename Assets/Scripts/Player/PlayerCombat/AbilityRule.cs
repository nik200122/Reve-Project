using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
public class AbilityRule
{
    [XmlElement("abilityType")]
    public AbilityType abilityType { get; set; }

    [XmlElement("quantity")]
    public int quantity { get; set; } // Nullable per gestire il valore vuoto
}