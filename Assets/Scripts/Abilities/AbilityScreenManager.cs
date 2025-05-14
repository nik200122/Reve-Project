using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityScreenManager : MonoBehaviour
{
    [SerializeField] private AbilityScreenUI abilityScreenUI;
    private AbilityList playerAbilityList = new AbilityList(); // Lista globale delle abilità
    private PlayerManager playerManager;
    private List<AbilityRef> abilityLoadout;

    //wrapper che contiene la lista di tutte le trigger-actions
    private AudioTriggerActionsWrapper wrapperTriggerActions;

    [SerializeField] private InputHandler input;
    private int selectedItem = 0;
    private int previousSelection = 0;

    void Start(){
        playerManager = FindAnyObjectByType<PlayerManager>();
        foreach(var triggerActionConfig in wrapperTriggerActions.TriggerActions){
            IAudioAction action = ActionFactory.CreateAudioAction(triggerActionConfig.Action.type, triggerActionConfig.Action.Parameters);
            if(action != null){
                AudioTriggerActionManager.Instance.RegisterAction(this.gameObject, triggerActionConfig.Trigger, action);
            }
        }
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
                AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuSelection);
            }else{
                AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnInvalidSelection);
            }

            input.selectionPerformed = false;
        }
    }

    private void CheckScrollUpActionPerformed(){
        if(input.scrollUpAction){
            --selectedItem;
            UpdateItemSelection();
            AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuNavigation);
        }
        input.scrollUpAction = false;
    }

    private void CheckScrollDownActionPerformed(){
        if(input.scrollDownAction){
            ++selectedItem;
            UpdateItemSelection();
            AudioTriggerActionManager.Instance.TriggerEvent(this.gameObject, TriggerType.OnMenuNavigation);
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

    public void Open(){
        abilityScreenUI.Show(playerAbilityList, abilityLoadout);
        if(abilityLoadout.Count!=0){
            abilityScreenUI.Select(selectedItem);
        }
    }

    public void Hide(){
        abilityScreenUI.Hide();
    }

    public void SetTriggerActions(AudioTriggerActionsWrapper wrapperTriggerActions){
        this.wrapperTriggerActions = wrapperTriggerActions;
    }
}
