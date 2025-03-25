using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable{

    [Header("Data Driven Settings")]
    [SerializeField] private string npcId; // Identificativo univoco associato al file XML

    // Variabili caricate dai dati
    private string interactText;
    private NPCData npcData;

    [SerializeField] private DeepSeek deepSeek;
    private NPCAnimator npcAnimator;

    private void Awake()
    {
        npcAnimator = GetComponent<NPCAnimator>();
        // Carica i dati dell'NPC dal manager
        
    }
    private void Start(){
        npcData = NPCDataManager.Instance.GetNPCData(npcId);
        if (npcData != null)
        {
            interactText = npcData.InteractText;
        }
        else
        {
            Debug.LogError("Dati NPC non trovati per id: " + npcId);
        }
    }

    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Interazione: " + interactText);
        deepSeek.ActivateDialogue();
        deepSeek.SetNPC(this);
    }

    public void SetTalk()
    {
        npcAnimator.SetTalk();
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public string GetName()
    {
        return npcData != null ? npcData.Name : gameObject.name;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void TerminateInteract()
    {
        deepSeek.DectivateDialogue();
    }
    public NPCData GetNPCData(){
        return npcData;
    }
}
