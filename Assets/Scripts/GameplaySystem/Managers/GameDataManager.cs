using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Manager<GameDataManager>
{
    public int g_animalsInPlay;
    
    public void SetAnimalsInPlay(int _value){
        g_animalsInPlay = _value;
        
    } 

}
