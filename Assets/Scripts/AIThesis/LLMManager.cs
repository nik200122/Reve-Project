

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
    public static LLMManager Instance { get; private set; }

    private WorldData worldData;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private LLMUI LLMUI;
    
    private NPCInteractable currentNPC;
    private string apiUrl;
    private string baseInstruction;
    private string automatedActionsInstructions;
    private LLMResponseHandler responseHandler = new LLMResponseHandler();
    private const string pattern = @"<think>.*?</think>";

    // Getters pubblici per permettere agli handlers di accedere ai dati
    public string GetApiUrl() => apiUrl;
    public string GetBaseInstruction() => baseInstruction;
    public string GetAutomatedActionsInstructions() => automatedActionsInstructions;
    public WorldData GetWorldData() => worldData;
    public LLMUI GetLLMUI() => LLMUI;
    public LLMResponseHandler GetResponseHandler() => responseHandler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opzionale se vuoi che persista tra le scene
            LoadConfiguration();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inputHandler.OnSendEvent += SendMessage;
    }

    private void LoadConfiguration()
    {
        LLMConfig config = XMLHelper.LoadFromXml<LLMConfig>("XML/DeepSeekConfig");
        
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

    // Callback per input field - ora usa il sistema di trigger
    public void SendMessage()
    {
        if(GameStateManager.Instance.CurrentState == GameState.Interaction && currentNPC != null)
        {
            string userMessage = LLMUI.GetInputText();
            LLMTriggerManager.Instance.TriggerLLMRequest(currentNPC, LLMTriggerType.UserInteraction, userMessage);
        }
    }

    // SERVIZIO per inviare richieste HTTP - usato dagli handlers
    public IEnumerator SendLLMRequest(NPCInteractable npc, string finalPrompt, string message)
    {
        Debug.Log($"[LLMManager] Sending LLM request for NPC '{npc.GetName()}' ");
        
        // Recupera lo storico della conversazione
        ConversationHistory history = npc.GetConversationHistory();
        
        // Se è la prima volta, aggiungi il prompt di sistema
        if(history.messages.Count == 0)
        {
            history.AddMessage(new RequestParameter("system", finalPrompt));
        }
        
        // Aggiungi il messaggio dell'utente allo storico
        history.AddMessage(new RequestParameter("user", message));
        
        // Prepara la richiesta con lo storico corrente
        RequestMessage requestMessage = new RequestMessage("mythomist-7b.Q6_K.gguf", 256, history.GetMessages());

        string jsonPayload = JsonUtility.ToJson(requestMessage);
        Debug.Log($"[LLMManager] JSON Payload: {jsonPayload}");
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
                ProcessLLMResponse(responseJson, npc, history);
            }
        }
    }

    private void ProcessLLMResponse(string responseJson, NPCInteractable npc, ConversationHistory history)
    {
        Debug.Log($"[LLMManager] Processing LLM response for NPC '{npc.GetName()}'");
        
        // Deserializzazione della risposta API standard
        LLMAPIResponse apiResponse = JsonUtility.FromJson<LLMAPIResponse>(responseJson);
        
        // Estrae il contenuto e lo pulisce
        string rawContent = apiResponse.choices[0].message.content;
        string cleanContent = Regex.Replace(rawContent, pattern, "", RegexOptions.Singleline);
        Debug.Log($"[LLMManager] Clean response: {cleanContent}");
        
        LLMResponse actionResponse = responseHandler.ParseResponse(cleanContent);
        
       
        LLMUI.SetResponseText(actionResponse.utterance);
        
        
        // Converti la stringa dell'azione in NPCTriggerType
        NPCTriggerType triggerTypeToExecute;
        bool parsingSuccessful = System.Enum.TryParse<NPCTriggerType>(actionResponse.action, true, out triggerTypeToExecute);

        if (parsingSuccessful)
        {
            // Execute the action using the action registry
            NPCTriggerActionManager.Instance.TriggerEvent(
                npc,
                triggerTypeToExecute,
                GameObject.FindGameObjectWithTag("Player").transform,
                actionResponse.utterance
            );
        }
        else
        {
            Debug.LogError($"[LLMManager] Impossibile convertire la stringa di azione '{actionResponse.action}' in un NPCTriggerType valido.");
            // Fallback action
            NPCTriggerActionManager.Instance.TriggerEvent(
                npc,
                NPCTriggerType.Talk,
                GameObject.FindGameObjectWithTag("Player").transform,
                actionResponse.utterance
            );
        }
        
        // Aggiungi la risposta allo storico
        history.AddMessage(new RequestParameter("assistant", cleanContent));
    }

    public void SetNPC(NPCInteractable nPC)
    {
        this.currentNPC = nPC;
    }

    public void ActivateDialogue()
    {
        LLMUI.ActivateDialogueBox();
    }

    public void DectivateDialogue()
    {
        LLMUI.DectivateDialogueBox();
    }

    public void SetWorldInfo(WorldData worldData)
    {
        this.worldData = worldData;
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