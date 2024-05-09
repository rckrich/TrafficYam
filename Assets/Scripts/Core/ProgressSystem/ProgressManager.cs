using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : Manager<ProgressManager>
{
    public GameProgress _m_gameProgress;

    public void Save()
    {
        FileManager.saveProgress(this._m_gameProgress);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        initApp();
    }

    private void initApp()
    {
        GameProgress gameProgress = FileManager.loadProgress();
        if (gameProgress == null)
        {
            Debug.Log("No se ha encontrado los datos de la app, inicializando datos de la app...");
            FileManager.saveProgress(this._m_gameProgress);
#if UNITY_IOS
            FileManager.setNoBackUpIOS();
#endif
        }
        else
        {
            this._m_gameProgress = gameProgress;
        }

    }

}
