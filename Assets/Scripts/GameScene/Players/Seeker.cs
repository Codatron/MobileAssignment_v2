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

        if (totalObjectsFound == maxObjectsToFind)
        {
            GameOver();

            // TODO: 
            //  - this spams the db...hwo to fix this?
            SaveManager.Instance.SavePlayerInfo();
            Debug.Log(SessionData.Instance.playerInGame);
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
                CountObjectsFound(findValue);
                CountTotalTries(tries);

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

        // make check here, if yes, call function
        SessionData.Instance.playerInGame.totalObjectsFound++;
    }

    public void CountTotalTries(int tries)
    {
        totalTries += tries;
        cellWithinRange.gameObject.SetActive(false);

        SessionData.Instance.playerInGame.attempts++;
    }

    public void GameOver()
    {
        onAllObjectsFound?.Invoke();
    }
}
      

