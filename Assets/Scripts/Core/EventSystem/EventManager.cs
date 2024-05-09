using System;
using System.Collections.Generic;

public class EventManager
{
    static EventManager m_instance;

    public static EventManager m_Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new EventManager();

            return m_instance;
        }
    }

    public delegate void EventDelegate<T>(T e) where T : GameEvent;

    readonly Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

    public void AddListener<T>(EventDelegate<T> _listener) where T : GameEvent
    {
        Delegate outDeletage;

        if (_delegates.TryGetValue(typeof(T), out outDeletage))
        {
            _delegates[typeof(T)] = Delegate.Combine(outDeletage, _listener);
        }
        else
        {
            _delegates[typeof(T)] = _listener;
        }
    }

    public void RemoveListener<T>(EventDelegate<T> _listener) where T : GameEvent
    {
        Delegate outDeletage;
        if (_delegates.TryGetValue(typeof(T), out outDeletage))
        {
            Delegate currentDelegate = Delegate.Remove(outDeletage, _listener);

            if (currentDelegate == null)
            {
                _delegates.Remove(typeof(T));
            }
            else
            {
                _delegates[typeof(T)] = currentDelegate;
            }
        }
    }

    public void InvokeEvent<T>(T _event) where T : GameEvent
    {
        if (_event == null)
        {
            throw new ArgumentNullException("Event of name " + _event.name + " is null");
        }

        Delegate outDelegate;
        if (_delegates.TryGetValue(typeof(T), out outDelegate))
        {
            EventDelegate<T> callback = outDelegate as EventDelegate<T>;
            if (callback != null)
            {
                callback(_event);
            }
        }
    }

    public void ResetEventManager()
    {
        _delegates.Clear();
    }
}
