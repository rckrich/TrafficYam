using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : TrafficObjectContainer
{
   private void OnTriggerEnter(Collider other) {
    if(other.gameObject.tag == "TrafficObject") {
        if(other.gameObject.GetComponent<TrafficObject>().m_type == TrafficObjectType.Bomb) {
            GameManager.m_Instance.OnTriggerSuccess();
        }
    }
   }
}
