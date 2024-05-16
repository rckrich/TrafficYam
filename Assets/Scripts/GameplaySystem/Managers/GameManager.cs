using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(TrafficRandomizer))]
public class GameManager : Manager<GameManager>
{
    public GameState state;
    public Transform m_centerPosition, m_initialPosition;
    public GameObject GameOverScreen;
    public float m_Timer;
    public int m_Coins;
    public int m_lives = 3;
    private float m_TrafficObjectWaitSeconds;
    private float m_trafficObjectSpeed;
    public GameObject m_trafficObjectInCenter;
    public TextMeshProUGUI m_lifeText;
    private IEnumerator co_gamePlayLoop, co_gameCounter;
    public List<Transform> m_animalTrack = new List<Transform>();
    public TrafficRandomizer m_trafficRandomizer;
    public List<GameObject> m_animalsToFeed = new List<GameObject>();
    private int m_animalsInPlay;
    private bool ColisionDoOnce;
    
    private void Start(){
        
        m_TrafficObjectWaitSeconds = 2f;
        m_trafficObjectSpeed = 2;
    }

    public void InitializeGame(){
        ConfigureAnimalsInPlay();
        if(co_gameCounter == null){
            co_gameCounter = GameCounter();
            StartCoroutine(co_gameCounter);
        }
    }

    private void InitializeGameLogic(){
        co_gameCounter = null;
        m_lives = 3;
        m_lifeText.text = m_lives.ToString();
        m_trafficRandomizer.Randomize(m_TrafficObjectWaitSeconds, m_Timer, m_animalsInPlay, CalculateAnimalCarnivores());
        if(co_gamePlayLoop == null){
            co_gamePlayLoop = GamePlayLoop();
            StartCoroutine(co_gamePlayLoop);
        }
    }

    private void ConfigureAnimalsInPlay(){
        m_animalsInPlay = GameDataManager.m_Instance.g_animalsInPlay;
        for (int i = 0; i < m_animalsToFeed.Count; i++)
        {
            if( i > m_animalsInPlay-1){
                m_animalsToFeed[i].SetActive(false);
            }
        }
    }
    
    private float CalculateAnimalCarnivores(){
        float _carnivores = 0;
        foreach (GameObject _animal in m_animalsToFeed)
        {
            if(_animal.GetComponentInChildren<Animal>().m_foodType == FoodType.Carnivorous){
                _carnivores++;
                break;
            }
            if(_animal.GetComponentInChildren<Animal>().m_foodType == FoodType.Omnivore){
                _carnivores = _carnivores + .5f;
                break;
            }
        }

        return _carnivores;
    }

    IEnumerator GameCounter(){
        int timer = 3;
        while(true){
            Debug.Log(timer);
            yield return new WaitForSeconds(1);
            if(timer < 2){
                break;
            }else{
                timer--;
            }
        }
        InitializeGameLogic();
    }
    IEnumerator GamePlayLoop(){
        while (true){
            InitializTrafficObject();
            yield return new WaitForSeconds(m_TrafficObjectWaitSeconds);
        }
    }

    private void InitializTrafficObject(){
        GameObject _instance = PoolManager.m_Instance.GetPooledObject();
        _instance.GetComponent<TrafficObject>().ChangeType(m_trafficRandomizer.GetTypeFromList());
        _instance.SetActive(true);
        ObjectTweenAnimator _tweenAnimator = _instance.GetComponent<ObjectTweenAnimator>();
        _tweenAnimator.SetValues(m_trafficObjectSpeed, m_centerPosition, m_initialPosition);
        _tweenAnimator.Play_MoveTocenter();	
    }

    public void OnClick_RestartButton(){
        GameOverScreen.SetActive(false);
        InitializeGame();
    }

    public void UpdateGameState(GameState _state)
    {
        state = _state;
        switch(_state)
        {
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
        }
    }

    public void UpdateTrafficObjectInCenter(GameObject _trafficObject){
        m_trafficObjectInCenter = _trafficObject;
    }

    public void TrafficObjectColision(GameObject _trafficObject, GameObject _trafficObject2){
        if(_trafficObject.activeSelf == true && _trafficObject2.activeSelf == true){
            if(_trafficObject.transform.position.z < _trafficObject2.transform.position.z){
                _trafficObject2.SetActive(false);
            }else{
                _trafficObject.SetActive(false);
            }
            LoseLife();
        }
        
        
    }

    public void OnSwipe(string _swipe)
    {
        if(m_trafficObjectInCenter != null){
            DOTween.Complete(m_trafficObjectInCenter);
            SwipeLogic(_swipe, m_trafficObjectInCenter);
        }
        Debug.Log(_swipe);
    }

    private void SwipeLogic(string _swipeDirection, GameObject m_trafficObject)
    {
        if(_swipeDirection == "Up"){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[0]);
            return;
        }
        if(_swipeDirection == "Right" && m_animalsInPlay > 0){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[1]);
            return;
        }
        if(_swipeDirection == "Left" && m_animalsInPlay > 1){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[2]);
            return;
        }
        if(_swipeDirection == "UpRight" && m_animalsInPlay > 2){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[3]);
            return;
        }
        if(_swipeDirection == "UpLeft" && m_animalsInPlay > 3){
           m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[4]);
           return;
        }
        if(_swipeDirection == "DownRight" && m_animalsInPlay > 4){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[5]);
            return;
        }
        if(_swipeDirection == "DownLeft" && m_animalsInPlay > 5){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[6]);
            return;
        }
    }

    public void LoseLife()
    {
        Debug.Log("lose");
        if(m_lives == 0)
        {
            UpdateGameState(GameState.Lose);
        }
        else
        {
            m_lives -= 1;
            
            m_lifeText.text = m_lives.ToString();
        }
    }

    public void TimerEnds()
    {
        UpdateGameState(GameState.Victory);
        
    }
    public enum GameState
    {
        Victory,
        Lose
    }

    
}


