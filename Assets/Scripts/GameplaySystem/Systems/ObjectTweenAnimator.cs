using UnityEngine;
using DG.Tweening;


public class ObjectTweenAnimator : RCKGameObject
{
    private Transform m_centerPosition, m_initialPosition;
    public float m_speed;
    [SerializeField] private float MovementTransitionDuration = 1;
    private Tweener[] m_MoveToCenter, m_SwipeMove;
    private bool m_DEBUGTWEENS = false;
    [SerializeField] private TrafficObject m_trafficObject;

    // Start is called before the first frame update

    private void Start() {
        AddEventListener<OnStreak>(Onstreak);
    }

    private void OnDestroy() {
        RemoveEventListener<OnStreak>(Onstreak);
    }

    public void Onstreak(OnStreak _event){
        Debug.Log(m_speed + " " + _event.m_speed);
        m_speed = _event.m_speed;
        if(DOTween.IsTweening(m_trafficObject) && !m_trafficObject.g_amIMoving){
            UpdateSpeed_MoveToCenter();
        }
    }

    public void SetValues(float _speed, Transform _centerPosition, Transform _initialPosition){
        
        m_centerPosition = _centerPosition;
        m_initialPosition = _initialPosition;
        m_speed = _speed;
    }

    private void SetUp_MoveToCenter()
    {
        m_MoveToCenter = new Tweener[1];
        m_MoveToCenter[0] = gameObject.transform.DOMove(m_centerPosition.position, m_speed, true).Pause();
        m_MoveToCenter[0].OnComplete(() => {
            
        });
    }

    private void SetUp_SwipeMove(Transform _destination)
    {
        
        m_SwipeMove = new Tweener[1];
        m_SwipeMove[0] = gameObject.transform.DOMove(_destination.position, MovementTransitionDuration, false).Pause().SetAutoKill(false);
        
        m_SwipeMove[0].OnComplete(() => {
            gameObject.SetActive(false);

        });
        
    }


    public void Play_MoveTocenter()
    {
        gameObject.transform.position = m_initialPosition.position;
        m_trafficObject.g_amIMoving = false;
        if(m_MoveToCenter == null)
        {
            SetUp_MoveToCenter();
        }else{
            Reset_MoveTocenter();
        }
        
        foreach (Tweener item in m_MoveToCenter)
        {
            item.SetAutoKill(false);
            item.Play();
        }

        if (m_DEBUGTWEENS) 
            Debug.Log(gameObject.name + "MoveToCenter" + Time.deltaTime);
    }


    public void Play_SwipeMove(Transform m_animalTrack)
    {
        m_trafficObject.g_amIMoving = true;
        if(m_SwipeMove == null)
        {
            SetUp_SwipeMove(m_animalTrack);
        }else{
            Reset_SwipeMove(m_animalTrack);
        }   
        
        foreach (Tweener item in m_SwipeMove)
        {
            item.Play();
        }

        if (m_DEBUGTWEENS) 
            Debug.Log(gameObject.name + "SwipeMove" + Time.deltaTime);
    }

    private void UpdateSpeed_MoveToCenter(){
        foreach (Tweener item in m_MoveToCenter)
            {
                item.ChangeEndValue(m_centerPosition.position, m_speed);
            }
    }

    private void Reset_MoveTocenter(){
        foreach (Tweener item in m_MoveToCenter)
            {
                item.Restart();
            }
    }

    private void Reset_SwipeMove(Transform m_animalTrack){
        m_SwipeMove[0].ChangeEndValue(m_animalTrack.position);
    }

}
