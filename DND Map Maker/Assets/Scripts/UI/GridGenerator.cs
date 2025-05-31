using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGenerator : MonoBehaviour
{
    [Header("References")]
    public Tilemap groundTilemap; // Assign your ground tilemap
    public GameObject tilePrefab; // Assign your grid tile overlay prefab

    private void Start()
    {
        if (groundTilemap == null)
        {
            //Debug.LogError("Ground Tilemap not assigned!");
            return;
        }

        if (tilePrefab == null)
        {
            //Debug.LogError("Tile Prefab not assigned!");
            return;
        }

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                if (!groundTilemap.HasTile(cellPos)) continue;

                Vector3 worldPos = groundTilemap.GetCellCenterWorld(cellPos);
                GameObject grid = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
                grid.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
    }
}
