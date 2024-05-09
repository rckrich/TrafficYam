using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficObject : MonoBehaviour
{
    public string m_name;
    public Sprite m_sprite;
    public TrafficObject trafficObject;
    public string m_audioSFX;
    public enum TrafficType
    {
        Food,
        Trash,
        Coin
    }
}
