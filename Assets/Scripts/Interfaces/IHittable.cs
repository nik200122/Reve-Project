using System.Collections.Generic;
using UnityEngine;

// public abstract class IHittable : MonoBehaviour
// {
//     public abstract void OnHit(IHittable hittable);
//     public abstract Stat GetStat(string statTag);
    
// }

public abstract class IHittable : MonoBehaviour
{  
    [SerializeField] private GameObject hitVfx;
    // public abstract void OnHit(IHittable hittable);
    public abstract Stat GetStat(string statTag);
    // public abstract void SetVulnerabilities(List<DamageTypeTag> vulnerabilities);
    public abstract List<DamageTypeTag> GetVulnerabilities();

    // public abstract void SetOffensiveDamageTypeList(List<DamageType> offensiveDamageType);
    public abstract List<DamageType> GetOffensiveDamageTypeList();
    public void SpawnHitVfx(){
        Instantiate(hitVfx, transform.position, Quaternion.identity);
    }
}
