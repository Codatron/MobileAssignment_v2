using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }

    public SaveData saveData;

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
        saveData = new SaveData();

        Load();
    }

    public void Load()
    {
        //string json = PlayerPrefs.GetString("SaveData");

        string json = LoadFromFile("saveFile");
        saveData = JsonUtility.FromJson<SaveData>(json);
    }

    public string LoadFromFile(string fileName)
    {
        // Open a stream for the supplied file name as a text file
        using var stream = File.OpenText(fileName);

        // Read the entire file and return the result. This assumes that we've written the file in UTF-8
        return stream.ReadToEnd();
    }

    public void SaveNameSingleP1(string name)
    {
        saveData.NameP1 = name;
        string json = JsonUtility.ToJson(saveData);
        //PlayerPrefs.SetString("SaveData", json);

        SaveToFile("saveFile", json);
    }
    public void SaveNameMultiP1(string name)
    {
        saveData.NameMultiP1 = name;
        string json = JsonUtility.ToJson(saveData);
        //PlayerPrefs.SetString("SaveData", json);

        SaveToFile("saveFile", json);
    }

    public void SaveNameMultiP2(string name)
    {
        saveData.NameMultiP2 = name;
        string json = JsonUtility.ToJson(saveData);
        //PlayerPrefs.SetString("SaveData", json);

        SaveToFile("saveFile", json);
    }

    public void SaveHighScore(int scoreToSave)
    {
        saveData.HighScore = scoreToSave;
        string json = JsonUtility.ToJson(saveData);
        //PlayerPrefs.SetString("SaveData", json);

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

