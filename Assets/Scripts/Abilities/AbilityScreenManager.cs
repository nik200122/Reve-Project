using UnityEditor.Search;
using UnityEngine;

public class AbilityScreenManager : MonoBehaviour
{
    [SerializeField] private AbilityScreenUI abilityScreenUI;
    private AbilityList globalAbilityList; // Lista globale delle abilità

    public void Initialize(AbilityList abilityList)
    {
        globalAbilityList = abilityList;
    }

    public void Show(PlayerLoadout loadout)
    {
        abilityScreenUI.Show(loadout, globalAbilityList);
    }

    public void Hide()
    {
        abilityScreenUI.Hide();
    }
}
