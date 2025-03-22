using System;
using TMPro;
using UnityEngine;

public class DeepSeekUI : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI responseTMPRO;
    [SerializeField] TMP_InputField inputFieldTMPRO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetInputText(){
        return inputFieldTMPRO.text;
    }

    public void SetResponseText(string responsetext){
        responseTMPRO.text = responsetext;
    }

    public void ActivateDialogueBox(){
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState= CursorLockMode.None;
    }

    public void DectivateDialogueBox(){
        gameObject.SetActive(false);
        Cursor.visible = false;
        responseTMPRO.text = "";
        inputFieldTMPRO.text = "";

    }
}
