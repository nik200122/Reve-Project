using UnityEngine;

[System.Serializable]
public class NPCData
{
    public string Name;
    public string Backstory;
    public string Personality;

    public string GetPrompt()
    {
        return $"NPC Name: {Name}\nBackstory: {Backstory}\nPersonality: {Personality}";
    }
}
