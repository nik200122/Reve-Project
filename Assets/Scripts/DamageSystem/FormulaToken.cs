using System.Xml.Serialization;

// [XmlInclude(typeof(OperatorToken))]
// [XmlInclude(typeof(ConstantToken))]
// [XmlInclude(typeof(StatToken))]

public abstract class FormulaToken
{   
    public FormulaToken(){}
    public abstract float Evaluate(IHittable attacker, IHittable defender);

}
