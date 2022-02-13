using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DisplayAttempts : MonoBehaviour
{
    public TMP_Text displayAttempts;

    private int attemptsTodisplay;

    private void OnEnable()
    {
        Seeker.onAttempt += UpdateDisplayAttempts;
    }

    private void OnDisable()
    {
        Seeker.onAttempt -= UpdateDisplayAttempts;
    }

    private void UpdateDisplayAttempts(int attemptsToAdd)
    {
        attemptsTodisplay += attemptsToAdd;
        displayAttempts.text = "Tries: " + attemptsTodisplay;
    }
}
