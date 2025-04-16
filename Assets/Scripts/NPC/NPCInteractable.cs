using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable{

    [Header("Data Driven Settings")]
    [SerializeField] private string npcId; // Identificativo univoco associato al file XML

    // Variabili caricate dai dati
    private string interactText;
    private NPCData npcData;
    [SerializeField] private ConversationHistory conversationHistory;
    private NPCUI nPCUI;
    

    

    [SerializeField] private LLMManager llmManager;

    private void Awake()
    {
        nPCUI = GetComponent<NPCUI>();
        
    }
    
    private void Start(){
        npcData = NPCDataManager.Instance.GetNPCData(npcId);
        if (npcData != null)
        {
            interactText = npcData.InteractText;
            Debug.Log($"[NPCInteractable Start] Loading NPC data for id: {npcId}, Name: {npcData.Name}");

            // Registra le azioni per questo NPC leggendo i trigger dal file di configurazione
            foreach (var triggerActionConfig in npcData.TriggerActions)
            {
                Debug.Log($"[NPCInteractable Start] Processing trigger '{triggerActionConfig.Trigger}' with action type '{triggerActionConfig.Action.type}' for NPC '{npcData.Name}'");
                INPCAction action = ActionFactory.CreateAction(triggerActionConfig.Action.type, triggerActionConfig.Action.Parameters);
                if (action != null)
                {
                    NPCTriggerActionManager.Instance.RegisterAction(this, triggerActionConfig.Trigger, action);
                    Debug.Log($"[NPCInteractable Start] Registered action '{action.GetType().Name}' for trigger '{triggerActionConfig.Trigger}' on NPC '{npcData.Name}'");
                }
                else
                {
                    Debug.LogWarning($"[NPCInteractable Start] ActionFactory returned null for action type '{triggerActionConfig.Action.type}' on NPC '{npcData.Name}'");
                }
            }
            
            // Registra questo NPC nel registro
            NPCDataManager.Instance.RegisterNPC(npcId, this);
        }
        else
        {
            Debug.LogError("Dati NPC non trovati per id: " + npcId);
        }
    }

    private void OnDestroy()
    {
        // Deregistra quando viene distrutto
        if (NPCDataManager.Instance != null)
        {
            NPCDataManager.Instance.UnregisterNPC(npcId);
        }
    }


    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Interazione: " + interactText);
        llmManager.ActivateDialogue();
        llmManager.SetNPC(this);
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public string GetName()
    {
        return npcData != null ? npcData.Name : gameObject.name;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void TerminateInteract()
    {
        llmManager.DectivateDialogue();
        GameStateManager.Instance.ChangeState(GameState.FreeRoam);
    }
    public NPCData GetNPCData(){
        return npcData;
    }
    // Metodo per accedere allo storico della conversazione
    public ConversationHistory GetConversationHistory()
    {
        /* uguale a 
        if(conversationHistory == null)
        {
            conversationHistory = new ConversationHistory();
        }*/
        conversationHistory ??= new ConversationHistory();
        return conversationHistory;
    }
    
    // Metodo per resettare la conversazione se necessario
    public void ResetConversation()
    {
        conversationHistory = new ConversationHistory();
    }

    public void ShowAlertIcon(){
        nPCUI.ShowAlertIcon();
    }

    public void OnPlayerDetected()
    {
        // Check if NPC is already in an interaction
        if (GameStateManager.Instance.CurrentState != GameState.FreeRoam)
            return;
            
        ShowNotedIcon();

        llmManager.SendAutomatedRequest(this);
    }

    private void ShowNotedIcon()
    {
        nPCUI.ShowNotedIcon();
    }

    public void OnPlayerLost()
    {
        // Optional: Handle when player exits the detection range
        // For example, cancel any pending reactions if they haven't started yet
    }


    
}
