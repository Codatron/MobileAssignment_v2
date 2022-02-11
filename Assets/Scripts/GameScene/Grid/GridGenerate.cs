using UnityEngine;

public class GridGenerate : MonoBehaviour
{
    public BoardSpace[,] grid;
    public int width;
    public int height;
    public Vector3 gridStartPosition;

    [SerializeField] private GameObject tilePrefab;

    void Start()
    {
        grid = GenerateGrid(gridStartPosition, tilePrefab);
    }

    private BoardSpace[,] GenerateGrid(Vector3 gridOffset, GameObject tilePrefab)
    {
        var grid = new BoardSpace[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPosition = new Vector3(x, y);
                grid[x, y] = Instantiate(tilePrefab, worldPosition + gridOffset, Quaternion.identity).GetComponent<BoardSpace>();
                grid[x, y].name = $"{tilePrefab.name} {x} {y}";
                grid[x, y].Init(x, y, false);
            }
        }
        return grid;
    }

    public BoardSpace GetGridCellWithinRange(float distance, Vector3 mousePos)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float dist = Vector3.Distance(grid[x, y].transform.position, mousePos);
                
                if (dist < distance)
                    return grid[x, y];
            }
        }
        return null;
    }

    public BoardSpace GridCellHasBeenPicked(float distance, Vector3 mousePos)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float dist = Vector3.Distance(grid[x, y].transform.position, mousePos);

                if (dist < distance)
                    return grid[x, y];
            }
        }
        return null;
    }
}