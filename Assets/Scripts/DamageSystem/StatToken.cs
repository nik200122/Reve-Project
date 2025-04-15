public class StatToken : FormulaToken
{
    public StatSource source;
    public string statTag;

    public StatToken(){}

    public override float Evaluate(IHittable attacker, IHittable defender)
    {
        var manager = source == StatSource.Attacker ? attacker : defender;
        var stat = manager.GetStat(statTag);
        return stat != null ? stat.GetCurrentValue() : 0f;
    }
}

public enum StatSource{
    Attacker,
    Defender
}
