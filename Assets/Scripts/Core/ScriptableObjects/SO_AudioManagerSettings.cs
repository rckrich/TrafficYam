using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioMixerGroupTypes
{
    None,
    Master,
    BGM,
    Game_SFX,
    UI_SFX
}

public enum SnapshotTypes
{
    None,
    Normal,
    Paused,
    Mute
}

[System.Serializable]
public class AudioMixerGroupObject
{
    public AudioMixerGroupTypes m_groupType;
    public AudioMixerGroup m_mixerGroup;
}

[System.Serializable]
public class AudioSnapshotObject
{
    public SnapshotTypes m_snapShotType;
    public AudioMixerSnapshot m_snapshot;
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

[CreateAssetMenu(fileName = "AudioObjectList", menuName = "ScriptableObjects/AudioManagerSettings", order = 0)]
public class SO_AudioManagerSettings : ScriptableObject
{
    [Header("Mixer")]
    [SerializeField]
    public AudioMixer m_audioMixer;
    [Space]
    [Header("Mixer Groups")]
    public List<AudioMixerGroupObject> m_audioMixerGroupObjects;
    [Space]
    [Header("Mixer Snapshots")]
    public List<AudioSnapshotObject> m_audioSnapshotObjects;
    [Space]
    [Header("Audio Clip Lists")]
    public AudioObjectList m_BGMAudioObjectList;
    public AudioObjectList m_GameSFXAudioObjectList;
    public AudioObjectList m_UISFXAudioObjectList;

}
