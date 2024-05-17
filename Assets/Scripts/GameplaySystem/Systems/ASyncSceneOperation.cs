
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ASyncSceneOperation : MonoBehaviour
{

    AsyncOperation m_asyncOperation;

    public Image m_progressBar;
    public string m_sceneToLoad;

    public void StartLoadScene()
    {
        m_asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_sceneToLoad);
        m_asyncOperation.allowSceneActivation = false;
        StartCoroutine("ProgressBar");
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        
        GameManager.m_Instance.InitializeGame();
    }

    private void StartLoadedScene()
    {
        m_asyncOperation.allowSceneActivation = true;
    }

    public float CheckStatus()
    {
        return m_asyncOperation.progress;
    }

    IEnumerator ProgressBar(){
        while (true){
            yield return new WaitForSeconds(.1f);
            if(m_progressBar != null){
                m_progressBar.fillAmount = CheckStatus();
            }
    
            if(CheckStatus() > .89f){
                StartLoadedScene();
                break;
            }
        }
        

    }
    
}
