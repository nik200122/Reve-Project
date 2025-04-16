using System.Collections.Generic;
using UnityEngine;

public class GiveItemAction : INPCAction
{

    private Dictionary<string, string> parameters;

    public GiveItemAction(Dictionary<string, string> parameters)
    {
        this.parameters = parameters;
    }
    
    public void Execute(NPCInteractable npc, Transform player, string utterance)
    {
        // Recupera i dati dell'NPC
        NPCData npcData = npc.GetNPCData();
        if (npcData == null || npcData.giveableItems == null || npcData.giveableItems.Count == 0)
        {
            Debug.LogWarning($"NPC {npc.GetName()} non ha oggetti da dare.");
            return;
        }

        // Seleziona un oggetto casuale dalla lista
        int randomIndex = Random.Range(0, npcData.giveableItems.Count);
        Item itemToGiveOriginal = npcData.giveableItems[randomIndex];
        if (itemToGiveOriginal == null)
        {
            Debug.LogWarning($"L'oggetto alla posizione {randomIndex} è nullo.");
            return;
        }
        
        // Crea una copia dell'oggetto da dare (assumendo che l'oggetto supporti la clonazione)
        Item itemToGive = itemToGiveOriginal.Clone();
        Debug.Log($"[GiveItemAction] {npc.GetName()} sta per dare l'oggetto {itemToGive.name} al player.");

        // Recupera il PlayerManager dalla scena
        PlayerManager playerManager = GameObject.FindAnyObjectByType<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("[GiveItemAction] Impossibile trovare il PlayerManager nella scena.");
            return;
        }

        // Recupera l'inventario del player
        var inventory = playerManager.GetInventory();
        if (inventory == null)
        {
            Debug.LogError("[GiveItemAction] Inventario non disponibile sul PlayerManager.");
            return;
        }

        // Aggiungi l'oggetto all'inventario del player
        inventory.AddItem(itemToGive);
        Debug.Log($"[GiveItemAction] {itemToGive.name} aggiunto all'inventario del player.");
         // Aggiorna la UI per informare il giocatore
        LLMUI ui = GameObject.FindAnyObjectByType<LLMUI>();
        if (ui != null)
        {
            // Ad esempio, dopo 1 secondo mostriamo il messaggio
            ui.StartCoroutine(ui.AppendDelayedMessage($"You received {itemToGive.name}", 4.0f));
        }
        else
        {
            Debug.LogWarning("LLMUI non trovato nella scena!");
        }
    }
}
