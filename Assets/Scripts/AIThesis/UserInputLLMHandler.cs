using System.Collections;
using UnityEngine;

public class UserInputLLMHandler : ILLMRequestHandler
{
    public IEnumerator HandleRequest(NPCInteractable npc, string userMessage)
    {
        Debug.Log($"[UserInputLLMHandler] Handling user input for NPC '{npc.GetName()}'");
        
        // Costruisci il prompt di sistema standard per le conversazioni
        string systemPrompt = BuildConversationPrompt(npc);
        
        // Usa LLMManager.Instance come servizio per inviare la richiesta
        yield return LLMManager.Instance.StartCoroutine(LLMManager.Instance.SendLLMRequest(npc, systemPrompt, userMessage));
    }

    private string BuildConversationPrompt(NPCInteractable npc)
    {
        string basePrompt = LLMManager.Instance.GetBaseInstruction();
        string worldPrompt = LLMManager.Instance.GetWorldData()?.Prompt ?? "";
        string npcPrompt = npc.GetNPCData().GetPrompt();
        
        return $"{basePrompt}\nThe following info is the info about the game world: {worldPrompt}\nThe following info is the info about the NPC you are: {npcPrompt}\n";
    }
}