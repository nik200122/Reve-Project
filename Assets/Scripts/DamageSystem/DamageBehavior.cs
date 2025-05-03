using UnityEngine;

public class DamageBehavior
{   
    //per ora un solo param, la classe è estendibile con diversi paramentri per garantire un bel po' di comportamenti!
    public bool useDamageFormula;

    public DamageBehavior(){}

    public void Apply(IHittable attacker, IHittable defender, BattleCalculatorManager battleCalculatorManager, string damageTargetStatTag){
        float damage;
        if(useDamageFormula){
            damage = battleCalculatorManager.EvaluateDamage(attacker, defender);
            TakeDamage(defender, damage, damageTargetStatTag);
        }
    }

    private void TakeDamage(IHittable defender, float damage, string damageTargetStatTag){
        PlayerManager playerManager = defender.GetComponent<PlayerManager>();

        if(playerManager == null){
            EnemyManager enemyManager = defender.GetComponent<EnemyManager>();
            enemyManager.GetEnemyModel().SetStat(damageTargetStatTag, enemyManager.GetStat(damageTargetStatTag).currentValue - damage);
            Debug.Log("VITA ENEMY: "+enemyManager.GetStat(damageTargetStatTag).currentValue);
        }else{
            playerManager.GetPlayerModel().SetStat(damageTargetStatTag, playerManager.GetStat(damageTargetStatTag).currentValue - damage);
            Debug.Log("VITA PLAYER: "+playerManager.GetStat(damageTargetStatTag).currentValue);
        }
    }
}
