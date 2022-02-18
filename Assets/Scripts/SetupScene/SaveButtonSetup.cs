using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class SaveButtonSetup : MonoBehaviour
{
    public TMP_InputField inputName;

    public void SaveName()
    {
        SessionData.Instance.playerInfo.name = inputName.text; // playerInGame.name or playerInfo.name???
        SessionData.SaveData();

        //string userPath = "users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        //string json = JsonUtility.ToJson(SessionData.playerInfo);

        //SaveManager.Instance.SaveData("users/" + userPath, json);
    }
}

        //SaveManager.Instance.SavePlayerInfo();
        // Old way
        //SaveManager.Instance.SaveName(inputName.text);