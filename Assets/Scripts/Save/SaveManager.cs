using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }

    public GameInfo gameInfo;
    public PlayerInfo playerInfo;

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
    }

    private void Start()
    {
        // Contains game informantion such as GameName, GameID, List of Players
        gameInfo = new GameInfo(); 

        // Create List of Players
        gameInfo.players = new List<PlayerInfo>();

        // Add new Players to the List
        gameInfo.players.Add(new PlayerInfo());

        Load();
    }

    public void Load()
    {
        string json = LoadFromFile("saveFile");
        gameInfo = JsonUtility.FromJson<GameInfo>(json);
    }

    public string LoadFromFile(string fileName)
    {
        // Open a stream for the supplied file name as a text file
        using var stream = File.OpenText(fileName);

        // Read the entire file and return the result. This assumes that we've written the file in UTF-8
        return stream.ReadToEnd();
    }

    public void Save(GameObject saveObject)
    {
        var playerInfo = new PlayerInfo();

        var component = saveObject.GetComponent<PlayerInfo>();

        playerInfo.Name = component.Name;
        playerInfo.Hidden = component.Hidden;
        playerInfo.GridLocation = new List<Vector2Int>();
        playerInfo.Attempts = component.Attempts;
        playerInfo.TotalObjectsFound = component.TotalObjectsFound;
        playerInfo.Time = component.Time;
    }

    public void SaveName(string name)
    {
        //var playerInfo = new PlayerInfo();

        playerInfo.Name = name;
        string json = JsonUtility.ToJson(playerInfo.Name);

        SaveToFile("saveFile", json);
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

