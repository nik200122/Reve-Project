using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable{

    [SerializeField] private string interactText;


    NPCUIDialogueManager uiDialogue;
    NPCAnimator nPCanimator;

    public void Awake()
    {
        uiDialogue = GetComponent<NPCUIDialogueManager>();
        nPCanimator = GetComponent<NPCAnimator>(); 
    }

    public void Interact(Transform interactorTransform){
        Debug.Log("interact!");
        uiDialogue.ActivateDialogueBox();
        uiDialogue.setText("Ciao!");
        nPCanimator.SetTalk();
    }

    public string GetInteractText(){
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void TerminateInteract()
    {
        uiDialogue.DectivateDialogueBox();
        uiDialogue.setText("");
    }
}
