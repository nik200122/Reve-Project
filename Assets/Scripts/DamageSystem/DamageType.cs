using UnityEngine;

public class DamageType 
{
    public DamageTypeTag damageTypeTag;
    public DamageBehavior damageBehaviour;

    public DamageType(){}
}

public enum DamageTypeTag{
    Impact,
    Fire
}
