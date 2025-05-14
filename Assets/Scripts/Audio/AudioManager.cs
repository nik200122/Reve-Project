using System;
using System.Collections.Generic;
using UnityEngine;
//VECCHIO, NON SERVE
public enum SoundTypeTag{
    Hit,
    Death,
    MenuNavigation,
    MenuSelection,
    InvalidSelection,
    Music
}

public class AudioManager : MonoBehaviour
{   
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<SoundGroup> soundGroupList;
    
    [SerializeField] List<EnemyCharacterStatus> enemyCharacterStatusList;

    private void Start(){
        //sfxSource = GetComponent<AudioSource>();
        //PlayMusic(SoundTypeTag.Music);
    }
    void Update(){
        foreach (EnemyCharacterStatus status in enemyCharacterStatusList){
            //CheckHasBeenHit(status);
        }
    }

    public void PlaySound(SoundTypeTag type){
        SoundGroup group = GetSoundGroupByType(type);

        if (group != null && group.audioClipList.Count > 0){
            AudioClip clip = group.audioClipList[UnityEngine.Random.Range(0, group.audioClipList.Count)];
            sfxSource.PlayOneShot(clip);
        }else{
            Debug.LogWarning($"No audio clips found for sound type: {type}");
        }
    }

    public void PlayMusic(SoundTypeTag type){
        SoundGroup group = GetSoundGroupByType(type);

        if (group != null && group.audioClipList.Count > 0){
            AudioClip clip = group.audioClipList[UnityEngine.Random.Range(0, group.audioClipList.Count)];
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }else{
            Debug.LogWarning($"No music audio clips found for sound type: {type}");
        }
    }


    private void CheckHasBeenHit(EnemyCharacterStatus status){
        //Debug.Log("CHECK: "+status.HasBeenHit());
        // if(status.IsHit())
        //     PlaySound(SoundTypeTag.Hit);
    }

    private SoundGroup GetSoundGroupByType(SoundTypeTag type){
        foreach (SoundGroup group in soundGroupList){
            if (group.soundType == type)
                return group;
        }

        Debug.LogWarning($"SoundGroup with type {type} not found.");
        return null;
    }



}
