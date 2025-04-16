using UnityEngine;

public class NPCDetectionZone : MonoBehaviour
{
    private NPCInteractable parentNPC;
    
    private void Awake()
    {
        parentNPC = GetComponentInParent<NPCInteractable>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the NPC that player entered detection zone
            parentNPC.OnPlayerDetected();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the NPC that player left detection zone
            parentNPC.OnPlayerLost();
        }
    }
}