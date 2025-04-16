using System;
using TMPro;
using UnityEngine;

public class LLMUIInput : MonoBehaviour
{
    [SerializeField] TMP_InputField inputFieldTMPRO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created    
    
    public string GetInputText()
    {
        return inputFieldTMPRO.text;
    }

    public void Hide()
    {
        inputFieldTMPRO.gameObject.SetActive(false);
        Cursor.visible = false;
    }
    public void Show()
    {
        inputFieldTMPRO.gameObject.SetActive(true);
        inputFieldTMPRO.ActivateInputField();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void FocusInput(){
        inputFieldTMPRO.ActivateInputField();
    }
}
