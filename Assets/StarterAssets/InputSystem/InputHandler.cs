using System;
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
		public bool interact;
		public bool sprint;
		public bool roll;
		public bool menuAction;
		public bool scrollUpAction;
		public bool scrollDownAction;
		public bool back;
		public bool esc;
		public bool attack;
		public bool ability;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		// Rimuovi il booleano attack e aggiungi l'evento
    	public event Action OnAttackEvent;

		public void OnMove(InputAction.CallbackContext context)
		{
			move=context.ReadValue<Vector2>();
		}
		public void OnBack(InputAction.CallbackContext context)
		{
			back=context.performed;
		}
		public void OnEsc(InputAction.CallbackContext context)
		{
			esc=context.performed;
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
		public void OnAttack(InputAction.CallbackContext context)
		{
			 if (context.performed)
            	OnAttackEvent?.Invoke();
		}

		public void OnAbilities(InputAction.CallbackContext context)
		{
			ability = context.performed;
		}

		public void OnSprint(InputAction.CallbackContext context)
		{
			
			sprint=context.performed;
		}
		public void OnInteract(InputAction.CallbackContext context)
		{
			
			interact=context.performed;
		}

		public void OnMenuAction(InputAction.CallbackContext context)
		{	
			menuAction = context.performed;
		}

		public void OnScrollDownAction(InputAction.CallbackContext context){
			scrollDownAction = context.performed;
		}

		public void OnScrollUpAction(InputAction.CallbackContext context){
			scrollUpAction = context.performed;
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
	