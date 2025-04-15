public class ConstantToken : FormulaToken
{
    public float value;

    public ConstantToken(){}

    public ConstantToken(float v)
    {
        this.value = v;
    }

    public override float Evaluate(IHittable a, IHittable d)
    {
        return value;
    }
}
