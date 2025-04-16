using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class LLMResponseHandler
{
    

    public LLMResponse ParseResponse(string jsonResponse)
    {
        

        // Deserializza nel formato personalizzato
        LLMResponse actionResponse = null;
        try
        {
            actionResponse = JsonUtility.FromJson<LLMResponse>(jsonResponse);
        }
        catch (Exception ex)
        {
            Debug.LogError("Errore nella deserializzazione della risposta personalizzata: " + ex.Message);
        }
        
        // Fallback: se il parsing fallisce, imposta default "talk"
        if(actionResponse == null)
        {
            Debug.LogWarning("Formato JSON non valido. Uso azione di default: 'talk'.");
            actionResponse = new LLMResponse
            {
                utterance = jsonResponse, // Usa comunque il testo per il dialogo
                action = "default"
            };
        }
        
        return actionResponse;
    }
}
