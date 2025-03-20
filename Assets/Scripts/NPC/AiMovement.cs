using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    NavMeshAgent agent;
    public float magnitude;
    public float animationBlend;
    private float targetSpeed = 6.0f;
    [SerializeField] private float SpeedChangeRate = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // if there is no input, set the target speed to 0
        if (player.transform.position - transform.position == Vector3.zero) targetSpeed = 0.0f;
        magnitude = agent.velocity.magnitude;

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        agent.destination = player.transform.position;
    }
}
