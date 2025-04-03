using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("AbilitiesRules")]
public class AbilitiesRules
{
    [XmlElement("AbilityRule")]
    public List<AbilityRule> Rules { get; set; }
}
