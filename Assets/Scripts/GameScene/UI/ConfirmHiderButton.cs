using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmHiderButton : MonoBehaviour
{
    public GameObject seeker;
    public GameObject confirmHiderButton;

    private BoardSpace tileSpriteRend;

    public void DoneHiding()
    {
        Hider hider = GameObject.Find("Hider").GetComponent<Hider>();
        hider.HideObjects();

        GameObject[] nuts = GameObject.FindGameObjectsWithTag("Nut");

        foreach (var nut in nuts)
        {
            // Removes box collider from game object so that it cannot be interacted with
            BoxCollider2D nutBox = nut.GetComponent<BoxCollider2D>();
            Destroy(nutBox);
        }

        Debug.Log("Time to seek");

        seeker.SetActive(true);
        confirmHiderButton.SetActive(false);

        SaveManager.Instance.playerInfo.Hidden = true;

        //TODO:
        //  - Tell grid cells to turn green again
        //  - Change color back to green when other player's turn


        //SaveManager.Instance.gameInfo.players[0].Hidden = true;
        //GameManager.UpdateGameState(SaveManager.Instance.gameInfo.players[0].Hidden);


 


    }
}
