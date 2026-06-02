using UnityEngine;
using UnityEngine.InputSystem; // IMPORTANT

public class MapController : MonoBehaviour
{
    public PlayerMarker player;
    public GridManager gridManager;
    [System.NonSerialized]
    private Tile currentTile;

    void Start()
    {
        //if (PlayerManager.Instance.HasLoadedGame)
        //{
            // MapSaveManager will restore position
            //return;
        //}

        // NEW GAME fallback
        //currentTile = gridManager.grid[0, 0];
        //player.MoveTo(currentTile);
        //UpdateTraversableTiles();
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            HandleClick();
    }

    void HandleClick()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        RaycastHit2D hit = Physics2D.Raycast(worldPos2D, Vector2.zero);

        if (hit.collider == null)
            return;

        Tile tile = hit.collider.GetComponent<Tile>();
        if (tile == null)
            return;

        if (tile.isTraversable)
            MovePlayer(tile);
    }

    public void MovePlayer(Tile tile)
    {
        player.MoveTo(tile);
        currentTile = tile;

        // Auto-save with explicit map context
        PlayerManager.Instance.SaveGameFromMap(gridManager, player);

        TriggerTileEvent(tile);

        UpdateTraversableTiles();
    }

    void UpdateTraversableTiles()
    {
        foreach (var tile in gridManager.grid)
            tile.SetTraversable(false);

        int x = currentTile.x;
        int y = currentTile.y;

        TrySetTraversable(x + 1, y);
        TrySetTraversable(x - 1, y);
        TrySetTraversable(x, y + 1);
        TrySetTraversable(x, y - 1);
    }

    void TrySetTraversable(int x, int y)
    {
        if (x >= 0 && x < gridManager.width && y >= 0 && y < gridManager.height)
            gridManager.grid[x, y].SetTraversable(true);
    }

    void TriggerTileEvent(Tile tile)
    {
        if (tile.isCleared)
            return;

        GameSceneManager.Instance.LoadEventScene(tile.eventType);

        tile.isCleared = true;
    }

    public void SetCurrentTile(Tile tile)
    {
        currentTile = tile;
        UpdateTraversableTiles();
    }

    public void MovePlayerWithoutEvent(Tile tile)
    {
        player.MoveTo(tile);
        currentTile = tile;
        UpdateTraversableTiles();
    }
}