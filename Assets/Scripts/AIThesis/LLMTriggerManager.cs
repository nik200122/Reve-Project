using System.Collections.Generic;
using UnityEngine;
public enum LLMTriggerType
{
    UserInteraction,     // Dialogo normale utente
    PlayerDetected,      // Quando il player viene rilevato
}
public class LLMTriggerManager : MonoBehaviour
{
    public static LLMTriggerManager Instance { get; private set; }

    // Mappatura: NPC -> Trigger Type -> Request Handler
    private Dictionary<NPCInteractable, Dictionary<LLMTriggerType, ILLMRequestHandler>> npcLLMHandlers =
        new Dictionary<NPCInteractable, Dictionary<LLMTriggerType, ILLMRequestHandler>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterHandler(NPCInteractable npc, LLMTriggerType triggerType, ILLMRequestHandler handler)
    {
        Debug.Log($"[LLMTriggerManager] Registering handler for NPC '{npc.GetName()}' on trigger '{triggerType}'");
        
        if (!npcLLMHandlers.ContainsKey(npc))
        {
            npcLLMHandlers[npc] = new Dictionary<LLMTriggerType, ILLMRequestHandler>();
        }
        
        npcLLMHandlers[npc][triggerType] = handler;
    }

    public void TriggerLLMRequest(NPCInteractable npc, LLMTriggerType triggerType, string userMessage = "")
    {
        Debug.Log($"[LLMTriggerManager] Triggering LLM request for NPC '{npc.GetName()}' with trigger '{triggerType}'");
        
        if (npcLLMHandlers.ContainsKey(npc) && npcLLMHandlers[npc].ContainsKey(triggerType))
        {
            var handler = npcLLMHandlers[npc][triggerType];
            
            // L'handler si occupa di tutto, usando LLMManager.Instance come servizio
            StartCoroutine(handler.HandleRequest(npc, userMessage));
            Debug.Log($"[LLMTriggerManager] Handler found and executed for trigger '{triggerType}'");
        }
        else
        {
            Debug.LogWarning($"No LLM handler registered for NPC {npc.GetName()} with trigger {triggerType}");
        }
    }

    public void UnregisterHandler(NPCInteractable npc, LLMTriggerType triggerType)
    {
        if (npcLLMHandlers.ContainsKey(npc) && npcLLMHandlers[npc].ContainsKey(triggerType))
        {
            npcLLMHandlers[npc].Remove(triggerType);
        }
    }
}