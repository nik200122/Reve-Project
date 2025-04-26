using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Enemy
{
    public List<Stat> stats;
    public List<AttackData> attackDataList;
    public List<DamageTypeTag> vulnerabilities;
    //indica i tipi di danni che infligge quando colpisce
    public List<DamageType> offensiveDamageType;

    public Enemy(){}
    
    public Stat GetStat(string statTag){
        return stats.Find(obj => obj.GetStatTag() == statTag);
    }

    public void SetStat(string statTag, float value){
        Stat stat = stats.Find(obj => obj.GetStatTag() == statTag);

        float newValue = Mathf.Min(value, stat.GetMaxValue());
        stat.SetCurrentValue(newValue);
    }

    public List<AttackData> GetAttackDataList(){
        return attackDataList;
    }

    public AttackData GetAttackData(int index){
        return attackDataList[index];
    }

    public void SetAnimatorOverrideControllers(){
        foreach (var attack in attackDataList){
            attack.LoadAnimatorOverrideController();
        }
    }
}
