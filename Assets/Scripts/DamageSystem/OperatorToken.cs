public enum MathOperator { Add, Subtract, Multiply, Divide }

public class OperatorToken : FormulaToken
{
    public MathOperator operation;

    public OperatorToken(){}

    public OperatorToken(MathOperator operation)
    {
        this.operation = operation;
    }

    public override float Evaluate(IHittable a, IHittable d)
    {
        throw new System.NotImplementedException("OperatorToken should not be evaluated directly.");
    }

    public static float Apply(float left, float right, MathOperator op)
    {
        return op switch
        {
            MathOperator.Add => left + right,
            MathOperator.Subtract => left - right,
            MathOperator.Multiply => left * right,
            MathOperator.Divide => right != 0 ? left / right : 0f,
            _ => 0f
        };
    }
}
