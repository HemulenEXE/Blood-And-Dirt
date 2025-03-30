using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioList", menuName = "AudioList/List", order = -1)]
public class AudioList : ScriptableObject
{
    [SerializeField]
    public List<AudioFile> list;
    [Serializable]
    public struct AudioFile
    {
        [SerializeField]
        AudioClip clip;
        [SerializeField]
        string name;
        [SerializeField]
        float basicVolume;
    }



    

    public AudioList()
    {
        list = new List<AudioFile>();
    }
}
