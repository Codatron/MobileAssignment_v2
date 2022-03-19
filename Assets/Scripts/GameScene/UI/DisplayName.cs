using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class DisplayName : MonoBehaviour
{
    public TMP_Text displayName;

    void OnAwake()
    {
        TurnManager.onGameStart += ChangeDisplayName;
    }

    void OnDestroy()
    {
        TurnManager.onGameStart -= ChangeDisplayName;
    }

    void Start()
    {
        //displayName.text = SessionData.Instance.playerInfo.name + ", hide your nuts!";
    }

    void ChangeDisplayName() // This does not work
    {
        if (SessionData.Instance.playerInGame.playerNumber == 0)
        {
            displayName.text = "Find " + SaveManager.Instance.gameInfo.players[1].name +  "'s nuts!";
        }
        else if (SessionData.Instance.playerInGame.playerNumber == 1)
        {
            displayName.text = "Find " + SaveManager.Instance.gameInfo.players[0].name + "'s nuts!";
        }
    }
}
