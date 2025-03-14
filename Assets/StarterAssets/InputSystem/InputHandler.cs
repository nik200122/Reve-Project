using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


	public class InputHandler : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool roll;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public void OnMove(InputAction.CallbackContext context)
		{
			move=context.ReadValue<Vector2>();
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			if(cursorInputForLook)
			{
				look=context.ReadValue<Vector2>();
			}
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			jump=context.performed;
		}
		public void OnRoll(InputAction.CallbackContext context)
		{
			roll = context.performed;
		}

		public void OnSprint(InputAction.CallbackContext context)
		{
			sprint=context.performed;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	