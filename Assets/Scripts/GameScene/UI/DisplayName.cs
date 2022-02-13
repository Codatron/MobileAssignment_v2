using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayName : MonoBehaviour
{
    public TMP_Text displayName;

    void Start()
    {
        displayName.text = "Squirrel: " + SaveManager.Instance.playerInfo.Name;
    }
}
