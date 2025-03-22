using TMPro;
using UnityEngine;

public class NPCUIDialogueManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TMPro.TextMeshProUGUI responseTMPRO;

    public void ActivateDialogueBox(){
        canvas.gameObject.SetActive(true);
    }

    public void DectivateDialogueBox(){
        canvas.gameObject.SetActive(false);
    }

    public void setText(string text){
        responseTMPRO.text = text;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
}
