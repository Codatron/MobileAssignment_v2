using UnityEngine;
using TMPro;

public class DragDrop : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public Transform originalPosition;
    public bool isDragging;
    public TMP_Text text;
    public float distanceToGridCellCenter = 0.5f;

    private Hider hider;
    private GridGenerate gridManager;
    private Camera cam;
    private int objectValue = 1;

    private void Start()
    {
        cam = Camera.main;

        transform.position = originalPosition.position;

        gridManager = FindObjectOfType<GridGenerate>();
        hider = GameObject.Find("Hider").GetComponent<Hider>();
    }

    private void Update()
    {
        if (!isDragging)
            return;
    }
    public void SnapToOriginalPosition()
    {
        transform.position = originalPosition.position;
    }

    public Vector3 MouseWorldPosition()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;

        return mousePos;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        Vector3 offset = transform.position - MouseWorldPosition();
        spriteRend.color = new Color(0.35f, 0.23f, 0.11f, 0.65f);

        BoardSpace cellWithinRange = gridManager.GetGridCellWithinRange(distanceToGridCellCenter, MouseWorldPosition());

        if (cellWithinRange != null)
        {
            cellWithinRange.isOccupied = false;
            hider.CountObjectsToHide(-objectValue);
        }
    }

    private void OnMouseDrag()
    {
        transform.position = MouseWorldPosition();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        BoardSpace cellWithinRange = gridManager.GetGridCellWithinRange(distanceToGridCellCenter, MouseWorldPosition());
        
        if (cellWithinRange != null)
        {
            if (!cellWithinRange.isOccupied)
            {
                cellWithinRange.isOccupied = true;
                hider.CountObjectsToHide(objectValue);
                transform.position = cellWithinRange.transform.position;
            }
            else
            {
                SnapToOriginalPosition();
            }
        }
        else
        {
            SnapToOriginalPosition();
        }
        
        spriteRend.color = new Color(0.35f, 0.23f, 0.11f, 1f);
    }
}