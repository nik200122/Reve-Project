using System.Collections.Generic;
using UnityEngine;

public class BeCautiousAction : INPCAction
{
    private Dictionary<string, string> parameters;

    public BeCautiousAction(Dictionary<string, string> parameters)
    {
        this.parameters = parameters;
    }
    
    public void Execute(NPCInteractable npc, Transform player, string utterance)
{
    Debug.Log($"NPC {npc.GetName()} is becoming cautious: {utterance}");
    npc.ShowAlertIcon();

    var npcStatus = npc.GetComponent<NPCStatus>();
    if (npcStatus != null)
    {
        // Recupera il parametro "cautiousDistance"
        if (parameters != null && parameters.TryGetValue("cautiousDistance", out string distStr))
        {
            if (float.TryParse(distStr, out float cautiousDist))
            {
                npcStatus.SetCautiousDistance(cautiousDist);
            }
        }

        npcStatus.TriggerAction(player, NPCStatus.MovementType.Cautious);
    }
}

}
