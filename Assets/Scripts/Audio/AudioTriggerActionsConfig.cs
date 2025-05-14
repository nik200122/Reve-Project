//classe che serve a deserializzare il dictionary trigger action (audio)

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class AudioTriggerActionsConfig
{
    [XmlElement("Trigger")]
    public TriggerType Trigger;

    [XmlElement("Action")]
    public AudioActionConfig Action;
}

[Serializable]
public class AudioActionConfig{   
    [XmlAttribute("type")]
    public AudioActionType type;

    // Se l'azione include uno o più Parameter
    [XmlElement("Parameter")]
    public List<ParameterConfig> Parameters;
}


[XmlRoot("ArrayOfTriggerActions")]
public class AudioTriggerActionsWrapper{
    [XmlArray("TriggerActions")]
    [XmlArrayItem("TriggerAction")]
    public List<AudioTriggerActionsConfig> TriggerActions;
}

