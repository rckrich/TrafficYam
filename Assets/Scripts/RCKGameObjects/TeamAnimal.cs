using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TeamAnimal : RCKGameObject
{
    
    public Animal m_Animal;
    public Image image;
 
    // Start is called before the first frame update
    void Start()
    {
        LockOrPlus();
    }

    public void LockOrPlus()
    {
        //ToDo if(lock){} else{}
    }


    public void InitializePhoto(){
        if(m_Animal.m_AnimalPhoto != null){
            image.sprite = m_Animal.m_AnimalPhoto.sprite;
        }
        
    }
    
}
