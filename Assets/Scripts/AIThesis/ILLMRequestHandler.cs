using System.Collections;
using UnityEngine;

public interface ILLMRequestHandler
{
    IEnumerator HandleRequest(NPCInteractable npc, string userMessage);
}