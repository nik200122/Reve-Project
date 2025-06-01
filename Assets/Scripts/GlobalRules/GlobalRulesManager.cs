using UnityEngine;
//contiene tutte le regole accessibili da chiunque, non solo il player
public class GlobalRulesManager : MonoBehaviour
{
    private DamageApplicationRule damageApplicationRule;
    private DefeatRule defeatRule;
    private CurrencyRule currencyRule;

    public DamageApplicationRule GetDamageApplicationRule()
    {
        return damageApplicationRule;
    }

    public void SetDamageApplicationRule(DamageApplicationRule LoadedDamageApplicationRule)
    {
        this.damageApplicationRule = LoadedDamageApplicationRule;
    }

    public DefeatRule GetDefeatRule()
    {
        return defeatRule;
    }

    public void SetDefeatRule(DefeatRule LoadedDefeatRule)
    {
        this.defeatRule = LoadedDefeatRule;
    }

    public CurrencyRule GetCurrencyRule()
    {
        return currencyRule;

    }

    public void SetCurrencyRule(CurrencyRule LoadedCurrencyRule)
    {
        currencyRule = LoadedCurrencyRule;
    }
}
