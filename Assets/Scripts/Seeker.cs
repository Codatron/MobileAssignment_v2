using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    private BoardSpace cell;
    private GridGenerate gridManager;
    private DragDrop dragDrop;
    public int totalObjectsFound;
    public int totalTries;
    private bool hasBeenPicked;

    void Start()
    {
        gameObject.SetActive(false);

        totalObjectsFound = 0;
        totalTries = 0;

        gridManager = FindObjectOfType<GridGenerate>();
        dragDrop = FindObjectOfType<DragDrop>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            BoardSpace cellWithinRange = gridManager.GetGridCellWithinRange(0.5f, dragDrop.MouseWorldPosition());

            if (cellWithinRange != null)
            {
                if (cellWithinRange.isOccupied && cellWithinRange.CompareTag("Tile"))
                {
                    totalTries++;
                    totalObjectsFound++;
                    cellWithinRange.gameObject.SetActive(false);
                }
                else
                {
                    totalTries++;
                    cellWithinRange.gameObject.SetActive(false);
                    //return;
                }
            }
            
            
        }
    }

    public void CountObjectsToFound(int objectFound)
    {
        totalObjectsFound += objectFound;
    }

    public void CountTotalTries(int tries)
    {
        totalTries += tries;
    }
}
      

