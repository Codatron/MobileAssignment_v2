using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public delegate void OnOpponentInfoLoaded();

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
    public static OnOpponentInfoLoaded onOpponentInfoLoaded;
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
        if (SaveManager.Instance.gameInfo.players[0].hidden)
        {
            // Get p2 info - name and grid locations;
            LoadOpponentName();
            LoadOpponentGridLocations();
            onOpponentInfoLoaded?.Invoke();
        }
       
    }

    private static void LoadOpponentName()
    {
        string nameP2 = SaveManager.Instance.gameInfo.players[1].name;
    }

    private static void LoadOpponentGridLocations()
    {
        // Does not compile, but am I on the right track?
        //Vector2Int gridLocationP2 = SaveManager.Instance.gameInfo.players[1].GridLocation;
    }
}


