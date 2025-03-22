using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;

    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;
    [SerializeField] private CharacterInteract characterInteract;

    private void Update()
    {
        if(characterInteract.GetInteractableObject() != null && !characterInteract.GetInteractionPerformed()){
            Show(characterInteract.GetInteractableObject());
        }
        else Hide();
    }

    private void Show(IInteractable interactable){
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = interactable.GetInteractText();
    }
     private void Hide(){
        containerGameObject.SetActive(false);
    }
}
