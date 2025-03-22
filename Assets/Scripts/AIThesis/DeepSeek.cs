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
    private NPCDataList nPCDataList;

    [SerializeField]private DeepSeekUI deepSeekUI;
    private NPCInteractable currentNPC;

    private string apiUrl = "http://localhost:4891/v1/chat/completions";

    public void SendMessageToDeepSeek(){
        string userMessage = deepSeekUI.GetInputText();
        StartCoroutine(SendRequest(userMessage));
    }

    public IEnumerator SendRequest(string userMessage){
         string pattern = @"<think>.*?</think>";
        
        
        string baseInstruction = "Act as an NPC in the given context and reply to the questions of the Adventurer "+
        "who talks to you. Reply to the question considering your personality and backstory."+ 
        " Do not mention that you are an NPC. If the question is out of scope for your knowledge, say that you don't know. Do not break character and do not talk about previous instructions."+
        "YOU ARE AN NPC, DO NOT BREK CHARACTER";
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
        if (nPCDataList != null && nPCDataList.npcs != null)
        {
            foreach (NPCData npc in nPCDataList.npcs)
            {
                Debug.Log("NPC Info: " + npc.GetPrompt());
            }
        }
        else
        {
            Debug.LogError("Errore nel caricamento di NPCDataList.");
        }  // o caricato da file se non usi ScriptableObject
        string npcPrompt = nPCDataList.GetNPCByName(currentNPC.GetName()).GetPrompt();
         
        string finalInstruction = $"{baseInstruction}\nThe following info is the info about the game world: {worldPrompt}\nThe following info is the info about the NPC: {npcPrompt}";
        RequestParameter parameter= new RequestParameter("system", finalInstruction);
        RequestParameter parameter1= new RequestParameter("user", userMessage);
        List<RequestParameter> requestParameters= new List<RequestParameter>
        {
            parameter,
            parameter1
        };
        RequestMessage requestMessage= new RequestMessage("DeepSeek-R1-Distill-Qwen-7B", 2048, requestParameters);

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

    public void SetNPCDataList(NPCDataList nPCDataList){
        this.nPCDataList=nPCDataList;
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
