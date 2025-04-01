
public class PlayerModifier
{
    public string targetStat;
    public float value;

    public ModifierType modifierType;

    public PlayerModifier(){}
    public PlayerModifier(string targetStat, float value, ModifierType modifierType){
        this.targetStat = targetStat;
        this.value = value;
        this.modifierType = modifierType;
    }
}
