using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DragDrop : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public Transform originalPosition;
    public Vector3 nutPosition;

    private Vector3 offset;
    private GridGenerate gridManager;
    private Camera cam;
    public bool isDragging;
    public bool isOnTile;
    public bool isLockedIn;
    public int numberOfNuts = 5;
    public TMP_Text text;

    private void Start()
    {
        cam = Camera.main;

        transform.position = originalPosition.position;
        gridManager = FindObjectOfType<GridGenerate>();
        
    }

    private void Update()
    {
        if (!isDragging)
            return;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - MouseWorldPosition();
        spriteRend.color = new Color(0.35f, 0.23f, 0.11f, 0.65f);

        BoardSpace cellWithinRange = gridManager.GetGridCellWithinRange(0.5f, MouseWorldPosition());

        if (cellWithinRange != null)
            cellWithinRange.isOccupied = false;
    }

    private void OnMouseDrag()
    {
        transform.position = MouseWorldPosition();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        BoardSpace cellWithinRange = gridManager.GetGridCellWithinRange(0.5f, MouseWorldPosition());
        
        if (cellWithinRange != null)
        {
            if (!cellWithinRange.isOccupied)
            {
                cellWithinRange.isOccupied = true;
                transform.position = cellWithinRange.transform.position;

                numberOfNuts--;
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

    public void SnapToOriginalPosition()
    {
        transform.position = originalPosition.position;
    }

    private Vector3 MouseWorldPosition()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;

        return mousePos;
    }
}