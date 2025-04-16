// Warn Others Action
using System.Collections.Generic;
using UnityEngine;

public class WarnOthersAction : INPCAction
{
    private Dictionary<string, string> parameters;

    // Passa i parametri al costruttore se necessario
    public WarnOthersAction(Dictionary<string, string> parameters)
    {
        this.parameters = parameters;
    }
     public void Execute(NPCInteractable npc, Transform player, string utterance)
    {
        Debug.Log($"NPC {npc.GetName()} is warning others: {utterance}");
        npc.ShowAlertIcon();
        // Recupera alertRadius dal dizionario e lo converte
        float alertRadius = 0f;
        if (parameters != null && parameters.TryGetValue("alertRadius", out string radiusStr))
        {
            float.TryParse(radiusStr, out alertRadius);
        }
        
        List<NPCInteractable> nearbyNPCs = NPCDataManager.Instance.GetNPCsInRadius(npc.transform.position, alertRadius);
        
        foreach (var nearbyNPC in nearbyNPCs)
        {
            // Assicurati di non processare l'NPC corrente
            if (nearbyNPC != npc)
            {
                // Invia il trigger "WarnedByOthers" agli NPC vicini
                NPCTriggerActionManager.Instance.TriggerEvent(
                    nearbyNPC,
                    NPCTriggerType.WarnedByOthers,
                    player,
                    $"Warned by {npc.GetName()}"
                );
            }
        }
    }
}