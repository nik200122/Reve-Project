using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("DeepSeekConfig")]
public class LLMConfig
{
    [XmlElement("ApiUrl")]
    public string ApiUrl;

    [XmlElement("Prompt")]
    public string Prompt;
    [XmlElement("Model")]
    public string Model;
     [XmlElement("automatedActionsInstructions")]
    public string automatedActionsInstructions;
}