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
        SessionData.Instance.playerInfo.name = inputName.text; 
        SessionData.Instance.playerInGame.name = SessionData.Instance.playerInfo.name;

        SessionData.SavePlayerInfoData();
        //SessionData.SavePlayerInGameData();
    }
}

        //SaveManager.Instance.SavePlayerInfo();
        // Old way
        //SaveManager.Instance.SaveName(inputName.text);