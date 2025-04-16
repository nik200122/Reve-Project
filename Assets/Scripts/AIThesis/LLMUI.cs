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

    // Aggiungiamo un metodo per appendere o aggiornare la risposta dopo un delay
    public IEnumerator AppendDelayedMessage(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        // Se vuoi appendere all'esistente, potresti fare:
        // lLMUIOutput.AppendResponseText(message);
        // Oppure, se preferisci sostituire il testo:
        SetResponseText(message);
    }
    

}