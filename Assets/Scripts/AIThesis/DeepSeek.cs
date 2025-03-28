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


public class DeepSeek : MonoBehaviour
{
    private WorldData worldData;

    [SerializeField]private DeepSeekUI deepSeekUI;
    private NPCInteractable currentNPC;
    private string apiUrl;
    private string baseInstruction;


     private void Awake()
    {
        // Carica il file XML dalla cartella Resources/XML/ (senza estensione)
        DeepSeekConfig config = XMLHelper.LoadFromXml<DeepSeekConfig>("Assets/Resources/XML/DeepSeekConfig.xml");
        
        if(config != null)
        {
            apiUrl = config.ApiUrl;
            baseInstruction = config.Prompt;
            Debug.Log("API URL caricato: " + apiUrl);
            Debug.Log("Prompt caricato: " + baseInstruction);
        }
        else
        {
            Debug.LogError("File di configurazione DeepSeekConfig.xml non trovato o non valido.");
        }
    }

    public void SendMessageToDeepSeek(){
        string userMessage = deepSeekUI.GetInputText();
        StartCoroutine(SendRequest(userMessage));
    }

    public IEnumerator SendRequest(string userMessage){
        string pattern = @"<think>.*?</think>";
        
        // Usa le informazioni del mondo e dell'NPC
        string worldPrompt = worldData.Prompt;
        if (worldData != null)
        {
            Debug.Log("World Prompt: " + worldData.Prompt);
        }
        else
        {
            Debug.LogError("Errore nel caricamento di WorldData.");
        }
        
         
        string finalInstruction = $"{baseInstruction}\nThe following info is the info about the game world: {worldPrompt}\nThe following info is the info about the NPC:{currentNPC.GetNPCData().GetPrompt()}";
        RequestParameter parameter= new RequestParameter("system", finalInstruction);
        RequestParameter parameter1= new RequestParameter("user", userMessage);
        List<RequestParameter> requestParameters= new List<RequestParameter>
        {
            parameter,
            parameter1
        };
        RequestMessage requestMessage= new RequestMessage("Mistral Instruct", 2048, requestParameters);

        string jsonPayload = JsonUtility.ToJson(requestMessage);
        Debug.Log(jsonPayload);
        byte [] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest (apiUrl, "POST")) {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader ("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if(request.result != UnityWebRequest.Result.Success){
                Debug.Log("Error:"+ request.error);
            }
            else{
                string responseJson = request.downloadHandler.text;
                DeepSeekResponse response = JsonUtility.FromJson<DeepSeekResponse>(responseJson);
                if(response != null && response.choices.Length > 0){
                    // L'opzione Singleline fa in modo che il punto (.) corrisponda anche ai caratteri di nuova linea.
                    string output = Regex.Replace(response.choices[0].message.content, pattern, "", RegexOptions.Singleline);
                    deepSeekUI.SetResponseText(output);
                    currentNPC.SetTalk();
                    Debug.Log(response.choices[0].message.content);

                }
            }
        }
        
       
        

    }
    public void SetNPC(NPCInteractable nPC){
        this.currentNPC = nPC;
    }

    public void ActivateDialogue(){
        deepSeekUI.ActivateDialogueBox();
    }

    public void DectivateDialogue(){
         deepSeekUI.DectivateDialogueBox();
    }

    public void SetWorldInfo(WorldData worldData)
    {
        this.worldData=worldData;
    }
}



[System.Serializable]
public class DeepSeekResponse {
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
