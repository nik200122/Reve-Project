using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public List<Stat> stats;
    public List<StatModifier> modifiers;

    //costruttore necessario per la deserializzazione dell'obj player dal file XML
    public Player(){}
    public Stat GetStat(string statTag){
        return stats.Find(obj => obj.GetStatTag() == statTag);
    }

    public void SetStat(string statTag, float value){
        Stat stat = stats.Find(obj => obj.GetStatTag() == statTag);

        float newValue = Mathf.Min(value, stat.GetMaxValue());
        stat.SetCurrentValue(newValue);
    }
}
