using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void OnEnable()
    {
        Seeker.onAllObjectsFound += RevealGameOverPanel;
    }

    private void OnDisable()
    {
        Seeker.onAllObjectsFound -= RevealGameOverPanel;
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
