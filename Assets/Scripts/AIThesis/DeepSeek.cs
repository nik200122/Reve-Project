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
    [SerializeField] TextMeshProUGUI responseTMPRO;
    [SerializeField] TMP_InputField inputFieldTMPRO;

    [SerializeField] UnityEvent OnReplyReceived;

    private string apiUrl = "http://localhost:4891/v1/chat/completions";

    public void SendMessageToDeepSeek(){
        string userMessage = inputFieldTMPRO.text;
        StartCoroutine(SendRequest(userMessage));
    }

    public IEnumerator SendRequest(string userMessage){
         string pattern = @"<think>.*?</think>";
        
        
        string directive = "Act as an NPC of a videogame who as to answer the question of the player. Your answers need to not too long";
        RequestParameter parameter= new RequestParameter("system", directive);
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
                    responseTMPRO.text = output;
                    OnReplyReceived?.Invoke();
                    Debug.Log(response.choices[0].message.content);

                }
            }
        }
        
       
        

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
