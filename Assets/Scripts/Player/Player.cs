using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    public List<Stat> stats;
    public List<StatModifier> modifiers;
    //indica i tipi di danni a cui è vulnerabile
    public List<DamageTypeTag> vulnerabilities;
    //indica i tipi di danni che infligge quando colpisce
    public List<DamageType> offensiveDamageType;

    //costruttore necessario per la deserializzazione dell'obj player dal file XML
    public Player() { }
    public Stat GetStat(string statTag)
    {
        return stats.Find(obj => obj.GetStatTag() == statTag);
    }

    public void SetStat(string statTag, float value)
    {
        Stat stat = stats.Find(obj => obj.GetStatTag() == statTag);

        float newValue = Mathf.Min(value, stat.GetMaxValue());
        stat.SetCurrentValue(newValue);
    }
    
    public void SetBaseStat(string statTag, float value)
    {
        Stat stat = stats.Find(obj => obj.GetStatTag() == statTag);

        float newValue = Mathf.Min(value, stat.GetMaxValue());
        stat.SetBaseValue(newValue);
    }
}
