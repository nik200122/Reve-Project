using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEditor;
using System.Text;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Events;


public class LLMManager : MonoBehaviour
{
    private WorldData worldData;
    [SerializeField] private InputHandler inputHandler;

    [SerializeField]private LLMUI LLMUI;
    private NPCInteractable currentNPC;
    private string apiUrl;
    private string baseInstruction;

    private string automatedActionsInstructions;
    private LLMResponseHandler responseHandler = new LLMResponseHandler();
    private const string pattern = @"<think>.*?</think>";

    private void Start()
    {
        inputHandler.OnSendEvent += SendMessage;
    }
    private void Awake(){
        // Carica il file XML dalla cartella Resources/XML/ (senza estensione)
        LLMConfig config = XMLHelper.LoadFromXml<LLMConfig>("Assets/Resources/XML/DeepSeekConfig.xml");
        
        if(config != null)
        {
            apiUrl = config.ApiUrl;
            baseInstruction = config.Prompt;
            automatedActionsInstructions = config.automatedActionsInstructions;
            Debug.Log("API URL caricato: " + apiUrl);
            Debug.Log("Prompt caricato: " + baseInstruction);
        }
        else
        {
            Debug.LogError("File di configurazione DeepSeekConfig.xml non trovato o non valido.");
        }
    }

    public void SendMessage(){
        if(GameStateManager.Instance.CurrentState == GameState.Interaction){
            string userMessage = LLMUI.GetInputText();
            StartCoroutine(SendRequest(userMessage));
        }
        
    }

    public void SendAutomatedRequest(NPCInteractable npc)
    {
        // Set the current NPC
        SetNPC(npc);
        
        // Send request without activating UI
        StartCoroutine(SendAutomatedRequestCoroutine());
    }

    private IEnumerator SendAutomatedRequestCoroutine()
    {
        // This is similar to your SendRequest method, but doesn't involve the UI
        
        // Usa le informazioni del mondo e dell'NPC
        string worldPrompt = worldData.Prompt;
        
        // Crea il prompt di sistema combinato
        string finalInstruction = $"{baseInstruction}\nThe following info is the info about the game world: {worldPrompt}\nThe following info is the info about the NPC you are: {currentNPC.GetNPCData().GetPrompt()}";
        
        // Recupera lo storico di conversazione dell'NPC
        ConversationHistory history = currentNPC.GetConversationHistory();
        
        // Se è la prima volta che parliamo, aggiungiamo il prompt di sistema
        if(history.messages.Count == 0)
        {
            history.AddMessage(new RequestParameter("system", finalInstruction));
        }
        
        // Aggiungi il messaggio automatico allo storico
        history.AddMessage(new RequestParameter("user", automatedActionsInstructions));
        
        // Rest of your code for sending the request and processing the response...
        RequestMessage requestMessage = new RequestMessage("mythomist-7b.Q6_K.gguf", 128, history.GetMessages());
        
        string jsonPayload = JsonUtility.ToJson(requestMessage);
        Debug.Log("Sending automated request: " + jsonPayload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Errore nella richiesta: " + request.error);
            }
            else
            {
                string responseJson = request.downloadHandler.text;
                // Deserializzazione della risposta API standard
                LLMAPIResponse apiResponse = JsonUtility.FromJson<LLMAPIResponse>(responseJson);
        
                // Estrae il contenuto e lo pulisce
                string rawContent = apiResponse.choices[0].message.content;
                string cleanContent = Regex.Replace(rawContent, pattern, "", RegexOptions.Singleline);
                Debug.Log("Automated response: " + cleanContent);
                
                LLMResponse actionResponse = responseHandler.ParseResponse(cleanContent);
                // Mostra anche il testo della risposta nel pannello
                LLMUI.SetResponseText(actionResponse.utterance);
                
                //LLMUI.SetResponseText(actionResponse.utterance);
                // Execute the action using the action registry
                 NPCTriggerActionManager.Instance.TriggerEvent(
                        currentNPC,
                        NPCTriggerType.GiveItem,
                        GameObject.FindGameObjectWithTag("Player").transform, 
                        actionResponse.utterance
                );
                
                // Add the response to conversation history
                history.AddMessage(new RequestParameter("assistant", cleanContent));
            }
        }
    }

    public IEnumerator SendRequest(string userMessage){
        
        // Usa le informazioni del mondo e dell'NPC
        string worldPrompt = worldData.Prompt;
        
        // Crea il prompt di sistema combinato
        string finalInstruction = $"{baseInstruction}\nThe following info is the info about the game world: {worldPrompt}\nThe following info is the info about the NPC you are: {currentNPC.GetNPCData().GetPrompt()}";
        
        // Recupera lo storico di conversazione dell'NPC
        ConversationHistory history = currentNPC.GetConversationHistory();
        
        // Se è la prima volta che parliamo, aggiungiamo il prompt di sistema
        if(history.messages.Count == 0)
        {
            history.AddMessage(new RequestParameter("system", finalInstruction));
        }
        
        // Aggiungi il messaggio dell'utente allo storico
        history.AddMessage(new RequestParameter("user", userMessage));
        
        // Prepara la richiesta con lo storico corrente
        RequestMessage requestMessage = new RequestMessage("mythomist-7b.Q6_K.gguf", 256, history.GetMessages());

        string jsonPayload = JsonUtility.ToJson(requestMessage);
        Debug.Log(jsonPayload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Errore nella richiesta: " + request.error);
                }
                else
                {
                    string responseJson = request.downloadHandler.text;
                    // Deserializzazione della risposta API standard
                    LLMAPIResponse apiResponse = JsonUtility.FromJson<LLMAPIResponse>(responseJson);
        
                    // Estrae il contenuto e lo pulisce
                    string rawContent = apiResponse.choices[0].message.content;
                    string cleanContent = Regex.Replace(rawContent, pattern, "", RegexOptions.Singleline);
                    Debug.Log(cleanContent);
                    LLMResponse actionResponse = responseHandler.ParseResponse(cleanContent);
                    LLMUI.SetResponseText(actionResponse.utterance);
                    // Execute the action using the action registry
                    NPCTriggerActionManager.Instance.TriggerEvent(
                        currentNPC,
                        NPCTriggerType.WarnOther,
                        GameObject.FindGameObjectWithTag("Player").transform, 
                        actionResponse.utterance
                    );
                    history.AddMessage(new RequestParameter("assistant", cleanContent));
                }
            }
}

    public void SetNPC(NPCInteractable nPC){
        this.currentNPC = nPC;
    }

    public void ActivateDialogue(){
        LLMUI.ActivateDialogueBox();
    }

    public void DectivateDialogue(){
         LLMUI.DectivateDialogueBox();
    }

    public void SetWorldInfo(WorldData worldData)
    {
        this.worldData=worldData;
    }
}



[System.Serializable]
public class LLMAPIResponse {
    public Choice[] choices;
}

[System.Serializable]
public class Choice {
    public Message message;
}
[System.Serializable]
public class Message {
    public string content;
}

[System.Serializable]
public class RequestMessage {
    public RequestMessage(String model, int max_tokens, List<RequestParameter> messages){
        this.model = model;
        this.max_tokens = max_tokens;
        this.messages = messages;
    }
    public string model;
    public int max_tokens;

    public List<RequestParameter> messages;
}

[System.Serializable]
public class RequestParameter {
    public RequestParameter(String role, String content){
        this.role = role;
        this.content = content;
    }
    public string role;
    public string content;

    
}
