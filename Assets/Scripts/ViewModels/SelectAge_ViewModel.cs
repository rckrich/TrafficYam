using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAge_ViewModel : ViewModel
{
    public void OnClick_Continue()
    {
        ViewModelManager.m_Instance.ChangeToMainView(MainViewID.Menu);
    }
}
