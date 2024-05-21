using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FoodType
{
    None,
    Herbivorous,
    Carnivorous,
    Omnivore
}

public class Animal : RCKGameObject
{
    public FoodType m_foodType;
    public int m_HungerCapacity;
    public Image m_AnimalPhoto;
    public string m_AnimalName;
}