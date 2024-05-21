using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameDataManager : Manager<GameDataManager>
{
    public Animal[] m_animalList;
    [SerializeField] private List<TextMeshProUGUI> m_text = new List<TextMeshProUGUI>();
    public List<TeamAnimal> g_animalTeam = new List<TeamAnimal>();

    private void Start() {
    }

    public Animal[] ReturnAnimalList(){
        return m_animalList;
    }
    
    public void OnClick_SelectAnimal(int _value){
        if(m_animalList[_value].m_foodType == FoodType.None){
            m_animalList[_value].m_foodType = FoodType.Omnivore;
            m_text[_value].text = "Omnivore";
        }else{
            m_animalList[_value].m_foodType = FoodType.None;
            m_text[_value].text = "None";
        }
    }
    public void OnClick_SetAnimalProperties(int _value){
        if(m_animalList[_value].m_foodType != FoodType.None){
            switch (m_animalList[_value].m_foodType)
            { 
                case FoodType.Herbivorous:
                m_animalList[_value].m_foodType = FoodType.Carnivorous;
                m_text[_value].text = "Carnivorous";
                break;

                case FoodType.Carnivorous:
                m_animalList[_value].m_foodType = FoodType.Omnivore;
                m_text[_value].text = "Omnivore";
                break;

                case FoodType.Omnivore:
                m_animalList[_value].m_foodType = FoodType.Herbivorous;
                m_text[_value].text = "Herbivorous";
                break;
            }
        }
    }

 
    

}
