using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject seeker;
    
    List<GameObject> nuts = new List<GameObject>();
    GridGenerate grid;

    private void Awake()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Child(SaveManager.Instance.gameInfo.gameID).ValueChanged += StartToSeek;
        FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Child(SaveManager.Instance.gameInfo.gameID).ValueChanged += CompareObjectsFound;
    }

    private void OnDestroy()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Child(SaveManager.Instance.gameInfo.gameID).ValueChanged -= StartToSeek;
        FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Child(SaveManager.Instance.gameInfo.gameID).ValueChanged -= CompareObjectsFound;
    }

    void Start()
    {
        nuts = GameObject.Find("Hider").GetComponent<Hider>().objects;
        grid = GameObject.Find("GridManager").GetComponent<GridGenerate>();

        StartToSeek(SaveManager.Instance.gameInfo);
    }

    void StartToSeek(object sender, ValueChangedEventArgs firebaseSnapshot)
    {
        if (firebaseSnapshot.DatabaseError != null)
        {
            Debug.LogError(firebaseSnapshot.DatabaseError.Message);
            return;
        }

        GameInfo gameInfo = JsonUtility.FromJson<GameInfo>(firebaseSnapshot.Snapshot.GetRawJsonValue());
        StartToSeek(gameInfo);
    }

    private void StartToSeek(GameInfo gameInfo)
    {
        if (gameInfo.players?[0].hidden == true && gameInfo.players?[1].hidden == true)
        {
            if (SessionData.Instance.playerInGame.playerNumber == 0)
            {
                ResetGrid();
                UpdateNutPosition(gameInfo.players[1].gridPositions);
            }
            else if (SessionData.Instance.playerInGame.playerNumber == 1)
            {
                ResetGrid();
                UpdateNutPosition(gameInfo.players[0].gridPositions);
            }

            seeker.SetActive(true);
        }
    }

    public void UpdateNutPosition(List<Vector3> getGridPosFirebase)
    {
        int index = 0;

        foreach (var nut in nuts)
        {
            nut.transform.position = getGridPosFirebase[index];

            SpriteRenderer spriteRend = nut.GetComponent<SpriteRenderer>();
            spriteRend.sortingOrder = -1;

            grid.grid[(int)getGridPosFirebase[index].x + 2, (int)getGridPosFirebase[index].y + 1].isOccupied = true;
            index++;
        }
        //SessionData.Instance.playerInGame.gridPositions = getGridPosFirebase;
    }

    void ResetGrid()
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                grid.grid[x, y].Init(x, y, false);
            }
        }
    }

    void CompareObjectsFound(object sender, ValueChangedEventArgs firebaseSnapshot)
    {
        if (firebaseSnapshot.DatabaseError != null)
        {
            Debug.LogError(firebaseSnapshot.DatabaseError.Message);
            return;
        }

        GameInfo gameInfo = JsonUtility.FromJson<GameInfo>(firebaseSnapshot.Snapshot.GetRawJsonValue());
        CompareObjectsFound(gameInfo);
    }

    private void CompareObjectsFound(GameInfo gameInfo) 
    {
        if (gameInfo.players?[0].allObjectsFound == true && gameInfo.players?[1].allObjectsFound == true)
        {
            if (gameInfo.players?[0].attempts < gameInfo.players?[1].attempts)
            {
                Debug.Log(gameInfo.players[0].name + " is the winner!");
            }
            else if (gameInfo.players?[0].attempts > gameInfo.players?[1].attempts)
            {
                Debug.Log(gameInfo.players[1].name + " is the winner!");
            }
            else if (gameInfo.players?[0].attempts == gameInfo.players?[1].attempts)
            {
                Debug.Log("You both suck ass, bitches!");
            }

            seeker.SetActive(true);
        }
    }
}
