using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public enum SnapshotTypes
{
    None,
    Normal,
    Paused,
    Mute
}

[System.Serializable]
public class AudioSnapshotObject
{
    public SnapshotTypes m_snapShotType;
    public AudioMixerSnapshot m_snapshot;
}

[System.Serializable]
public class AudioMixerGroupObject
{
    public AudioMixerGroupTypes m_groupType;
    public AudioMixerGroup m_mixerGroup;
}

public class AudioManager : Manager<AudioManager>
{
    private const int AUDIO_BGM_SOURCE_SPAWN_MAX = 1;
    private const int AUDIO_SFX_SOURCE_SPAWN_MAX = 5;
    private const float MAX_WEIGHT_OF_SNAPSHOT = 1.0f;

    [Header("Audio Data")]
    [SerializeField]
    private SO_AudioObjectList m_audioObjectList;
    [SerializeField]
    private List<AudioMixerGroupObject> m_audioMixerGroupObjects;

    [Space]
    [Header("Audio Source Instance References")]
    [SerializeField]
    private AudioMixer m_audioMixer;
    [SerializeField]
    private List<AudioSnapshotObject> m_audioSnapshotObjects;
    [SerializeField]
    private Transform m_bgmAudioSourceInstanceParent;
    [SerializeField]
    private Transform m_sfxAudioSourceInstanceParent;

    private int m_lastBGMAudioSourceIndex = 0;
    private int m_lastSFXAudioSourceIndex = 0;
    private AudioMixerSnapshot[] m_snapshots;
    private float[] m_weights;

    private void Start()
    {
        m_snapshots = new AudioMixerSnapshot[m_audioSnapshotObjects.Count];
        for(int i = 0; i < m_audioSnapshotObjects.Count; i++)
        {
            m_snapshots[i] = m_audioSnapshotObjects[i].m_snapshot;
        }

        m_weights = new float[m_audioSnapshotObjects.Count];
        for (int i = 0; i < m_audioSnapshotObjects.Count; i++)
        {
            m_weights[i] = 0;
        }

        //Setting first Snapshot of the array, to be the first.
        m_weights[0] = MAX_WEIGHT_OF_SNAPSHOT;
    }

    public void PlayBGM(string _audioObjectName)
    {
        AudioSource audioSource = GetAvailableAudioSource(AudioMixerGroupTypes.BGM);
        audioSource.clip = GetAudioClipFromAudioObject(_audioObjectName, AudioMixerGroupTypes.BGM);
        audioSource.Play();
    }

    public void PlayGameSFX(string _audioObjectName)
    {
        AudioSource audioSource = GetAvailableAudioSource(AudioMixerGroupTypes.Game_SFX);
        audioSource.clip = GetAudioClipFromAudioObject(_audioObjectName, AudioMixerGroupTypes.Game_SFX);
        audioSource.Play();
    }

    public void PlayUISFX(string _audioObjectName)
    {
        AudioSource audioSource = GetAvailableAudioSource(AudioMixerGroupTypes.UI_SFX);
        audioSource.clip = GetAudioClipFromAudioObject(_audioObjectName, AudioMixerGroupTypes.UI_SFX);
        audioSource.Play();
    }

    public void TransitionSnapshotTo(SnapshotTypes _snapshotType, float _timeToReach)
    {
        for(int i = 0; i < m_weights.Length; i++)
        {
            m_weights[i] = 0;
        }

        int snapshotIndex = m_audioSnapshotObjects.FindIndex((audioSnapshotObject) => _snapshotType == audioSnapshotObject.m_snapShotType);
        m_weights[snapshotIndex] = MAX_WEIGHT_OF_SNAPSHOT;
        m_audioMixer.TransitionToSnapshots(m_snapshots, m_weights, _timeToReach);
    }

    public void StopAllAudio()
    {
        for(int i = 0; i < m_bgmAudioSourceInstanceParent.childCount; i++)
        {
            m_bgmAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>().Stop();
        }

        for (int i = 0; i < m_sfxAudioSourceInstanceParent.childCount; i++)
        {
            m_sfxAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>().Stop();
        }
    }

    public void PauseAllAudio()
    {
        for (int i = 0; i < m_bgmAudioSourceInstanceParent.childCount; i++)
        {
            m_bgmAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>().Pause();
        }

        for (int i = 0; i < m_sfxAudioSourceInstanceParent.childCount; i++)
        {
            m_sfxAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>().Pause();
        }
    }

    public void UnPauseAllAudio()
    {
        for (int i = 0; i < m_bgmAudioSourceInstanceParent.childCount; i++)
        {
            m_bgmAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>().UnPause();
        }

        for (int i = 0; i < m_sfxAudioSourceInstanceParent.childCount; i++)
        {
            m_sfxAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>().UnPause();
        }
    }

    private AudioSource isAnyAudioSouceAvailable(AudioMixerGroupTypes _audioMixerGroupType)
    {
        AudioSource audioSource = null;

        if (_audioMixerGroupType == AudioMixerGroupTypes.BGM)
        {
            for (int i = 0; i < m_bgmAudioSourceInstanceParent.childCount; i++)
            {
                audioSource = m_bgmAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>();
                if (!audioSource.isPlaying)
                {
                    m_lastBGMAudioSourceIndex = audioSource.transform.GetSiblingIndex();
                    return audioSource;
                }
            }
        }
        else if (_audioMixerGroupType == AudioMixerGroupTypes.Game_SFX || _audioMixerGroupType == AudioMixerGroupTypes.UI_SFX)
        {
            for (int i = 0; i < m_sfxAudioSourceInstanceParent.childCount; i++)
            {
                audioSource = m_sfxAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>();
                if (!audioSource.isPlaying)
                {
                    m_lastSFXAudioSourceIndex = audioSource.transform.GetSiblingIndex();
                    return audioSource;
                }
            }
        }

        return audioSource;
    }

