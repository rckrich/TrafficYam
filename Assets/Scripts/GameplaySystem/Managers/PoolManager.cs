using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PoolManager : Manager<PoolManager>
{
    
    [SerializeField] private int m_prefabsToInitialize = 3;
    [SerializeField] private GameObject m_prefab;

    private List<GameObject> m_pooledObjects = new List<GameObject>();

    void Start()
    {
        for(int i = 0; i < m_prefabsToInitialize; i++)
        {
            CreateObjectInPool();
        }
    }

    public void RecoverPooledObjects(){
        foreach (GameObject item in m_pooledObjects)
        {
            DOTween.Pause(item.transform);
            item.SetActive(false);
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

