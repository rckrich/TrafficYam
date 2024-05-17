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
    public SpriteRenderer m_spriteRenderer;
    public Animator m_animator;

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
    private void OnTriggerEnter(Collider other) {
        if(other.tag =="TrafficObject"){
            GameManager.m_Instance.TrafficObjectColision(gameObject, other.gameObject);
        }
    }
}
