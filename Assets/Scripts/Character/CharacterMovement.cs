using StarterAssets;
using UnityEditor.PackageManager;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif
public class CharacterMovement : MonoBehaviour
{
    [Header("Player")]

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    [SerializeField] private float CameraAngleOverride = 0.0f;
    
    [Tooltip("The height the player can jump")]
    [SerializeField] private float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    [SerializeField] private float Gravity = -15.0f;

    // cinemachine
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;
    
    private CharacterController controller;
    private CharacterStatus characterStatus;
 
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] private GameObject CinemachineCameraTarget;

    [Tooltip("Acceleration and deceleration")]
    [SerializeField] private float SpeedChangeRate = 10.0f;

    private float speed;

    private void Awake()
    {   
        characterStatus=GetComponent<CharacterStatus>();
        controller=GetComponent<CharacterController>();
    }

    private void Update()
    {
       Move();
       JumpAndGravity();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(characterStatus.GetCinemachineTargetPitch() + CameraAngleOverride,
            characterStatus.GetCinemachineTargetYaw(), 0.0f);
    }

    private void Move()
    {
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
        float speedOffset = 0.1f;
        
        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < characterStatus.GetTargetSpeed() - speedOffset ||
            currentHorizontalSpeed > characterStatus.GetTargetSpeed() + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            speed = Mathf.Lerp(currentHorizontalSpeed, characterStatus.GetTargetSpeed() * characterStatus.GetInputMagnitude(),
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = characterStatus.GetTargetSpeed();
        }
        // normalise input direction

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (characterStatus.GetMoveInput() != Vector2.zero)
        {
            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, characterStatus.GetRotation(), 0.0f);
        }

        // move the player
        controller.Move(characterStatus.GetTargetDirection().normalized * (speed * Time.deltaTime) +
                        new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void JumpAndGravity()
    {
        if (characterStatus.IsGrounded())
        {
            // stop our velocity dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }
            // Jump
            if (characterStatus.IsJumping())
            {
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += Gravity * Time.deltaTime;
        }
    }
}