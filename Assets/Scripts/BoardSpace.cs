using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    SpriteRenderer spriteRend;

    public bool isOccupied;
    public int x;
    public int y;

    public void Init(int x, int y, bool occupied)
    {
        this.x = x;
        this.y = y;
        this.isOccupied = occupied;
    }

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        spriteRend.color = isOccupied ? Color.red : Color.green;
    }
}
