using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/NormalAttack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOverrideController;
    public float damage;
}
