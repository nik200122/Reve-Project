using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Ability
{
    public string id;  // Identificativo univoco
    public string name;  // Nome dell'abilità
    public List<PlayerModifier> modifiers;  // Modificatori applicati
    public List<AttackRef> equippableAttacks;  // Lista di attacchi che il player può assegnare
    public string spritePath;
    public string description;

    [XmlIgnore]
    public Sprite sprite;

    public Ability()
    {
        modifiers = new List<PlayerModifier>();
        equippableAttacks = new List<AttackRef>();
    }
    // Metodo per caricare l'AnimatorOverrideController dalla risorsa
    public void LoadSprite()
    {
        if (!string.IsNullOrEmpty(spritePath))
        {
            sprite= Resources.Load<Sprite>(spritePath);
            if (sprite == null)
            {
                Debug.LogError("Sprite non trovato nel percorso: " + spritePath);
            }
        }
    }

}