    private AudioMixerGroup GetAudioMixerGroupFromType(AudioMixerGroupTypes _audioMixerGroupType)
    {
        AudioMixerGroup audioMixerGroup = null;

        try
        {
            foreach (AudioMixerGroupObject audioMixerGroupObject in m_audioMixerGroupObjects)
            {
                if (audioMixerGroupObject.m_groupType == _audioMixerGroupType)
                    audioMixerGroup = audioMixerGroupObject.m_mixerGroup;
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Could not find AudioMixerGroupObject of certain type. " + ex);
        }

        return audioMixerGroup;
    }

    private AudioSource InstanceAudioSource(AudioMixerGroupTypes _audioMixerGroupType)
    {
        GameObject audioSourceGameObject = new GameObject("AudioSource", typeof(AudioSource));
        AudioSource audioSource = null;

        if (_audioMixerGroupType == AudioMixerGroupTypes.BGM)
        {
            audioSource = Instantiate(audioSourceGameObject, m_bgmAudioSourceInstanceParent).GetComponent<AudioSource>();
            m_lastBGMAudioSourceIndex = audioSource.transform.GetSiblingIndex();
        }
        else if (_audioMixerGroupType == AudioMixerGroupTypes.Game_SFX || _audioMixerGroupType == AudioMixerGroupTypes.UI_SFX)
        {
            audioSource = Instantiate(audioSourceGameObject, m_sfxAudioSourceInstanceParent).GetComponent<AudioSource>();
            m_lastSFXAudioSourceIndex = audioSource.transform.GetSiblingIndex();
        }

        audioSource.outputAudioMixerGroup = GetAudioMixerGroupFromType(_audioMixerGroupType);

        return audioSource;
    }

    private AudioSource GetLastIndexAudioSource(AudioMixerGroupTypes _audioMixerGroupType)
    {
        AudioSource audioSource = null;

        if (_audioMixerGroupType == AudioMixerGroupTypes.BGM)
        {
            audioSource = m_bgmAudioSourceInstanceParent.GetChild(m_lastBGMAudioSourceIndex).GetComponent<AudioSource>();
        }
        else if (_audioMixerGroupType == AudioMixerGroupTypes.Game_SFX || _audioMixerGroupType == AudioMixerGroupTypes.UI_SFX)
        {
            audioSource = m_sfxAudioSourceInstanceParent.GetChild(m_lastSFXAudioSourceIndex).GetComponent<AudioSource>();
        }

        return audioSource;
    }

    private AudioSource GetAvailableAudioSource(AudioMixerGroupTypes _audioMixerGroupType)
    {
        AudioSource audioSource = isAnyAudioSouceAvailable(_audioMixerGroupType);

        if (audioSource == null)
        {
            if(_audioMixerGroupType == AudioMixerGroupTypes.BGM)
            {
                if(m_bgmAudioSourceInstanceParent.childCount <= AUDIO_BGM_SOURCE_SPAWN_MAX)
                {
                    audioSource = InstanceAudioSource(_audioMixerGroupType);
                }
                else
                {
                    audioSource = GetLastIndexAudioSource(_audioMixerGroupType);
                }
            }
            else if (_audioMixerGroupType == AudioMixerGroupTypes.Game_SFX || _audioMixerGroupType == AudioMixerGroupTypes.UI_SFX)
            {
                if (m_sfxAudioSourceInstanceParent.childCount <= AUDIO_SFX_SOURCE_SPAWN_MAX)
                {
                    audioSource = InstanceAudioSource(_audioMixerGroupType);
                }
                else
                {
                    audioSource = GetLastIndexAudioSource(_audioMixerGroupType);
                }
            }

            audioSource = InstanceAudioSource(_audioMixerGroupType);
        }

        return audioSource;
    }

    private AudioClip GetAudioClipFromAudioObject(string _audioObjectName, AudioMixerGroupTypes _audioMixerGroupType)
    {
        AudioClip audioClip = null;

        try
        {
            switch (_audioMixerGroupType)
            {
                case AudioMixerGroupTypes.Master:
                    return audioClip = null;
                case AudioMixerGroupTypes.BGM:
                    audioClip = m_audioObjectList.m_BGMAudioObjectList.m_audioObjects.Find((audioObject) => audioObject.m_audioID == _audioObjectName).m_audioClip;
                    break;
                case AudioMixerGroupTypes.Game_SFX:
                    audioClip = m_audioObjectList.m_GameSFXAudioObjectList.m_audioObjects.Find((audioObject) => audioObject.m_audioID == _audioObjectName).m_audioClip;
                    break;
                case AudioMixerGroupTypes.UI_SFX:
                    audioClip = m_audioObjectList.m_UISFXAudioObjectList.m_audioObjects.Find((audioObject) => audioObject.m_audioID == _audioObjectName).m_audioClip;
                    break;
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Audio Clip of name " + _audioObjectName + " doesn't exist in Audio Scriptable Object. " + ex);
        }

        return audioClip;
    }

}