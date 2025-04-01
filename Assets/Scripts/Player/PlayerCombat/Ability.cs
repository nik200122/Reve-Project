using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string id;  // Identificativo univoco
    public string name;  // Nome dell'abilità
    public bool isActive;  // Abilità attiva/disattiva
    public List<PlayerModifier> modifiers;  // Modificatori applicati
    public List<AttackRef> equippableAttacks;  // Lista di attacchi che il player può assegnare 

    public Ability()
    {
        modifiers = new List<PlayerModifier>();
        equippableAttacks = new List<AttackRef>();
    }

    public void Activate()
    {
        isActive = true;
        Debug.Log($"🔹 Abilità {name} attivata!");
    }

    public void Deactivate()
    {
        isActive = false;
        Debug.Log($"🔻 Abilità {name} disattivata!");
    }
}
