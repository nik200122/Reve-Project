using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;



public class AbilityScreenManager : MonoBehaviour
{
    [SerializeField] private AbilityScreenUI abilityScreenUI;
    private AbilityList playerAbilityList = new AbilityList(); // Lista globale delle abilità
    private PlayerManager playerManager;
    private List<AbilityRef> abilityLoadout;
    [SerializeField] private InputHandler input;
    private int selectedItem = 0;
    private int previousSelection = 0;

    void Start(){
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    public void Initialize(AbilityList abilityList, PlayerLoadout loadout)
    {
        foreach (var abilityRef in loadout.Abilities)
        {
            // Cerca l'abilità completa corrispondente all'ID
            Ability actualAbility = abilityList.Abilities.FirstOrDefault(a => a.id == abilityRef.Id); // Risolve l'ID in un oggetto reale

            if (actualAbility != null)
            {
                playerAbilityList.Abilities.Add(actualAbility);
            }
            else
            {
                Debug.LogWarning($"Abilità con ID {abilityRef.Id} non trovata!");
            }
        }
        this.abilityLoadout = loadout.Abilities;
    }

    public void Update(){
        if(GameStateManager.Instance.CurrentState == GameState.AbilitiesScreen){
            CheckScrollDownActionPerformed();
            CheckScrollUpActionPerformed();
            CheckSelectionPerformed();
        }
        // 🔹 Resetta il valore subito dopo l'uso
        //input.scrollUpAction = false;
        //input.scrollDownAction = false;
    }

    private void CheckSelectionPerformed() {
        if (input.selectionPerformed) {
            bool abilityChanged = playerManager.CheckAbility(abilityLoadout[selectedItem]);

            if (abilityChanged) {
                abilityScreenUI.UpdateSingleAbility(selectedItem, abilityLoadout[selectedItem].IsActive);
            }

            input.selectionPerformed = false;
        }
    }


    private void CheckScrollUpActionPerformed(){
        if(input.scrollUpAction){
            --selectedItem;
            UpdateItemSelection();
        }
        input.scrollUpAction = false;
    }

    private void CheckScrollDownActionPerformed(){
        
        if(input.scrollDownAction){
            ++selectedItem;
            UpdateItemSelection();
        }

        input.scrollDownAction = false;
    
    }

    private void UpdateItemSelection(){
        //ci assicuriamo che non avvenga un outOfindex
        selectedItem = Mathf.Clamp(selectedItem, 0, abilityLoadout.Count - 1);
        previousSelection = Mathf.Clamp(previousSelection, 0, abilityLoadout.Count - 1);

        abilityScreenUI.Deselect(previousSelection);
        abilityScreenUI.Select(selectedItem);

        previousSelection = selectedItem;
    }

    

    public void Open()
    {
        abilityScreenUI.Show(playerAbilityList, abilityLoadout);
        if(abilityLoadout.Count!=0){
            abilityScreenUI.Select(selectedItem);
        }
    }

    public void Hide()
    {
        abilityScreenUI.Hide();
    }

}
