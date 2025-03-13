using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterStatus : MonoBehaviour
{
    private StarterAssetsInputs _input;
    [Space(10)]
    

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField] private float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    [SerializeField] private float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    [SerializeField] private bool Grounded = true;

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
    private float _targetRotation = 0.0f;
    private GameObject _mainCamera;
    private Vector3 _targetDirection;
    private Vector2 _moveInput;
     [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField] private float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField] private float BottomClamp = -30.0f;
    [Tooltip("For locking the camera position on all axis")]
    [SerializeField] private bool LockCameraPosition = false;

    // player
    
    private float targetSpeed;
    private float _rotationVelocity;
    private float _rotation;
    private float inputMagnitude;
    
    private bool isJumping;
    private bool isFalling;
    private const float _threshold = 0.01f;
    [SerializeField] private float SpeedChangeRate = 10.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private float _animationBlend;
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] private GameObject CinemachineCameraTarget;
    private PlayerInput _playerInput;
    

    private void Awake()
    {
        _input= GetComponent<StarterAssetsInputs>();
         _playerInput = GetComponent<PlayerInput>();
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        
    }
    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
			return false;
#endif
        }
    }
    void Start()
    {
         // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        GroundedCheck();
        JumpingAndFallingCheck();
        HandleMovement();
        HandleCameraRotation();
        CheckMovement();
    }

    private void CheckMovement()
    {
        _moveInput=_input.move;
    }

    private void HandleMovement()
    {
        //set target speed based on move speed, sprint speed and if sprint is pressed
        targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_moveInput == Vector2.zero) targetSpeed = 0.0f;
        inputMagnitude = _input.analogMovement ? _moveInput.magnitude : 1f;

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
        
        // normalise input direction
        Vector3 inputDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;
        if (_moveInput != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);
        }
        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
    }
    //Questa funzione disegna un Gizmo (una sfera colorata e semitrasparente) solo quando selezioni l'oggetto nella scena di Unity. Serve per visualizzare qualcosa nella Scene View (NON durante il gioco, ma solo mentre sviluppi).
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }

    private void HandleCameraRotation(){
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void JumpingAndFallingCheck()
    {
        if(Grounded){
             // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            isJumping = false;
            isFalling = false;
            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                isJumping = true;
            }
            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                isFalling = true;
            }

            // if we are not grounded, do not jump
            _input.jump = false;
        }
    }
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }
    
    public bool IsGrounded()
    {
        return Grounded;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public bool IsFalling()
    {
        return isFalling;
    }
    public float GetCinemachineTargetYaw(){
        return _cinemachineTargetYaw;
    }
    public float GetCinemachineTargetPitch(){
        return _cinemachineTargetPitch;
    }
    public float GetTargetSpeed(){
        return targetSpeed;
    }
    public float GetInputMagnitude(){
        return inputMagnitude;
    }
    public float GetRotation(){
        return _rotation;
    }

    public float GetAnimationBlend()
    {
        return _animationBlend;
    }

    public Vector3 GetTargetDirection(){
        return _targetDirection;
    }
    public Vector2 GetMoveInput(){
        return _moveInput;
    }
}
