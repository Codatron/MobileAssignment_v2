using UnityEngine;

public delegate void ObjectsFound();

public class Seeker : MonoBehaviour
{
    public static event ObjectsFound onAllObjectsFound;
    
    public ObjectsFound allObjectsFound;
    private BoardSpace cellWithinRange;
    private GridGenerate gridManager;
    private DragDrop dragDrop;
    [SerializeField] private int maxObjectsToFind;
    [SerializeField] private int maxTries;
    [SerializeField] private int findValue;
    [SerializeField] private int totalObjectsFound;
    [SerializeField] private int tries;
    [SerializeField] private int totalTries;

    //public int findValue;
    //public int totalObjectsFound;
    //public int tries;
    //public int totalTries;

    void Start()
    {
        gameObject.SetActive(false);

        gridManager = FindObjectOfType<GridGenerate>();
        dragDrop = FindObjectOfType<DragDrop>();

        totalObjectsFound = 0;
        findValue = 1;
        maxObjectsToFind = 5;

        totalTries = 0;
        tries = 1;
        maxTries = 15;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SeekHiddenObjects();
        }

        if (totalObjectsFound == maxObjectsToFind)
            GameOver();
    }

    private void SeekHiddenObjects()
    {
        cellWithinRange = gridManager.GetGridCellWithinRange(dragDrop.distanceToGridCellCenter, dragDrop.MouseWorldPosition());

        if (cellWithinRange != null && !cellWithinRange.hasBeenPicked)
        {
            if (cellWithinRange.isOccupied && cellWithinRange.CompareTag("Tile"))
            {
                cellWithinRange.hasBeenPicked = true;
                CountObjectsToFound(findValue);
                CountTotalTries(tries);
            }
            else
            {
                cellWithinRange.hasBeenPicked = true;
                CountTotalTries(tries);
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
        cellWithinRange.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        onAllObjectsFound?.Invoke();
    }
}
      

