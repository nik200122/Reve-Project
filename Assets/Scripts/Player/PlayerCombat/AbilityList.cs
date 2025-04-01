using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Abilities")]
public class AbilityList
{
    [XmlElement("Ability")]
    public List<Ability> Abilities = new List<Ability>();
}
