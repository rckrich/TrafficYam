using System.Collections.Generic;
using UnityEngine;

public enum AudioMixerGroupTypes
{
    None,
    Master,
    BGM,
    Game_SFX,
    UI_SFX
}

[System.Serializable]
public class AudioObject
{
    public string m_audioID;
    public AudioClip m_audioClip;
}

[System.Serializable]
public class AudioObjectList
{
    public AudioMixerGroupTypes m_groupType;
    public List<AudioObject> m_audioObjects;
}

[CreateAssetMenu(fileName = "AudioObjectList", menuName = "ScriptableObjects/AudioObjectList", order = 0)]
public class SO_AudioObjectList : ScriptableObject
{
    public AudioObjectList m_BGMAudioObjectList;
    public AudioObjectList m_GameSFXAudioObjectList;
    public AudioObjectList m_UISFXAudioObjectList;

}
