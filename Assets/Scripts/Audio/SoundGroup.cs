using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundGroup
{
    public SoundTypeTag soundType;
    public List<AudioClip> audioClipList;

    public SoundGroup(){}
}
