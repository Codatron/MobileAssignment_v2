using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class GameInfo
{
    public string GameName;
    public string GameID;
    public List<PlayerInfo> players;
}

[Serializable]
public class PlayerInfo
{
    public string Name;
    public bool Hidden;
    public List<Vector2Int> GridLocation;
    public int Attempts;
    public int TotalObjectsFound;
    public float Time;
}
   
