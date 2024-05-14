using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualitySettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
