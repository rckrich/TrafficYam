using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using TMPro;

public class GameplayManager : MonoBehaviour
{

    private static GameplayManager _instance;
    

    public static GameplayManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameplayManager>();
            }
            return _instance;
        }
    }
    public GameObject PF_Dummy, DummyContainer, GameOverScreen;
    public Transform targetFinish, realFinish, targetStart;
    [SerializeField]public List<Destiny> destinations = new List<Destiny>();

    private GameObject ActiveDummy;
    private int numberOfRoads;

    public TextMeshProUGUI pointText, LifeText;
    private int Points;

    private int Life = 3;

    private IEnumerator GamePlayeLoopCo, WaitForReactionCo;

    //Difficulty Settings
    private float reactionTime, dummyPerMinute, proportionOfDummyCar, dummySpeed;


    private void Start() {
        reactionTime = 2;
        dummyPerMinute = 60f;
        dummySpeed = 2;
        numberOfRoads = 7;
    }

    private void InitializeGame(){
        Points = 0;
        Life = 3;
        LifeText.text = Life.ToString();
        pointText.text = Points.ToString();
        if(GamePlayeLoopCo == null){
            GamePlayeLoopCo = GamePlayLoop();
            StartCoroutine(GamePlayeLoopCo);
        }
        
    }

    IEnumerator GamePlayLoop(){
        while (true){
            InitializeDummy();
            yield return new WaitForSeconds(dummyPerMinute/60);
        }
    }

    IEnumerator WaitforReaction(){

        yield return new WaitForSeconds(reactionTime);
        WaitForReactionCo = null;
        LoseLife();

    }


    private void InitializeDummy(){
        GameObject Instance = Instantiate(PF_Dummy, targetStart.transform.position, Quaternion.identity);
            int RandomTemp = UnityEngine.Random.Range(0, numberOfRoads);
            Instance.transform.parent = DummyContainer.transform;
            Instance.GetComponent<Renderer>().material = destinations[RandomTemp].color;
            Instance.name = destinations[RandomTemp].color.name;
            Instance.transform.DOMove(targetFinish.position, dummySpeed).OnComplete(() => { 
                if(WaitForReactionCo == null){
                    Instance.transform.DOMove(realFinish.position, reactionTime);
                    ActiveDummy = Instance; 
                    WaitForReactionCo = WaitforReaction();
                    StartCoroutine(WaitForReactionCo);
                }else{
                    Destroy(ActiveDummy);
                    LoseLife();
                }
                
                });
    }

    private void LoseLife(){
        Life--;
        LifeText.text = Life.ToString();
        if(Life == 0){
            StopCoroutine(GamePlayeLoopCo);
            GamePlayeLoopCo = null;
            foreach (Transform item in DummyContainer.transform)
            {
                Destroy(item.gameObject);
            }
            GameOverScreen.SetActive(true);
        }
    }

    private void PointLogic(){
        Points++;
        pointText.text = Points.ToString();
        
    }

    public void OnClick_RestartButton(){
        GameOverScreen.SetActive(false);
        InitializeGame();
    }

    private void SwipeColor(GameObject target, string swipeDirection){
        if(swipeDirection == "Right"){
            target.transform.DOMove(destinations[0].Endpoint.position, dummySpeed).OnComplete(() => { Destroy(target);});
            if(target.name != destinations[0].color.name){
                LoseLife();
            }else{
                PointLogic();
            }
        }
        
        if(swipeDirection == "Up"){
            target.transform.DOMove(destinations[1].Endpoint.position, dummySpeed).OnComplete(() => { Destroy(target);});
            if(target.name != destinations[1].color.name){
                LoseLife();
            }else{
                PointLogic();
            }
        }

        if(swipeDirection == "Left"){
            target.transform.DOMove(destinations[2].Endpoint.position, dummySpeed).OnComplete(() => { Destroy(target);});
            if(target.name != destinations[2].color.name){
                LoseLife();
            }else{
                PointLogic();
            }
        }
        if(swipeDirection == "UpRight"){
            target.transform.DOMove(destinations[3].Endpoint.position, dummySpeed).OnComplete(() => { Destroy(target);});
            if(target.name != destinations[3].color.name){
                LoseLife();
            }else{
                PointLogic();
            }
        }
        if(swipeDirection == "UpLeft"){
            target.transform.DOMove(destinations[4].Endpoint.position, dummySpeed).OnComplete(() => { Destroy(target);});
            if(target.name != destinations[4].color.name){
                LoseLife();
            }else{
                PointLogic();
            }
        }
        if(swipeDirection == "DownRight"){
            target.transform.DOMove(destinations[5].Endpoint.position, dummySpeed).OnComplete(() => { Destroy(target);});
            if(target.name != destinations[5].color.name){
                LoseLife();
            }else{
                PointLogic();
            }
        }
        if(swipeDirection == "DownLeft"){
            target.transform.DOMove(destinations[6].Endpoint.position, dummySpeed).OnComplete(() => { Destroy(target);});
            if(target.name != destinations[6].color.name){
                LoseLife();
            }else{
                PointLogic();
            }
        }
        
        
    }


    public void OnSwipe(string swipe){

        if(ActiveDummy != null){
            DOTween.Complete(ActiveDummy);
            StopCoroutine(WaitForReactionCo);
            WaitForReactionCo = null;
            SwipeColor(ActiveDummy, swipe);
            ActiveDummy = null;
        }

        Debug.Log(swipe);
    }
}

[Serializable]
public class Destiny{
    public Material color;
    public GameObject Road;
    public Transform Endpoint;
}