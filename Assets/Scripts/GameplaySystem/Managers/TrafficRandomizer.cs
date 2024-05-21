using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TrafficRandomizer : MonoBehaviour
{
    public int g_foodPerAnimal;

    //private vvvvvvvv
    public List<int> m_trafficObjectRandomList = new List<int>();
    public List<int> m_ComplementaryTrafficObjects = new List<int>();
    public List<SO_TrafficObject> m_typesOfTrafficObjects = new List<SO_TrafficObject>();
    
    public List<int> Randomize(float _waitSeconds, float _timer, float _animalTrack, float _proportionCarnivore){

        float _totalTrafficObjects = _timer/ _waitSeconds;
        float _CarnivoreFood = g_foodPerAnimal * _proportionCarnivore;
        float _HerviboreFood = (_animalTrack * g_foodPerAnimal) - _CarnivoreFood;

        if(_HerviboreFood > 0 ){
            for (int i = 0; i < _HerviboreFood; i++)
            {
                m_trafficObjectRandomList.Add(0);
                m_ComplementaryTrafficObjects.Add(0);
            }
        }
        if(_CarnivoreFood > 0 ){
            for (int i = 0; i < _CarnivoreFood; i++)
            {
                m_trafficObjectRandomList.Add(1);
                m_ComplementaryTrafficObjects.Add(1);
            }
        }
        _totalTrafficObjects = _totalTrafficObjects - _CarnivoreFood - _HerviboreFood;
        for (int i = 0; i < _totalTrafficObjects; i++)
        {
            if( i % 2 == 0 ){
                m_trafficObjectRandomList.Add(2);
                m_ComplementaryTrafficObjects.Add(2);
            }else{
                m_trafficObjectRandomList.Add(3);
                m_ComplementaryTrafficObjects.Add(3);
            }
            
        } 
        Shuffle(m_ComplementaryTrafficObjects);
        Shuffle(m_trafficObjectRandomList);
        return m_trafficObjectRandomList;
    }

    public SO_TrafficObject GetTypeFromList(){
        int _value;
        if(m_trafficObjectRandomList.Count != 0){
            _value = m_trafficObjectRandomList[0]; 
            m_trafficObjectRandomList.RemoveAt(0);
        }else{
            _value = m_ComplementaryTrafficObjects[0]; 
            m_ComplementaryTrafficObjects.RemoveAt(0);
        }
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
