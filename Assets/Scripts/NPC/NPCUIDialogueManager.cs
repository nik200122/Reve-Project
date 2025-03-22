using TMPro;
using UnityEngine;

public class NPCUIDialogueManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TMPro.TextMeshProUGUI responseTMPRO;

    public void ActivateDialogueBox(){
        canvas.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DectivateDialogueBox(){
        canvas.gameObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void setText(string text){
        responseTMPRO.text = text;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
}
