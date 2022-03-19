using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void OnEnable()
    {
        TurnManager.onAllObjectsFound += RevealGameOverPanel;
        //Seeker.onAllObjectsFound += RevealGameOverPanel;
    }

    private void OnDisable()
    {
        TurnManager.onAllObjectsFound -= RevealGameOverPanel;

        //Seeker.onAllObjectsFound -= RevealGameOverPanel;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void RevealGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
}
