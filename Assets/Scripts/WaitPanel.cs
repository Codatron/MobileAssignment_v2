using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPanel : MonoBehaviour
{
    public GameObject waitPanel;

    //void Awake()
    //{
    //    Hider.onAllObjectsHidden += RevealWaitPanel;
    //    waitPanel.SetActive(false);
    //}

    //void OnDisable()
    //{
    //    Hider.onAllObjectsHidden -= RevealWaitPanel;
    //}

    private void RevealWaitPanel(bool amReady)
    {
        waitPanel.SetActive(true);
    }

    private void HideWaitPanel(bool isOtherPlayerReady)
    {
        waitPanel.SetActive(false);
    }

}
