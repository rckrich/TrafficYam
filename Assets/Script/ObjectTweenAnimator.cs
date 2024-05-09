using UnityEngine;
using DG.Tweening;


public class ObjectTweenAnimator : MonoBehaviour
{
    public Transform m_centerPosition, m_initialPosition;
    public bool m_isAvailable;
    public float m_speed = 5;
    public float m_speedModifier = 1;
    public float MovementTransitionDuration = 1;
    private Tweener[] m_MoveToCenter, m_MoveToDestination;
    private bool m_DEBUGTWEENS = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void SetUp_MoveToCenter()
    {
        m_MoveToCenter = new Tweener[1];
        m_MoveToCenter[0] = gameObject.transform.DOMove(m_centerPosition.position, m_speed*m_speedModifier, true).Pause().SetAutoKill(false);
        
        m_MoveToCenter[0].OnComplete(() => {

        });
        
    }

    private void SetUp_MoveToDestination(Transform _destination)
    {
        
        m_MoveToDestination = new Tweener[1];
        m_MoveToDestination[0] = gameObject.transform.DOMove(_destination.position, MovementTransitionDuration, true).Pause().SetAutoKill(false);
        
        m_MoveToDestination[0].OnComplete(() => {
            
        });
        
    }


    public void Play_MoveTocenter()
    {
        gameObject.transform.position = m_initialPosition.position;

        if(m_MoveToCenter == null)
        {
            SetUp_MoveToCenter();
        }
        
        foreach (Tweener item in m_MoveToCenter)
        {
            item.Play();
        }

        if (m_DEBUGTWEENS) 
            Debug.Log(gameObject.name + "MoveToCenter");
    }


    public void Play_MoveToDestination(Transform m_destination)
    {
        if(m_MoveToDestination == null)
        {
            SetUp_MoveToDestination(m_destination);
        }else{
            Reset_MoveToDestination(m_destination);
        }   
        
        foreach (Tweener item in m_MoveToDestination)
        {
            item.Play();
        }

        if (m_DEBUGTWEENS) 
            Debug.Log(gameObject.name + "MoveToDestination");
    }

    private void Reset_MoveToDestination(Transform m_destination){
        m_MoveToDestination[0].ChangeEndValue(m_destination);
    }

}
