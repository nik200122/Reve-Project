using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class DamageFormula
{   
    [XmlArrayItem("StatToken", typeof(StatToken))]
    [XmlArrayItem("OperatorToken", typeof(OperatorToken))]
    [XmlArrayItem("ConstantToken", typeof(ConstantToken))]
    public List<FormulaToken> tokens;

    public DamageFormula(){}
}
