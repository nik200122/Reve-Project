using System.Collections.Generic;
using UnityEngine;

// public abstract class IHittable : MonoBehaviour
// {
//     public abstract void OnHit(IHittable hittable);
//     public abstract Stat GetStat(string statTag);
    
// }

public abstract class IHittable : MonoBehaviour
{  
    // public abstract void OnHit(IHittable hittable);
    public abstract Stat GetStat(string statTag);
    public abstract void SetVulnerabilities(HashSet<DamageTypeTag> vulnerabilities);
    public abstract HashSet<DamageTypeTag> GetVulnerabilities();

    public abstract void SetOffensiveDamageTypeList(List<DamageType> offensiveDamageType);
    public abstract List<DamageType> GetOffensiveDamageTypeList();
}
