using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public delegate void OnAllObjectsHidden(bool amReady); // make bool to trigger enable/disable in ConfirmButton class

public class Hider : MonoBehaviour
{
    public static OnAllObjectsHidden onAllObjectsHidden;
    public Button confirmButton;
    public int totalObjectsHidden;

    public List<GameObject> objects = new List<GameObject>();

    private void OnEnable()
    {
        ConfirmHiderButton.onGridPlacementConfirmation += HideObjects;    
    }
    private void OnDisable()
    {
        ConfirmHiderButton.onGridPlacementConfirmation -= HideObjects;
    }

    void Start()
    {
        confirmButton.interactable = false;
    }

    public void CountObjectsToHide(int objectToHide)
    {
        totalObjectsHidden += objectToHide;

        ChangeUI(totalObjectsHidden);
    }

    public void ChangeUI(int totalObjectsHidden)
    {
        if (totalObjectsHidden > 4)
            confirmButton.interactable = true;
        else
            confirmButton.interactable = false;

        //onAllObjectsHidden?.Invoke();
    }

    public void HideObjects()
    {
        SessionData.Instance.playerInGame.gridPositions = new List<Vector3>();

        foreach (var nut in objects)
        {
            SpriteRenderer spriteRend = nut.GetComponent<SpriteRenderer>();
            spriteRend.sortingOrder = -1;

            var position = nut.GetComponent<Transform>().position;
            SessionData.Instance.playerInGame.gridPositions.Add(new Vector3(position.x, position.y));

            BoxCollider2D nutBox = nut.GetComponent<BoxCollider2D>();
            Destroy(nutBox);
        }

        SessionData.Instance.playerInGame.hidden = true;
        SessionData.SavePlayerInfoData();


        SessionData.SavePlayerInGameData();

        onAllObjectsHidden?.Invoke(true);   // Enables WaitPanel.cs

        Debug.Log(SessionData.Instance.playerInGame.name + ", time to seek");
    }
}
