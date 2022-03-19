using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public delegate void OnGameOver(string name);
public delegate void OnGameStart();
public delegate void ObjectsFound();

public class TurnManager : MonoBehaviour
{
    public GameObject seeker;
    public static event ObjectsFound onAllObjectsFound;
    public static OnGameStart onGameStart;
    public static OnGameOver onGameOver;
    public TMP_Text winnerName;
    public TMP_Text playerComparison;
    public GameObject waitPanel;

    private List<GameObject> nuts = new List<GameObject>();
    private GridGenerate grid;

    private void OnEnable()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Child(SaveManager.Instance.gameInfo.gameID).ValueChanged += StartToSeek;
        FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Child(SaveManager.Instance.gameInfo.gameID).ValueChanged += CompareObjectsFound;
    }

    private void OnDisable()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Child(SaveManager.Instance.gameInfo.gameID).ValueChanged -= StartToSeek;
        FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Child(SaveManager.Instance.gameInfo.gameID).ValueChanged -= CompareObjectsFound;
    }

    void Start()
    {
        nuts = GameObject.Find("Hider").GetComponent<Hider>().objects;
        grid = GameObject.Find("GridManager").GetComponent<GridGenerate>();

        //StartToSeek(SaveManager.Instance.gameInfo);
    }

    void StartToSeek(object sender, ValueChangedEventArgs firebaseSnapshot)
    {
        if (firebaseSnapshot.DatabaseError != null)
        {
            Debug.LogError(firebaseSnapshot.DatabaseError.Message);
            return;
        }

        // This is where we retrieve the info from firebase
        GameInfo gameInfo = JsonUtility.FromJson<GameInfo>(firebaseSnapshot.Snapshot.GetRawJsonValue());
        StartToSeek(gameInfo);  // Argument out of range exception

        if (gameInfo.players[0].userID == FirebaseAuth.DefaultInstance.CurrentUser.UserId)
        {
            SessionData.Instance.playerInGame = gameInfo.players[0];
        }
        else if (gameInfo.players[1].userID == FirebaseAuth.DefaultInstance.CurrentUser.UserId)
        {
            SessionData.Instance.playerInGame = gameInfo.players[1];
        }

        SaveManager.Instance.gameInfo = gameInfo;

        CompareObjectsFound(gameInfo);
    }

    private void StartToSeek(GameInfo gameInfo)
    {
        if (gameInfo.players.Count != 2)
            return;

        if (gameInfo.players?[0].hidden == true && gameInfo.players?[1].hidden == true)
        {
            //waitPanel.SetActive(false);
            onGameStart?.Invoke();

            if (SessionData.Instance.playerInGame.playerNumber == 0)
            {
                ResetGrid();
                UpdateNutPosition(gameInfo.players[1].gridPositions);   // Argument out of range exception
            }
            else if (SessionData.Instance.playerInGame.playerNumber == 1)
            {
                ResetGrid();
                UpdateNutPosition(gameInfo.players[0].gridPositions);
            }

            seeker.SetActive(true);
        }
    }

    public void UpdateNutPosition(List<Vector3> getGridPosFirebase) // Argument out of range exception
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

    public void CompareObjectsFound(object sender, ValueChangedEventArgs firebaseSnapshot)
    {
        if (firebaseSnapshot.DatabaseError != null)
        {
            Debug.LogError(firebaseSnapshot.DatabaseError.Message);
            return;
        }

        GameInfo gameInfo = JsonUtility.FromJson<GameInfo>(firebaseSnapshot.Snapshot.GetRawJsonValue());
        //CompareObjectsFound(gameInfo);
    }

    public void CompareObjectsFound(GameInfo gameInfo) 
    {
        if (gameInfo.players?[0].allObjectsFound == true && gameInfo.players?[1].allObjectsFound == true)
        {
            onAllObjectsFound?.Invoke();    // Links to Seeker.cs
            playerComparison.text = gameInfo.players[0].name + ": " + gameInfo.players[0].attempts + "       " + gameInfo.players[1].name + ": " + gameInfo.players[1].attempts;

            if (gameInfo.players?[0].attempts < gameInfo.players?[1].attempts)
            {
                winnerName.text = gameInfo.players[0].name + " is the winner!";
                Debug.Log(gameInfo.players[0].name + " is the winner!");
            }
            else if (gameInfo.players?[0].attempts > gameInfo.players?[1].attempts)
            {
                winnerName.text = gameInfo.players[1].name + " is the winner!";
                Debug.Log(gameInfo.players[1].name + " is the winner!");
            }
            else if (gameInfo.players?[0].attempts == gameInfo.players?[1].attempts)
            {
                winnerName.text = "It's a tie!";
                Debug.Log("You both suck ass, bitches!");
            }

            seeker.SetActive(true);
        }
    }
}
