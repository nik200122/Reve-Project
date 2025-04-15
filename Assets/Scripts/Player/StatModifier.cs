
public class StatModifier
{
    public string targetStat;
    public float value;

    public ModifierType modifierType;

    public StatModifier(){}
    public StatModifier(string targetStat, float value, ModifierType modifierType){
        this.targetStat = targetStat;
        this.value = value;
        this.modifierType = modifierType;
    }
    public override string ToString()
    {
        return $"TargetStat: {targetStat} value: {value} modifierType: {modifierType}";
    }
}
