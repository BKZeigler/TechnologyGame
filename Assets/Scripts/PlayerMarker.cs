using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    public void MoveTo(Tile tile)
    {
        transform.position = new Vector3(tile.x, tile.y, 0);
    }
}
