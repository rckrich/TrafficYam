using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioObject
{
    public string m_audioID;
    public AudioClip m_audioClip;
}

[CreateAssetMenu(fileName = "AudioObjectList", menuName = "ScriptableObjects/AudioObjectList", order = 0)]
public class SO_AudioObjectList : ScriptableObject
{
    public List<AudioObject> m_audioObjects;
}
