using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterStatus : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private InputHandler input;
    [Space(10)]

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField] private float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    [SerializeField] private float FallTimeout = 0.15f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    [SerializeField] private float RollTimeout = 1.1f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    [SerializeField] private bool isGrounded = true;

    [Tooltip("Useful for rough ground")]
    [SerializeField] private float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    [SerializeField] private float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    [SerializeField] private LayerMask GroundLayers;

    [Tooltip("Move speed of the character in m/s")]
    [SerializeField] private float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    [SerializeField] private float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    [SerializeField] private float RotationSmoothTime = 0.12f;
    private float targetRotation = 0.0f;

    private Vector3 targetDirection;
    private Vector3 rollDirection;
    private Vector2 moveInput;

    private static bool isAttacking = false;
    private Vector3 attackDirection;
    
    

    // player
    private float targetSpeed;
    private float rotationVelocity;
    private float rotation;
    private float inputMagnitude;
    
    private bool isJumping;
    private bool isWalking;
    private bool isFalling;
    private static bool isRolling;
    private bool canMove = true;
    private bool isHit;
    private bool isDead = false;

    private const float _threshold = 0.01f;
    [SerializeField] private float SpeedChangeRate = 10.0f;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    private float rollTimeoutDelta;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject mainCamera;
    private float animationBlend;
    private static bool canAttack;

   

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        //input= GetComponent<InputHandler>();
        //playerInput = GetComponent<PlayerInput>();
        // get a reference to our main camera
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
   
    void Start()
    {
        // reset our timeouts on start
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        
    }

    
    
    private void OnDisable()
    {
        if(GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }
    
    private void HandleGameStateChanged(GameState newState)
    {
        // Blocca il movimento se non siamo in FreeRoam
        canMove = newState == GameState.FreeRoam;
    }

    // Update is called once per frame
    void Update()
    {
        AttackCheck();
        RollingCheck();
        //Debug.Log("input"+ input.roll+"isRolling Check:"+isRolling);
        GroundedCheck();
        JumpingAndFallingCheck();
        HandleMovement();
        CheckMovement();
        CheckIsWalking();
    }

    private void CheckIsWalking()
    {
        isWalking = moveInput != Vector2.zero;;
    }

    private void CheckMovement()
    {
        moveInput=input.move;
    }

    private void HandleMovement()
    {
        //set target speed based on move speed, sprint speed and if sprint is pressed
        
        targetSpeed = input.sprint ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (moveInput == Vector2.zero) targetSpeed = 0.0f;
        inputMagnitude = input.analogMovement ? moveInput.magnitude : 1f;

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;
        
        // normalise input direction
        Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;
        if (moveInput != Vector2.zero)
        {
            // Calcola l'angolo target in base all'input e alla rotazione della camera
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                            mainCamera.transform.eulerAngles.y;
            
            // Calcola la differenza angolare (in valore assoluto)
            float currentY = transform.eulerAngles.y;
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentY, targetRotation));
            
            // Imposta un tempo di smorzamento "base", poi lo riduci se la differenza è elevata
            float adjustedSmoothTime = RotationSmoothTime;
            if (angleDifference > 120f) // Se la differenza è maggiore di 120 gradi (puoi modificare il valore)
            {
                adjustedSmoothTime = RotationSmoothTime * 0.25f; // dimezza il tempo per una rotazione più veloce
            }
            
            // Calcola la rotazione con lo smooth time modificato
            rotation = Mathf.SmoothDampAngle(currentY, targetRotation, ref rotationVelocity, adjustedSmoothTime);
        }
        targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
    }
    
    //Questa funzione disegna un Gizmo (una sfera colorata e semitrasparente) solo quando selezioni l'oggetto nella scena di Unity. Serve per visualizzare qualcosa nella Scene View (NON durante il gioco, ma solo mentre sviluppi).
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }

    private void JumpingAndFallingCheck()
    {
        if(isGrounded){
             // reset the fall timeout timer
            fallTimeoutDelta = FallTimeout;

            isJumping = false;
            isFalling = false;
            if (jumpTimeoutDelta > 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
            else if (input.jump && jumpTimeoutDelta <= 0.0f && !isRolling)
            {
                isJumping = true;
            }
            // jump timeout
            
        }
        else
        {
            // reset the jump timeout timer
            jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                isFalling = true;
            }
        }
    }
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }
    private void RollingCheck(){   
        

        // Il personaggio può rollare solo se è a terra e non sta già rollando
        //Debug.Log(isAttacking);
        if (!isAttacking && isGrounded && input.roll){
            isRolling = true;
             // Cattura la direzione attuale al momento dell'attivazione del roll
            rollDirection = GetTargetDirection();
        }
    }
    private void AttackCheck(){
        if(!isRolling && !isJumping && canMove){
            canAttack = true;
        }
        else canAttack = false;
    }
    // Metodi per attivare/disattivare l'attacco
    public void StartAttackMovement()
    {
        isAttacking = true;
        //attackDirection = GetTargetDirection();
    }

    public void EndAttackMovement()
    {
        isAttacking = false;
    }
    public bool IsAttacking()
    {
        return isAttacking;
    }

    public Vector3 GetAttackDirection()
    {
        return attackDirection;
    }
    
    public bool GetCanAttack()
    {
        return canAttack;
    }

    public void RollingCompleted(){
        isRolling = false;
    }


    
    public bool IsGrounded()
    {
        return isGrounded;
    }
    public bool IsRolling()
    {
        return isRolling;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public bool IsFalling()
    {
        return isFalling;
    }
    public float GetTargetSpeed(){
        return targetSpeed;
    }
    public float GetInputMagnitude(){
        return inputMagnitude;
    }
    public float GetRotation(){
        return rotation;
    }

    public float GetAnimationBlend()
    {
        return animationBlend;
    }

    public Vector3 GetTargetDirection(){
        return targetDirection;
    }
    public Vector3 GetRollDirection(){
        return rollDirection;
    }
    public Vector2 GetMoveInput(){
        return moveInput;
    }

    public bool IsDead(){
        return isDead;
    }

    public void SetIsDead(bool value){
        isDead = value;
        Collider collider= GetComponent<Collider>();
        collider.enabled = false;
    }

    public bool IsHit(){
        return isHit;
    }

    public void SetIsHit(bool value){
        isHit = value;
        audioManager.PlaySound(SoundTypeTag.Hit);
        isHit = false;
    }
}
