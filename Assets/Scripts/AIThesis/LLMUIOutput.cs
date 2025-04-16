using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LLMUIOutput : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI responseTMPRO;

    [SerializeField] GameObject responsePanel; // Riferimento al pannello della risposta
    private Coroutine hideResponseCoroutine;

    public void Hide()
    {
        responsePanel.gameObject.SetActive(false);
    }
    public void Show()
    {
        responsePanel.gameObject.SetActive(true);
    }

    private IEnumerator HideResponseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Nascondi il pannello della risposta dopo il ritardo
        if (responsePanel != null)
            Hide();
        
        hideResponseCoroutine = null;
    }
    public void SetResponseText(string responseText){
        Show();
        responseTMPRO.text = responseText;
        
        // Mostra il pannello della risposta
        if (responsePanel != null)
            responsePanel.SetActive(true);
        
        // Cancella eventuali coroutine attive
        if (hideResponseCoroutine != null)
            StopCoroutine(hideResponseCoroutine);
        
        // Avvia una nuova coroutine per nascondere la risposta dopo 3 secondi
        hideResponseCoroutine = StartCoroutine(HideResponseAfterDelay(5f));
    }
}
