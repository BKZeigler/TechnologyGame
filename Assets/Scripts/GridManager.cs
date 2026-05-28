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
                var tileObj = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                var tile = tileObj.GetComponent<Tile>();
                tile.Initialize(x, y, false); // start as not traversable
                grid[x, y] = tile;
            }
        }

        // Example: make start tile traversable
        grid[0, 0].SetTraversable(true);
    }
}
