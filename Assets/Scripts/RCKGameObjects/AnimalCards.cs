using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AnimalCards : RCKGameObject
{
    
    public Image m_FoodImage;
    public TMP_Text m_AnimalName;
    public Image m_AnimalImage;
    private Animal m_Animal;
    public FoodType m_FoodType;
    
    public void Lock()
    {

    }

    public void OnClick_AnimalCard(int value)
    {
        GameDataManager.m_Instance.m_animalList[value].m_AnimalName = m_AnimalName.text;
        GameDataManager.m_Instance.m_animalList[value].m_AnimalPhoto = m_AnimalImage;
        GameDataManager.m_Instance.m_animalList[value].m_foodType = m_FoodType;
    }
}
