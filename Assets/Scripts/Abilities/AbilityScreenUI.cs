using UnityEngine;

public class AbilityScreenUI : MonoBehaviour
{
    [SerializeField] PassiveAbilitiesPanel passiveAbilityPanel;

    public void Show(PlayerLoadout loadout, AbilityList abilityList)
    {
        this.gameObject.SetActive(true);
        passiveAbilityPanel.Initialize(abilityList); // Inizializza la lista
        passiveAbilityPanel.SetData(loadout);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}

