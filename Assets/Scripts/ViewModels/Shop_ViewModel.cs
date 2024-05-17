using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_ViewModel : ViewModel
{
    public void OnClick_BackToPreviusScreen() 
    { 
       ViewModelManager.m_Instance.BackToPreviousView();
    }

    public void OnClick_BuyEnergy()
    {
        ViewModelManager.m_Instance.ChangeToSpawnedView("BuyEnergy");
    }

    public void OnClick_BuyMoney()
    {
        ViewModelManager.m_Instance.ChangeToSpawnedView("BuyMoney");
    }

    public void OnClick_VIP()
    {

    }

    public void OnClick_Energy()
    {

    }

    public void OnClick_BuyRoomButton()
    {

    }

    public void OnClick_ForestButton()
    {

    }

    public void OnClick_BeachButton()
    {

    }


}
