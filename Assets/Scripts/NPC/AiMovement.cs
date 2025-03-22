using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    NavMeshAgent agent;
    public float magnitude;
    public float maxTime = -1.0f;
    public float maxDistance = -1.0f;

    float timer = 0.0f;
    public float animationBlend;
    private float targetSpeed;
    private float MoveSpeed = 2.0f;
    private float SprintSpeed = 6.0f;
    [SerializeField] private float SpeedChangeRate = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //per abbozzo di movimento fare uncomment
        
        /*timer -= Time.deltaTime;
        if(timer<0.0f){
            
            
            float sgDistance = (player.transform.position - agent.destination).sqrMagnitude;
            if(sgDistance > maxDistance*maxDistance){
                targetSpeed = MoveSpeed;
                agent.destination = player.transform.position;
            }
            else targetSpeed = 0;
            timer= maxTime;
        }*/

        magnitude = agent.velocity.magnitude;

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;
    }
}
