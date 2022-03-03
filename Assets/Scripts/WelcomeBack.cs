using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WelcomeBack : MonoBehaviour
{
    public TMP_Text welcomeText;

    private void Start()
    {
        WelcomeText();
    }

    public void WelcomeText()
    {
        if (SessionData.Instance.playerInfo.name == null || SessionData.Instance.playerInfo.name == "")
            welcomeText.text = "Welcome, stranger! Would you like to change your name? If not, just press Play";
        else
            welcomeText.text = "Welcome back, " + SessionData.Instance.playerInfo.name + "! You can change your name below. Otherwise, just press Play.";
    }
}
