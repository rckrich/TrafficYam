using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ViewModel : RCKGameObject
{
    private const int NETWORKLOADINGPANEL_ANIMATION_LAYER = 0;

    [Header("Main View ID")]
    public MainViewID _m_mainViewID;
    [Header("General View Model Game Object Reference")]
    public GameObject m_networkLoadingCanvas;
    public Transform m_scrollViewContent;

    [SerializeField]
    public System.Action m_backAction;

    public virtual MainViewID GetViewID() { return _m_mainViewID; }

    public virtual void StartSearch()
    {
        StartCoroutine(CR_StartSearch());
    }

    public virtual void EndSearch()
    {
        StartCoroutine(CR_EndSearch());
    }

    protected virtual void CallWaitAFrame()
    {
        StartCoroutine(CR_WaitAFrame());
    }

    protected virtual IEnumerator CR_WaitAFrame()
    {
        yield return new WaitForSeconds(0.5f);
        if (m_scrollViewContent != null) LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)m_scrollViewContent);
        yield return new WaitForSeconds(0.2f);
    }

    protected virtual IEnumerator CR_StartSearch()
    {
        if (m_networkLoadingCanvas == null)
            m_networkLoadingCanvas = GameObject.FindWithTag("NetworkLoadingCanvas");

        if (m_networkLoadingCanvas != null)
        {
            m_networkLoadingCanvas.GetComponent<Animator>().SetBool("ExitBool", false);
            m_networkLoadingCanvas.GetComponent<Animator>().SetBool("EnterBool", true);
        }
        yield return null;
    }

    protected virtual IEnumerator CR_EndSearch()
    {
        if (m_networkLoadingCanvas == null)
            m_networkLoadingCanvas = GameObject.FindWithTag("NetworkLoadingCanvas");

        if (m_networkLoadingCanvas != null)
        {
            m_networkLoadingCanvas.GetComponent<Animator>().SetBool("EnterBool", false);
            m_networkLoadingCanvas.GetComponent<Animator>().SetBool("ExitBool", true);
        }

        yield return null;
    }

    public virtual void OnExternalLogIn()
    {

    }

    public virtual void OnExternalLogOut()
    {

    }
}