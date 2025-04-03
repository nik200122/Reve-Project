using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
public class AttackStep
{
    [XmlAttribute("index")]
    public int index { get; set; }
    
    // Specifica il tipo di step (es. "StepOne", "StepTwo", "StepThree") per confrontarlo con il tag dell'abilità
    [XmlElement("stepType")]
    public StepType stepType { get; set; }
    
    // L'attacco di default da usare se non ci sono abilità attive che influenzano lo step
    [XmlElement("DefaultAttack")]
    public AttackRef defaultAttack { get; set; }
    
    // Modificatori specifici per questo step (es. bonus base o moltiplicatori)
    [XmlArray("modifiers")]
    [XmlArrayItem("PlayerModifier")]
    public List<PlayerModifier> modifiers { get; set; } = new List<PlayerModifier>();
}