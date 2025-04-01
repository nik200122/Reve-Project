using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
public class Combo
{
    [XmlElement("AttackStep")]
    public List<AttackStep> AttackSteps { get; set; } = new List<AttackStep>();
}
