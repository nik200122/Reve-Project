
public class PlayerModifier
{
    public string targetStat;
    public float value;

    public PlayerModifier(){}
    public PlayerModifier(string targetStat, float value){
        this.targetStat = targetStat;
        this.value = value;
    }
}
