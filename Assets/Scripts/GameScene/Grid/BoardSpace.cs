using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    SpriteRenderer spriteRend;

    public bool isOccupied;
    public bool hasBeenPicked;
    public int x;
    public int y;

    public void Init(int x, int y, bool isOccupied)
    {
        this.x = x;
        this.y = y;
        this.isOccupied = isOccupied;
    }

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //spriteRend.color = isOccupied ?  new Color(0.79f, 0.14f, 0.11f, 1f) : new Color(0.44f, 0.57f, 0.11f, 1f);

        //TODO:
        //  - isOccupied color ONLY when hider's turn - use GameState to change?
        //  - Perhaps replace leaf sprite with nut instead of using colors in the hide phase?
    }
}
