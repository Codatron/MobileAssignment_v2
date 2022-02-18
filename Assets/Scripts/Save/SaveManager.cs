using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Firebase.Database;
using Firebase.Extensions;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }

    public GameInfo gameInfo;
    //public PlayerInGame playerInGame;

    public delegate void OnLoadedDelegateMultiple(List<string> jsonData);
    public delegate void OnLoadedDelegate(string json);
    public delegate void OnSaveDelegate();
    FirebaseDatabase db;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        db = FirebaseDatabase.DefaultInstance;
    }

    private void Start()
    {
        // Contains game informantion such as GameName, GameID, List of Players
        gameInfo = new GameInfo();

        // Create List of Players
        gameInfo.players = new List<PlayerInGame>();

        // Add new Players to the List
        gameInfo.players.Add(new PlayerInGame());

        Load();

    }

    public void Load()
    {
        string json = LoadFromFile("SaveData.json");
        gameInfo = JsonUtility.FromJson<GameInfo>(json);
    }

    public string LoadFromFile(string fileName)
    {
        // Open a stream for the supplied file name as a text file
        using var stream = File.OpenText(fileName);

        // Read the entire file and return the result. This assumes that we've written the file in UTF-8
        return stream.ReadToEnd();
    }

    public void LoadData(string path, OnLoadedDelegate onLoadedDelegate)
    {
        db.RootReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);

            onLoadedDelegate(task.Result.GetRawJsonValue());
        });
    }

    public void LoadData(string path, OnLoadedDelegateMultiple onLoadedDelegates)
    {
        db.RootReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            List<string> loadedJson = new List<string>();

            foreach (var item in task.Result.Children)
            {
                loadedJson.Add(item.GetRawJsonValue());
            }

            onLoadedDelegates(loadedJson);
        });
    }

    public void SaveData(string path, string data, OnSaveDelegate onSaveDelegate = null)
    {
        db.RootReference.Child(path).SetRawJsonValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);

            onSaveDelegate?.Invoke();
        });
    }

    public string GetKey(string path)
    {
        return db.RootReference.Child(path).Push().Key;
    }

    public void SavePlayerInfo()
    {
        var gameInfo = new GameInfo();

        gameInfo.displayGameName = "Session 1";
        gameInfo.gameID = "297";
        gameInfo.players = new List<PlayerInGame>();

        gameInfo.players.Add(SessionData.Instance.playerInGame);

        string json = JsonUtility.ToJson(gameInfo);
        SaveToFile("SaveData.json", json);

        Debug.Log(json);
    }

    public void SaveName(string name)
    {
        SessionData.Instance.playerInGame.name = name;
        string json = JsonUtility.ToJson(SessionData.Instance.playerInGame.name);

        SaveToFile("SaveData.json", json);
    }

    public void SavePosition(Vector3 gridLocation)
    {
        SessionData.Instance.playerInGame.gridPositions.Add(gridLocation);
        string json = JsonUtility.ToJson(SessionData.Instance.playerInGame.gridPositions);

        SaveToFile("SaveData.json", json);
    }

    public void SaveIsHidden(bool isHidden)
    {
        SessionData.Instance.playerInGame.hidden = isHidden;
        string json = JsonUtility.ToJson(SessionData.Instance.playerInGame.hidden);

        SaveToFile("SaveData.json", json);
    }

    public void SaveToFile(string fileName, string jsonString)
    {
        // Open a file in write mode. This will create the file if it's missing.
        // It is assumed that the path already exists.
        using (var stream = File.OpenWrite(fileName))
        {
            // Truncate the file if it exists (we want to overwrite the file)
            stream.SetLength(0);

            // Convert the string into bytes. Assume that the character-encoding is UTF8.
            // Do you not know what encoding you have? Then you have UTF-8
            var bytes = Encoding.UTF8.GetBytes(jsonString);

            // Write the bytes to the hard-drive
            stream.Write(bytes, 0, bytes.Length);

            // The "using" statement will automatically close the stream after we leave
            // the scope - this is VERY important
        }
    }
}