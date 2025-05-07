using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public float tileSpacing = 1f;

    [Header("Tile Prefab")]
    public GameObject tilePrefab;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("Tile prefab not assigned!");
            return;
        }

        Vector2 origin = transform.position;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 localOffset = new Vector2(x * tileSpacing, y * tileSpacing);
                Vector2 spawnPosition = origin + localOffset;

                Instantiate(tilePrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }
}
