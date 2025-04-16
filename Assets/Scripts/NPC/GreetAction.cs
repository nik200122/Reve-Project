using System.Collections.Generic;
using UnityEngine;

public class GreetAction : INPCAction
{
    private Dictionary<string, string> parameters;
    AnimatorOverrideController animatorOverrideController;

    public GreetAction(Dictionary<string, string> parameters)
    {
        this.parameters = parameters;
        LoadAnimatorOverrideController();
    }
    public void LoadAnimatorOverrideController()
    {
        // recupera tali parametri dal dizionario:
        if(parameters != null && parameters.TryGetValue("greetAnimation", out string OverrideControllerPath))
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
            npcAnimator.TriggerGreet(animatorOverrideController);
            // In alternativa, se l'NPC usa uno specifico stato di "approach", potresti attivarlo qui.
        }
    }

}
