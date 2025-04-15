using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<Stat> stats;
    //definisce la combo che il nemico pùò fare
    public List<AttackData> attackDataList;
    
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

    public void SetAttackDataList(List<AttackData> loadedAttackData){
        // Stampa il contenuto della lista di attacchi per il debug
        // foreach (var attackData in loadedAttackData) {
        //     Debug.Log($"Attack ID: {attackData.Id}, OverrideControllerPath: {attackData.OverrideControllerPath}");
        // }
        this.attackDataList = loadedAttackData;
    }
}
