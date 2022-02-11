using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuButton : MonoBehaviour
{
    public void LoadScene(int sceneNumber)
    {
        string sceneName = SceneManager.GetSceneByBuildIndex(sceneNumber).name;
        LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
