// Ignore Action
using System.Collections.Generic;
using UnityEngine;

public class IgnoreAction : INPCAction
{
    private Dictionary<string, string> parameters;

    // Passa i parametri al costruttore se necessario
    public IgnoreAction(Dictionary<string, string> parameters)
    {
        this.parameters = parameters;
    }
    public void Execute(NPCInteractable npc, Transform player, string utterance)
    {
        Debug.Log($"NPC {npc.GetName()} is ignoring player: {utterance}");
        
        // Just display text but then terminate interaction
        npc.TerminateInteract();
    }
}