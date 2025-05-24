using System.Collections;
using UnityEngine;

public class AutomatedLLMHandler : ILLMRequestHandler
{
    private string customPrompt;

    public AutomatedLLMHandler(string customPrompt)
    {
        this.customPrompt = customPrompt;
    }

    public IEnumerator HandleRequest(NPCInteractable npc, string userMessage)
    {
        Debug.Log($"[AutomatedLLMHandler] Handling automated request for NPC '{npc.GetName()}' '");
        
        // Determina il messaggio da inviare: custom prompt o automated instructions
        string messageToSend = GetMessageToSend();
        
        // Costruisci il prompt di sistema per azioni automatiche
        string systemPrompt = BuildAutomatedPrompt(npc);
        
        // Usa LLMManager.Instance come servizio per inviare la richiesta
        yield return LLMManager.Instance.StartCoroutine(LLMManager.Instance.SendLLMRequest(npc, systemPrompt, messageToSend));
    }

    private string GetMessageToSend()
    {
        // Se c'è un prompt personalizzato nell'XML, usalo
        if (!string.IsNullOrEmpty(customPrompt))
        {
            Debug.Log($"[AutomatedLLMHandler] Using custom prompt: '{customPrompt}'");
            return customPrompt;
        }
        
        // Altrimenti usa le automated instructions standard
        Debug.Log($"[AutomatedLLMHandler] Using standard automated instructions");
        return LLMManager.Instance.GetAutomatedActionsInstructions();
    }

    private string BuildAutomatedPrompt(NPCInteractable npc)
    {
        string basePrompt = LLMManager.Instance.GetBaseInstruction();
        string worldPrompt = LLMManager.Instance.GetWorldData()?.Prompt ?? "";
        string npcPrompt = npc.GetNPCData().GetPrompt();
        
        return $"{basePrompt}\nThe following info is the info about the game world: {worldPrompt}\nThe following info is the info about the NPC you are: {npcPrompt}";
    }

}