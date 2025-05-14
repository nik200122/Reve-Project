using System.Collections.Generic;
using UnityEngine;

public class PlayMusicAction : IAudioAction
{
    private List<AudioClip> clips = new List<AudioClip>();
    
        public PlayMusicAction(Dictionary<string, string> parameters){
            if(parameters.TryGetValue("musicClips", out string audioClipList)){
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
        if (clips.Count == 0){
            Debug.LogWarning("⚠️ Nessuna clip musicale trovata.");
            return;
        }

        AudioClip selectedClip = clips[Random.Range(0, clips.Count)];
        AudioSource musicSource = AudioService.Instance.GetMusicSource();

        if (musicSource == null){
            Debug.LogError("❌ AudioSource musicale non trovato.");
            return;
        }

        if (musicSource.clip == selectedClip && musicSource.isPlaying){
            Debug.Log($"▶️ Musica già in riproduzione: {selectedClip.name}");
            return;
        }

        musicSource.clip = selectedClip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
