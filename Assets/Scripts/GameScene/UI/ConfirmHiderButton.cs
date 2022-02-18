using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnGridPlacementConfirmation();

public class ConfirmHiderButton : MonoBehaviour
{
    public static OnGridPlacementConfirmation onGridPlacementConfirmation;
    public GameObject confirmHiderButton;
    public GameObject seeker;

    private BoardSpace tileSpriteRend;

    public void DoneHiding()
    {
        // Tells hider it's time to hide objects --- should the hider also be reponsible for removing BoxColliders
        //  instead of doing it here in l.21-29?
        onGridPlacementConfirmation?.Invoke();

        GameObject[] nuts = GameObject.FindGameObjectsWithTag("Nut");

        foreach (var nut in nuts)
        {
            BoxCollider2D nutBox = nut.GetComponent<BoxCollider2D>();
            Destroy(nutBox);

            SaveGridPosition(nut.transform.position);
        }

        SaveHidden(true);
        
        Debug.Log("Time to seek");

        seeker.SetActive(true);
        confirmHiderButton.SetActive(false);


        //TODO:
        //  - SaveManager.Instance.gameInfo.players[0].Hidden = true;
        //  - GameManager.UpdateGameState(SaveManager.Instance.gameInfo.players[0].Hidden);
        //  - Tell grid cells to turn green again
        //  - Change color back to green when other player's turn
    }

    private void SaveGridPosition(Vector3 gridPosition)
    {
        SaveManager.Instance.SavePosition(gridPosition);
    }

    private void SaveHidden(bool isHidden)
    {
        SaveManager.Instance.SaveIsHidden(isHidden);
    }

    IEnumerator DisplayPrompt()
    {
        yield return new WaitForSeconds(2f); 
    }
}
