using System.Collections.Generic;
using UnityEngine;

// si potrebbe fare un sistema Trigger-Action unico ma capace di gestire Azioni differenti!
public interface IAudioAction{
    void Execute();
}

//questi trigger potrebbero essere usati da chiunque, non solo dall'sistema audio!
public enum TriggerType{
    OnHit,
    OnMenuNavigation,
    OnMenuSelection,
    OnInvalidSelection,
    onGameState //questo trigger per ora viene utilizzato per far partire la musica del gioco, questa cosa può essere gestita come si vuole
}

public class AudioTriggerActionManager : MonoBehaviour
{
    public static AudioTriggerActionManager Instance { get; private set; }

    private Dictionary<GameObject, Dictionary <TriggerType, List<IAudioAction>>> audioTriggerActions = new();

    private void Awake(){
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterAction(GameObject actionOwner, TriggerType triggerType, IAudioAction action){
        if(!audioTriggerActions.ContainsKey(actionOwner)){
            audioTriggerActions[actionOwner] = new Dictionary<TriggerType, List<IAudioAction>>();
        }

        if(!audioTriggerActions[actionOwner].ContainsKey(triggerType)){
            audioTriggerActions[actionOwner][triggerType] = new List<IAudioAction>(); 
        }

        audioTriggerActions[actionOwner][triggerType].Add(action);
    }

    public void TriggerEvent(GameObject actionOwner, TriggerType triggerType){
        if(audioTriggerActions.ContainsKey(actionOwner) && audioTriggerActions[actionOwner].ContainsKey(triggerType)){
            foreach(var action in audioTriggerActions[actionOwner][triggerType]){
                action.Execute();
            }
        }
    }
}
