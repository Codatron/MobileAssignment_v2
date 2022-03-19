using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPanel : MonoBehaviour
{
    public GameObject waitPanel;

    void Awake()
    {
        Hider.onAllObjectsHidden += RevealWaitPanel;
        TurnManager.onGameStart += HideWaitPanel;
        waitPanel.SetActive(false);
    }

    void OnDisable()
    {
        Hider.onAllObjectsHidden -= RevealWaitPanel;
        TurnManager.onGameStart -= HideWaitPanel;
    }

    private void RevealWaitPanel(bool amReady)
    {
        waitPanel.SetActive(true);
    }

    private void HideWaitPanel()
    {
        waitPanel.SetActive(false);
    }

}
