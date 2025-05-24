using System.Collections.Generic;
using UnityEngine;

public class DamageSystemManager : MonoBehaviour
{     
    [SerializeField] BattleCalculatorManager battleCalculatorManager;

    [SerializeField] private GlobalRulesManager globalRulesManager;
    
    public void ApplyEffectiveDamage(IHittable attacker, IHittable defender){
        List<DamageType> offensiveTypes = attacker.GetOffensiveDamageTypeList();
        List<DamageTypeTag> vulnerabilities = defender.GetVulnerabilities();

        defender.SpawnHitVfx();
    
        foreach (var damageType in offensiveTypes){
            // Se il difensore è vulnerabile a quel tipo di danno
            if (vulnerabilities.Contains(damageType.damageTypeTag)){
                // E se c’è un comportamento associato
                Debug.Log("ECCO COSA Cè: "+damageType.damageTypeTag);
                damageType.damageBehaviour.Apply(attacker, defender, battleCalculatorManager, globalRulesManager.GetDamageApplicationRule().damageTargetStatTag);
            }
        }
    }
}
