using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("DeepSeekConfig")]
public class DeepSeekConfig
{
    [XmlElement("ApiUrl")]
    public string ApiUrl;

    [XmlElement("Prompt")]
    public string Prompt;
}