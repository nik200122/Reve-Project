using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable{

    [SerializeField] private string interactText;
    [SerializeField] private string Name;

    [SerializeField] private DeepSeek deepSeek;

    NPCAnimator nPCanimator;

    public void Awake()
    {
        nPCanimator = GetComponent<NPCAnimator>(); 
    }

    public void Interact(Transform interactorTransform){
        Debug.Log("interact!");
        deepSeek.ActivateDialogue();
        deepSeek.SetNPC(this);
    }

    public void SetTalk(){
        nPCanimator.SetTalk();
    }

    public string GetInteractText(){
        return interactText;
    }
    public string GetName(){
        return Name;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void TerminateInteract()
    {
        deepSeek.DectivateDialogue();
    }
}
