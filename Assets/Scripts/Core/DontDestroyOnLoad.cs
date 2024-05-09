using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : RCKGameObject
{
	public static DontDestroyOnLoad m_dontDestroyOnLoad;

	private void Start()
	{
		if (m_dontDestroyOnLoad == null)
		{
			DontDestroyOnLoad(gameObject);
			m_dontDestroyOnLoad = this;
		}
		else if (m_dontDestroyOnLoad != this)
		{
			Destroy(gameObject);
		}
	}
}
