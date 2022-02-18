using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class DisplayGameName : MonoBehaviour
{
    public TMP_Text displayGameName;

    void Start()
    {
        displayGameName.text = "Nut Hunt: " + SessionData.Instance.gameInfo.displayGameName;
    }
}
