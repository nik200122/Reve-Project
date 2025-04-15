using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class AttackData
{
    [XmlElement("id")]
    public string Id { get; set; }

    [XmlElement("animationName")]
    public string AnimationName { get; set; }

    // Nuovo campo per il percorso dell'override controller
    [XmlElement("overrideControllerPath")]
    public string OverrideControllerPath { get; set; }

    [XmlArray("modifiers")]
    [XmlArrayItem("PlayerModifier")]
    public List<StatModifier> Modifiers { get; set; }

    // Campo non serializzato, lo caricheremo da Resources
    [XmlIgnore]
    public AnimatorOverrideController AnimatorOverrideController { get; set; }

    // Metodo per caricare l'AnimatorOverrideController dalla risorsa
    public void LoadAnimatorOverrideController()
    {
        if (!string.IsNullOrEmpty(OverrideControllerPath))
        {
            AnimatorOverrideController = Resources.Load<AnimatorOverrideController>(OverrideControllerPath);
            if (AnimatorOverrideController == null)
            {
                Debug.LogError("AnimatorOverrideController non trovato nel percorso: " + OverrideControllerPath);
            }
        }
    }

    public override string ToString()
    {
        return $"AttackDefinition: Id={Id}, AnimationName={AnimationName}, OverrideControllerPath={OverrideControllerPath}";
    }   

}