using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    private object[] m_list;

    public GameEvent(params object[] _list)
    {
        this.m_list = _list;
    }

    public object[] GetParameters()
    {
        return m_list;
    }
}
