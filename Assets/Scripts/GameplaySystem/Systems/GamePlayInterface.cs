using UnityEngine;

public class GamePlayInterface: RCKGameObject 
{
    private void OnTriggerEnter(Collider other) {
        GameManager.m_Instance.UpdateTrafficObjectInCenter(other.gameObject);
    }

}
