using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Hide,
    Seek,
    GameOver
}

public enum Players
{
    Hider,
    Seeker
}

public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public Players player;
    public UIManager UIManager;
}
