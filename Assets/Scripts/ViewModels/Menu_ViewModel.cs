using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu_ViewModel : ViewModel
{
   public TMP_Text m_TeamCapacity;

   public void OnClick_Shop()
   {
        ViewModelManager.m_Instance.ChangeToSpawnedView("Store");
    }

    public void OnClick_Money()
    {
        ViewModelManager.m_Instance.ChangeToSpawnedView("BuyMoney");
    }

    public void OnClick_Energy()
    {
        ViewModelManager.m_Instance.ChangeToSpawnedView("BuyEnergy");
    }

    public void OnClick_Forest()
    {

    }

    public void OnClick_Beach()
    {

    }

    public void OnClick_City()
    {

    }

    public void OnClick_Play()
    {

    }
}
