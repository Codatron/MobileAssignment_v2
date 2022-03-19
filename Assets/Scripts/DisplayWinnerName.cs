using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayWinnerName : MonoBehaviour
{
    public TMP_Text winnerName;

    void OnEnable()
    {
        TurnManager.onGameOver += DisplayWinner;
    }

    void OnDisable()
    {
        TurnManager.onGameOver -= DisplayWinner;
    }

    void DisplayWinner(string winner)
    {
        winnerName.text = winner + " is the winner!";
    }
}
