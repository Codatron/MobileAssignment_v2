using UnityEngine;

public delegate void ObjectFound(int found);
//public delegate void ObjectsFound();
public delegate void Attempts(int attempts);

public class Seeker : MonoBehaviour
{
    public static event ObjectFound onObjectFound;
    //public static event ObjectsFound onAllObjectsFound;
    public static event Attempts onAttempt;
    public bool isGameOver;

    private BoardSpace cellWithinRange;
    private GridGenerate gridManager;
    private DragDrop dragDrop;
    [SerializeField] private int maxObjectsToFind;
    [SerializeField] private int findValue;
    [SerializeField] private int totalObjectsFound;
    [SerializeField] private int tries;
    [SerializeField] private int totalTries;

    void OnAwake()
    {
        TurnManager.onAllObjectsFound += GameOver;
    }

    void OnDestroy()
    {
        TurnManager.onAllObjectsFound -= GameOver;
    }

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
    }

    private void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetMouseButtonUp(0))
            {
                SeekHiddenObjects();
            }
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

            SessionData.Instance.playerInGame.totalObjectsFound = totalObjectsFound;
            SessionData.SavePlayerInGameData();
        }
    }

    public void CountTotalTries(int tries)
    {
        totalTries += tries;
        cellWithinRange.gameObject.SetActive(false);

        SessionData.Instance.playerInGame.attempts++;
        SessionData.SavePlayerInGameData();
    }

    public void GameOver()
    {
        //onAllObjectsFound?.Invoke();
        isGameOver = true;

        SessionData.Instance.playerInGame.allObjectsFound = true;
        SessionData.SavePlayerInGameData();
    }
}
      

