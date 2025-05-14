using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacterStatus : MonoBehaviour
{
  private NavMeshAgent agent;

    [Header("Grounded Settings")]
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float GroundedOffset = -0.14f;
    [SerializeField] private float GroundedRadius = 0.98f;
    [SerializeField] private LayerMask GroundLayers;

    [Header("Movement")]
    private float animationBlend;
    private float inputMagnitude;

    [SerializeField] private float SpeedChangeRate = 10.0f;

    [Header("Falling Detection")]
    [SerializeField] private float FallTimeout = 0.15f;
    private float fallTimeoutDelta;
    private bool isFalling = false;

    private bool canMove = true;
    private bool isAttacking = false;
    private bool isHit = false;
    private bool isAttackInProgress = false;
    private bool isDead = false;

    // Properties per EnemyAnimator
    public float GetAnimationBlend() => animationBlend;
    public float GetInputMagnitude() => inputMagnitude;
    public bool IsGrounded() => isGrounded;
    public bool IsFalling() => isFalling;
    public bool IsRolling() => false; // non implementato per il nemico

    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start(){
        fallTimeoutDelta = FallTimeout;
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void Update(){
        HandleMovement();
        GroundedCheck();
        JumpingAndFallingCheck();
    }

    private void HandleMovement(){
        float currentSpeed = agent.velocity.magnitude;

        inputMagnitude = currentSpeed;
        animationBlend = Mathf.Lerp(animationBlend, currentSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f)
            animationBlend = 0f;
    }

    private void GroundedCheck(){
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }

    private void JumpingAndFallingCheck(){
        if (isGrounded){
            fallTimeoutDelta = FallTimeout;
            isFalling = false;
        }else{
            if (fallTimeoutDelta >= 0.0f){
                fallTimeoutDelta -= Time.deltaTime;
                isFalling = false;
            }else{
                isFalling = true;
            }
        }
    }

    private void HandleGameStateChanged(GameState newState){
        // Blocca il movimento se non siamo in FreeRoam
        canMove = newState == GameState.FreeRoam;
    }

    private void OnDisable(){
        if(GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    // Optional: per vedere la sfera nel Scene view
    private void OnDrawGizmosSelected(){
        Color color = isGrounded ? new Color(0, 1, 0, 0.35f) : new Color(1, 0, 0, 0.35f);
        Gizmos.color = color;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }

    public bool GetCanMove(){
        return canMove;
    }

    public void SetIsAttacking(bool value){
        isAttacking = value;
    }

    public bool IsAttacking(){
        return isAttacking;
    }

    public void SetIsAttackInProgress(bool value){
        isAttackInProgress = value;
    }

    public bool IsAttackInProgress(){
        return isAttackInProgress;
    }

    public bool IsDead(){
        return isDead;
    }

    public void SetIsDead(bool value){
        isDead = value;
        Collider collider= GetComponent<Collider>();
        collider.enabled = false;
    }

}
