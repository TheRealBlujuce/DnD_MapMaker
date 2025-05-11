using UnityEngine;
using TMPro; // If you’re using TextMeshPro

[RequireComponent(typeof(LineRenderer))]
public class ExpandingCircle : MonoBehaviour
{
    public Camera mainCamera;
    public TextMeshProUGUI radiusText; // Replace with `public Text distanceText;` if not using TMP
    public int segments = 100;

    private LineRenderer lineRenderer;
    private Vector3 origin;
    private bool isDragging = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = true;
        if (radiusText != null)
            radiusText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance.IsToolActive(GameManager.ToolState.Circle) && Input.GetMouseButtonDown(1))
        {
            origin = GetMouseWorldPosition();
            isDragging = true;

            if (radiusText != null)
                radiusText.gameObject.SetActive(true);
        }

        if (GameManager.Instance.IsToolActive(GameManager.ToolState.Circle) && Input.GetMouseButton(1) && isDragging)
        {
            Vector3 currentMousePos = GetMouseWorldPosition();
            float radius = Vector3.Distance(origin, currentMousePos);
            lineRenderer.enabled = true;
            DrawCircle(origin, radius);

            if (radiusText != null)
            {
                radiusText.text = $"Radius: {radius:F2}";
                Vector3 screenPos = Input.mousePosition;
                radiusText.transform.position = screenPos + new Vector3(10, -10); // Slight offset
            }
        }

        if (GameManager.Instance.IsToolActive(GameManager.ToolState.Circle) && Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            lineRenderer.positionCount = 0;
            lineRenderer.enabled = false;
            if (radiusText != null)
                radiusText.gameObject.SetActive(false);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Distance from camera
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;
        return worldPos;
    }

    void DrawCircle(Vector3 center, float radius)
    {
        float angleStep = 360f / segments;
        Vector3[] points = new Vector3[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float x = Mathf.Cos(angle) * radius + center.x;
            float y = Mathf.Sin(angle) * radius + center.y;
            points[i] = new Vector3(x, y, 0f);
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
}
