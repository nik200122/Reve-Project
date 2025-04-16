using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCStatus : MonoBehaviour
{

    public enum MovementType
    {
        Idle,
        Passive,
        Follow,
        Flee,
        Aggressive,
        Cautious,
        Approach,
    }

    private Vector3 lastKnownTargetPosition;
    private bool hasDestination = false;
    private MovementType currentMovementType = MovementType.Idle;
    [SerializeField] private Transform target;
    private float currentSpeed = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speedChangeRate = 5.0f;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float fleeDistance = 10.0f;
    [SerializeField] private float cautiousDistance = 5.0f;
    [SerializeField] private float followDistance = 3.0f;
    [SerializeField] private float approachSpeed = 3f;

    private Vector3 destination;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null && currentMovementType != MovementType.Idle && !hasDestination)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * speedChangeRate);
            return;
        }
        CheckIdle();
        CheckPassive();
        CheckFollow();
        CheckFlee();
        CheckAggressive();
        CheckCautious();
        CheckApproach();

    }


private void CheckCautious()
{
    if (currentMovementType == MovementType.Cautious){
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            Debug.Log("dt"+distanceToTarget);
            Debug.Log("fd"+cautiousDistance);            lastKnownTargetPosition = target.position;
            
            if (distanceToTarget < cautiousDistance)
            {
                // Step back a bit
                Vector3 retreatDirection = transform.position - target.position;
                Vector3 retreatPosition = transform.position + retreatDirection.normalized * 2f;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(retreatPosition, out hit, 2f, NavMesh.AllAreas))
                {
                    destination = hit.position;
                    hasDestination = true;
                    Debug.Log(destination);
                }
                currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, Time.deltaTime * speedChangeRate);
            }
            else
            {
                // Se è abbastanza lontano, non serve ritirarsi
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * speedChangeRate);
                ClearDestination();
            }

            
        }
        //Debug.Log($"[Cautious] MoveTo: {destination}, CurrentPos: {transform.position}, Speed: {currentSpeed}, HasDestination: {hasDestination}");
    }
}

    private void CheckAggressive()
    {
        if(currentMovementType == MovementType.Aggressive){
            if (target != null)
                {
                    destination = target.position;
                    lastKnownTargetPosition = target.position;
                    currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, Time.deltaTime * speedChangeRate);
                }
        }
                
    }

    private void CheckFlee()
    {
        if(currentMovementType == MovementType.Flee){
             float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (target != null)
                {
                    Debug.Log("flee");
                    Vector3 fleeDirection = transform.position - target.position;
                    Vector3 fleePosition = transform.position + fleeDirection.normalized * fleeDistance;
                    if (NavMesh.SamplePosition(fleePosition, out var hit, 2f, NavMesh.AllAreas)) {
                        destination = hit.position;
                        hasDestination = true;
                    }
                    currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, Time.deltaTime * speedChangeRate);
                }
                // Check if reached destination
            float distanceToDestination = Vector3.Distance(transform.position, destination);
            if (hasDestination && distanceToDestination < 0.5f && distanceToTarget > fleeDistance)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * speedChangeRate);
                ClearDestination();
                Debug.Log("Flee complete, switching to Idle.");
            }
        }
                
    }

    private void CheckFollow()
    {
        if (currentMovementType == MovementType.Follow){
            if (target != null)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    lastKnownTargetPosition = target.position;
                    
                    if (distanceToTarget > followDistance)
                    {
                        currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, Time.deltaTime * speedChangeRate);
                        destination = target.position;
                    }
                    else
                    {
                        currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * speedChangeRate);

                    }
                }
        }
                
    }

    private void CheckApproach()
    {
        if (currentMovementType == MovementType.Approach && target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget > 1f) // condizione per avvicinarsi gradualmente
            {
                currentSpeed = Mathf.Lerp(currentSpeed, approachSpeed, Time.deltaTime * speedChangeRate);
                destination = target.position;
                hasDestination = true;
            }
            else
            {
                // Raggiunto il target
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * speedChangeRate);
                ClearDestination();
            }
        }
    }

    private void CheckPassive()
    {
        if(currentMovementType == MovementType.Passive){
            currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed * 0.5f, Time.deltaTime * speedChangeRate);
        }
    }

    private void CheckIdle()
    {
        if(currentMovementType == MovementType.Idle){
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * speedChangeRate);
        }
    }

    public Vector3 GetDestination(){
        return destination;
    }

    public float GetSpeed(){
        return currentSpeed / runSpeed;
    }
    
    public float GetCurrentSpeed(){
        return currentSpeed;
    }

    public void TriggerAction(Transform newTarget, MovementType movementType)
    {
        SetTarget(newTarget, movementType);
    }
    public void SetCautiousDistance(float value)
    {
        cautiousDistance = value;
    }

    public void SetTarget(Transform newTarget, MovementType movementType)
    {
        target = newTarget;
        currentMovementType = movementType;
        hasDestination = false;
    }

    public void ClearDestination() {
        hasDestination = false;
        if (currentMovementType != MovementType.Idle)
            currentMovementType = MovementType.Idle;
    }



    public void SetApproachSpeed(float speed)
    {
        approachSpeed = speed;
    }





}
