using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrafficObjectType
{
    None,
    Food,
    Trash,
    Coin
}

public class TrafficObject : RCKGameObject
{
    public string m_name;
    public Sprite m_sprite;
    public TrafficObjectType m_type;
    public string m_audioSFX;
}
