using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    public int CurrentX { get; private set; }
    public int CurrentY { get; private set; }

    public void MoveTo(Tile tile)
    {
        CurrentX = tile.x;
        CurrentY = tile.y;
        transform.position = new Vector3(tile.x, tile.y, 0);
    }

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
        transform.position = new Vector3(x, y, 0);
    }
}