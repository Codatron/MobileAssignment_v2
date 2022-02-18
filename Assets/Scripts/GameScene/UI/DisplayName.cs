using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class DisplayName : MonoBehaviour
{
    public TMP_Text displayName;

    void Start()
    {
        displayName.text = SessionData.Instance.playerInfo.name + ", hide your nuts!";
    }
}
