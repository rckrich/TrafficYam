using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrafficObjectType
{
    None,
    Food,
    Bomb,
    Coin
}

public class TrafficObject : RCKGameObject
{
    public TrafficObjectType m_type;
    public SO_TrafficObject m_typeReference;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Animator m_animator;
    public bool g_amIMoving;

    public void ChangeType(SO_TrafficObject _reference){
        m_typeReference = _reference;
        SetProperties();
    }

    private void SetProperties(){
        m_type = m_typeReference.m_type;
        m_spriteRenderer.sprite = m_typeReference.m_sprite;
        
        
        if(m_typeReference.m_usesAnimController){
            m_animator.runtimeAnimatorController = m_typeReference.m_animatorController;
        }
    }

    public FoodType ReturnFoodType(){
        if(m_typeReference is SO_FoodTrafficObject){
            var _food = m_typeReference as SO_FoodTrafficObject;
            return _food.m_foodType;
        }else{
            return FoodType.None;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag =="TrafficObject" && !g_amIMoving){
            GameManager.m_Instance.TrafficObjectColision(gameObject, other.gameObject);
        }
    }

    private void OnMouseDown() {
        if(m_type == TrafficObjectType.Coin){
            GameManager.m_Instance.OnClickCoin();
            DG.Tweening.DOTween.Pause(gameObject);
            gameObject.SetActive(false);
        }
    }
}
