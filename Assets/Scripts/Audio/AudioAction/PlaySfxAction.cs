using System.Collections.Generic;
using UnityEngine;

public class PlaySfxAction : IAudioAction
{

    private List<AudioClip> clips = new List<AudioClip>();

    public PlaySfxAction(Dictionary<string, string> parameters){
        if(parameters.TryGetValue("sfxClips", out string audioClipList)){
            string[] audioClips = audioClipList.Split(",");
            foreach(var audioClip in audioClips){
                AudioClip clip = Resources.Load<AudioClip>(audioClip.Trim());
                if(clip != null)
                    clips.Add(clip);
                else 
                    Debug.Log("clip non trovata: "+audioClip);
            }
        }
    }

    public void Execute(){
        // Scegli una clip casuale e riproducila
        AudioClip clipToPlay = clips[Random.Range(0, clips.Count)];
        AudioService.Instance.GetSfxSource().PlayOneShot(clipToPlay);
    }
}
