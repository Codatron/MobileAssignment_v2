using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class GameInfo
{
    public string displayGameName; // Nut Hunt
    public string gameID;   // Session #
    //public int numberOfPlayers = 2;
    public int seed;
    public List<PlayerInGame> players;
}

[Serializable]
public class PlayerInfo
{
    public string name;
    public List<string> activeGames;
}

[Serializable]
public class PlayerInGame
{
    public string userID;
    public int playerNumber;
    public string name;
    public bool hidden;
    public List<Vector3> gridPositions;
    public int attempts;
    public bool allObjectsFound;
    public int totalObjectsFound;
    public float time;
}

// Update List<PlayerInfo> in GameInfo 
// - database should check if other is hidden
//      if !isHidden
//          not then wait for db
//      else
//          get other info - PLAY!
// - send update PlayerInfo to db
// - check other PlayerInfo
//      if
// 