using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

using DG.Tweening;
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
    private GameObject m_trafficObjectInCenter;
    public TextMeshProUGUI m_lifeText;
    private IEnumerator co_gamePlayLoop;
    public List<Transform> m_animalTrack = new List<Transform>();
    public TrafficRandomizer m_trafficRandomizer;
    public List<Animal> m_animalsToFeed = new List<Animal>();
    
    private void Awake()
    {
        m_TrafficObjectWaitSeconds = 1f;
        m_trafficObjectSpeed = 2;
    }

    private void InitializeGame(){
        m_lives = 3;
        m_lifeText.text = m_lives.ToString();
        m_trafficRandomizer.Randomize(m_TrafficObjectWaitSeconds, m_Timer, m_animalsToFeed.Count-1, CalculateAnimalCarnivores());
        if(co_gamePlayLoop == null){
            co_gamePlayLoop = GamePlayLoop();
            StartCoroutine(co_gamePlayLoop);
        }
    }
    
    private float CalculateAnimalCarnivores(){
        float _carnivores = 0;
        foreach (Animal _animal in m_animalsToFeed)
        {
            if(_animal.m_foodType == FoodType.Carnivorous){
                _carnivores++;
                break;
            }
            if(_animal.m_foodType == FoodType.Omnivore){
                _carnivores = _carnivores + .5f;
                break;
            }
        }

        return _carnivores;
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
        int m_numberOfAnimalsTracks = m_animalsToFeed.Count;
        if(_swipeDirection == "Up"){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[0]);
            return;
        }
        if(_swipeDirection == "Right" && m_numberOfAnimalsTracks > 1){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[1]);
            return;
        }
        if(_swipeDirection == "Left" && m_numberOfAnimalsTracks > 2){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[2]);
            return;
        }
        if(_swipeDirection == "UpRight" && m_numberOfAnimalsTracks > 3){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[3]);
            return;
        }
        if(_swipeDirection == "UpLeft" && m_numberOfAnimalsTracks > 4){
           m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[4]);
           return;
        }
        if(_swipeDirection == "DownRight" && m_numberOfAnimalsTracks > 5){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[5]);
            return;
        }
        if(_swipeDirection == "DownLeft" && m_numberOfAnimalsTracks > 6){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[6]);
            return;
        }
    }

    public void LoseLife()
    {
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


