using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityScreenUI : MonoBehaviour
{
    [SerializeField] AbilitiesPanel passiveAbilityPanel;

    public void Show(AbilityList abilityList, List<AbilityRef> playerAbilityRef)
    {
        this.gameObject.SetActive(true);
        passiveAbilityPanel.SetData(abilityList, playerAbilityRef);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void UpdateSingleAbility(int selectedIndex, bool isActive)
    {
        passiveAbilityPanel.UpdateAbilityState(selectedIndex, isActive);
    }


    public void Select(int selectedItem){
        passiveAbilityPanel.Select(selectedItem);
    }

    internal void Deselect(int previousSelection)
    {
        passiveAbilityPanel.Deselect(previousSelection);
    }

}

