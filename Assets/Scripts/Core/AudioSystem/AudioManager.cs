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
    private bool m_musicIsInTransition = false;
    private bool m_mute = false;
    private bool m_paused = false;

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

    public void PlayBGM(string _audioObjectName, bool _loop = true)
    {
        if (m_mute || m_paused) return;

        try
        {
            AudioSource audioSource = getAvailableAudioSource(AudioMixerGroupTypes.BGM);
            audioSource.clip = getAudioClipFromAudioObject(_audioObjectName, AudioMixerGroupTypes.BGM);
            audioSource.loop = _loop;
            audioSource.Play();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Could not find AudioSource available. " + ex);
        }
    }

    public void PlayBGMWithTransition(string _audioObjectName, float _timeToMoveFadeOut = 1.0f, float _timeToMoveFadeIn = 1.0f, bool _loop = true)
    {
        if (m_mute || m_paused) return;

        try
        {
            AudioSource audioSource = getAvailableAudioSource(AudioMixerGroupTypes.BGM);
            audioTransition(audioSource, getAudioClipFromAudioObject(_audioObjectName, AudioMixerGroupTypes.BGM), audioSource.volume, _timeToMoveFadeOut, _timeToMoveFadeIn, _loop);
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Could not find AudioSource available. " + ex);
        }
    }

    public void PlayGameSFX(string _audioObjectName, bool _loop = false)
    {
        if (m_mute || m_paused) return;

        try
        {
            AudioSource audioSource = getAvailableAudioSource(AudioMixerGroupTypes.Game_SFX);
            audioSource.clip = getAudioClipFromAudioObject(_audioObjectName, AudioMixerGroupTypes.Game_SFX);
            audioSource.loop = _loop;
            audioSource.Play();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Could not find AudioSource available. " + ex);
        }
    }

    public void PlayUISFX(string _audioObjectName, bool _loop = false)
    {
        if (m_mute || m_paused) return;

        try
        {
            AudioSource audioSource = getAvailableAudioSource(AudioMixerGroupTypes.UI_SFX);
            audioSource.clip = getAudioClipFromAudioObject(_audioObjectName, AudioMixerGroupTypes.UI_SFX);
            audioSource.loop = _loop;
            audioSource.Play();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Could not find AudioSource available. " + ex);
        }
    }

    public void TransitionToSnapshot(SnapshotTypes _snapshotType, float _timeToReach)
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

        m_paused = true;
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

        m_paused = false;
    }

    public void MuteUnMuteAllAudio(bool _value)
    {
        for (int i = 0; i < m_bgmAudioSourceInstanceParent.childCount; i++)
        {
            m_bgmAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>().mute = _value;
        }

        for (int i = 0; i < m_sfxAudioSourceInstanceParent.childCount; i++)
        {
            m_sfxAudioSourceInstanceParent.GetChild(i).GetComponent<AudioSource>().mute = _value;
        }

        m_mute = _value;
    }

    private void audioTransition(AudioSource _audioSource, AudioClip _newAudioClip, float _maxVolume = 1.0f, float _timeToMoveFadeOut = 1.0f, float _timeToMoveFadeIn = 1.0f, bool _loop = true)
    {
        StartCoroutine(CR_audioTransitionRoutine(_audioSource, _newAudioClip, _maxVolume, _timeToMoveFadeOut, _timeToMoveFadeIn, _loop));
    }

    private IEnumerator CR_audioTransitionRoutine(AudioSource _audioSource, AudioClip _newAudioClip, float _maxVolume = 1.0f, float _timeToMoveFadeOut = 1.0f, float _timeToMoveFadeIn = 1.0f, bool _loop = true)
    {
        if (!m_musicIsInTransition)
        {
            m_musicIsInTransition = true;

            // Turn down volume
            bool reachedDestinationFadeOut = false;
            float elapsedTimeFadeOut = 0f;

            while (!reachedDestinationFadeOut)
            {
                if (_audioSource.volume <= 0f)
                {
                    _audioSource.volume = 0f;
                    reachedDestinationFadeOut = true;
                    break;
                }

                elapsedTimeFadeOut += Time.deltaTime;
                float t = Mathf.Clamp(elapsedTimeFadeOut / _timeToMoveFadeOut, 0f, 1f);
                t = t * t * t * (t * (t * 6 - 15) + 10);

                _audioSource.volume = Mathf.Lerp(_maxVolume, 0f, t);
                yield return null;
            }

            yield return null;

            // Change AudioClip
            _audioSource.clip = _newAudioClip;
            _audioSource.loop = _loop;
            _audioSource.Play();

            // Turn up volume
            bool reachedDestinationFadeIn = false;
            float elapsedTimeFadeIn = 0f;

            while (!reachedDestinationFadeIn)
            {
                if (_audioSource.volume >= _maxVolume)
                {
                    _audioSource.volume = _maxVolume;
                    reachedDestinationFadeIn = true;
                    break;
                }

                elapsedTimeFadeIn += Time.deltaTime;
                float t = Mathf.Clamp(elapsedTimeFadeIn / _timeToMoveFadeIn, 0f, 1f);
                t = t * t * t * (t * (t * 6 - 15) + 10);

                _audioSource.volume = Mathf.Lerp(0f, _maxVolume, t);
                yield return null;
            }

            m_musicIsInTransition = false;
        }
    }

    private AudioMixerGroup getAudioMixerGroupFromType(AudioMixerGroupTypes _audioMixerGroupType)
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


    private AudioClip getAudioClipFromAudioObject(string _audioObjectName, AudioMixerGroupTypes _audioMixerGroupType)
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

    private AudioSource getAnyAudioSouceAvailable(AudioMixerGroupTypes _audioMixerGroupType)
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

        audioSource = null;
        return audioSource;
    }

    private AudioSource instanceAudioSource(AudioMixerGroupTypes _audioMixerGroupType)
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

        return audioSource;
    }

    private AudioSource getLastIndexAudioSource(AudioMixerGroupTypes _audioMixerGroupType)
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

    private AudioSource getAvailableAudioSource(AudioMixerGroupTypes _audioMixerGroupType)
    {
        AudioSource audioSource = getAnyAudioSouceAvailable(_audioMixerGroupType);

        if (audioSource == null)
        {
            if(_audioMixerGroupType == AudioMixerGroupTypes.BGM)
            {
                if(m_bgmAudioSourceInstanceParent.childCount < AUDIO_BGM_SOURCE_SPAWN_MAX)
                {
                    audioSource = instanceAudioSource(_audioMixerGroupType);
                }
                else
                {
                    audioSource = getLastIndexAudioSource(_audioMixerGroupType);
                }

                audioSource.outputAudioMixerGroup = getAudioMixerGroupFromType(_audioMixerGroupType);
                return audioSource;

            }
            else if (_audioMixerGroupType == AudioMixerGroupTypes.Game_SFX || _audioMixerGroupType == AudioMixerGroupTypes.UI_SFX)
            {
                if (m_sfxAudioSourceInstanceParent.childCount < AUDIO_SFX_SOURCE_SPAWN_MAX)
                {
                    audioSource = instanceAudioSource(_audioMixerGroupType);
                }
                else
                {
                    audioSource = getLastIndexAudioSource(_audioMixerGroupType);
                }

                audioSource.outputAudioMixerGroup = getAudioMixerGroupFromType(_audioMixerGroupType);
                return audioSource;

            }
        }

        audioSource.outputAudioMixerGroup = getAudioMixerGroupFromType(_audioMixerGroupType);
        return audioSource;
    }
}