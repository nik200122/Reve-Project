using UnityEngine;

public class NPCDetectionZone : MonoBehaviour
{
    [Header("Detection Settings")]
    private float detectionRadius;
    private float detectionAngle;

    private Transform playerTransform;
    private NPCInteractable parentNPC;
    private bool isPlayerDetected = false;

    private void Awake()
    {
        parentNPC = GetComponentInParent<NPCInteractable>();
        

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player non trovato nella scena!");
        }
    }


    private void Update()
    {
        if (playerTransform == null) return;

        // Calcola la direzione e la distanza verso il player
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Determina se il player è visibile in questo frame
        bool playerIsVisible = distanceToPlayer <= detectionRadius && angleToPlayer <= detectionAngle / 2f;

        // Solo se cambia lo stato, notifichiamo
        if (playerIsVisible && !isPlayerDetected)
        {
            isPlayerDetected = true;
            parentNPC.OnPlayerDetected();
        }
        else if (!playerIsVisible && isPlayerDetected)
        {
            isPlayerDetected = false;
            parentNPC.OnPlayerLost();
        }
    }
    public void ConfigureDetection(float radius, float angle)
    {
        detectionRadius = radius;
        detectionAngle = angle;
    }

}
