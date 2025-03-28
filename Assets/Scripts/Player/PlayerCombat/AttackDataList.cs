using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("AttackDefinitions")]
public class AttackDataList
{
    [XmlElement("Attack")]
    public List<AttackData> Attacks { get; set; }
}
