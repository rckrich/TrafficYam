using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ViewModelType
{
    public string m_viewModelType;
    public GameObject m_viewModelPrefab;
}

[CreateAssetMenu(fileName = "SpawnableViewModelTypes", menuName = "ScriptableObjects/SpawnableViewModelTypes", order = 0)]
public class SO_SpawnableViewModelTypes : ScriptableObject
{
    public List<ViewModelType> m_viewTypes = new List<ViewModelType>();
}
