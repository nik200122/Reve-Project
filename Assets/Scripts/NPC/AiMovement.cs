using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent agent;
    private Rigidbody rb;

    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float chaseDistance = 5.0f;
    [SerializeField] private float speedChangeRate = 5.0f;

    private float currentSpeed = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        /*float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Se il player è vicino, cammina, altrimenti corri
        float targetSpeed = distanceToPlayer > chaseDistance ? runSpeed : walkSpeed;
        agent.destination = player.position;

        // Interpola la velocità per evitare cambi bruschi
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate);*/
        agent.speed = 0.0f;

    }
    void FixedUpdate()
    {
        // Applica la posizione calcolata dal NavMeshAgent al Rigidbody
        Vector3 nextPosition = agent.nextPosition;
        //rb.MovePosition(nextPosition);
    }

    public float GetSpeed(){
        return currentSpeed/runSpeed;
    }
}


