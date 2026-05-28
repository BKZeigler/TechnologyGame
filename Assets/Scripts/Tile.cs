using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public bool isTraversable;
    private SpriteRenderer sr;

    public void Initialize(int x, int y, bool traversable)
    {
        this.x = x;
        this.y = y;
        sr = GetComponent<SpriteRenderer>();
        SetTraversable(traversable);
    }

    public void SetTraversable(bool value)
    {
        isTraversable = value;
        sr.color = value ? Color.gray : Color.white;
    }
}
