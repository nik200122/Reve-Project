using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public List<PlayerStat> stats;

    //costruttore necessario per la deserializzazione dell'obj player dal file XML
    public Player(){}
    public PlayerStat GetStat(string statTag){
        return stats.Find(obj => obj.GetStatTag() == statTag);
    }

    public void SetStat(string statTag, float value){
        PlayerStat stat = this.stats.Find(obj => obj.GetStatTag() == statTag);

        float newValue = Mathf.Min(value, stat.GetMaxValue());
        stat.SetCurrentValue(newValue);
    }
}
