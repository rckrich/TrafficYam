using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MainViewID
{
    None,
}

public class ViewModelManager : Manager<ViewModelManager>
{
    private const int MIN_BACK_STACK_COUNT = 1;

    private Stack<ViewModel> m_backViewStack;
    private List<ViewModel> m_spawnedViewsList;

    [Header("Views Array")]
    [SerializeField]
    private ViewModel[] m_mainViews = null;

    [Header("Current View")]
    [SerializeField]
    private ViewModel m_currentView = null;

    [Header("Header View")]
    [SerializeField]
    private ViewModel m_headerView = null;

    [Header("General Loading Canvas")]
    [SerializeField]
    private GameObject m_loadingCanvas = null;

    [Header("View Types Data")]
    [SerializeField]
    private SO_SpawnableViewModelTypes m_viewModelTypesData;

    [Space]

    public RectTransform m_spawnedViewsParent;

    private bool m_isCurrentViewSpawned = false;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_backViewStack = new Stack<ViewModel>();
        m_spawnedViewsList = new List<ViewModel>();
    }

    public void BackToPreviousView()
    {
        if (m_backViewStack.Count >= MIN_BACK_STACK_COUNT)
        {
            m_isCurrentViewSpawned = CeckIfCurrentViewSpawned(m_currentView);

            if (m_isCurrentViewSpawned)
            {
                RemoveSpawnedViewFromList(m_currentView);
                Destroy(m_currentView.gameObject);

            }
            else
            {
                m_currentView.SetActive(false);
            }

            m_currentView = m_backViewStack.Peek();
            m_backViewStack.Pop();
        }
    }

    public void ChangeToMainView(MainViewID _viewID, bool _isSubMainView = false)
    {
        SetChangeOfMainViews(_viewID, _isSubMainView);
    }

    public void ChangeToSpawnedView(string _SpawnedViewType)
    {
        SetChangeOfSpawnedViews(_SpawnedViewType);
    }

    public ViewModel GetMainView(MainViewID viewID)
    {
        foreach (ViewModel viewInstance in m_mainViews)
        {
            if (((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).GetViewID() == viewID)
            {
                return viewInstance;
            }
        }

        return null;
    }

    public ViewModel GetCurrentView()
    {
        return m_currentView;
    }

    public ViewModel GetHeaderView()
    {
        return m_headerView;
    }

    public void SetHeaderViewActive(bool _value)
    {
        m_headerView.SetActive(_value);
    }

    public bool HasSubViews()
    {
        if (m_backViewStack.Count <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetCurrentViewSiblingIndex(int _value)
    {
        m_currentView.transform.SetSiblingIndex(_value);
    }

    public void SetCurrentViewAsFirstSibling()
    {
        m_currentView.transform.SetAsFirstSibling();
    }

    public void SetCurrentViewAsLastSibling()
    {
        m_currentView.transform.SetAsLastSibling();
    }

    public void SetMainViewSiblingIndex(MainViewID _mainViewID, int _value)
    {
        GetMainView(_mainViewID).transform.SetSiblingIndex(_value);
    }

    public void SetMainViewAsFirstSibling(MainViewID _mainViewID)
    {
        GetMainView(_mainViewID).transform.SetAsFirstSibling();
    }

    public void SetMainViewAsLastSiblig(MainViewID _mainViewID)
    {
        GetMainView(_mainViewID).transform.SetAsLastSibling();
    }

    private void SetChangeOfMainViews(MainViewID _mainViewID, bool _isSubView = false)
    {

        if (_isSubView)
        {
            if (_mainViewID != ((ViewModel)m_currentView.GetComponent(typeof(ViewModel))).GetViewID())
                m_backViewStack.Push(m_currentView);
        }
        else
        {
            ClearMainViews();
            DestroyAllSpawnedViews();
            m_backViewStack.Clear();
        }

        ChangeToMainView(_mainViewID);
    }

    private void SetChangeOfSpawnedViews(string _spawnedViewType)
    {
        m_backViewStack.Push(m_currentView);

        SpawnViewOfType(_spawnedViewType);
    }

    private void ChangeToMainView(MainViewID _mainViewID)
    {
        foreach (ViewModel viewInstance in m_mainViews)
        {
            if (((ViewModel)viewInstance.GetComponent(typeof(ViewModel))).GetViewID() == _mainViewID)
            {
                this.m_currentView = viewInstance;

                ((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).SetActive(true);
            }
        }
    }

    private void ClearMainViews()
    {
        foreach (ViewModel viewInstance in m_mainViews)
        {
            ((ViewModel)viewInstance.GetComponent(typeof(ViewModel))).SetActive(false);
        }
    }

    private void SpawnViewOfType(string _type)
    {
        GameObject searchedView = m_viewModelTypesData.m_viewTypes.Find(x => x.m_viewModelType.Equals(_type)).m_viewModelPrefab;

        ViewModel spawnedView = Instantiate(searchedView, m_spawnedViewsParent).GetComponent<ViewModel>();

        m_currentView = spawnedView;

        m_spawnedViewsList.Add(spawnedView);

        spawnedView.transform.SetAsLastSibling();

        LayoutRebuilder.ForceRebuildLayoutImmediate(m_spawnedViewsParent);
    }

    private void DestroyAllSpawnedViews()
    {
        foreach (ViewModel view in m_spawnedViewsList)
        {
            Destroy(view.gameObject);
        }

        m_spawnedViewsList.Clear();
    }

    private void RemoveSpawnedViewFromList(ViewModel _currentView)
    {
        if (CeckIfCurrentViewSpawned(_currentView))
        {
            m_spawnedViewsList.Remove(_currentView);
        }
    }

    private bool CeckIfCurrentViewSpawned(ViewModel _currentView)
    {
        return m_spawnedViewsList.Contains(_currentView);
    }
}
