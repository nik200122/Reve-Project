using System.Collections.Generic;
using UnityEngine;

public class ApproachAction : INPCAction
{
    private Dictionary<string, string> parameters;

    public ApproachAction(Dictionary<string, string> parameters)
    {
        this.parameters = parameters;
    }

    public void Execute(NPCInteractable npc, Transform player, string utterance)
    {
        Debug.Log($"NPC {npc.GetName()} is approaching the player: {utterance}");
        
        // Recupera il componente NPCStatus per controllare la movimentazione
        var npcStatus = npc.GetComponent<NPCStatus>();
        if(npcStatus != null)
        {
            // Imposta il target sul giocatore
            npcStatus.SetTarget(player, NPCStatus.MovementType.Approach);
            
            // Se desideri applicare parametri specifici per l'approach, ad esempio la velocità o la distanza,
            // recupera tali parametri dal dizionario:
            float approachSpeed = 0;
            if(parameters != null && parameters.TryGetValue("approachSpeed", out string speedStr))
            {
                float.TryParse(speedStr, out approachSpeed);
            }
            
            // Se hai definito un metodo in NPCStatus per gestire questi parametri, invocalo
            npcStatus.SetApproachSpeed(approachSpeed);
            
            // In alternativa, se l'NPC usa uno specifico stato di "approach", potresti attivarlo qui.
        }
    }
}
