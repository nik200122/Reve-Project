using System.Collections.Generic;
using UnityEngine;

public interface INPCAction
{
    void Execute(NPCInteractable npc, Transform player, string utterance);
}
public enum NPCTriggerType
{
    WarnOthers,        // Ricezione di un avviso da parte di un altro NPC
    Custom,
    WarnedByOthers,
    Talk,
    Approach,
    Greet,
    Ignore,
    Flee,
    GiveItem,
}



public class NPCTriggerActionManager : MonoBehaviour
{
    public static NPCTriggerActionManager Instance { get; private set; }

    // La mappatura: ad ogni NPC (chiave: NPCInteractable) associamo un dizionario che mappa il trigger ad una lista di azioni.
    private Dictionary<NPCInteractable, Dictionary<NPCTriggerType, List<INPCAction>>> npcTriggerActions =
        new Dictionary<NPCInteractable, Dictionary<NPCTriggerType, List<INPCAction>>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Metodo per registrare un'azione per un dato NPC e trigger
    public void RegisterAction(NPCInteractable npc, NPCTriggerType triggerType, INPCAction action)
    {
        Debug.Log($"[NPCTriggerActionManager.RegisterAction] Registering action '{action.GetType().Name}' for NPC '{npc.GetName()}' on trigger '{triggerType}'");
        
        if (!npcTriggerActions.ContainsKey(npc))
        {
            npcTriggerActions[npc] = new Dictionary<NPCTriggerType, List<INPCAction>>();
            Debug.Log($"[NPCTriggerActionManager.RegisterAction] Created new dictionary for NPC '{npc.GetName()}'");
        }
        if (!npcTriggerActions[npc].ContainsKey(triggerType))
        {
            npcTriggerActions[npc][triggerType] = new List<INPCAction>();
            Debug.Log($"[NPCTriggerActionManager.RegisterAction] Created new trigger list for trigger '{triggerType}' for NPC '{npc.GetName()}'");
        }
        npcTriggerActions[npc][triggerType].Add(action);
        Debug.Log($"[NPCTriggerActionManager.RegisterAction] Action '{action.GetType().Name}' registered successfully for NPC '{npc.GetName()}' on trigger '{triggerType}'");
    }


    // Metodo per deregistrare (se necessario)
    public void UnregisterAction(NPCInteractable npc, NPCTriggerType triggerType, INPCAction action)
    {
        if (npcTriggerActions.ContainsKey(npc) && npcTriggerActions[npc].ContainsKey(triggerType))
        {
            npcTriggerActions[npc][triggerType].Remove(action);
        }
    }

    // Metodo per scatenare un trigger per un dato NPC; esegue tutte le azioni associate
    public void TriggerEvent(NPCInteractable npc, NPCTriggerType triggerType, Transform target, string context)
    {
        
        if (npcTriggerActions.ContainsKey(npc) && npcTriggerActions[npc].ContainsKey(triggerType))
        {
            foreach (var action in npcTriggerActions[npc][triggerType])
            {
                action.Execute(npc, target, context);
                Debug.Log("azione triggerata");
            }
        }
        else
        {
            Debug.LogWarning($"Nessuna azione registrata per NPC {npc.GetName()} con trigger {triggerType}");
        }
    }
}
