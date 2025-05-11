using UnityEngine;

public class DragAndDropToken : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;

    // Set the grid size (usually 1 unit in Unity)
    public float gridSize = 1f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPosition = new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z) + offset;
        transform.position = newPosition;
    }

    void OnMouseUp()
    {
        isDragging = false;
        SnapToGrid();
    }

    void SnapToGrid()
    {
        Vector3 pos = transform.position;
        float snappedX = Mathf.Round(pos.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(pos.y / gridSize) * gridSize;
        transform.position = new Vector3(snappedX, snappedY, pos.z);
    }
}
