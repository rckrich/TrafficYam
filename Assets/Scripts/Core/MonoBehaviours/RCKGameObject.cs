using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RCKGameObject : MonoBehaviour
{
    public virtual void Initialize(params object[] _list) { }

    public virtual void SetActive(bool _value) { this.gameObject.SetActive(_value); }

    protected void AddEventListener<T>(EventManager.EventDelegate<T> _listener) where T : GameEvent
    {
        EventManager.m_Instance.AddListener<T>(_listener);
    }

    protected void InvokeEvent<T>(T _event) where T : GameEvent
    {
        EventManager.m_Instance.InvokeEvent<T>(_event);
    }

    protected void RemoveEventListener<T>(EventManager.EventDelegate<T> _listener) where T : GameEvent
    {
        EventManager.m_Instance.RemoveListener<T>(_listener);
    }

    protected virtual bool CheckDeviceNetworkReachability()
    {
        return (Application.internetReachability != NetworkReachability.NotReachable);
    }
}
