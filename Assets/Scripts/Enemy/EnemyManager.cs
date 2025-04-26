using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : IHittable
{
    [SerializeField] private GlobalRulesManager globalRulesManager;
    private Enemy enemy;
    private EnemyCharacterStatus enemyCharacterStatus;

    private void Awake(){
        enemyCharacterStatus = GetComponent<EnemyCharacterStatus>();
    }
    private void Update(){
        CheckIsDead();
    }

    public Enemy GetEnemyModel(){
        return enemy;
    }

    public void SetEnemyModel(Enemy loadedEnemy){
        enemy=loadedEnemy;
        //Debug.Log(enemy.offensiveDamageType[0].damageTypeTag);
    }

    public override List<DamageType> GetOffensiveDamageTypeList(){
        return enemy.offensiveDamageType;
    }

    public override List<DamageTypeTag> GetVulnerabilities(){
        return enemy.vulnerabilities;
    }

    public override Stat GetStat(string statTag){
        return enemy.GetStat(statTag);
    }

    private void CheckIsDead(){
        if(globalRulesManager == null)
        Debug.Log("PISELLONE");
        string defeatTargetStat = globalRulesManager.GetDefeatRule().defeatTargetStatTag;
        float defeatValue = globalRulesManager.GetDefeatRule().defeatValue;
        if(GetStat(defeatTargetStat).currentValue < defeatValue){
            enemyCharacterStatus.SetIsDead(true);
        }
    }
}
