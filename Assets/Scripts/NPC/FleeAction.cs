// Flee Action
using System.Collections.Generic;
using UnityEngine;

public class FleeAction : INPCAction
{
    private Dictionary<string, string> parameters;

    // Passa i parametri al costruttore se necessario
    public FleeAction(Dictionary<string, string> parameters)
    {
        this.parameters = parameters;
    }
    public void Execute(NPCInteractable npc, Transform player, string utterance)
    {
        Debug.Log($"NPC {npc.GetName()} is fleeing: {utterance}");
         // Chiude la finestra di dialogo
        npc.TerminateInteract();
        
        // Get the AI movement component and set flee behavior
        var nPCStatus = npc.GetComponent<NPCStatus>();
        if (nPCStatus != null)
        {
            nPCStatus.TriggerAction(player, NPCStatus.MovementType.Flee);
        }
    }
}