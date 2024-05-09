using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : RCKGameObject where T : RCKGameObject
{
	private static T m_instance;

	public static T m_Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindAnyObjectByType<T>();
			}
			return m_instance;
		}
	}

    private void Awake()
    {
		OnAwake();
    }

    protected virtual void OnAwake()
	{
		if (m_instance == null)
		{
			m_instance = this as T;
		}
		else
		{
			Destroy(gameObject);
		}
	}


}
