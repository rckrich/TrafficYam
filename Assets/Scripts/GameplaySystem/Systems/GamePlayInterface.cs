using UnityEngine;

public enum InterfaceType
{
    None,
    LimitObject,
    AnimalObject
}

public class GamePlayInterface: RCKGameObject 
{
    [SerializeField] private InterfaceType m_InterfaceType;
    private void OnTriggerEnter(Collider other) {
        if(m_InterfaceType == InterfaceType.LimitObject){
            GameManager.m_Instance.UpdateTrafficObjectInCenter(other.gameObject);
            return;
        }
        if(m_InterfaceType == InterfaceType.AnimalObject){
            OnAnimalObjectTriggerEnter(other.gameObject);
            return;
        }
    }
    private void OnAnimalObjectTriggerEnter(GameObject _Object){
        if(_Object.GetComponent<TrafficObject>().m_type != TrafficObjectType.Food){
            GameManager.m_Instance.OnTriggerFail();
            return;
        }
        FoodType _value = _Object.GetComponent<TrafficObject>().ReturnFoodType();
        FoodType _selfvalue = gameObject.GetComponent<Animal>().m_foodType;

        if( _value== _selfvalue){
            GameManager.m_Instance.OnTriggerSuccess();
            return;
        }
        if( _selfvalue == FoodType.Omnivore){
            GameManager.m_Instance.OnTriggerSuccess();
            return;
        }
        GameManager.m_Instance.OnTriggerFail();
        
    }
}
