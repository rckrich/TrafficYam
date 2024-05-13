using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TrafficRandomizer : MonoBehaviour
{

    public float m_foodPerAnimal;
    public List<int> m_TrafficObjectRandomList = new List<int>();
    public List<SO_TrafficObject> m_typesOfTrafficObjects = new List<SO_TrafficObject>();
    
    public List<int> Randomize(float _waitSeconds, float _timer, float _animalTrack, float _proportionCarnivore){

        float _totalTrafficObjects = _waitSeconds * _timer;
        float _totalFoodforAnimals = _animalTrack * m_foodPerAnimal;

        float _CarnivoreFood = m_foodPerAnimal * _proportionCarnivore;
        float _HerviboreFood = _totalFoodforAnimals - _CarnivoreFood;

        if(_HerviboreFood > 0 ){
            for (int i = 0; i < _HerviboreFood; i++)
            {
                m_TrafficObjectRandomList.Add(0);
            }
        }
        if(_CarnivoreFood > 0 ){
            for (int i = 0; i < _CarnivoreFood; i++)
            {
                m_TrafficObjectRandomList.Add(1);
            }
        }
        _totalTrafficObjects = _totalTrafficObjects - _CarnivoreFood - _HerviboreFood;
        for (int i = 0; i < _totalTrafficObjects; i++)
        {
            if( i % 2 == 0 ){
                m_TrafficObjectRandomList.Add(2);
            }else{
                m_TrafficObjectRandomList.Add(3);
            }
            
        } 
        Shuffle(m_TrafficObjectRandomList);
        return m_TrafficObjectRandomList;
    }

    public SO_TrafficObject GetTypeFromList(){
        int _value = m_TrafficObjectRandomList[0]; 
        m_TrafficObjectRandomList.RemoveAt(0);
        return m_typesOfTrafficObjects[_value];
    }

    public void Shuffle(List<int> self){
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = self.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            int value = self[k];
            self[k] = self[n];
            self[n] = value;
        }
        
    }
}
