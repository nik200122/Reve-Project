using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{

    
    private NavMeshAgent agent;
    private Rigidbody rb;

    private NPCStatus nPCStatus;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        nPCStatus = GetComponent<NPCStatus>();

    }

    void Update()
    {
        

        if (agent.hasPath && agent.remainingDistance <= agent.stoppingDistance + 0.1f) {
            if (!agent.pathPending && agent.velocity.sqrMagnitude < 0.01f) {
                agent.speed = 0f;
                nPCStatus.ClearDestination();// per resettare lo stato
            }
        }
        else {
            agent.destination = nPCStatus.GetDestination();
            agent.speed = nPCStatus.GetCurrentSpeed();
        }
        Debug.DrawLine(transform.position, agent.destination, Color.yellow);


    }

}