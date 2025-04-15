using UnityEngine;
//contiene tutte le regole accessibili da chiunque, non solo il player
public class GlobalRulesManager : MonoBehaviour
{
    private DamageApplicationRule damageApplicationRule;

    public DamageApplicationRule GetDamageApplicationRule(){
        return damageApplicationRule;
    }

    public void SetDamageApplicationRule(DamageApplicationRule LoadedDamageApplicationRule){
        this.damageApplicationRule = LoadedDamageApplicationRule;
        Debug.Log("STATTAG: "+damageApplicationRule.damageTargetStatTag);
    }
}
