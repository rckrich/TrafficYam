using UnityEngine;
using DG.Tweening;


public class ObjectTweenAnimator : RCKGameObject
{
    private Transform m_centerPosition, m_initialPosition;
    private float m_speed = 5;
    private float m_speedModifier = 1;
    public float MovementTransitionDuration = 1;
    private Tweener[] m_MoveToCenter, m_SwipeMove;
    private bool m_DEBUGTWEENS = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetValues(float _speed, Transform _centerPosition, Transform _initialPosition){
        
        m_centerPosition = _centerPosition;
        m_initialPosition = _initialPosition;
        m_speed = _speed;
    }

    private void SetUp_MoveToCenter()
    {
        m_MoveToCenter = new Tweener[1];
        m_MoveToCenter[0] = gameObject.transform.DOMove(m_centerPosition.position, m_speed*m_speedModifier, true).Pause();
        
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
            Debug.Log(gameObject.name + "MoveToCenter");
    }


    public void Play_SwipeMove(Transform m_animalTrack)
    {
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
            Debug.Log(gameObject.name + "SwipeMove");
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
