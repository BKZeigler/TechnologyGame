using UnityEngine;

public enum TileEventType
{
    None,
    Battle,
    TechReward,
    Boss
}

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public bool isTraversable;
    public TileEventType eventType;
    public bool isCleared;

    private SpriteRenderer sr;

    public void Initialize(int x, int y, bool traversable, TileEventType eventType)
    {
        this.x = x;
        this.y = y;
        this.eventType = eventType;
        this.isCleared = false;

        sr = GetComponent<SpriteRenderer>();
        SetTraversable(traversable);
    }

    public void SetTraversable(bool value)
    {
        isTraversable = value;
        sr.color = value ? Color.gray : Color.white;
    }
}