using System.Collections.Generic;

[System.Serializable]
public class ConversationHistory
{
    // Parametro configurabile: numero massimo di messaggi da mantenere (può essere impostato da editor o in un file di configurazione)
    public int maxHistoryMessages = 20;
    
    // Lista dei messaggi della conversazione
    public List<RequestParameter> messages = new List<RequestParameter>();

    // Aggiunge un nuovo messaggio e "trimma" lo storico se necessario
    public void AddMessage(RequestParameter newMessage)
    {
        messages.Add(newMessage);
        TrimHistory();
    }

    // Se lo storico supera il massimo, rimuove i messaggi più vecchi (per esempio, rimuovi metà dei messaggi più vecchi)
    private void TrimHistory()
    {
        if(messages.Count > maxHistoryMessages)
        {
            // Rimuovi il 50% dei messaggi più vecchi
            int removeCount = messages.Count - maxHistoryMessages;
            messages.RemoveRange(0, removeCount);
        }
    }
    
    // Restituisce una copia della lista (per evitare modifiche esterne non controllate)
    public List<RequestParameter> GetMessages()
    {
        return new List<RequestParameter>(messages);
    }
}
