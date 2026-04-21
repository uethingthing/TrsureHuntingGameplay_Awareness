using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AudioClipSet
{
    public AudioNames Key;
    public AudioClip Clip;
}

[CreateAssetMenu(fileName = "AudioClipSetObject", menuName = "Scriptable Objects/AudioClipSetObject")]
public class AudioClipSetObject : ScriptableObject
{
    public List<AudioClipSet> Audios;
}
