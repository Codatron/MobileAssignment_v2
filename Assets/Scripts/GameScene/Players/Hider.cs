using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public delegate void OnAllObjectsHidden();

public class Hider : MonoBehaviour
{
    public OnAllObjectsHidden onAllObjectsHidden;

    [SerializeField] private List<GameObject> objects = new List<GameObject>();

    public Button confirmButton;
    public int totalObjectsHidden;

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
        foreach (var nut in objects)
        {
            SpriteRenderer spriteRend = nut.GetComponent<SpriteRenderer>();
            spriteRend.sortingOrder = -1;
        }
    }
}
        // TODO: Use event delegate instead
        //  - Tell UI button to become interactable
        //  - Tell nuts in the list to 'hide'.
        //  - Tell GameState to change
