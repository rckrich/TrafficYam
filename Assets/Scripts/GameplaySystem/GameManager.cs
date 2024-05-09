using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState state;
    public float m_Timer;
    public int m_Coins;
    public int m_lives = 3;
    public int m_NumberOfAnimals;
    private float m_reactionTime;
    private float m_foodPerMinutes;
    private float m_dummySpeed;


    private void Awake()
    {
        Instance = this;
    }

    public void UpdateGameState(GameState _state)
    {
        state = _state;
        switch(_state)
        {
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
        }
    }

    public void OnSwipe(string side)
    {

    }

    private void SwipeLogic(string swipe)
    {

    }

    private void LoseLife()
    {
        if(m_lives == 0)
        {
            UpdateGameState(GameState.Lose);
        }
        else
        {
            m_lives -= 1;
        }
    }

    public void TimerEnds()
    {
            UpdateGameState(GameState.Victory);
        
    }
    public enum GameState
    {
        Victory,
        Lose
    }
}
