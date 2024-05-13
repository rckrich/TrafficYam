using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public float startingTime;
    public float currenTime = 0f;
    void Start()
    {
        currenTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        currenTime -= 1 * Time.deltaTime;
        if (currenTime<= 0)
        {
            GameManager.m_Instance.TimerEnds();
        }
    }
}
