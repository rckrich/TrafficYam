using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class AudioManagerTest : MonoBehaviour
{
    public TMP_InputField m_transitionSeconds;
    public TMP_InputField m_transitionToNormalSnapshotSeconds;
    public TMP_InputField m_transitionToPausedSnapshotSeconds;
    public TMP_InputField m_transitionToMuteSnapshotSeconds;

    private bool m_pause = false;
    private bool m_mute = false;
    private bool m_changeBGMWithTransition = false;

    public void OnClick_PlayBGM()
    {
        AudioManager.m_Instance.PlayBGM("Test");
    }

    public void OnClick_PlayGameSFX()
    {
        AudioManager.m_Instance.PlayGameSFX("Test");
        AudioManager.m_Instance.PlayGameSFX("Test");
        AudioManager.m_Instance.PlayGameSFX("Test");
    }

    public void OnClick_PlayUISFX()
    {
        AudioManager.m_Instance.PlayUISFX("Test");
    }

    public void OnClick_TransitionToOtherBGM()
    {
        string bgmName = m_changeBGMWithTransition ? "Test" : "Test2";
        AudioManager.m_Instance.PlayBGMWithTransition(bgmName, float.Parse(m_transitionSeconds.text, CultureInfo.InvariantCulture.NumberFormat), float.Parse(m_transitionSeconds.text, CultureInfo.InvariantCulture.NumberFormat));
        m_changeBGMWithTransition = !m_changeBGMWithTransition;
    }

    public void OnClick_Pause()
    {
        if (!m_pause)
        {
            AudioManager.m_Instance.PauseAllAudio();
        }
        else
        {
            AudioManager.m_Instance.UnPauseAllAudio();
        }

        m_pause = !m_pause;
    }

    public void OnClick_Mute()
    {
        m_mute = !m_mute;

        AudioManager.m_Instance.MuteUnMuteAllAudio(m_mute);
    }

    public void OnClick_Stop()
    {
        AudioManager.m_Instance.StopAllAudio();
    }

    public void OnClick_TransitionToNormalSnapshot()
    {
        AudioManager.m_Instance.TransitionToSnapshot(SnapshotTypes.Normal, float.Parse(m_transitionToNormalSnapshotSeconds.text));
    }

    public void OnClick_TransitionToPausedSnapshot()
    {
        AudioManager.m_Instance.TransitionToSnapshot(SnapshotTypes.Paused, float.Parse(m_transitionToPausedSnapshotSeconds.text));
    }

    public void OnClick_TransitionToMuteSnapshot()
    {
        AudioManager.m_Instance.TransitionToSnapshot(SnapshotTypes.Mute, float.Parse(m_transitionToMuteSnapshotSeconds.text));
    }
}
