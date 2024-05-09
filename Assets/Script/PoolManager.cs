using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    
    private int m_prefabsToInitialize = 10;
    public GameObject m_prefab;

    private List<GameObject> m_pooledObjects = new List<GameObject>();

    private static PoolManager _instance;

    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindFirstObjectByType<PoolManager>();
            }
            return _instance;
        }
    }

    void Start()
    {
        for(int i = 0; i < m_prefabsToInitialize; i++)
        {
            CreateObjectInPool();
        }
    }


    //Crea un nuevo objeto para la pool.
    private GameObject CreateObjectInPool(){
        GameObject _prefab = Instantiate(m_prefab);
            
        _prefab.name = _prefab.name + m_pooledObjects.Count;
        _prefab.transform.SetParent(gameObject.transform);
        m_pooledObjects.Add(_prefab);
        _prefab.SetActive(false);

        return _prefab;
    }

    public GameObject GetPooledObject()
    {
        
        //Busca en la lista si hay un objeto en la pool disponible.
        for (int i = 0; i < m_pooledObjects.Count; i++)
        {
            if (!m_pooledObjects[i].activeInHierarchy)
            {
                return m_pooledObjects[i];
            }
        }
        //Sino, crea uno nuevo y lo añade
        return CreateObjectInPool();
    }

}

