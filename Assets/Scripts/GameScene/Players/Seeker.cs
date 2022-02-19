using UnityEngine;

public delegate void ObjectFound(int found);
public delegate void ObjectsFound();
public delegate void Attempts(int attempts);

public class Seeker : MonoBehaviour
{
    public static event ObjectFound onObjectFound;
    public static event ObjectsFound onAllObjectsFound;
    public static event Attempts onAttempt;

    private BoardSpace cellWithinRange;
    private GridGenerate gridManager;
    private DragDrop dragDrop;
    [SerializeField] private int maxObjectsToFind;
    //[SerializeField] private int maxTries;
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
        //maxTries = 15;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SeekHiddenObjects();
        }
    }

    private void SeekHiddenObjects()
    {
        cellWithinRange = gridManager.GetGridCellWithinRange(dragDrop.distanceToGridCellCenter, dragDrop.MouseWorldPosition());

        if (cellWithinRange != null && !cellWithinRange.hasBeenPicked)
        {
            if (cellWithinRange.isOccupied && cellWithinRange.CompareTag("Tile"))
            {
                cellWithinRange.hasBeenPicked = true;
                CountTotalTries(tries);
                CountObjectsFound(findValue);

                onObjectFound?.Invoke(findValue);
                onAttempt?.Invoke(tries);
            }
            else
            {
                cellWithinRange.hasBeenPicked = true;
                CountTotalTries(tries);
                onAttempt?.Invoke(tries);
            }
        }
    }

    public void CountObjectsFound(int objectFound)
    {
        totalObjectsFound += objectFound;
        SessionData.Instance.playerInGame.totalObjectsFound++;

        if (totalObjectsFound == maxObjectsToFind)
        {
            GameOver();

            //SaveManager.Instance.SavePlayerInfo();
            SessionData.Instance.playerInGame.totalObjectsFound = totalObjectsFound;

            //TODO
            //  - 1st round, totalObjectsFound increased from 0 - 5, as it should
            //  - if PlayAgain pressed, totalObjectsFound increases from 5 - 10
            //  - if PlayAgain again, totalObjectsFound increases from 5 - 10, etc
        }
    }

    public void CountTotalTries(int tries)
    {
        totalTries += tries;
        SessionData.Instance.playerInGame.attempts++;
        cellWithinRange.gameObject.SetActive(false);

        //TODO
        //  - remove BoxColliders from remaining unclicked boxes
        //  - if not removed and clicked, extra attempts added in friebase at the beginning of a round
    }

    public void GameOver()
    {
        onAllObjectsFound?.Invoke();
        Debug.Log(SessionData.Instance.playerInGame);
        SessionData.SavePlayerInGameData();
    }
}
      

