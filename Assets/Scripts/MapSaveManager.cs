using UnityEngine;
using System.Collections.Generic;

public class MapSaveManager : MonoBehaviour
{
    public static MapSaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // -----------------------------
    // SAVE MAP (called from MapController via PlayerManager)
    // -----------------------------
    public void SaveMap(GameSaveData save, GridManager grid, PlayerMarker marker)
    {
        if (grid == null || marker == null)
        {
            Debug.LogWarning("MapSaveManager: Grid or Marker missing during SaveMap");
            return;
        }
        Debug.Log("SaveMap writing position: " + marker.CurrentX + ", " + marker.CurrentY);

        save.playerX = marker.CurrentX;
        save.playerY = marker.CurrentY;

        save.mapEvents.Clear();

        foreach (var tile in grid.AllTiles)
        {
            save.mapEvents.Add(new TileEventSaveData
            {
                x = tile.x,
                y = tile.y,
                eventType = tile.eventType,
                isCleared = tile.isCleared,
                isTraversable = tile.isTraversable
            });
        }
    }

    // -----------------------------
    // LOAD MAP (called when entering MapScene)
    // -----------------------------
    public void LoadMap(GameSaveData save)
    {
        GridManager grid = FindFirstObjectByType<GridManager>();
        PlayerMarker marker = FindFirstObjectByType<PlayerMarker>();
        MapController mapController = FindFirstObjectByType<MapController>();

        if (grid == null || marker == null || mapController == null)
        {
            Debug.LogWarning("MapSaveManager: Missing components during LoadMap");
            return;
        }

        // Restore tile states (BUT NOT isTraversable)
        foreach (var ev in save.mapEvents)
        {
            Tile loadedTile = grid.GetTile(ev.x, ev.y);
            if (loadedTile == null) continue;

            loadedTile.eventType = ev.eventType;
            loadedTile.isCleared = ev.isCleared;
            loadedTile.RefreshVisual();
        }

        // Move player and recompute traversables
        Tile tile = grid.GetTile(save.playerX, save.playerY);
        mapController.MovePlayerWithoutEvent(tile);
    }
}