using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public FoodType m_foodType;
    public int m_HungerCapacity;

    public enum FoodType
    {
        herbivorous,
        carnivorous,
        omnivore
    }
}
