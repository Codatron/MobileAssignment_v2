using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class SaveButtonSetup : MonoBehaviour
{
    public TMP_InputField inputName;

    private void Start()
    {
        // Displays the player's last chosen name in the text field
        if (SessionData.Instance.playerInGame.userID == FirebaseAuth.DefaultInstance.CurrentUser.UserId)
        {
            inputName.text = SessionData.Instance.playerInfo.name;
        }
    }

    public void SaveName()
    {
        SessionData.Instance.playerInfo.name = inputName.text; 
        SessionData.Instance.playerInGame.name = SessionData.Instance.playerInfo.name;
        SessionData.SavePlayerInfoData();
    }
}