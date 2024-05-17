using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo_ViewModel : ViewModel
{
    private const float WAIT_TIME_FOR_LOGO = 1.0f;
    private const float WAIT_TIME_FOR_SCREEN = 3.0f;
    public GameObject m_RCK_Logo;
    void Start()
    {
        StartCoroutine(WaitForLogo());
        StartCoroutine(WaitForAgeScreen());
    }

   private IEnumerator WaitForLogo()
    {
        yield return new WaitForSeconds(WAIT_TIME_FOR_LOGO);
        m_RCK_Logo.SetActive(true);
    }

    private IEnumerator WaitForAgeScreen()
    {
        yield return new WaitForSeconds(WAIT_TIME_FOR_SCREEN);
        ViewModelManager.m_Instance.ChangeToMainView(MainViewID.SelectAge);
    }
}
