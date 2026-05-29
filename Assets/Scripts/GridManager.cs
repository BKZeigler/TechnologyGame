using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public GameObject tilePrefab;
    public Tile[,] grid;

    void Awake()
    {
        grid = new Tile[width, height];
        GenerateGrid();
    }
    void Start()
    {

    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileEventType eventType = RollEventForTile(x, y);

                var tileObj = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                var tile = tileObj.GetComponent<Tile>();
                tile.Initialize(x, y, false, eventType);

                grid[x, y] = tile;
            }
        }
    }

    TileEventType RollEventForTile(int x, int y)
    {
        // Boss tile (top-right)
        if (x == width - 1 && y == height - 1)
        {
            return TileEventType.Boss;
        }

        // Random event for now
        int roll = Random.Range(0, 2);

        if (roll == 0) return TileEventType.Battle;
        if (roll == 1) return TileEventType.TechReward;

        return TileEventType.None;
    }
}

