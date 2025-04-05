using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteract : MonoBehaviour
{
    [SerializeField] private InputHandler input;
    private bool interactionPerformed= false;

    private void Awake()
    {
        //input = GetComponent<InputHandler>();
    }
    // Update is called once per frame
    void Update()
    {
        // Controlla se il gioco è in stato FreeRoam prima di iniziare l'interazione
        if(GameStateManager.Instance.CurrentState == GameState.FreeRoam)
        {
            if(input.interact)
            {
                IInteractable interactable = GetInteractableObject();
                if(interactable != null)
                {
                    // Cambia lo stato in InteractionInProgress
                    GameStateManager.Instance.ChangeState(GameState.Interaction);
                    interactable.Interact(transform);
                }
            }
        }
        else if(GameStateManager.Instance.CurrentState == GameState.Interaction || GameStateManager.Instance.CurrentState == GameState.ShopScreen){
            if(input.esc){
                IInteractable interactable = GetInteractableObject();
                if(interactable!=null){
                    interactable.TerminateInteract();
                    // Torna allo stato FreeRoam
                    GameStateManager.Instance.ChangeState(GameState.FreeRoam);
                }
            }
        }
    }

    public IInteractable GetInteractableObject(){
        List<IInteractable> interactables = new List<IInteractable>();
        float interactRange = 1.5f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach(Collider collider in colliderArray){
                if(collider.TryGetComponent(out IInteractable interactable)){
                    interactables.Add(interactable);
                    
                }
            }
            IInteractable closestInteractable = null;
            foreach (IInteractable interactable in interactables) {
                if (closestInteractable == null) {
                    closestInteractable = interactable;
                } 
                else {
                    if (Vector3. Distance(transform.position, interactable.GetTransform().position) <
                        Vector3. Distance(transform.position, closestInteractable.GetTransform().position)) {
                        // Closer
                        closestInteractable = interactable;
                    }
                }
            }

            return closestInteractable;
    }

    public bool GetInteractionPerformed(){
        return interactionPerformed;
    }
}
