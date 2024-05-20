using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(TrafficRandomizer))] [RequireComponent(typeof(CountdownTimer))]
public class GameManager : Manager<GameManager>
{
    [SerializeField] private CountdownTimer m_Countdowntimer;
    [SerializeField] private GameState state;
    [SerializeField] private Transform m_centerPosition, m_initialPosition;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private int m_Coins;
    [SerializeField] private int m_lives = 3;
    private float m_TrafficObjectWaitSeconds;
    private float m_trafficObjectSpeed;
    [SerializeField] private GameObject m_trafficObjectInCenter;
    [SerializeField] private TextMeshProUGUI m_lifeText;
    private IEnumerator co_gamePlayLoop, co_gameCounter;
    [SerializeField] private List<Transform> m_animalTrack = new List<Transform>();
    [SerializeField] private TrafficRandomizer m_trafficRandomizer;
    [SerializeField] private List<GameObject> m_animalsToFeed = new List<GameObject>();
    private int m_animalsInPlay;
    public Animal[] m_animalList;
    private int m_streak;
    
    public static GameEvent OnStreak;
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
        m_Countdowntimer.InitializeTimer();
        m_trafficRandomizer.Randomize(m_TrafficObjectWaitSeconds, m_Countdowntimer.g_startingTime, m_animalsInPlay, CalculateAnimalCarnivores());
        if(co_gamePlayLoop == null){
            co_gamePlayLoop = GamePlayLoop();
            StartCoroutine(co_gamePlayLoop);
        }
    }

    private void ConfigureAnimalsInPlay(){
        
        m_animalList = GameDataManager.m_Instance.ReturnAnimalList();

        for (int i = 0; i < m_animalList.Length; i++)
        {
            if(m_animalList[i].m_foodType == FoodType.None){
                m_animalsToFeed[i].SetActive(false);
            }else{
                m_animalsInPlay++;
                Animal _animal = m_animalsToFeed[i].GetComponentInChildren<Animal>();
                _animal.m_foodType = m_animalList[i].m_foodType;
                _animal.m_HungerCapacity = m_trafficRandomizer.g_foodPerAnimal;
            }
        }
        
    }
    
    private float CalculateAnimalCarnivores(){
        float _carnivores = 0;
        foreach (Animal _animal in m_animalList)
        {
            if(_animal.m_foodType == FoodType.Carnivorous){
                _carnivores++;
                
            }
            if(_animal.m_foodType == FoodType.Omnivore){
                _carnivores = _carnivores + .5f;
                
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
            InitializeTrafficObject();
            yield return new WaitForSeconds(m_TrafficObjectWaitSeconds);
        }
    }

    private void InitializeTrafficObject(){
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
            OnTriggerFail();
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
        PointLogic();
        if(_swipeDirection == "Up"){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[0]);
            return;
        }
        if(_swipeDirection == "Right" && m_animalList[0].m_foodType != FoodType.None){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[1]);
            return;
        }
        if(_swipeDirection == "Left" && m_animalList[1].m_foodType != FoodType.None){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[2]);
            return;
        }
        if(_swipeDirection == "UpRight" && m_animalList[2].m_foodType != FoodType.None){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[3]);
            return;
        }
        if(_swipeDirection == "UpLeft" && m_animalList[3].m_foodType != FoodType.None){
           m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[4]);
           return;
        }
        /*
        if(_swipeDirection == "DownRight" && m_animalsInPlay > 4){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[5]);
            return;
        }
        if(_swipeDirection == "DownLeft" && m_animalsInPlay > 5){
            m_trafficObject.GetComponent<ObjectTweenAnimator>().Play_SwipeMove(m_animalTrack[6]);
            return;
        }
        */
    }

    private void PointLogic(){
        m_streak++;
        if(m_streak > 5){
            //InvokeEvent<GameEvent>(new GameEvent(1));
            Debug.Log("OnStreak");
        }
    }

    public void OnClickCoin(){
        Debug.Log("Coin");
    }

    public void OnTriggerSuccess(){
        Debug.Log("success");
    }

    public void OnTriggerFail()
    {
        Debug.Log("fail");
        //InvokeEvent<GameEvent>(new GameEvent(0));
        if(m_lives == 0)
        {
            m_Countdowntimer.StopTimer();
            UpdateGameState(GameState.Lose);
            Debug.Log("Lose");
        }
        else
        {
            m_streak = 0;
            m_lives -= 1;
            
            m_lifeText.text = m_lives.ToString();
        }
    }


    public void TimerEnds()
    {
        UpdateGameState(GameState.Victory);

        Debug.Log("Win");
    }
    public enum GameState
    {
        Victory,
        Lose
    }

    
}


