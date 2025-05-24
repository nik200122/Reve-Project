using UnityEngine;

public static class LLMHandlerFactory
{
    public static ILLMRequestHandler CreateHandler(LLMTriggerType triggerType, string customPrompt)
    {
        Debug.Log($"[LLMHandlerFactory] Creating handler for trigger '{triggerType}' with custom prompt: '{customPrompt}'");
        
        return triggerType switch
        {
            LLMTriggerType.UserInteraction => new UserInputLLMHandler(),
            LLMTriggerType.PlayerDetected => new AutomatedLLMHandler(customPrompt),
            _ => throw new System.NotImplementedException(),
        };
    }
}