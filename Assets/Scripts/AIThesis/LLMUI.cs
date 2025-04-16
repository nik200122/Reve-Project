using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LLMUI : MonoBehaviour
{
    [SerializeField] LLMUIInput lLMUIInput;
    [SerializeField] LLMUIOutput lLMUIOutput;
    
    void Start()
    {
        lLMUIInput.Hide();
        lLMUIOutput.Hide();
    }

    public string GetInputText()
    {
        lLMUIInput.FocusInput();
        return lLMUIInput.GetInputText();
        
    }


    public void SetResponseText(string responseText)
    {
        lLMUIOutput.SetResponseText(responseText);    
    }

    public void ActivateDialogueBox()
    {
        lLMUIInput.Show();
    }

    public void DectivateDialogueBox()
    {
       lLMUIInput.Hide();
    }
    

}