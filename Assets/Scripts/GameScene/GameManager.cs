using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Hide,
    Seek,
    GameOver
}

[Serializable]
public class OnGameStateChange : UnityEvent<GameState>
{

}

public class GameManager : MonoBehaviour
{
    public UnityEvent<GameState> OnGameStateChange;
    public UIManager UIManager;

    public GameState statePlayer1; // hide, seek, or game over
    public GameState statePlayer2; // hide, seek, or game over

    private void Awake()
    {
        if (OnGameStateChange == null)
            OnGameStateChange = new OnGameStateChange();
    }

    public static void UpdateGameState(bool saveUpdate)
    {
        if (SaveManager.Instance.gameInfo.players[0].Hidden)
        {
            // get p2 info
        }
       
    }
}


