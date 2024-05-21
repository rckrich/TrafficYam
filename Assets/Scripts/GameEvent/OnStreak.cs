using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStreak : GameEvent
{
    public float m_speed;

    public OnStreak(float speed) : base(speed)
    {
        m_speed = speed;
    }

}
