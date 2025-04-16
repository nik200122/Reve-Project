// Talk Action (Default)
using System.Collections.Generic;
using UnityEngine;

public class TalkAction : INPCAction
{
    private Dictionary<string, string> parameters;
    AnimatorOverrideController animatorOverrideController;

    // Passa i parametri al costruttore se necessario
    public TalkAction(Dictionary<string, string> parameters)
    {
        this.parameters = parameters;
        LoadAnimatorOverrideController();
    }

    public void LoadAnimatorOverrideController()
    {
        // recupera tali parametri dal dizionario:
        if(parameters != null && parameters.TryGetValue("talkAnimation", out string OverrideControllerPath))
        {
            animatorOverrideController= Resources.Load<AnimatorOverrideController>(OverrideControllerPath);
            if (animatorOverrideController == null)
            {
                Debug.LogError("AnimatorOverrideController non trovato nel percorso: " + OverrideControllerPath);
            }
        }
    }
    public void Execute(NPCInteractable npc, Transform player, string utterance)
    {
        // Recupera il componente NPCStatus per controllare la movimentazione
        var npcAnimator = npc.GetComponent<NPCAnimator>();
        if(npcAnimator != null)
        {   
            npcAnimator.TriggerTalk(animatorOverrideController);
            // In alternativa, se l'NPC usa uno specifico stato di "approach", potresti attivarlo qui.
        }
    }
}