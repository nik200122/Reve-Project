using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PassiveAbilitiesPanel : MonoBehaviour
{
    [SerializeField] private AbilityUI passiveAbilityPrefab;
    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private List<AbilityUI> abilityUISlots;
    [SerializeField] protected GameObject scrollbarHorizontal;
    protected Scrollbar scrollbar;
    
    private AbilityList abilityList; // Riferimento alla lista globale delle abilità
    int minViewPort = 0;
    int maxViewPort = 2;

    protected void Awake(){
        abilityUISlots = new List<AbilityUI>();
        scrollbar = scrollbarHorizontal.GetComponent<Scrollbar>();
    }
    

    public void Initialize(AbilityList globalAbilityList)
    {
        abilityList = globalAbilityList;
    }

    protected void DestroyGameObjects()
    {
        abilityUISlots.Clear();
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetData(PlayerLoadout loadout)
    {
        if (abilityList == null)
        {
            Debug.LogError("AbilityList non è stata inizializzata!");
            return;
        }

        DestroyGameObjects();

        foreach (var abilityRef in loadout.Abilities)
        {
            // Cerca l'abilità completa corrispondente all'ID
            Ability actualAbility = abilityList.Abilities.FirstOrDefault(a => a.id == abilityRef.Id); // Risolve l'ID in un oggetto reale

            if (actualAbility != null && actualAbility.equippableAttacks.Count == 0)
            {
                var slotUIobj = Instantiate(passiveAbilityPrefab, scrollViewContent.transform);
                slotUIobj.SetData(actualAbility);
                abilityUISlots.Add(slotUIobj);
            }
            else
            {
                Debug.LogWarning($"Abilità con ID {abilityRef.Id} non passiva!");
            }
        }
    }
    public void HandleScrolling(int selectedItem){   
        if(scrollbar == null)
            scrollbar = scrollbarHorizontal.GetComponent<Scrollbar>();
        
        
        //verifica se l'item selezionato è al momento visualizzato. Se non lo è aggiorna la scrollbar per farlo vedere
        if(selectedItem <= maxViewPort && selectedItem >= minViewPort){
        }
        else{
            if(selectedItem > maxViewPort){
                scrollbar.value = Mathf.Clamp(scrollbar.value - 0.7f, 0, 1);
                ++maxViewPort;
                ++minViewPort;
            }
            else if(selectedItem<minViewPort){
                scrollbar.value = Mathf.Clamp(scrollbar.value + 0.7f, 0, 1);
                --maxViewPort;
                --minViewPort;
            }
        }
    }
}
