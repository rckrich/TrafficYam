using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public float g_startingTime;
    [SerializeField] private float m_currenTime;
    [SerializeField] private TextMeshProUGUI m_timerTxt;
    IEnumerator co_timer;
    void Start()
    {
        m_currenTime = g_startingTime;
    }

    IEnumerator TimerCountdown(){
        while (true){
            m_timerTxt.text = m_currenTime.ToString();
            yield return new WaitForSeconds(1);
            
            if (m_currenTime<= 0)
            {
                GameManager.m_Instance.TimerEnds();
                co_timer = null;
                break;
            }
            m_currenTime--;
        }
    }

    public void InitializeTimer(){
        if(co_timer == null){
            co_timer = TimerCountdown();
            StartCoroutine(co_timer);
        }else{
            StopTimer();
            m_currenTime = g_startingTime;
            InitializeTimer();
        }
        
    }

    public void StopTimer(){
        StopCoroutine(co_timer);
        co_timer = null;
    }

}
