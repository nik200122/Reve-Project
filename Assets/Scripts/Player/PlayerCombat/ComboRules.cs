using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("ComboRules")]
public class ComboRules
{
    [XmlElement("Combo")]
    public Combo Combo { get; set; }
}


