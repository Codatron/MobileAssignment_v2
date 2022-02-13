using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DisplayFound : MonoBehaviour
{
    public TMP_Text displayFound;

    private int findsToDisplay;

    private void OnEnable()
    {
        Seeker.onObjectFound += UpdateDisplayObjectFound;
    }

    private void OnDisable()
    {
        Seeker.onObjectFound -= UpdateDisplayObjectFound;
    }

    private void UpdateDisplayObjectFound(int findsToAdd)
    {
        findsToDisplay += findsToAdd;
        displayFound.text = "Found " + findsToDisplay;
    }
}
